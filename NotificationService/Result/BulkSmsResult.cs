using NotificationService.Model;

namespace NotificationService.Result
{
    public class BulkSmsResult : MessageResult<BulkSms>
    {
        public BulkSmsResult(BulkSms mes) : base(mes)
        {
        }
        public string ErrorCode { get; set; }
    }
}
