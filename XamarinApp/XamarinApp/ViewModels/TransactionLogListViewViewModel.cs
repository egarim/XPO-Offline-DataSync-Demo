using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;
using BIT.Xpo.Observable;
using BIT.Xpo.OfflineDataSync;
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
    public class TransactionLogListViewViewModel : AppMapViewModelBase, IActiveAware
    {

#pragma warning disable 67
        public event EventHandler IsActiveChanged;
#pragma warning restore 67

        public bool IsActive { get; set; }

        public TransactionLogListViewViewModel(INavigationService navigationService) : base (navigationService)
        {
            Trackers = new XpoObservableCollection<TransactionLog>(uoW);
            UpdateCount();
            SetupAddCustomerCommand();
            SetupSyncCommand();
            this.Identity = App.Identity;
        }
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

        UnitOfWork uoW = new UnitOfWork(App.DataStore.TransactionLogDataLayer);
        private XpoObservableCollection<TransactionLog> trackers;
        

        private void UpdateCount()
        {
            this.TotalRecords = Trackers.Count;
        }

       

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
                RaisePropertyChanged(nameof(__AllowAddCustomer));
            }
        }

        public XpoObservableCollection<TransactionLog> Trackers
        {
            get => trackers; set
            {
                if (trackers == value)
                {
                    return;
                }

                trackers = value;
                RaisePropertyChanged(nameof(Trackers));
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
                RaisePropertyChanged(nameof(Identity));
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

                XPCollection<Customer> Customers = new XPCollection<Customer>(XpoDefault.Session);
                XpoDefault.Session.Delete(Customers);
                XpoDefault.Session.PurgeDeletedObjects();


                UnitOfWork UoW = new UnitOfWork(App.DataStore.TransactionLogDataLayer);
                XPCollection<TransactionLog> Trackers = new XPCollection<TransactionLog>(UoW);
                TransactionState Pointer = TransactionState.GetInstance(UoW);
                Pointer.Delete();
                UoW.PurgeDeletedObjects();




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
                RaisePropertyChanged(nameof(__AllowSync));
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
                    //App.Ds.PullModificationsFromApi("http://192.168.0.53:62614/");

                    //App.Ds.PushModificationsToApi("http://192.168.0.53:62614/");
                    this.Trackers = new XpoObservableCollection<TransactionLog>(this.uoW);
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
    }
}
