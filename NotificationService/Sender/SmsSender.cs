using NotificationService.Model;
using NotificationService.Result;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace NotificationService.Sender
{
    public class SmsSender : BaseMessageSender<Sms, SmsResult>
    {
        protected override SmsResult SendMessage(Sms message)
        {
            SmsResult result = new SmsResult(message);        
            try
            {
                if (!string.IsNullOrEmpty(message.MessageContent))
                {
                    result.SendDateTime  = DateTime.Now;
                    DateTime dtGiris = DateTime.Now;
                    string Id = SendSms(message);
                    if (!String.IsNullOrWhiteSpace(Id) && Id != "")
                    {
                        if (Id.Substring(0, 1) == "0")
                        {
                            AddLog(Convert.ToInt32(result.Message.SmsId), result.Message.Id.ToString(), result.SendDateTime, DateTime.Today);
                        }
                        else
                        {
                            Logger.EnterLog(LogType.Error, "Sms isn't sent for " +  Common.CreateParameterString(Id,message.SmsId), null, "Sms");
                        }
                    }
                }
                else
                {
                    Logger.EnterLog(LogType.Error, "Sms isn't sent for " + message.SmsId + " SmsId because of message content is null", null, "Sms");
                }            
            }
            catch (Exception ex)
            {
                result.IsSuccesful = false;
                result.Exception = ex;
                result.Message = message;
                Logger.EnterLog(LogType.Error, "Sms isn't sent for  "+ Common.CreateParameterString(message.SmsId, message.Id), ex, "Sms");
            }

            return result;
        }


        public void AddLog(int smsId,string messageId, DateTime sendDate,DateTime responseDate)
        {            
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO ... VALUES (@SmsId,@MessageId,@SendDate,@ResponseDate) ");
            using (SqlConnection connection = new SqlConnection(ConfigReader.ConnectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandTimeout = 9999;
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@SmsId", smsId);
                    cmd.Parameters.AddWithValue("@SendDate", sendDate);
                    cmd.Parameters.AddWithValue("@MessageId", messageId);
                    cmd.Parameters.AddWithValue("@ResponseDate", responseDate);
                    cmd.CommandText = sb.ToString();
                    int resultCount = cmd.ExecuteNonQuery(); 
                }
                connection.Close();
            }
           
        }

        protected override void AfterProcessResult(SmsResult result)
        {
            try
            {
                SqlConnection connection = new SqlConnection(ConfigReader.ConnectionString);
                if (result.IsSuccesful)
                {                   
                    Logger.EnterLog(LogType.Trace, " Sms record status succesfully updated on ... for " + Common.CreateParameterString(result.Message.SmsId, result.Message.Id));
                }
                else
                {
                    if (!String.IsNullOrWhiteSpace(result.Message.Id.ToString()) && result.Message.Id.ToString() != "")
                    {                     
                    }
                }
            }
            catch (Exception ex)
            {
                result.Exception = ex;
                Logger.EnterLog(LogType.Error, "Fast Sms not updated on ... for  " + Common.CreateParameterString(result.Message.SmsId, result.Message.Id), ex, "Sms");
            }
        }

        /// <summary>
        /// /// SendMessage methodu çalıştıktan sonra sms gönderim işlemi başarılıysa tabloyu güncelliyoruz.
        /// </summary>
        /// <param name="sms"></param>
        /// <returns></returns>
        protected static string SendSms(Sms sms)
        {
            object returncode = "";
            try
            {               
             

                return returncode.ToString();
            }
            catch (Exception ex)
            {
                Logger.EnterLog(LogType.Error, "Sms isn't sent for "+ Common.CreateParameterString(sms.SmsId,sms.Id, returncode), ex, "Sms", "SmsResult->SendSmsWeb");
                return returncode.ToString();
            }
        }
    }
}
