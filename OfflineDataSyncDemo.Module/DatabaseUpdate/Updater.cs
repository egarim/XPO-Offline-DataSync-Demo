using System;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Updating;
using DevExpress.Xpo;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl;
using Demo.ORM;

namespace OfflineDataSyncDemo.Module.DatabaseUpdate {
    // For more typical usage scenarios, be sure to check out https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.Updating.ModuleUpdater
    public class Updater : ModuleUpdater {
        public Updater(IObjectSpace objectSpace, Version currentDBVersion) :
            base(objectSpace, currentDBVersion) {
        }
        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();
            CreateCustomer("Jose Manuel Ojeda", "001");
            CreateCustomer("Jose Javier Columbie", "002");
            CreateCustomer("Douglas Coto", "003");
            CreateCustomer("Oscar Ojeda", "004");

            ObjectSpace.CommitChanges(); //Uncomment this line to persist created object(s).
        }

        private void CreateCustomer(string name, string Code)
        {
            Customer theObject = ObjectSpace.FindObject<Customer>(CriteriaOperator.Parse("Code=?", Code));
            if (theObject == null)
            {
                theObject = ObjectSpace.CreateObject<Customer>();
                theObject.Name = name;
                theObject.Code = Code;
            }
        }
        public override void UpdateDatabaseBeforeUpdateSchema() {
            base.UpdateDatabaseBeforeUpdateSchema();
            //if(CurrentDBVersion < new Version("1.1.0.0") && CurrentDBVersion > new Version("0.0.0.0")) {
            //    RenameColumn("DomainObject1Table", "OldColumnName", "NewColumnName");
            //}
        }
    }
}
