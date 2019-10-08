using System;
using Prism;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using XamarinApp.Infrastructure;

namespace XamarinApp.ViewModels
{
    public class CustomerDetailViewViewModel : AppMapViewModelBase
    {


        public CustomerDetailViewViewModel(INavigationService navigationService) : base (navigationService)
        {
        }
    }
}
