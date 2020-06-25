using OutgoingMessageService.Result;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OutgoingMessageService
{
    public class UpdateSendNotification
    {
        public static UpdateSendNotification SingletonInstance;
        public ConcurrentQueue<object> ResultsToProcess { get; } = new ConcurrentQueue<object>();
        public SqlConnection Con { get; set; }
        public CancellationToken Token { get; set; }
        private string LogParameter { get; set; }
        public void AddResultToQueue(object result,string logParameter)
        {
            LogParameter = logParameter;
            ResultsToProcess.Enqueue(result);
        }

        private UpdateSendNotification(CancellationToken token)
        {
            Token = token;
            Con = new SqlConnection(ConfigReader.ConnectionString);
            Task.Factory.StartNew(Start, token);
        }

        private void Start()
        {
            Con.Open();

            while (!Token.IsCancellationRequested)
            {
                if (ResultsToProcess.Count > 0)
                {
                    object result;
                    ResultsToProcess.TryDequeue(out result);
                    if (result is MailResult)
                    {
                        MailResult mailResult = result as MailResult;
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = Con;
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@EmailId", mailResult.Message.EmailId);
                        cmd.CommandText = "UPDATE Fny_OutgoingEmail SET GonderimOK=1,GonderimTarihi=GETDATE() WHERE EmailId=@EmailId AND GonderimOK=2";
                        int resultCount = cmd.ExecuteNonQuery();
                        if (resultCount >= 1)
                        {
                            Logger.EnterLog(LogType.Trace, "Mail record status (GönderimOK) succesfully updated on table Fny_OutgoingEmail " + LogParameter, null, "Mail");
                        }
                        else
                        {
                            Logger.EnterLog(LogType.Error, "Mail record status (GönderimOK) not updated on table Fny_OutgoingEmail" + LogParameter, null, "Mail");
                        }
                    }
                    else if (result is BulkMailResult)
                    {
                        BulkMailResult bulkMailResult = result as BulkMailResult;
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = Con;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "Fny_Send_BatchEmail";
                        cmd.Parameters.AddWithValue("@Tip", 2);
                        cmd.Parameters.AddWithValue("@Emailid", Convert.ToInt32(bulkMailResult.Message.EmailId));
                        SqlDataReader rdr = cmd.ExecuteReader();
                        if (rdr.RecordsAffected > 0)
                            Logger.EnterLog(LogType.Trace, "Bulk mail record status (GönderimOK) succesfully updated on Fny_OutgoingEmailBulk for " + bulkMailResult.Message.EmailId, null, "BulkMail");
                    }
                    else if (result is SmsResult)
                    {
                        SmsResult smsResult = result as SmsResult;
                        if (smsResult.IsSuccesful)
                        {
                            SqlCommand cmd = new SqlCommand();
                            cmd.Connection = Con;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandText = "Fny_upSendSMS_Batch";
                            cmd.Parameters.AddWithValue("@Tip", 30);
                            cmd.Parameters.AddWithValue("@SmsID", Convert.ToInt32(smsResult.Message.SmsId));
                            cmd.Parameters.AddWithValue("@MesajId", smsResult.Message.Id.ToString());
                            SqlDataReader rdr = cmd.ExecuteReader();
                            if (rdr.RecordsAffected > 0)
                                Logger.EnterLog(LogType.Trace, "Fast Sms record status (GönderimOK) succesfully updated on Fny_OutgoingSms for " + LogParameter);
                        }
                        else
                        {
                            if (!String.IsNullOrWhiteSpace(smsResult.Message.Id.ToString()) && smsResult.Message.Id.ToString() != "")
                            {
                                StringBuilder sb = new StringBuilder();
                                sb.Append("UPDATE Fny_OutgoingSMS SET ErrorCode = @ErrorCode, GonderimTarihi=GETDATE() WHERE SmsId=@SmsId ");
                                SqlCommand cmd = new SqlCommand();
                                cmd.Connection = Con;
                                cmd.CommandType = CommandType.Text;
                                cmd.Parameters.AddWithValue("@ErrorCode", smsResult.ErrorCode);
                                cmd.Parameters.AddWithValue("@SmsId", smsResult.Message.SmsId);
                                cmd.CommandText = sb.ToString();     
                                int resultCount = cmd.ExecuteNonQuery();
                            }
                        }
                    }
                    else if (result is BulkSmsResult)
                    {
                        string gonderimOK = "";
                        BulkSmsResult bulkSmsResult = result as BulkSmsResult;
                        if (bulkSmsResult.IsSuccesful)
                        {
                            gonderimOK = "1";
                            StringBuilder sb = new StringBuilder();
                            sb.Append("UPDATE Fny_OutgoingSMSBulk SET GonderimOK=@GonderimOK,Status='3',GonderimTarihi=GETDATE(),MesajId=@MesajId WHERE SmsId IN( ");
                            sb.Append(bulkSmsResult.Message.CombinedSmsId);
                            sb.Append(")");
                            SqlCommand cmd = new SqlCommand();
                            cmd.Connection = Con;
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@GonderimOK", gonderimOK);
                            cmd.Parameters.AddWithValue("@MesajId", bulkSmsResult.Message.Id);
                            int resultCount = cmd.ExecuteNonQuery();
                            if (resultCount > 0)
                                Logger.EnterLog(LogType.Trace, "Bulk Sms record status (GönderimOK) succesfully updated on Fny_OutgoingSmsBulk for " + LogParameter,null,"BulkSms");
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(bulkSmsResult.Message.MessageContent))
                            {
                                StringBuilder sb = new StringBuilder();
                                sb.Append("UPDATE Fny_OutgoingSMSBulk SET ErrorCode = @ErrorCode,GonderimTarihi=GETDATE() WHERE SmsId IN(");
                                sb.Append(bulkSmsResult.Message.CombinedSmsId);
                                sb.Append(") ");
                                SqlCommand cmd = new SqlCommand();
                                cmd.Connection = Con;
                                cmd.CommandType = CommandType.Text;
                                cmd.Parameters.AddWithValue("@ErrorCode", bulkSmsResult.ErrorCode);
                                cmd.CommandText = sb.ToString();
                                int resultCount = cmd.ExecuteNonQuery();                                    
                            }
                            Logger.EnterLog(LogType.Error, "Bulk Sms not updated for  " + bulkSmsResult.Message.CombinedSmsId + " SmsId Result Code: " + bulkSmsResult.Message.Id, null, "BulkSms");
                        }
                    }
                    
                }
            }
            Con.Close();
        }
    }
}

