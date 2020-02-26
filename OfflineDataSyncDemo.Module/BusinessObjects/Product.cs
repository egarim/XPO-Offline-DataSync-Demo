
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;

namespace Demo.ORM
{
    public class Product : BaseObject
    {
        public Product(Session session) : base(session)
        { }

        string _description;
        string _name;
        decimal _unitPrice;

        public decimal UnitPrice
        {
            get => _unitPrice;
            set => SetPropertyValue(nameof(UnitPrice), ref _unitPrice, value);
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Name
        {
            get => _name;
            set => SetPropertyValue(nameof(Name), ref _name, value);
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Description
        {
            get => _description;
            set => SetPropertyValue(nameof(Description), ref _description, value);
        }

    }
}