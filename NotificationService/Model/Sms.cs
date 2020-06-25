using System;
using System.Data;

namespace NotificationService.Model
{
    public class Sms:BaseMessage
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Sender { get; set; }
        public string Phone { get; set; }
        public string MessageContent { get; set; }
        public string MsgSpecialId { get; set; }
        public Boolean IsOtn { get; set; }
        public string HeaderCode { get; set; }
        public int ResponseType { get; set; }
        public string OptionalParameters { get; set; }
        public string SmsId { get; set; }
        public string MesajGrup { get; set; }
        public string MesajId { get; set; }

        public Sms(DataRow dr)
        {
            Phone = dr["Telefon"].ToString();
            MessageContent = dr["Mesaj"].ToString();
            MsgSpecialId = dr["MesajGrup"].ToString();
            SmsId = dr["SmsId"].ToString();
            MesajId = dr["MesajId"].ToString();
            IsOtn = false;
            ResponseType = 1;
        }

    }
}
