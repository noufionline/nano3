using AutoMapper;
using DevExpress.Mvvm;
using FluentValidation.Results;
using Jasmine.Core.Contracts;
using PostSharp.Patterns.Diagnostics;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using FluentValidation;
using Jasmine.Core.Tracking;
using PostSharp.Patterns.Model;
using DelegateCommand = Prism.Commands.DelegateCommand;
using IDialogService = Prism.Services.Dialogs.IDialogService;
using Prism.Services.Dialogs;
using Microsoft.AspNetCore.JsonPatch;
using System.Windows.Input;
using PostSharp.Patterns.Xaml;
using AutoBogus;
using Jasmine.Core.Helpers;

namespace Jasmine.Core.Mvvm
{
    //public abstract class LineItemViewModelBase<T> : DialogAwareViewModelBase
    //    where T : class, IEntity, INotifyDataErrorInfo, ITrackable, INotifyPropertyChanged, ISupportValidation, IDirty, new()
    //{
    //    private readonly IEventAggregator _eventAggregator;
    //    private IMapper _mapper;
    //    private readonly LogSource _logger;

    //    public bool EntityAdded { get; set; }
    //    protected LineItemViewModelBase(IEventAggregator eventAggregator, IDialogService dialogService,
    //        IAuthorizationCache authCache) : base(eventAggregator, dialogService, authCache)
    //    {
    //        _eventAggregator = eventAggregator;
    //        SaveAndNewCommand = new DelegateCommand(SaveAndNew, CanSave);
    //        SaveAndCloseCommand = new DelegateCommand(SaveAndClose, CanSave);
    //        ResetCommand = new DelegateCommand(Reset, CanReset);
    //        _logger = LogSource.Get();

    //    }

    //    private bool CanReset() => Entity != null && Entity.IsDirty;

    //    private void Reset()
    //    {
    //        MessageBoxResult result = MessageBoxService.Show("Reset Changes?", "Reset", MessageBoxButton.YesNo,
    //            MessageBoxImage.Question, MessageBoxResult.No);

    //        if (result == MessageBoxResult.Yes)
    //        {
    //            // ReSharper disable once ConvertIfStatementToConditionalTernaryExpression
    //            if (Entity.Id == 0)
    //                Entity = new T();
    //            else
    //                Entity = _mapper.Map<T>(_line);
    //            Entity.StartDirtyTracking();
    //            RaisePropertyChanged();
    //        }
    //    }




    //    public DelegateCommand ResetCommand { get; set; }

    //    private bool CanSave()
    //    {
    //        bool result = Entity != null && Entity.IsDirty && !Entity.HasErrors;
    //        return result;
    //    }

    //    private void SaveAndClose()
    //    {
    //        if (SaveCore())
    //        {
    //            ExecuteClose();
    //        }
    //    }

    //    private bool SaveCore()
    //    {
    //        var args = new CancelEventArgs();
    //        OnBeforeSave(args);

    //        if (Entity.HasErrors) return false;

    //        if (Entity.Id > 0)
    //        {
    //            T entityWithChanges = _changeTracker.GetChanges().FirstOrDefault();
    //            _line = _mapper.Map(entityWithChanges, _line);
    //        }
    //        else if (Entity.Id == 0 && Entity.TrackingState == TrackingState.Added)
    //        {
    //            _line = _mapper.Map(Entity, _line);
    //        }
    //        else
    //        {
    //            _eventAggregator.GetEvent<LineItemAddedEvent<T>>().Publish(Entity);
    //        }

    //        return true;
    //    }

    //    public virtual void OnBeforeSave(CancelEventArgs args)
    //    {

    //    }

    //    private void SaveAndNew()
    //    {
    //        if (SaveCore())
    //        {
    //            Entity = new T();
    //            Entity.StartDirtyTracking();
    //        }
    //    }


    //    public DelegateCommand SaveAndNewCommand { get; }
    //    public DelegateCommand SaveAndCloseCommand { get; }

    //    public T Entity
    //    {
    //        get => _entity;
    //        set
    //        {
    //            _entity = value;
    //            _entity.PropertyChanged += (s, e) =>
    //            {
    //                RaisePropertyChanged();
    //            };
    //            _entity.ErrorsChanged += EntityErrorsChanged;
    //            EntityAdded = !EntityAdded;
    //            ShowValidationSummary = _entity.HasErrors;
    //            _entity.MakeDirty(false);
    //            RefreshErrors();
    //        }
    //    }

    //    protected void RaisePropertyChanged()
    //    {
    //        SaveAndCloseCommand.RaiseCanExecuteChanged();
    //        SaveAndNewCommand.RaiseCanExecuteChanged();
    //        ResetCommand.RaiseCanExecuteChanged();
    //    }

    //    private void EntityErrorsChanged(object sender, DataErrorsChangedEventArgs e) => RefreshErrors();


    //    private void RefreshErrors()
    //    {
    //        ShowValidationSummary = Entity.HasErrors;
    //        ValidationSummary = new ObservableCollection<ValidationFailure>(Entity.ValidationSummary);
    //    }



    //    private T _line;
    //    private T _entity;

    //    private ChangeTrackingCollection<T> _changeTracker;
    //    private List<T> _lines;

    //    protected bool IsDuplicated(Func<T, bool> predicate)
    //    {
    //        bool duplicated;
    //        if (Entity.Id == 0)
    //        {
    //            var line = _lines.SingleOrDefault(predicate);
    //            if (line != null && !ReferenceEquals(line, _line))
    //            {
    //                duplicated = true;
    //            }
    //            else
    //            {
    //                duplicated = false;
    //            }
    //        }
    //        else
    //        {
    //            duplicated = _lines.Any(i => predicate.Invoke(i) && i.Id != Entity.Id);
    //        }

    //        return duplicated;

    //    }
    //    public override void OnDialogOpened(IDialogParameters parameters)
    //    {
    //        base.OnDialogOpened(parameters);
    //        _line = parameters.GetValue<T>("line");

    //        if (_line == null)
    //        {
    //            throw new NullReferenceException("Entity cannot be null");
    //        }

    //        var config = new MapperConfiguration(cfg => cfg.CreateMap<T, T>());
    //        _mapper = config.CreateMapper();

    //        Entity = _mapper.Map<T>(_line);
    //        Entity.StartDirtyTracking();

    //        if (Entity.Id > 0)
    //        {
    //            _changeTracker = new ChangeTrackingCollection<T>(Entity);
    //        }
    //        EntityAdded = !EntityAdded;
    //        //  Entity.MakeDirty(false);


    //        if (parameters.TryGetValue("lines", out List<T> lines))
    //        {
    //            _lines = lines;
    //        }
    //    }


    //}

    [NotifyPropertyChanged]
    public class LineItemBase : EntityBase
    {
        public byte[] RowVersion { get; set; }

        public JsonPatchDocument CreatePatchDocument()
        {
            var patchDoc = new JsonPatchDocument();
            var modifiedProperties = this.ModifiedProperties.Distinct();

            foreach (var property in modifiedProperties)
            {
                if (TryGetPropValue(this, property, out var value))
                {
                    patchDoc.Replace(property, value);
                }
            }

            patchDoc.Replace(nameof(ModifiedProperties), modifiedProperties);
            patchDoc.Replace(nameof(TrackingState), this.TrackingState);

            return patchDoc;
        }

        private bool TryGetPropValue(object src, string propName, out object value)
        {
            var propertyInfo = src.GetType().GetProperty(propName);
            if (propertyInfo.CanWrite)
            {
                value = propertyInfo.GetValue(src, null);
                return true;
            }
            else
            {
                value = null;
                return false;
            }
        }

    }


    //public abstract class LineItemViewModelBase<TLine, TEntity> : DialogAwareViewModelBase
    //   where TEntity : class, IEntity, INotifyDataErrorInfo, ITrackable, IMergeable, INotifyPropertyChanged, ISupportFluentValidator<TEntity>, IDirty, new()
    //   where TLine : class, IEntity, IDirty, ITrackable, IMergeable, INotifyPropertyChanged
    //{
    //    private readonly IEventAggregator _eventAggregator;
    //    private readonly List<IValidator<TEntity>> _validators;
    //    protected IMapper _mapper;
    //    private readonly LogSource _logger;

    //    public bool EntityAdded { get; set; }
    //    protected LineItemViewModelBase(IEventAggregator eventAggregator, IDialogService dialogService,
    //        IAuthorizationCache authCache, List<IValidator<TEntity>> validators) : base(eventAggregator, dialogService, authCache)
    //    {
    //        _eventAggregator = eventAggregator;
    //        _validators = validators;
    //        SaveAndNewCommand = new DelegateCommand(SaveAndNew, CanSave);
    //        SaveAndCloseCommand = new DelegateCommand(SaveAndClose, CanSave);
    //        ResetCommand = new DelegateCommand(Reset, CanReset);
    //        _logger = LogSource.Get();

    //    }



    //    private bool CanReset() => Entity != null && Entity.IsDirty;

    //    private void Reset()
    //    {
    //        MessageBoxResult result = MessageBoxService.Show("Reset Changes?", "Reset", MessageBoxButton.YesNo,
    //            MessageBoxImage.Question, MessageBoxResult.No);

    //        if (result == MessageBoxResult.Yes)
    //        {
    //            Entity = _mapper.Map<TEntity>(_line);
    //            RaisePropertyChanged();
    //        }
    //    }


    //    public DelegateCommand ResetCommand { get; set; }

    //    private bool CanSave()
    //    {
    //        bool result = Entity != null && Entity.IsDirty && !Entity.HasErrors;
    //        return result;
    //    }

    //    private void SaveAndClose()
    //    {
    //        if (SaveCore())
    //        {
    //            ExecuteClose();
    //        }
    //    }

    //    private bool SaveCore()
    //    {
    //        var args = new CancelEventArgs();
    //        OnBeforeSave(args);

    //        if (Entity.HasErrors) return false;

    //        if (Entity.Id > 0) // Existing persisted entity
    //        {
    //            TEntity entityWithChanges = _changeTracker.GetChanges().FirstOrDefault();
    //            _line = _mapper.Map(entityWithChanges, _line);
    //        }
    //        else if (Entity.Id == 0 && Entity.TrackingState == TrackingState.Added) // Not persisted but being modified
    //        {
    //            _line = _mapper.Map(Entity, _line);
    //        }
    //        else // New entity
    //        {
    //            var newEntity = _mapper.Map<TLine>(Entity);
    //            newEntity.TrackingState = TrackingState.Added;

    //            _eventAggregator.GetEvent<LineItemAddedEvent<TLine>>().Publish(newEntity);
    //        }

    //        _line?.MakeDirty(false);

    //        return true;
    //    }

    //    public virtual void OnBeforeSave(CancelEventArgs args)
    //    {

    //    }

    //    private void SaveAndNew()
    //    {
    //        if (SaveCore())
    //        {
    //            Entity = new TEntity();
    //        }
    //    }


    //    public DelegateCommand SaveAndNewCommand { get; }
    //    public DelegateCommand SaveAndCloseCommand { get; }

    //    public TEntity Entity
    //    {
    //        get => _entity;
    //        set
    //        {
    //            _entity = value;
    //            _entity.PropertyChanged += (s, e) =>
    //            {
    //                RaisePropertyChanged();
    //            };
    //            _entity.ErrorsChanged += EntityErrorsChanged;
    //            EntityAdded = !EntityAdded;

    //            if (_validators != null)
    //            {
    //                _entity.SetValidators(_validators);
    //                _entity.ValidateSelf();
    //            }

    //            ShowValidationSummary = _entity.HasErrors;
    //            RefreshErrors();
    //            _entity.MakeDirty(false);
    //        }
    //    }

    //    protected void RaisePropertyChanged()
    //    {
    //        SaveAndCloseCommand.RaiseCanExecuteChanged();
    //        SaveAndNewCommand.RaiseCanExecuteChanged();
    //        ResetCommand.RaiseCanExecuteChanged();
    //    }

    //    private void EntityErrorsChanged(object sender, DataErrorsChangedEventArgs e) => RefreshErrors();


    //    private void RefreshErrors()
    //    {
    //        ShowValidationSummary = Entity.HasErrors;
    //        ValidationSummary = new ObservableCollection<ValidationFailure>(Entity.ValidationSummary);
    //    }


    //    protected TLine _line;
    //    private TEntity _entity;

    //    private ChangeTrackingCollection<TEntity> _changeTracker;
    //    private List<TLine> _lines;


    //    protected bool IsDuplicated(Func<TLine, bool> predicate)
    //    {
    //        bool duplicated;
    //        if (Entity.Id == 0)
    //        {
    //            var line = _lines.SingleOrDefault(predicate);
    //            if (line != null && !ReferenceEquals(line, _line))
    //            {
    //                duplicated = true;
    //            }
    //            else
    //            {
    //                duplicated = false;
    //            }
    //        }
    //        else
    //        {
    //            duplicated = _lines.Any(i => predicate.Invoke(i) && i.Id != Entity.Id);
    //        }

    //        return duplicated;

    //    }

    //    //Todo - Create custom exception for null value for line
    //    private void ParseParameters(IDialogParameters parameters)
    //    {
    //        if (!parameters.ContainsKey("line")) throw new NullReferenceException("Entity cannot be null");

    //        _line = parameters.GetValue<TLine>("line");

    //        var config = new MapperConfiguration(cfg =>
    //        {
    //            cfg.CreateMap<TLine, TEntity>().ForMember(d => d.IsDirty, opt => opt.MapFrom(s => false));
    //            cfg.CreateMap<TEntity, TLine>();
    //        });
    //        _mapper = config.CreateMapper();

    //        Entity = _mapper.Map<TEntity>(_line);
    //        Entity.StartDirtyTracking();

    //        if (Entity.Id > 0)
    //        {
    //            _changeTracker = new ChangeTrackingCollection<TEntity>(Entity);
    //        }


    //        EntityAdded = !EntityAdded;

    //        if (parameters.TryGetValue("lines", out List<TLine> lines))
    //        {
    //            _lines = lines;
    //        }

    //        ISplashScreenService splashScreen = GetService<ISplashScreenService>();
    //        if (splashScreen != null && splashScreen.IsSplashScreenActive)
    //        {
    //            splashScreen.HideSplashScreen();
    //        }
    //    }



    //    public override void OnDialogOpened(IDialogParameters parameters)
    //    {
    //        base.OnDialogOpened(parameters);
    //        ParseParameters(parameters);
    //        FillLookupItems(parameters);

    //    }

    //    protected virtual void FillLookupItems(IDialogParameters parameters)
    //    {

    //    }


    //    #region Fill Dummy Data
    //    [Command]
    //    public ICommand FillDummyDataCommand { get; set; }

    //    private void ExecuteFillDummyData()
    //    {
    //        var faker = ConfigureDummayData();
    //        faker.Populate(Entity);

    //    }

    //    protected virtual Bogus.Faker<TEntity> ConfigureDummayData()
    //    {
    //        var faker = new AutoFaker<TEntity>()
    //             .Ignore(x => x.Id)
    //             .Ignore(x => x.ModifiedProperties)
    //             .Ignore(x => x.TrackingState).Ignore(x => x.EntityIdentifier);
    //        return faker;
    //    }
    //    #endregion

    //}


    //public abstract class LineItemViewModelBase<TLine, TEntity, TParent> : DialogAwareViewModelBase
    // where TEntity : class, IEntity, INotifyDataErrorInfo, ITrackable, IMergeable, INotifyPropertyChanged, ISupportFluentValidator<TEntity>, IDirty, new()
    // where TLine : class, IEntity, IDirty, ITrackable, IMergeable, INotifyPropertyChanged
    //{
    //    private readonly List<IValidator<TEntity>> _validators;
    //    private IMapper _mapper;
    //    private ChangeTrackingCollection<TEntity> _changeTracker;
    //    protected LineItemActions Action { get; private set; }

    //    public bool EntityAdded { get; set; }
    //    protected LineItemViewModelBase(IEventAggregator eventAggregator, IDialogService dialogService,
    //        IAuthorizationCache authCache, List<IValidator<TEntity>> validators) : base(eventAggregator, dialogService, authCache)
    //    {
    //        _validators = validators;

    //        var config = new MapperConfiguration(cfg =>
    //          {
    //              cfg.CreateMap<TLine, TEntity>().ForMember(d => d.IsDirty, opt => opt.MapFrom(s => false));
    //              cfg.CreateMap<TEntity, TLine>();
    //          });
    //        _mapper = config.CreateMapper();

    //        UpdateCommand = new DelegateCommand(ExecuteUpdate, CanExecuteUpdate);
    //    }


    //    #region UpdateCommand
    //    public DelegateCommand UpdateCommand { get; set; }

    //    private void ExecuteUpdate()
    //    {
    //        var args = new CancelEventArgs();
    //        Line = _mapper.Map<TLine>(Entity);
    //        switch (Action)
    //        {
    //            case LineItemActions.Add:
    //                OnBeforeSave(args);
    //                if (!args.Cancel)
    //                {
    //                    OnAdd();
    //                }
    //                break;
    //            case LineItemActions.Edit:
    //                var index = GetLineIndex(_line);
    //                if (index >= 0)
    //                {
    //                    OnUpdate(index);
    //                }
    //                ExecuteClose();
    //                break;
    //            case LineItemActions.Insert:
    //                OnInsert();
    //                ExecuteClose();
    //                break;
    //            default:
    //                break;
    //        }
    //    }



    //    //protected bool IsDuplicated(Func<TLine, bool> predicate)
    //    //{
    //    //    bool duplicated;
    //    //    if (Entity.Id == 0)
    //    //    {
    //    //        var line = _lines.SingleOrDefault(predicate);
    //    //        if (line != null && !ReferenceEquals(line, _line))
    //    //        {
    //    //            duplicated = true;
    //    //        }
    //    //        else
    //    //        {
    //    //            duplicated = false;
    //    //        }
    //    //    }
    //    //    else
    //    //    {
    //    //        duplicated = _lines.Any(i => predicate.Invoke(i) && i.Id != Entity.Id);
    //    //    }

    //    //    return duplicated;

    //    //}

    //    public virtual void OnBeforeSave(CancelEventArgs args)
    //    {

    //    }
    //    protected virtual void OnInsert()
    //    {

    //    }

    //    protected abstract void OnUpdate(int index);

    //    protected abstract int GetLineIndex(TLine line);
    //    protected abstract void OnAdd();

    //    protected virtual bool CanExecuteUpdate() => Entity != null && !Entity.HasErrors;
    //    #endregion


    //    protected TParent Parent { get; set; }
    //    private TEntity _entity;
    //    public TEntity Entity
    //    {
    //        get => _entity;
    //        set
    //        {
    //            _entity = value;
    //            _entity.PropertyChanged += (s, e) =>
    //            {
    //                UpdateCommand.RaiseCanExecuteChanged();
    //            };

    //            _entity.ErrorsChanged += (s, e) => RefreshErrors();

    //            if (_validators != null)
    //            {
    //                _entity.SetValidators(_validators);
    //                _entity.ValidateSelf();
    //            }

    //            ShowValidationSummary = _entity.HasErrors;
    //            RefreshErrors();
    //            _entity.MakeDirty(false);
    //        }
    //    }

    //    private void RefreshErrors()
    //    {
    //        ShowValidationSummary = Entity.HasErrors;
    //        ValidationSummary = new ObservableCollection<ValidationFailure>(Entity.ValidationSummary);
    //    }

    //    protected TLine Line { get; set; }


    //    public override void OnDialogOpened(IDialogParameters parameters)
    //    {
    //        base.OnDialogOpened(parameters);
    //        ParseParameters(parameters);
    //        FillLookupItems(parameters);
    //    }

    //    protected virtual void FillLookupItems(IDialogParameters parameters)
    //    {

    //    }
    //    private TLine _line;
    //    private void ParseParameters(IDialogParameters parameters)
    //    {
    //        Action = parameters.GetValue<LineItemActions>("action");

    //        if (!parameters.ContainsKey("line")) throw new NullReferenceException("Line cannot be null");

    //        Parent = parameters.GetValue<TParent>("parent");

    //        _line = parameters.GetValue<TLine>("line");

    //        Entity = _mapper.Map<TEntity>(_line);
    //        Entity.StartDirtyTracking();

    //        if (Entity.Id > 0)
    //        {
    //            _changeTracker = new ChangeTrackingCollection<TEntity>(Entity);
    //        }

    //        ISplashScreenService splashScreen = GetService<ISplashScreenService>();
    //        if (splashScreen != null && splashScreen.IsSplashScreenActive)
    //        {
    //            splashScreen.HideSplashScreen();
    //        }
    //    }


    //    #region Fill Dummy Data
    //    [Command]
    //    public ICommand FillDummyDataCommand { get; set; }

    //    private void ExecuteFillDummyData()
    //    {
    //        var faker = ConfigureDummayData();
    //        faker.Populate(Entity);

    //    }

    //    protected virtual Bogus.Faker<TEntity> ConfigureDummayData()
    //    {
    //        var faker = new AutoFaker<TEntity>()
    //             .Ignore(x => x.Id)
    //             .Ignore(x => x.ModifiedProperties)
    //             .Ignore(x => x.TrackingState).Ignore(x => x.EntityIdentifier);
    //        return faker;
    //    }
    //    #endregion
    //}


    public abstract class LineItemViewModelBase<TLine, TEntity> : DialogAwareViewModelBase
    where TEntity : class, IEntity, INotifyDataErrorInfo, ITrackable, IMergeable, INotifyPropertyChanged, ISupportFluentValidator<TEntity>, IDirty, new()
    where TLine : class, IEntity, IDirty, ITrackable, IMergeable, INotifyPropertyChanged, new()
    {
        private readonly List<IValidator<TEntity>> _validators;
        private ChangeTrackingCollection<TEntity> _changeTracker;
        private TLine _editingLine;
        private IMapper _mapper;
        protected LineItemActions Action { get; private set; }

        public bool EntityAdded { get; set; }
        protected LineItemViewModelBase(IEventAggregator eventAggregator, IDialogService dialogService,
            IAuthorizationCache authCache, List<IValidator<TEntity>> validators) : base(eventAggregator, dialogService, authCache)
        {
            _validators = validators;

            UpdateCommand = new DelegateCommand(ExecuteUpdate, CanExecuteUpdate);
        }


        #region UpdateCommand
        public DelegateCommand UpdateCommand { get; set; }

        private void ExecuteUpdate()
        {


            var line = _mapper.Map<TLine>(Entity);

            var args = new LineItemCancelEventArgs<TLine>(line);

            switch (Action)
            {
                case LineItemActions.Add:
                    OnBeforeSave(args);
                    if (!args.Cancel)
                    {
                        Lines.Add(line);

                        if (args.AddAndClose)
                        {
                            ExecuteClose();
                        }
                        else
                        {
                            Entity = new TEntity();
                        }

                    }
                    break;
                case LineItemActions.Edit:
                    OnBeforeSave(args);
                    if (!args.Cancel)
                    {
                        OnEditing(_changeTracker.GetChanges().SingleOrDefault(),line);
                        var index = Lines.IndexOf(_editingLine);
                        if (index >= 0)
                        {
                            Lines[index] = line;
                        }
                        ExecuteClose();
                    }
                    break;
                case LineItemActions.Insert:
                    OnInsert(line);
                    ExecuteClose();
                    break;
                default:
                    break;
            }
        }

        protected virtual void OnEditing(TEntity entity,TLine line)
        {
            
        }




        //  private TLine Line { get; set; }

        protected ChangeTrackingCollection<TLine> Lines { get; set; }

        protected bool IsDuplicated(Func<TLine, bool> predicate)
        {
            bool duplicated;
            if (Entity.Id == 0)
            {
                var line = Lines.SingleOrDefault(predicate);
                if (line != null && !ReferenceEquals(line, _editingLine))
                {
                    duplicated = true;
                }
                else
                {
                    duplicated = false;
                }
            }
            else
            {
                duplicated = Lines.Any(i => predicate.Invoke(i) && i.Id != Entity.Id);
            }

            return duplicated;

        }

        public virtual void OnBeforeSave(LineItemCancelEventArgs<TLine> args)
        {

        }
        protected virtual void OnInsert(TLine line)
        {

        }



        protected virtual bool CanExecuteUpdate() => Entity != null && !Entity.HasErrors;
        #endregion


        private TEntity _entity;
        public TEntity Entity
        {
            get => _entity;
            set
            {
                if (_entity != null)
                {
                    _entity.PropertyChanged -= _entity_PropertyChanged;
                    _entity.ErrorsChanged -= _entity_ErrorsChanged;
                }


                _entity = value;


                _entity.PropertyChanged += _entity_PropertyChanged;
                _entity.ErrorsChanged += _entity_ErrorsChanged;

                if (_validators != null)
                {
                    _entity.SetValidators(_validators);
                    _entity.ValidateSelf();
                }

                ShowValidationSummary = _entity.HasErrors;
                RefreshErrors();
                _entity.MakeDirty(false);
            }
        }

        private void _entity_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateCommand.RaiseCanExecuteChanged();
        }

        private void _entity_ErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            RefreshErrors();
        }

        private void RefreshErrors()
        {
            ShowValidationSummary = Entity.HasErrors;
            ValidationSummary = new ObservableCollection<ValidationFailure>(Entity.ValidationSummary);
        }




        public override void OnDialogOpened(IDialogParameters parameters)
        {
            base.OnDialogOpened(parameters);

            var config = new MapperConfiguration(CreateMapperConfiguraton);
            _mapper = config.CreateMapper();

            ParseParameters(parameters);
            FillLookupItems(parameters);
        }

        protected virtual void CreateMapperConfiguraton(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<TLine, TEntity>().ForMember(d => d.IsDirty, opt => opt.MapFrom(s => false));
            cfg.CreateMap<TEntity, TLine>();
        }

        protected virtual void FillLookupItems(IDialogParameters parameters)
        {

        }

        private void ParseParameters(IDialogParameters parameters)
        {

            if (!parameters.ContainsKey("action")) throw new NullReferenceException("Action cannot be null");
            if (!parameters.ContainsKey("lines")) throw new NullReferenceException("Lines cannot be null");


            Action = parameters.GetValue<LineItemActions>("action");

            Lines = parameters.GetValue<ChangeTrackingCollection<TLine>>("lines");


            if (Action == LineItemActions.Add && !parameters.ContainsKey("line"))
            {
                Entity = new TEntity();
            }
            else
            {
                if (!parameters.ContainsKey("line"))
                {
                    throw new NullReferenceException("Line cannot be null");
                }
                _editingLine = parameters.GetValue<TLine>("line");
                Entity = _mapper.Map<TEntity>(_editingLine);
            }


            Entity.StartDirtyTracking();

            if (Entity.Id > 0)
            {
                _changeTracker = new ChangeTrackingCollection<TEntity>(Entity);
            }

            ISplashScreenService splashScreen = GetService<ISplashScreenService>();
            if (splashScreen != null && splashScreen.IsSplashScreenActive)
            {
                splashScreen.HideSplashScreen();
            }
        }


        #region Fill Dummy Data
        [Command]
        public ICommand FillDummyDataCommand { get; set; }

        private void ExecuteFillDummyData()
        {
            var faker = ConfigureDummayData();
            faker.Populate(Entity);

        }

        protected virtual Bogus.Faker<TEntity> ConfigureDummayData()
        {
            var faker = new AutoFaker<TEntity>()
                 .Ignore(x => x.Id)
                 .Ignore(x => x.ModifiedProperties)
                 .Ignore(x => x.TrackingState).Ignore(x => x.EntityIdentifier);
            return faker;
        }
        #endregion
    }

    public class LineItemAddedEvent<T> : PubSubEvent<T> where T : class, IEntity
    {

    }


    public class LineItemCancelEventArgs<T> : CancelEventArgs
    {
        public bool AddAndClose { get; set; }
        public T Line { get; }
        public LineItemCancelEventArgs(T line)
        {
            Line = line;
        }
    }

    //public class LineItemOnBeforeSaveEventArgs<T> : CancelEventArgs where T:class,IEntity
    //{
    //    public T Line{get;}
    //    public LineItemActions Action{get;}
    //    public LineItemOnBeforeSaveEventArgs(T line,LineItemActions action)
    //    {
    //        Line=line;
    //        Action=action;
    //    }
    //}
}