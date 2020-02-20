using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;
using BIT.Xpo.Observables;
using Demo.ORM;
using DevExpress.Xpo;
using Prism;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using Xamarin.Forms;
using XamarinApp.Infrastructure;

namespace XamarinApp.ViewModels
{
    public class HomeViewModel : AppMapViewModelBase, IActiveAware
    {

#pragma warning disable 67
        public event EventHandler IsActiveChanged;
#pragma warning restore 67

        public bool IsActive { get; set; }
        private IEventAggregator _ea;
        public HomeViewModel(INavigationService navigationService, IEventAggregator ea) : base (navigationService)
        {
            _ea = ea;
            Customers = new XpoObservableCollection<Customer>(uoW);
            UpdateCount();
            SetupAddCustomerCommand();
            SetupSyncCommand();
            SetupRefreashCollectionCommand();
            SetupItemTappedCommand();
            this.Identity = App.Identity;
        }
        //xcbu
        int totalRecords;
        public int TotalRecords
        {
            get { return totalRecords; }
            set
            {
                if (totalRecords == value)
                    return;
                totalRecords = value;
            
                RaisePropertyChanged(nameof(TotalRecords));
            }
        }

        UnitOfWork uoW = new UnitOfWork();
        private XpoObservableCollection<Customer> customers;
       

        private void UpdateCount()
        {
            this.TotalRecords = Customers.Count;
        }

        
        //public event PropertyChangedEventHandler PropertyChanged;
        #region 'AddCustomer Command'
        public ICommand AddCustomer { protected set; get; }
        private bool _AllowAddCustomer;
        public bool __AllowAddCustomer
        {
            get { return _AllowAddCustomer; }
            set
            {
                if (_AllowAddCustomer == value)
                    return;
                _AllowAddCustomer = value;
                this.RaisePropertyChanged(nameof(__AllowAddCustomer));
            }
        }

        public XpoObservableCollection<Customer> Customers
        {
            get => customers; set
            {
                if (customers == value)
                {
                    return;
                }

                customers = value;
                RaisePropertyChanged(nameof(Customers));
            
            }
        }
        string identity;
        public string Identity
        {
            get { return identity; }
            set
            {
                if (identity == value)
                    return;
                identity = value;
                this.RaisePropertyChanged(nameof(Identity));
            }
        }

        private void SetupAddCustomerCommand()
        {
            this.__AllowAddCustomer = true;
            Action<object> CommandExecuteAction = (nothing) =>
            {
                this.__AllowAddCustomer = false;
                ((Command)this.AddCustomer).ChangeCanExecute();
                Debug.WriteLine("AddCustomer executed");

                Customer TestCustomer = new Customer(uoW);
                string CodeAndName = DateTime.Now.ToString("yyyyMMddhhmmssfff");
                TestCustomer.Name = this.Identity;
                TestCustomer.Code = CodeAndName;
                customers.Add(TestCustomer);
                uoW.CommitChanges();
                UpdateCount();

                //TODO implement the command code body for the AddCustomer Command
                this.__AllowAddCustomer = true;
                ((Command)this.AddCustomer).ChangeCanExecute();
            };
            Func<object, bool> CommandEvaluation = (nothing) =>
            {
                return this.__AllowAddCustomer;
            };

            this.AddCustomer = new Command(CommandExecuteAction, CommandEvaluation);
        }
        #endregion "AddCustomer 


        #region 'Sync Command'
        public ICommand Sync { protected set; get; }
        private bool _AllowSync;
        public bool __AllowSync
        {
            get { return _AllowSync; }
            set
            {
                if (_AllowSync == value)
                    return;
                _AllowSync = value;
                this.RaisePropertyChanged(nameof(__AllowSync));
            }
        }
        private void SetupSyncCommand()
        {
            this.__AllowSync = true;
            Action<object> CommandExecuteAction = (nothing) =>
            {
                this.__AllowSync = false;
                ((Command)this.Sync).ChangeCanExecute();
                Debug.WriteLine("Sync executed");
                try
                {

                    

                    var counter= App.DataStore.PullModificationsFromApi();

                    App.DataStore.PushModificationsToApi();
                    uoW.ReloadChangedObjects();
                    this.Customers = new XpoObservableCollection<Customer>(this.uoW);
                    UpdateCount();
                }
                catch (Exception exception)
                {

                    Debug.WriteLine(string.Format("{0}:{1}", "exception.Message", exception.Message));
                    if (exception.InnerException != null)
                    {
                        Debug.WriteLine(string.Format("{0}:{1}", "exception.InnerException.Message", exception.InnerException.Message));

                    }
                    Debug.WriteLine(string.Format("{0}:{1}", " exception.StackTrace", exception.StackTrace));
                }
                this.__AllowSync = true;
                ((Command)this.Sync).ChangeCanExecute();
            };
            Func<object, bool> CommandEvaluation = (nothing) =>
            {
                return this.__AllowSync;
            };

            this.Sync = new Command(CommandExecuteAction, CommandEvaluation);
        }
        #endregion "Sync 

        #region 'RefreashCollection Command'
        public ICommand RefreshCollection { protected set; get; }
        private bool _AllowRefreashCollection;
        public bool __AllowRefreshCollection
        {
            get { return _AllowRefreashCollection; }
            set
            {
                if (_AllowRefreashCollection == value)
                    return;
                _AllowRefreashCollection = value;
                RaisePropertyChanged(nameof(__AllowRefreshCollection));
            }
        }
        private void SetupRefreashCollectionCommand()
        {
            this.__AllowRefreshCollection = true;
            Action<object> CommandExecuteAction = (nothing) =>
            {
                this.__AllowRefreshCollection = false;
                ((Command)this.RefreshCollection).ChangeCanExecute();
                Debug.WriteLine("RefreshCollection executed");
                uoW.ReloadChangedObjects();
                this.Customers = new XpoObservableCollection<Customer>(this.uoW);
                UpdateCount();

                this.__AllowRefreshCollection = true;
                ((Command)this.RefreshCollection).ChangeCanExecute();
            };
            Func<object, bool> CommandEvaluation = (nothing) =>
            {
                return this.__AllowRefreshCollection;
            };

            this.RefreshCollection = new Command(CommandExecuteAction, CommandEvaluation);
        }
        #endregion "RefreashCollection 
        #region 'ItemTapped Command'

        void CustomerSavedEvent(string Parameter)
        {
            this.RefreshCollection.Execute(null);
            customerSaved.Unsubscribe(CustomerSavedEvent);
        }

        public ICommand ItemTapped { protected set; get; }
        private bool _AllowItemTapped;
        CustomerSaved customerSaved;
        public bool __AllowItemTapped
        {
            get { return _AllowItemTapped; }
            set
            {
                if (_AllowItemTapped == value)
                    return;
                _AllowItemTapped = value;
                RaisePropertyChanged(nameof(__AllowItemTapped));
            }
        }
        private void SetupItemTappedCommand()
        {
            this.__AllowItemTapped = true;
            Action<object> CommandExecuteAction = (Paramater) =>
            {

                this.__AllowItemTapped = false;
                ((Command)this.ItemTapped).ChangeCanExecute();
                Debug.WriteLine("ItemTapped executed");

                Customer customer = (Customer)Paramater;
                var navParameters = new NavigationParameters();
                navParameters.Add("Oid", customer.Oid.ToString());

                customerSaved = _ea.GetEvent<CustomerSaved>();
                customerSaved.Subscribe(CustomerSavedEvent, ThreadOption.UIThread);

                this.NavigationService.NavigateAsync("CustomerDetailView", navParameters,true);


                this.__AllowItemTapped = true;
                ((Command)this.RefreshCollection).ChangeCanExecute();
            };
            Func<object, bool> CommandEvaluation = (nothing) =>
            {
                return this.__AllowItemTapped;
            };

            this.ItemTapped = new Command(CommandExecuteAction, CommandEvaluation);
        }
        #endregion "RefreashCollection 



    }
}
