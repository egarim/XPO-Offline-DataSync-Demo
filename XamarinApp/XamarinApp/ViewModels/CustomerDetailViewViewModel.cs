using System;
using Demo.ORM;
using DevExpress.Xpo;
using Prism;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using XamarinApp.Infrastructure;

namespace XamarinApp.ViewModels
{
    public class CustomerDetailViewViewModel : AppMapViewModelBase
    {


        Customer customer;
        UnitOfWork uoW;
        private IEventAggregator _ea;
        public Customer Customer
        {
            get { return customer; }
            set
            {
                if (customer == value)
                    return;
                customer = value;
                RaisePropertyChanged();
            }
        }
        
        public CustomerDetailViewViewModel(INavigationService navigationService, IEventAggregator ea) : base (navigationService)
        {
            _ea = ea;
            uoW = new UnitOfWork();
            SaveCommand = new DelegateCommand(Save, CanSubmit).ObservesCanExecute(() => IsEnabled);
            this.IsEnabled = true;
        }

        private void Save()
        {
            if(this.uoW.InTransaction)
                this.uoW.CommitChanges();

            this.NavigationService.GoBackAsync();

        }
        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
            _ea.GetEvent<CustomerSaved>().Publish(null);
        }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
          
            Customer = uoW.GetObjectByKey<Customer>(Guid.Parse(parameters["Oid"].ToString()));

        }
        private bool _isEnabled;
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set { SetProperty(ref _isEnabled, value); }
        }
        bool CanSubmit()
        {
            return IsEnabled;
        }
        public DelegateCommand SaveCommand { get; private set; }
    }
}
