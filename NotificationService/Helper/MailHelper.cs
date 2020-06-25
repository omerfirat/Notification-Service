using System;
using System.Net.Mail;
using System.Net;
using NotificationService;

public class MailHelper
{
    public static void SendMailMessage(string recepient, string subject, string body, string bcc = "", string cc = "", string attachment = "", string @from = "bilgi@qnbfi.com", string MailServer = "mail.fnylocal.com")
    {
        string fromdisplay = null;
        int I = 0;

        I = (from.IndexOf("/") + 1);
        if ((I > 0))
        {
            fromdisplay = from.Substring(0, (I - 1));
            from = from.Substring(I);
        }
        else
        {
            fromdisplay = "";
        }
        // Instantiate a new instance of MailMessage
        MailMessage mMailMessage = new MailMessage();

        // Set the sender address of the mail message
        mMailMessage.From = new MailAddress(@from, fromdisplay);

        // Set the recepient address of the mail message
        string[] strArrTo = null;
        strArrTo = recepient.Split(';');
        for (int count = 0; count <= strArrTo.Length - 1; count++)
        {
            mMailMessage.To.Add(new MailAddress(strArrTo[count]));
        }

        // Check if the bcc value is nothing or an empty string
        if ((bcc != null) & bcc != string.Empty)
        {
            // Set the Bcc address of the mail message
            string[] strArrBCC = null;
            strArrBCC = bcc.Split(';');
            for (int count = 0; count <= strArrBCC.Length - 1; count++)
            {
                mMailMessage.Bcc.Add(new MailAddress(strArrBCC[count]));
            }
        }

        // Check if the cc value is nothing or an empty value
        if ((cc != null) & cc != string.Empty)
        {
            // Set the CC address of the mail message
            string[] strArrCC = null;
            strArrCC = cc.Split(';');
            for (int count = 0; count <= strArrCC.Length - 1; count++)
            {
                mMailMessage.CC.Add(new MailAddress(strArrCC[count]));
            }
        }

        // Set the subject of the mail message
        mMailMessage.Subject = subject;

        // Set the body of the mail message
        mMailMessage.Body = body;

        // Set the body encoding of the mail message
        mMailMessage.BodyEncoding = System.Text.Encoding.GetEncoding("Windows-1254");

        // Set the format of the mail message body as HTML
        mMailMessage.IsBodyHtml = true;

        // Set the priority of the mail message to normal
        mMailMessage.Priority = MailPriority.Normal;

        // Instantiate a new instance of SmtpClient
        SmtpClient mSmtpClient = new SmtpClient();

        // Set the smtp host of the mail message
        mSmtpClient.Host = MailServer;

        // Send the mail message
        mSmtpClient.Send(mMailMessage);

        mMailMessage.Dispose();
        mMailMessage = null;
        mSmtpClient = null;
    }
    public static bool SendMail(string EmailId,string ToAddress, string Subject, string Body, string attachment = "", string bcc = "", string FromDisplayName = "QNB Finansinvest", string FromEmail = "bilgi@qnbfi.com")
    {
        MailMessage message = new MailMessage();
        try
        {

            MailAddress FromAddress = new MailAddress(FromEmail, FromDisplayName);

            message.From = FromAddress;
            if (string.IsNullOrWhiteSpace(ToAddress) || ToAddress.Length < 3)
            {
                return false;
            }
            string[] toAdresses = ToAddress.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < toAdresses.Length; i++)
            {
                //if (StringFunctions.IsValidEmail(toAdresses[i]))
                //    message.To.Add(toAdresses[i]);
            }

            // Check if the bcc value is nothing or an empty string
            if ((bcc != null) & bcc != string.Empty)
            {
                // Set the Bcc address of the mail message
                string[] strArrBCC = null;
                strArrBCC = bcc.Split(';');
                for (int count = 0; count <= strArrBCC.Length - 1; count++)
                {
                    message.Bcc.Add(new MailAddress(strArrBCC[count]));
                }
            }
            message.Subject = Subject;
            message.Body = Body;
            message.IsBodyHtml = true;
            message.BodyEncoding = System.Text.Encoding.Default;
            if (message.BodyEncoding != System.Text.Encoding.GetEncoding("windows-1254") && message.Body.ToLower().Contains("windows-1254"))
                message.BodyEncoding = System.Text.Encoding.GetEncoding("windows-1254");

            if (!string.IsNullOrWhiteSpace(attachment))
            {
                try
                {
                    message.Attachments.Add(new Attachment(attachment));
                }
                catch (Exception ex)
                {
                    Logger.EnterLog(LogType.Error, ex.Message + Common.CreateParameterString(EmailId, attachment), ex, "Mail", "SendMail");
                    throw (ex);
                }
            }


            SmtpClient client = new SmtpClient("mail.fnylocal.com");
            client.Credentials = CredentialCache.DefaultNetworkCredentials;
            client.Send(message);
            message.Dispose();
            return true;

        }
        catch (Exception ex)
        {
            message.Dispose();
            Logger.EnterLog(LogType.Error, ex.Message + Common.CreateParameterString(EmailId, attachment), ex, "Mail", "SendMail");
            GC.Collect();
            return false;
        }
    }


    public static bool SendMail(string EmailId, string ToAddress, string Subject, string Body, Attachment attachment, string bcc = "", string FromDisplayName = "QNB Finansinvest", string FromEmail = "bilgi@qnbfi.com")
    {
        MailMessage message = new MailMessage();
        try
        {

            MailAddress FromAddress = new MailAddress(FromEmail, FromDisplayName);

            message.From = FromAddress;
            if (string.IsNullOrWhiteSpace(ToAddress) || ToAddress.Length < 3)
            {
                return false;
            }
            string[] toAdresses = ToAddress.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < toAdresses.Length; i++)
            {
                //if (StringFunctions.IsValidEmail(toAdresses[i]))
                //    message.To.Add(toAdresses[i]);
            }

            // Check if the bcc value is nothing or an empty string
            if ((bcc != null) & bcc != string.Empty)
            {
                // Set the Bcc address of the mail message
                string[] strArrBCC = null;
                strArrBCC = bcc.Split(';');
                for (int count = 0; count <= strArrBCC.Length - 1; count++)
                {
                    message.Bcc.Add(new MailAddress(strArrBCC[count]));
                }
            }
            message.Subject = Subject;
            message.Body = Body;
            message.IsBodyHtml = true;
            message.BodyEncoding = System.Text.Encoding.Default;
            if (message.BodyEncoding != System.Text.Encoding.GetEncoding("windows-1254") && message.Body.ToLower().Contains("windows-1254"))
                message.BodyEncoding = System.Text.Encoding.GetEncoding("windows-1254");


            try
            {
                message.Attachments.Add(attachment);
            }
            catch (Exception ex)
            {
                Logger.EnterLog(LogType.Error, ex.Message + Common.CreateParameterString(EmailId, attachment), ex, "Mail", "SendMail");
                throw (ex);
            }


            SmtpClient client = new SmtpClient("mail.fnylocal.com");
            client.Credentials = CredentialCache.DefaultNetworkCredentials;
            client.Send(message);
            message.Dispose();
            return true;

        }
        catch (Exception ex)
        {
            message.Dispose();
            Logger.EnterLog(LogType.Error, ex.Message + Common.CreateParameterString(EmailId, attachment), ex, "Mail", "SendMail");
            GC.Collect();
            return false;
        }
    }
}


