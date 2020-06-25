using OutgoingMessageService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutgoingMessageService.Result
{
    public class BulkMailResult : MessageResult<BulkMail>
    {
        public BulkMailResult(BulkMail mes)
            : base(mes)
        {
        }

        public string ServerDelayTime { get; set; }
    }
}
