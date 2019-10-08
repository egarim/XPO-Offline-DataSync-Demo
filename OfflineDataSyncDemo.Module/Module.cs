using System;
using System.Text;
using System.Linq;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using System.Collections.Generic;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Model.Core;
using DevExpress.ExpressApp.Model.DomainLogics;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.Xpo;
using System.Configuration;
using System.Reflection;
using Demo.ORM;

namespace OfflineDataSyncDemo.Module {
    // For more typical usage scenarios, be sure to check out https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.ModuleBase.
    public sealed partial class OfflineDataSyncDemoModule : ModuleBase {
        public OfflineDataSyncDemoModule() {
            InitializeComponent();
			BaseObject.OidInitializationMode = OidInitializationMode.AfterConstruction;
            this.AdditionalExportedTypes.Add(typeof(Demo.ORM.Customer));
            this.AdditionalExportedTypes.Add(typeof(Demo.ORM.CustomBaseObject));
            this.AdditionalExportedTypes.Add(typeof(Demo.ORM.Invoice));
        }
        public override IEnumerable<ModuleUpdater> GetModuleUpdaters(IObjectSpace objectSpace, Version versionFromDB) {
            ModuleUpdater updater = new DatabaseUpdate.Updater(objectSpace, versionFromDB);
            return new ModuleUpdater[] { updater };
        }
        public override void Setup(XafApplication application) {
            base.Setup(application);
            // Manage various aspects of the application UI and behavior at the module level.
            application.CustomCheckCompatibility += new EventHandler<CustomCheckCompatibilityEventArgs>(application_CustomCheckCompatibility);
            application.CreateCustomObjectSpaceProvider += new EventHandler<CreateCustomObjectSpaceProviderEventArgs>(application_CreateCustomObjectSpaceProvider);
        }
        public override void CustomizeTypesInfo(ITypesInfo typesInfo) {
            base.CustomizeTypesInfo(typesInfo);
            CalculatedPersistentAliasHelper.CustomizeTypesInfo(typesInfo);
            var CustomerTypeInfo = typesInfo.FindTypeInfo(typeof(Customer));
            CustomerTypeInfo.AddAttribute(new DefaultClassOptionsAttribute());
            var InvoiceTypeInfo = typesInfo.FindTypeInfo(typeof(Invoice));
            InvoiceTypeInfo.AddAttribute(new DefaultClassOptionsAttribute());
        }
        private static SyncDataStoreProvider provider;
        void application_CreateCustomObjectSpaceProvider(object sender, CreateCustomObjectSpaceProviderEventArgs e)
        {
            if (provider == null)
            {
                provider = new SyncDataStoreProvider(new Assembly[] { typeof(Customer).Assembly });
            }
            e.ObjectSpaceProvider = new XPObjectSpaceProvider(provider);
        }
        void application_CustomCheckCompatibility(object sender, CustomCheckCompatibilityEventArgs e)
        {
            if (provider != null && !provider.IsInitialized)
            {
                provider.Initialize(((XPObjectSpaceProvider)e.ObjectSpaceProvider).XPDictionary,
                    ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString,
                    ConfigurationManager.ConnectionStrings["LogConnectionString"].ConnectionString);
            }
        }
    }
}
