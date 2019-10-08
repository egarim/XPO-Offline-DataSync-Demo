using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BIT.Xpo.OfflineDataSync;
using BIT.Xpo.OfflineDataSync.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BIT.Xpo.Sync.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SyncController : SyncControllerBase
    {
        public SyncController(SyncDataStore SyncDataStore) : base(SyncDataStore)
        {

        }
    }
}