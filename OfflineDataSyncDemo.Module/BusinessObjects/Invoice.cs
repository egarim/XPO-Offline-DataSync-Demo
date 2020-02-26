
using System;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;

namespace Demo.ORM
{
    [DefaultClassOptions()]
    public class Invoice : BaseObject
    {
        public Invoice() : base()
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public Invoice(Session session) : base(session)
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place here your initialization code.
        }
        DateTime date;
        public DateTime Date
        {
            get => date;
            set => SetPropertyValue(nameof(Date), ref date, value);
        }
        Customer customer;
        public Customer Customer
        {
            get => customer;
            set => SetPropertyValue(nameof(Customer), ref customer, value);
        }
        [Association("Invoice-InvoiceDetails")]
        public XPCollection<InvoiceDetail> InvoiceDetails
        {
            get
            {
                return GetCollection<InvoiceDetail>(nameof(InvoiceDetails));
            }
        }
    }
    public class InvoiceDetail : BaseObject
    {
        public InvoiceDetail(Session session) : base(session)
        { }

        Product _product;
        Invoice _invoice;

        [Association("Invoice-InvoiceDetails")]
        public Invoice Invoice
        {
            get => _invoice;
            set => SetPropertyValue(nameof(Invoice), ref _invoice, value);
        }
        
        public Product Product
        {
            get => _product;
            set => SetPropertyValue(nameof(Product), ref _product, value);
        }

    }
}