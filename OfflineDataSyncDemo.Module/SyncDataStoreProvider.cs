using BIT.Xpo.OfflineDataSync;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Xpo.Metadata;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OfflineDataSyncDemo.Module
{
    public class SyncDataStoreProvider : IXpoDataStoreProvider
    {
        private SyncDataStore _DataStore;
        Assembly[] _Assemblies;
        public SyncDataStoreProvider(params Assembly[] Assemblies)
        {
            SyncDataStore.EnableTransactionHistory=true;
            _Assemblies = Assemblies;

            //TryToPushOnEveryCommit
            DataStore = new SyncDataStore(ConfigurationManager.AppSettings["Identity"],bool.Parse(ConfigurationManager.AppSettings["TryToPushOnEveryCommit"]), ConfigurationManager.AppSettings["SyncServerUrl"]);
        }
        public DevExpress.Xpo.DB.IDataStore CreateUpdatingStore(bool allowUpdateSchema, out IDisposable[] disposableObjects)
        {
            disposableObjects = null;
            return DataStore;
        }
        public DevExpress.Xpo.DB.IDataStore CreateWorkingStore(out IDisposable[] disposableObjects)
        {
            disposableObjects = null;
            return DataStore;
        }
        public DevExpress.Xpo.DB.IDataStore CreateSchemaCheckingStore(out IDisposable[] disposableObjects)
        {
            disposableObjects = null;
            return DataStore;
        }
        public XPDictionary XPDictionary
        {
            get { return null; }
        }
        public string ConnectionString
        {
            get { return null; }
        }
        public bool IsInitialized
        {
            get;
            private set;
        }
        public SyncDataStore DataStore { get => _DataStore; set => _DataStore = value; }

        public void Initialize(XPDictionary dictionary, string LocalDatabase, string LogDatabase)
        {

            //proxy.Initialize(new Assembly[] { this.GetType().Assembly }, legacyConnectionString, tempConnectionString);
            DataStore.Initialize(LocalDatabase, LogDatabase, false, DevExpress.Xpo.DB.AutoCreateOption.DatabaseAndSchema, _Assemblies);
            IsInitialized = true;
        }
    }
}
