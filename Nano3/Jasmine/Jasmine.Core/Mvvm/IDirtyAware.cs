using DevExpress.Mvvm;
using DevExpress.Xpf.Core.Native;
using DevExpress.Xpf.Docking;
using DevExpress.XtraReports;
using DevExpress.XtraReports.UI;
using FluentValidation;
using FluentValidation.Results;
using Humanizer;
using Jasmine.Core.Aspects;
using Jasmine.Core.Audit;
using Jasmine.Core.Contracts;
using Jasmine.Core.Events;
using Jasmine.Core.Mvvm.LookupItems;
using Jasmine.Core.Properties;
using Jasmine.Core.Tracking;
using PostSharp.Patterns.Diagnostics;
using Prism.Events;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Jasmine.Core.Repositories;
using IDialogService = Prism.Services.Dialogs.IDialogService;

namespace Jasmine.Core.Mvvm
{

    public interface IDirtyAware
    {
        bool IsEntityIDirty();
    }

    public interface ISupportAsyncOperation
    {
        bool IsExecuting{get;}
    }

}