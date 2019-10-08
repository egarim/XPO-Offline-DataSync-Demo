using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;
using BIT.Xpo.Observable;
using Demo.ORM;
using DevExpress.Xpo;
using Prism;
using Prism.Commands;
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

        public HomeViewModel(INavigationService navigationService) : base (navigationService)
        {
            Customers = new XpoObservableCollection<Customer>(uoW);
            UpdateCount();
            SetupAddCustomerCommand();
            SetupSyncCommand();
            SetupRefreashCollectionCommand();
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
                OnPropertyChanged(this, new PropertyChangedEventArgs(nameof(TotalRecords)));
            }
        }

        UnitOfWork uoW = new UnitOfWork();
        private XpoObservableCollection<Customer> customers;
       

        private void UpdateCount()
        {
            this.TotalRecords = Customers.Count;
        }

        protected virtual void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(sender, e);
        }
        public event PropertyChangedEventHandler PropertyChanged;
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
                OnPropertyChanged(this, new PropertyChangedEventArgs("AllowAddCustomer"));
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
                OnPropertyChanged(this, new PropertyChangedEventArgs(nameof(Customers)));
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
                OnPropertyChanged(this, new PropertyChangedEventArgs(nameof(Identity)));
            }
        }

        private void SetupAddCustomerCommand()
        {
            this.__AllowAddCustomer = true;
            Action<object> CommandExecuteAction = (nothing) =>
            {
                this.__AllowAddCustomer = false;
                ((Command)this.AddCustomer).ChangeCanExecute();
                Debug.WriteLine("AddCustomer excuted");

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
                OnPropertyChanged(this, new PropertyChangedEventArgs("AllowSync"));
            }
        }
        private void SetupSyncCommand()
        {
            this.__AllowSync = true;
            Action<object> CommandExecuteAction = (nothing) =>
            {
                this.__AllowSync = false;
                ((Command)this.Sync).ChangeCanExecute();
                Debug.WriteLine("Sync excuted");
                try
                {

                    //App.Ds.PullModificationsFromApi("http://192.168.0.53:62614/");

                    //App.Ds.PushModificationsToApi("http://192.168.0.53:62614/");

                    App.Ds.PullModificationsFromApi();

                    App.Ds.PushModificationsToApi();
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
        public ICommand RefreashCollection { protected set; get; }
        private bool _AllowRefreashCollection;
        public bool __AllowRefreashCollection
        {
            get { return _AllowRefreashCollection; }
            set
            {
                if (_AllowRefreashCollection == value)
                    return;
                _AllowRefreashCollection = value;
                OnPropertyChanged(this, new PropertyChangedEventArgs("AllowRefreashCollection"));
            }
        }
        private void SetupRefreashCollectionCommand()
        {
            this.__AllowRefreashCollection = true;
            Action<object> CommandExecuteAction = (nothing) =>
            {
                this.__AllowRefreashCollection = false;
                ((Command)this.RefreashCollection).ChangeCanExecute();
                Debug.WriteLine("RefreashCollection excuted");
                uoW.ReloadChangedObjects();
                this.Customers = new XpoObservableCollection<Customer>(this.uoW);
                UpdateCount();
                this.__AllowRefreashCollection = true;
                ((Command)this.RefreashCollection).ChangeCanExecute();
            };
            Func<object, bool> CommandEvaluation = (nothing) =>
            {
                return this.__AllowRefreashCollection;
            };

            this.RefreashCollection = new Command(CommandExecuteAction, CommandEvaluation);
        }
        #endregion "RefreashCollection 
    }
}
