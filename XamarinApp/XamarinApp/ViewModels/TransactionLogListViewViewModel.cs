using System;
using Prism;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using XamarinApp.Infrastructure;

namespace XamarinApp.ViewModels
{
    public class TransactionLogListViewViewModel : AppMapViewModelBase, IActiveAware
    {

#pragma warning disable 67
        public event EventHandler IsActiveChanged;
#pragma warning restore 67

        public bool IsActive { get; set; }

        public TransactionLogListViewViewModel(INavigationService navigationService) : base (navigationService)
        {
        }
    }
}
