using NotificationService.Model;
using System.Collections.Generic;
using System.Data;
using System.ServiceProcess;
using System.Timers;

namespace NotificationService
{
    public partial class Service1 : ServiceBase
    {
        private string _serviceName = string.Empty;
        public Service1()
        {
            InitializeComponent();
        }
        System.Timers.Timer timer = new System.Timers.Timer();
        protected override void OnStart(string[] args)
        {
            _serviceName = ConfigReader.ServisName;
            Logger.EnterLog(LogType.Info, " Service started..", null, _serviceName, "OnStart");
            timer.Elapsed += new ElapsedEventHandler(Run);
            timer.Interval = ConfigReader.Timer;
            timer.Enabled = true;
            timer.Start();
        }

        public void Run(object source, ElapsedEventArgs e)
        {
            if (_serviceName == "Sms")
            {
                List<Sms> smsList = GetSendMessage.GetSmsList();
                if (smsList.Count > 0)
                {
                    NotificationManager.SendMessage(smsList);
                }
            }
            else if (_serviceName == "Mail")
            {
                List<Mail> mailList = GetSendMessage.GetMailList();
                if (mailList.Count > 0)
                {
                    NotificationManager.SendMessage(mailList);
                }
            }

            else if (_serviceName == "BulkMail")
            {
                List<BulkMail> bulkMailList = GetSendMessage.GetBulkMailList();
                if (bulkMailList.Count > 0)
                {
                    NotificationManager.SendMessage(bulkMailList);
                }
            }
            else if (_serviceName == "BulkSms")
            {
                DataTable dtBulkSms = GetSendMessage.GetBulkSmsList();

                if (dtBulkSms != null && dtBulkSms.Rows.Count > 0)
                {
                    BulkSms bulkSms = new BulkSms(dtBulkSms);
                    NotificationManager.SendMessage(bulkSms);
                }
            }
            else if (_serviceName == "SmsStatusFast")
            {
                List<Sms> smsList = GetRequestMessage.GetSmsList();
                if (smsList.Count > 0)
                {
                    NotificationManager.RequestMessage(smsList);
                }
            }
            else if (_serviceName == "SmsStatusBulk")
            {
                DataTable dtBulkSmsList = GetRequestMessage.GetBukSmsList();
                if (dtBulkSmsList != null && dtBulkSmsList.Rows.Count > 0)
                {
                   NotificationManager.RequestMessage(new BulkSms(dtBulkSmsList));                   
                }
            }
        }

        // For Debug
        public void OnDebug()
        {
            OnStart(null);
        }

        protected override void OnStop()
        {
            timer.Enabled = false;
            timer.Stop();
            Logger.EnterLog(LogType.Info, "Service stopped..", null, _serviceName, "Service1->OnStop");
        }
    }
}
