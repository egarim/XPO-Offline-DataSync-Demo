### Note

I have created this demo using 2 commercial components created by me Joche Ojeda from the company https://www.bitframeworks.com.
The components are:

- BIT.Xpo.OfflineDataSync
- BIT.Xpo.OfflineDataSync.AspNetCore

The components listed above can be used in design time without any restriction, but they will not run on release build since they required a valid license for that.
If you need info about licensing or assistant running this demo, please contact dev@xari.io and we will assist you as soon as possible 

### Requirements
1.	Setup BIT frameworks NuGet repository https://nuget.bitframeworks.com/nuget/main/ you can learn how to setup the repository here https://docs.microsoft.com/en-us/nuget/consume-packages/install-use-packages-visual-studio#package-sources
2.	DevExpress references or nugets for version19.1.3 because the project is compiled with that version and the OfflineDataSync component is compiled with that version
3.	IIS or IIS express setup to enable external request, you can read about that here https://www.lakshmikanth.com/2015/10/16/enable-external-request-on-iis-express/

### How to run the demo

#### XAF and Xamarin forms scenario

The create the correct scenario for a data synchronization we need to have sample data to replicate so for that you should do the following steps

1.	Open the app config file on the project OfflineDataSyncDemo.Win, there are 2 connection strings, these connection strings can point to different database engines, also is preferred that you use connection strings that use username and password since allow IIS express to use integrated security requires additional configuration outside the scope of this tutorial
    a.	ConnectionString: this should be the connection to the database for your Master
    b.	LogConnectionString: this should be the connection to the database for your transaction logs
2.	Run the project OfflineDataSyncDemo.Win it create some sample customers and this transaction will create the first transaction log
3.	Now go to the BIT.Xpo.Sync.Server project and edit the appsettings.json file. Setup the same 2 connection strings that you used in step 1, now the sync server is ready to run
4.	Set BIT.Xpo.Sync.Server as your start up project and run it one time, a browser will open and you will see this url http://localhost/BIT.Xpo.Sync.ServerNew/api/test , copy the address and replace the address with your local IP and remove the api/test path, so you will see something like this http://192.168.1.21/BIT.Xpo.Sync.ServerNew/ this is the address oy your sync server
5.	Right click over the solution, go to properties and the select startup projects > multiple startup projects and in the action, column set the following projects to start
 
    a.	BIT.Xpo.Sync.Server
    
    b.	OfflineDataSyncDemo.Win
    
    c.	XamarinApp.Android
    
6. Now you can do the CRUD operations in the XAF app (OfflineDataSyncDemo.Win) and sync them with the XAMARIN app (XamarinApp.Android)
7. Have fun 😊 adding or editing records on the Xamarin App
