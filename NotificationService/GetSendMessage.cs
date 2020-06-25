using NotificationService.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace NotificationService
{
    public static class GetSendMessage
    {

        /// <summary>
        /// Mail gönderilecek dataları getirir.
        /// </summary>
        /// <returns></returns>
        public static List<Mail> GetMailList()
        {
            List<Mail> mailList = new List<Mail>();
            try
            {

            }
            catch (Exception ex)
            {
                Logger.EnterLog(LogType.Error, ex.Message, ex, "Mail", "GetMailList");
            }
            return mailList;
        }

        /// <summary>
        /// Sms gönderilecek datayı getirir.
        /// </summary>
        /// <returns></returns>
        public static List<Sms> GetSmsList()
        {
            DateTime dtGiris = DateTime.Now;
            List<Sms> smsList = new List<Sms>();
            try
            {

            }
            catch (Exception ex)
            {
                Logger.EnterLog(LogType.Error, ex.Message, ex, "Sms", "GetSmsList");
            }
            return smsList;
        }

        /// <summary>
        /// Mail gönderilecek bulk datayı getirir.
        /// </summary>
        /// <returns></returns>
        public static List<BulkMail> GetBulkMailList()
        {
            SqlConnection connection = new SqlConnection(ConfigReader.ConnectionString);
            List<BulkMail> mailList = new List<BulkMail>();
            try
            {

            }
            catch (Exception ex)
            {
                Logger.EnterLog(LogType.Error, ex.Message, ex, "BulkMail", "GetBulkMailList");
            }
            return mailList;
        }

        /// <summary>
        /// Toplu sms gönderilecek datayı getirir.
        /// </summary>
        /// <returns></returns>
        public static DataTable GetBulkSmsList()
        {
            try
            {
                return new DataTable();
            }
            catch (Exception ex)
            {
                Logger.EnterLog(LogType.Error, ex.Message, ex, "BulkSms", "GetBulkSmsList");
                return null;
            }
        }

    }
}
