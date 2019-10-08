using DevExpress.Xpo.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BIT.Xpo.OfflineDataSync;
using DevExpress.Xpo;
using Demo.ORM;

namespace ConsoleDemo
{
    class Program
    {
        static void Main(string[] args)
        {



            Console.WriteLine("Press any key to start");
            Console.ReadKey();

            SyncDataStore SyncDataStore = new SyncDataStore("ConsoleApp", true, "http://localhost/BIT.Xpo.Sync.ServerNew/api/Sync", null);


            string localDatabaseConnectionString = AccessConnectionProvider.GetConnectionString("LocalDatabase.mdb");
            string logDatabaseConnectionString = AccessConnectionProvider.GetConnectionString("LogDatabase.mdb");



            SyncDataStore.Initialize(
             localDatabaseConnectionString,
              logDatabaseConnectionString, false, AutoCreateOption.DatabaseAndSchema
            , new Assembly[] { typeof(Demo.ORM.CustomBaseObject).Assembly });


            Console.WriteLine("Pulling data from the api");
            var Transactions = SyncDataStore.PullModificationsFromApi();
            Console.WriteLine($"{Transactions} transactions pulled from the API");
            XpoDefault.DataLayer = new SimpleDataLayer(SyncDataStore);

            Console.WriteLine("Creating customers");
            DemoData();
            Console.WriteLine("Pushing data from the api");
            SyncDataStore.PushModificationsToApi();
            Console.WriteLine("Done!!");
            Console.ReadKey();


           

        }

        private static void DemoData()
        {
           
            UnitOfWork UoW = new UnitOfWork();

            if (UoW.Query<Customer>().Count() > 0)
                return;

                List<Customer> Customers = new List<Customer>();
            for (int i = 0; i < 900; i++)
            {
                Console.WriteLine("Creating customer:" + i);
                Customer Customer = new Customer(UoW);
                Customer.Code = i.ToString();
                Customer.Name = "Customer " + i.ToString();
                Customers.Add(Customer);
            }
            Console.WriteLine("Creating Invoices");
            foreach (Customer customer in Customers)
            {
                Invoice Invoice = new Invoice(UoW);
                Invoice.Customer = customer;
                Invoice.Date = DateTime.Now;
            }
            Console.WriteLine("Committing to the database");
            UoW.CommitChanges();
            Console.WriteLine("Adding new customers");
            Customers = new List<Customer>();
            for (int i = 900; i < 910; i++)
            {
                Console.WriteLine("Creating customer:" + i);
                Customer Customer = new Customer(UoW);
                Customer.Code = i.ToString();
                Customer.Name = "Customer " + i.ToString();
                Customers.Add(Customer);
            }
            Console.WriteLine("Committing to the database");
            UoW.CommitChanges();
            Console.WriteLine("Updating customers");
            foreach (Customer customer in Customers)
            {
                customer.Name = "New Name";
                //UoW.Delete(customer);
            }

            Console.WriteLine("Committing to the database");
            UoW.CommitChanges();
        }
    }
}
