using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleAccess.Domain.Models
{
    public class PickingSession
    {
        public string id
        {
            get; set;
        }
        public string pickerUri { get; set; }

        public string expireTime { get; set; }

        public bool mediaItemsSet { get; set; }

        public PickingConfig pickingConfig { get; set; }

        public PollingConfig pollingConfig { get; set; }
        public class PollingConfig
        {
            public string pollInterval { get; set; }
            public string timeoutIn { get; set; }
        }
        public class PickingConfig
        {
            public string maxItemCount { get; set; }
        }

    }
}
