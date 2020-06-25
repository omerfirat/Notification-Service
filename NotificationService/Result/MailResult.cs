using NotificationService.Model;

namespace NotificationService.Result
{
    /// <summary>
    /// Mail de farklı bir alan olmayuacaksa kaldırılabilir
    /// </summary>
    public class MailResult:MessageResult<Mail>
    {
        public MailResult(Mail mes)
            : base(mes)
        {
        }

        public string ServerDelayTime { get; set; }
    }
}
