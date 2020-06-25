using NotificationService.Model;
using System;
using System.Collections.Generic;
using System.Data;

namespace NotificationService
{
    public static class GetRequestMessage
    {
        public static List<Sms> GetSmsList()
        {
            List<Sms> smsList = new List<Sms>();
            try
            {
                
            }
            catch (Exception ex)
            {
                Logger.EnterLog(LogType.Error, ex.Message, ex, "FastSmsRequest", "GetSmsList");
            }

            return smsList;
        }

        public static DataTable GetBulkSmsListByMesajId(string mesajId)
        {
            
            try
            {
                return new DataTable();
            }
            catch (Exception ex)
            {
                Logger.EnterLog(LogType.Error, ex.Message, ex, "BulkSmsRequest", "GetBulkSmsList");
                return null;
            }

        }

        public static DataTable GetBukSmsList()
        {
            try
            {
                
                return new DataTable(); ;
            }
            catch (Exception ex)
            {
                Logger.EnterLog(LogType.Error, ex.Message, ex, "BulkSmsRequest", "GetBulkSmsList");
                return null;
            }

        }

     
    }
}
