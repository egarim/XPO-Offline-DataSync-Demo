using BIT.Xpo.OfflineDataSync;
using System;
using System.Linq;

namespace OfflineDataSyncDemo.Module
{
    public interface ISyncDataStore
    {
        SyncDataStore SyncDataStore { get; set; }
    }
}
