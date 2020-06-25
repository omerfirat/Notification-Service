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
using static OutgoingMessageService.Model.BulkSms;

namespace OutgoingMessageService
{
    public class UpdateRequestNotification
    {
        public static UpdateRequestNotification _instance;
        public ConcurrentQueue<object> ResultsToProcess { get; } = new ConcurrentQueue<object>();
        public SqlConnection Con { get; set; }
        public CancellationToken Token { get; set; }
        private string LogParameter { get; set; }
        public void AddResultToQueue(object result,string logParameter)
        {
            LogParameter = logParameter;
            ResultsToProcess.Enqueue(result);
        }

        public static UpdateRequestNotification Instance(CancellationToken token)//tek sefer service1 de 
        {
            object locker = new object();

            if (_instance == null)
            {
                lock (locker)
                {
                    _instance = new UpdateRequestNotification(token);
                }
            }
            return _instance;
        }

        public static UpdateRequestNotification Instance()//add que için kullanılacak
        {
            if (_instance == null)
            {
                throw new Exception("Instance başlatılamadı");
            }
            return _instance;
        }

        private UpdateRequestNotification(CancellationToken token)
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

                try
                {

               
                    if (ResultsToProcess.Count > 0)
                    {
                        object result;
                        ResultsToProcess.TryDequeue(out result);
                    
                        if (result is SmsResult)
                        {
                            SmsResult smsResult = result as SmsResult;
                            if (smsResult.IsSuccesful == true)
                            {
                                if(smsResult.Message.SmsId== "10884103" || smsResult.Message.SmsId=="10884102")
                                {
                                    // bunlar boş gelecek mi buraya bakalım.
                                }

                                string msgRefId = "";
                                string status = "";
                                string errorCode = "";
                                string gonderimOK = "";
                              //  SqlConnection con = new SqlConnection(ConfigReader.ConnectionString);
                                string id = smsResult.Message.Id.ToString().Substring(0, 1);
                                if (id == "0")
                                {
                                    string outOfResultCode = smsResult.Message.Id.ToString().Substring(2, smsResult.Message.Id.ToString().Length - 2);
                                    string[] seperatedOutOfResultCode = outOfResultCode.Split('|');
                                    msgRefId = seperatedOutOfResultCode[0];
                                    status = seperatedOutOfResultCode[1];
                                    errorCode = seperatedOutOfResultCode[2];
                                    if ((status == "0" || status == "5") && errorCode == "0")
                                    {
                                        gonderimOK = "1";
                                    }
                                    else
                                    {
                                        gonderimOK = "3";
                                    }
                                }
                                else
                                {
                                    string[] errorCodeArr = smsResult.Message.Id.ToString().Split(';');
                                    errorCode = errorCode[0].ToString();
                                    gonderimOK = "3";
                                }

                                StringBuilder sb = new StringBuilder();
                                sb.Append("UPDATE Fny_OutgoingSMS SET ErrorCode = @ErrorCode,Status=@Status,GonderimOK=@GonderimOK WHERE SmsId=@SmsId ");
                                SqlCommand cmd = new SqlCommand();
                                cmd.Connection = Con;
                                cmd.CommandType = CommandType.Text;
                                cmd.Parameters.AddWithValue("@SmsId", smsResult.Message.SmsId);
                                cmd.Parameters.AddWithValue("@Status", status);
                                cmd.Parameters.AddWithValue("@ErrorCode", errorCode);
                                cmd.Parameters.AddWithValue("@GonderimOK", gonderimOK);
                                cmd.CommandText = sb.ToString();
                                int resultCount = cmd.ExecuteNonQuery();
                           
                            }

                        }
                        else if (result is BulkSmsResult)
                        {
                            BulkSmsResult bulkSmsResult = result as BulkSmsResult;
                            if (bulkSmsResult.IsSuccesful)
                            {
                                string gonderdimOK = "";
                                //0|5322061328|3|0810151100|0~2|5327683529|2|0810151103|-444
                                //(MSGINDEX|MSISID|MSGSTATUS|STATUSDATE|ERRCODE)
                                string[] resultMsgStatusArrMain = bulkSmsResult.Message.Id.ToString().Split('~');

                                if (bulkSmsResult.Message.Id.ToString().Length < 27) //27 '0|5322061328|3|0810151100|0' length 27 den küçükse hata gelmiştir.
                                {
                                   // SqlConnection con = new SqlConnection(ConfigReader.ConnectionString);//tekrar tekrar connection oluşturma
                                    StringBuilder sb = new StringBuilder();
                                    sb.Append("UPDATE Fny_OutgoingSMSBulk SET ErrorCode = @ErrorCode WHERE MesajId=@MesajId");
                                    SqlCommand cmd = new SqlCommand();
                                    cmd.Connection = Con;
                                    cmd.CommandType = CommandType.Text;
                                    cmd.Parameters.AddWithValue("@ErrorCode", resultMsgStatusArrMain[0].ToString());
                                    cmd.Parameters.AddWithValue("@MesajId", bulkSmsResult.Message.MesajId);
                                    cmd.CommandText = sb.ToString();
                                    int resultCount = cmd.ExecuteNonQuery();
                                }
                                else
                                {
                                    foreach (string msgStsatus in resultMsgStatusArrMain)
                                    {
                                        //0|5322061328|3|0810151100|0
                                        string[] resultMsgStatusArrChild = msgStsatus.Split('|');
                                        string index = resultMsgStatusArrChild[0].ToString();
                                        string phone = resultMsgStatusArrChild[1].ToString();
                                        string msgStatus = resultMsgStatusArrChild[2].ToString();
                                        string date = resultMsgStatusArrChild[3].ToString();
                                        string errorCode = resultMsgStatusArrChild[4].ToString();

                                        BulkPhone bulkPhone = (from p in bulkSmsResult.Message.BulkPhoneList
                                                               where p.IndexId == Convert.ToInt32(index)
                                                               select p).FirstOrDefault();
                                        if (msgStatus == "1" && errorCode == "0")
                                            gonderdimOK = "1";
                                        else
                                            gonderdimOK = "3"; // Hatada alsa beklemede de olsa statu 3 te kalsın.

                                       // SqlConnection con = new SqlConnection(ConfigReader.ConnectionString);
                                        StringBuilder sb = new StringBuilder();
                                        sb.Append("UPDATE Fny_OutgoingSMSBulk SET ErrorCode = @ErrorCode, GonderimOK=@GonderimOK,Status=@Status WHERE SmsId=@SmsId AND GonderimOK=3");

                                        SqlCommand cmd = new SqlCommand();
                                        cmd.Connection = Con;
                                        cmd.CommandType = CommandType.Text;
                                        cmd.Parameters.AddWithValue("@ErrorCode", errorCode);
                                        cmd.Parameters.AddWithValue("@GonderimOK", gonderdimOK);
                                        cmd.Parameters.AddWithValue("@SmsId", bulkPhone.SmsId);
                                        cmd.Parameters.AddWithValue("@Status", msgStatus);
                                        cmd.CommandText = sb.ToString();
                                        int resultCount = cmd.ExecuteNonQuery();

                                    }
                                }
                            }
                        }
                    
                    }
                }
                catch (Exception ex)
                {
                    //LOGLAMA YAP
                    //throw;
                }
            }
            Con.Close();
        }
    }
}

