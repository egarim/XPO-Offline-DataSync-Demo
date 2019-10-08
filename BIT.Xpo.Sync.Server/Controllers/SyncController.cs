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

        public override IActionResult GetAllByIdentity(string Identity, int FromTransactionId)
        {
            IActionResult actionResult = base.GetAllByIdentity(Identity, FromTransactionId);
            return actionResult;
        }

        public override IActionResult GetTransactionLogCountByIdentity(string Identity)
        {
            return base.GetTransactionLogCountByIdentity(Identity);
        }
        public override Task<IActionResult> Post()
        {
            var Result= base.Post();
            return Result;
        }

    }
}