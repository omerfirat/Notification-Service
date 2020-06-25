using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace NotificationService.Model
{
    public class BulkSms : BaseMessage
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ServiceCode { get; set; } //Hangi kısa numara üzerinden gönderileceğini tanımlamaktadır. (Aktif olan bir servis kodu tanımlı değilse 7261 olarak ilk değer ataması yapabilirsiniz.)
        public string Sender { get; set; } //Mesaj başlığı (alfanumerik)
        public string Phone { get; set; } //pMsisdn: telefon listesi, ayraç: tilda(~)
        public string MessageContent { get; set; }
        public string MsgSpecialId { get; set; } // CODEC Raporlaması için atanabilecek özel parametre (opsiyonel)
        public DateTime SendDate { get; set; } // Mesaj gönderim tarihi (yyyy-MM-dd HH:mm:ss) 
        public DateTime EndDate { get; set; } // Mesaj geçerlilik tarihi; sendDate ile arasındaki fark 60-1710dk aralığında olmalı!  (yyyy-MM-dd HH:mm:ss)   
        public string FirstSmsId { get; set; }
        public string LastSmsId { get; set; }

        public string  CombinedSmsId { get; set; }

        public string CombinedSmsIdIndex { get; set; }
        public List<BulkPhone> BulkPhoneList { get; set; }

        public string MesajId { get; set; } = "";

        public class BulkPhone
        {
            public int IndexId  { get; set; }
            public int SmsId { get; set; }
            public string Phone { get; set; }
            public string  ErrorCode { get; set; }
            public string Status { get; set; }
        }

        public BulkSms(DataTable dtSms)
        {
            double validForHours = 28;
            string senderDomestic = "";
            TimeSpan ts = new TimeSpan();
            try
            {
                BulkPhoneList = new List<BulkPhone>();
                StringBuilder sbPhone = new StringBuilder();
                StringBuilder sbContent = new StringBuilder();
                StringBuilder sbCombinedSmsIdIndex = new StringBuilder();
                StringBuilder sbCombinedSmsId = new StringBuilder();
                DateTime currentDateTime = DateTime.Now;

                int firstSmsId = dtSms.AsEnumerable().Min(x => x.Field<int>("SmsId"));
                int lastFirstId = dtSms.AsEnumerable().Max(x => x.Field<int>("SmsId"));
                FirstSmsId = firstSmsId.ToString();
                LastSmsId = lastFirstId.ToString();
                int index = 0;
                foreach (DataRow drSms in dtSms.Rows)
                {
                    BulkPhone bulkPhone = new BulkPhone();
                    bulkPhone.Phone = drSms["Telefon"].ToString();
                    bulkPhone.SmsId = Convert.ToInt32(drSms["SmsId"].ToString());
                    bulkPhone.IndexId = index;
                    BulkPhoneList.Add(bulkPhone);

                    sbPhone.Append(Convert.ToString(drSms["Telefon"])).Append("~");
                    sbContent.Append(Convert.ToString(drSms["Mesaj"])).Append("~");
                    sbCombinedSmsIdIndex.Append(index).Append("|");
                    sbCombinedSmsId.Append(bulkPhone.SmsId).Append(",");
                    index++;
                }

                Phone = sbPhone.ToString().TrimEnd('~');
                MessageContent = sbContent.ToString().TrimEnd('~');
                CombinedSmsIdIndex = sbCombinedSmsIdIndex.ToString().TrimEnd('|');
                CombinedSmsId = sbCombinedSmsId.ToString().TrimEnd(',');

                MsgSpecialId = "BULK"
                    + "-" + currentDateTime.ToString("yyyyMMddHHmmss")
                    + "-" + Convert.ToString(FirstSmsId)
                    + "-" + Convert.ToString(LastSmsId); //BULK-yyyyMMddHHmmss-1-76376-78656

                MesajId = dtSms.Rows[0]["MesajId"].ToString();
                SendDate = currentDateTime;

                /* Mesajın kaç saat geçerli olacağı (ValidForHours) config'den alınarak endDate hesaplanır. 
                Günün belirli bir saatinden sonra sms gitmemesi (ValidUntilTime) isteniyorsa saat config'de belirlenir. */

                if (validForHours > 0)
                    EndDate = currentDateTime.AddHours(validForHours);

                if (ts != TimeSpan.Zero && EndDate.TimeOfDay > ts)
                    EndDate = EndDate.Date + ts;
                ServiceCode = "";
                Sender = senderDomestic;
            }

            catch (Exception ex)
            {
                Logger.EnterLog(LogType.Error, ex.Message, ex, ConfigReader.ServisName);
            }
        }

    }
}
