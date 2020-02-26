using System;
using DevExpress.Xpo;

namespace Demo.ORM
{
    [NonPersistent()]
    public abstract class CustomBaseObject : XPCustomObject
    {
        public CustomBaseObject() : base()
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public CustomBaseObject(Session session) : base(session)
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            this.oid = Guid.NewGuid();
            // Place here your initialization code.
        }
        [Key(false), Persistent("Oid")]
        Guid oid = Guid.Empty;

        [PersistentAlias("oid")]
        public Guid Oid
        {
            get { return oid; }
        }
    }
}