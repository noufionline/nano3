using DevExpress.Export.Xl;
using FluentValidation.Results;
using Jasmine.Core.Contracts;
using Prism.Events;
using Prism.Services.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using IDialogService = Prism.Services.Dialogs.IDialogService;

namespace Jasmine.Core.Mvvm
{
    public abstract class CriteriaViewModelBase<T> : DialogAwareViewModelBase
        where T : class, IDirty, INotifyPropertyChanged, INotifyDataErrorInfo, ISupportValidation, new()
    {
        private T _entity;

        protected CriteriaViewModelBase(IEventAggregator eventAggregator, IDialogService dialogService,
            IAuthorizationCache authCache) : base(eventAggregator, dialogService, authCache) => Entity = new T();

        protected virtual void OnEntitySet(T entity)
        {

        }

        public T Entity
        {
            get => _entity;
            protected set
            {

                if (_entity != null)
                {
                    _entity.PropertyChanged -= _entity_PropertyChanged;
                    _entity.ErrorsChanged -= _entity_ErrorsChanged;
                }

                _entity = value;
                
                if (_entity != null)
                {
                    _entity.PropertyChanged += _entity_PropertyChanged;
                    _entity.ErrorsChanged += _entity_ErrorsChanged;

                    RefreshErrors();
                }

                OnEntitySet(_entity);
            }
        }

        private void _entity_ErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
             RefreshErrors();
        }

        private void _entity_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RaiseCanExecuteChanged();
        }

        private void RefreshErrors()
        {
            ShowValidationSummary = Entity.HasErrors;
            ValidationSummary = new ObservableCollection<ValidationFailure>(Entity.ValidationSummary);
        }


        protected abstract void RaiseCanExecuteChanged();

        public bool Processing { get; set; }


        protected string GetExportPath(string fileName)
        {
            string fileToWriteTo = Path.GetRandomFileName();

            string tempDirectory = Path.GetTempPath();

            string directory = Path.Combine(tempDirectory, fileToWriteTo);

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            if (fileName.IndexOf(".Xlsx", StringComparison.Ordinal) == -1)
            {
                fileName = $"{fileName}.Xlsx";
            }

            string path = Path.Combine(directory, fileName);
            return path;
        }

           protected void CreateHeading(IXlSheet sheet, XlCellFormatting formatting, string[] headings)
            {
                var verticalCenter = new XlCellFormatting { Alignment = new XlCellAlignment { VerticalAlignment = XlVerticalAlignment.Center } };
                using (IXlRow row = sheet.CreateRow())
                {
                    row.HeightInPoints = 18;

                    foreach (var heading in headings)
                    {
                        using (IXlCell cell = row.CreateCell())
                        {
                            cell.Value = heading;
                            cell.ApplyFormatting(formatting);
                            cell.ApplyFormatting(verticalCenter);
                        }
                    }

                }
            }

    }
}