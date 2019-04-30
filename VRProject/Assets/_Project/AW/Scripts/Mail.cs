using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using System.Net.Mime;
using UnityEngine;

namespace VRGame.Mail
{
    class MailManager
    {
        string m_HostName;

        public MailManager(string hostName)
        {
            m_HostName = hostName;
        }

        public void SendMail(EmailSendConfigure emailConfig, EmailContent content)
        {
            MailMessage msg = ConstructEmailMessage(emailConfig, content);
            Send(msg, emailConfig);
        }

        MailMessage ConstructEmailMessage(EmailSendConfigure emailConfig, EmailContent content)
        {
            MailMessage msg = new MailMessage();
            foreach(var to in emailConfig.TOs)
            {
                if (string.IsNullOrEmpty(to) == false)
                {
                    msg.To.Add(to);
                }
            }

            foreach (var cc in emailConfig.CCs)
            {
                if (string.IsNullOrEmpty(cc) == false)
                {
                    msg.CC.Add(cc);
                }
            }

            msg.From = new MailAddress(emailConfig.From, emailConfig.FromDisplayName, System.Text.Encoding.UTF8);

            msg.IsBodyHtml = content.isHtml;
            msg.Body = content.Content;
            msg.Priority = emailConfig.Priority;
            msg.Subject = emailConfig.Subject;
            msg.BodyEncoding = System.Text.Encoding.UTF8;
            msg.SubjectEncoding = System.Text.Encoding.UTF8;

            if(content.AttachFileName != null)
            {
                Attachment data = new Attachment(content.AttachFileName, MediaTypeNames.Application.Zip);
                msg.Attachments.Add(data);
            }

            return msg;
        }

        void Send(MailMessage msg, EmailSendConfigure emailConfig)
        {
            string userToken = "userToken";
            SmtpClient client = new SmtpClient();
            client.UseDefaultCredentials = false;
            //client.Credentials = new System.Net.NetworkCredential(emailConfig.ClientCredentialUserName, emailConfig.ClientCredentialPassword);

            client.Host = m_HostName;
            client.Port = 587;
            client.EnableSsl = true;
                client.Timeout = 15;
            client.SendCompleted += (s, e) => Debug.Log("SendCompleted"); client.Dispose(); msg.Dispose();

            try
            {
                client.SendAsync(msg, null);
            }
            catch(System.Exception ex)
            {
                Debug.LogError(ex.Message);
            }
        }

    }

    public class EmailSendConfigure
    {
        public string[] TOs { get; set; }
        public string[] CCs { get; set; }
        public string From { get; set; }
        public string FromDisplayName { get; set; }
        public string Subject { get; set; }
        public MailPriority Priority { get; set; }
        public string ClientCredentialUserName { get; set; }
        public string ClientCredentialPassword { get; set; }
        public EmailSendConfigure()
        {

        }
    }

    public class EmailContent
    {
        public bool isHtml { get; set; }
        public string Content { get; set; }
        public string AttachFileName { get; set; }
    }

    public class Email
    {
        string m_SmtpServer;
        string m_ClientUserName;
        string m_ClientPassword;

        public Email(string smtpServer, string userName, string password)
        {
            m_SmtpServer = smtpServer;
            m_ClientUserName = userName;
            m_ClientPassword = password;
        }

        public void Send()
        {
            MailManager mailManager = new MailManager(m_SmtpServer);

            EmailSendConfigure myConfig = new EmailSendConfigure();
            myConfig.ClientCredentialUserName = m_ClientUserName;
            myConfig.ClientCredentialPassword = m_ClientPassword;
            myConfig.TOs = new string[] { "19.Aaron.Wiens@ksd.org" };
            myConfig.CCs = new string[] { };
            myConfig.From = "19.Aaron.Wiens@ksd.org";
            myConfig.FromDisplayName = "Aaron Wiens";
            myConfig.Priority = MailPriority.Normal;
            myConfig.Subject = "Testing";

            EmailContent myContent = new EmailContent();
            myContent.Content = "Did it work?";

            mailManager.SendMail(myConfig, myContent);
        }

    }

}
