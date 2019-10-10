using System;
using Demo.ORM;
using DevExpress.Xpo;
using Prism;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using XamarinApp.Infrastructure;

namespace XamarinApp.ViewModels
{
    public class CustomerDetailViewViewModel : AppMapViewModelBase
    {


        Customer customer;
        UnitOfWork uoW;

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
        
        public CustomerDetailViewViewModel(INavigationService navigationService) : base (navigationService)
        {
            uoW = new UnitOfWork();
            SaveCommand = new DelegateCommand(Save, CanSubmit).ObservesCanExecute(() => IsEnabled);
            this.IsEnabled = true;
        }

        private void Save()
        {
            if(this.uoW.InTransaction)
                this.uoW.CommitChanges();
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
