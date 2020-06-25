using System.Data;

namespace NotificationService.Model
{
    public class Mail:BaseMessage
    {
        public string EmailId { get; set; }
        public string FromDisplayName { get; set; }
        public string FromEmail { get; set; }
        public string RecipientDisplayName { get; set; }
        public string RecipientEmail { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public string Attachment { get; set; }
        public string BCC { get; set; }
        public Mail(DataRow dr)
        {
            EmailId              = dr["EmailId"].ToString();
            FromDisplayName      = dr["FromDisplayName"].ToString();
            FromEmail            = dr["FromEmail"].ToString();
            RecipientDisplayName = dr["RecipientDisplayName"].ToString();
            RecipientEmail       = dr["RecipientEmail"].ToString();
            Subject              = dr["Subject"].ToString();
            Message              = dr["Message"].ToString();
            Attachment           = dr["Attachment"].ToString();
            BCC                  = dr["BCC"].ToString();
        }
    }
}
