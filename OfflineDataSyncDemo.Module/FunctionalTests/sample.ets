;#DropDB OfflineDataSyncDemoEasyTest

#Application OfflineDataSyncDemo.Win
#Application OfflineDataSyncDemo.Web
#IfDef OfflineDataSyncDemo.Win

*Action About...
 
*Action OK

#EndIf

#IfDef OfflineDataSyncDemo.Web
!Action Navigation(My Details)
#EndIf
