using NotificationService.Model;

namespace NotificationService.Result
{
    public class SmsResult : MessageResult<Sms>
    {
        public SmsResult(Sms mes) : base(mes)
        {
        }

        public string ErrorCode { get; set; } = "";
    }
}
