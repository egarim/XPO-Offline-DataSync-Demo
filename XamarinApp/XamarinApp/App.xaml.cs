using Prism;
using Prism.Ioc;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinApp.Views;
using XamarinApp.ViewModels;
using BIT.Xpo.OfflineDataSync;
using Xamarin.Essentials;
using System;
using System.Diagnostics;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using System.IO;
using Demo.ORM;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.Reflection;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace XamarinApp
{
    public partial class App
    {
        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App() : this(null) { }

        public static string Identity = string.Format("{0} {1} {2}", DeviceInfo.Model, DeviceInfo.Version, DeviceInfo.Name);
        public static SyncDataStore Ds;
        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();



            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;



            //Ds = new TrackerProxy(Identity, true, "http://192.168.0.53:62614/", ClientType.Slave);
            Ds = new SyncDataStore(Identity, false, "http://aadcb62e.ngrok.io/BIT.XpoSyncFramework.Server/");

            try
            {

                var MainPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                var CacheShared = ";Cache=Shared;";
                var MainDb = Path.Combine(MainPath, "MainDb.db");
                Debug.WriteLine(string.Format("{0}:{1}", "MainDb", MainDb));
                var Tracker = Path.Combine(MainPath, "Tracker.db");
                Debug.WriteLine(string.Format("{0}:{1}", "Tracker", Tracker));

                //Ds.InitTrackerDal((SQLiteConnectionProvider.GetConnectionString(Tracker) + CacheShared), true);

                Ds.Initialize(

                    SQLiteConnectionProvider.GetConnectionString(MainDb) + CacheShared,
                    SQLiteConnectionProvider.GetConnectionString(Tracker) + CacheShared, true, AutoCreateOption.DatabaseAndSchema, new Assembly[] { typeof(Demo.ORM.CustomBaseObject).Assembly });
                XpoDefault.DataLayer = new SimpleDataLayer(Ds);



                UnitOfWork UoW = new UnitOfWork();
                CreateInitialData(UoW);

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



            await NavigationService.NavigateAsync("NavigationPage/MainPage");
        }
        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {

            if (e.NetworkAccess == NetworkAccess.Internet)
            {
                Debug.WriteLine(string.Format("{0}:{1}", "App", "Connected to the internet"));
                App.Ds.PullModificationsFromApi();

                App.Ds.PushModificationsToApi();
            }
        }

        private static void CreateInitialData(UnitOfWork UoW)
        {
            int count = (int)UoW.Evaluate(typeof(Customer), CriteriaOperator.Parse("Count()"), null);
            if (count == 0)
            {
                List<Customer> Customers = new List<Customer>();
                for (int i = 1; i < 100; i++)
                {

                    Customer Customer = new Demo.ORM.Customer(UoW);
                    Customer.Code = $"{DeviceInfo.Model} {i.ToString()}";
                    Customer.Name = "Customer " + i.ToString();
                    Customers.Add(Customer);
                }

                UoW.CommitChanges();
            }
        }
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<Home, HomeViewModel>();
            containerRegistry.RegisterForNavigation<CustomerListView, CustomerListViewViewModel>();
            containerRegistry.RegisterForNavigation<TransactionLogListView, TransactionLogListViewViewModel>();
            containerRegistry.RegisterForNavigation<CustomerDetailView, CustomerDetailViewViewModel>();
        }
    }
}
