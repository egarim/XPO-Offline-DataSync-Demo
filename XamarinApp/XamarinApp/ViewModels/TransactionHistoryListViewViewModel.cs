using BIT.Xpo.Observable;
using BIT.Xpo.OfflineDataSync;
using DevExpress.Xpo;
using Prism.Navigation;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinApp.Infrastructure;

namespace XamarinApp.ViewModels
{
    public class TransactionHistoryListViewViewModel : AppMapViewModelBase
    {

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

                    this.TransactionHistory = new XpoObservableCollection<SynLog>(uoW);

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
        public TransactionHistoryListViewViewModel(INavigationService navigationService) : base(navigationService)
        {
            this.TransactionHistory = new XpoObservableCollection<SynLog>(uoW);
            SetupSyncCommand();

        }
        UnitOfWork uoW = new UnitOfWork(App.DataStore.TransactionLogDataLayer);
        XpoObservableCollection<SynLog> transactionHistory;
        public XpoObservableCollection<SynLog> TransactionHistory
        {
            get => transactionHistory; set
            {
                if (transactionHistory == value)
                {
                    return;
                }

                transactionHistory = value;
                RaisePropertyChanged(nameof(TransactionHistory));
            }
        }
    }
}
