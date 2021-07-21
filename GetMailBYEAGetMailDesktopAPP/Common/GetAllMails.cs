using GetMailBYEAGetMailDesktopAPP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EAGetMail;
using System.Threading;

namespace GetMailBYEAGetMailDesktopAPP.Common
{
   public class GetAllMails
    {
        WebmailMessagesBLL messagesBLL = new WebmailMessagesBLL();
        AttachmentsBLL attachmentsBLL = new AttachmentsBLL();
        GetMailEntities db = new GetMailEntities();
        public void GetAllUnseenMails()
        {
            try
            {
                List<WebmailMessages> Unseenmails = new List<WebmailMessages>();
                /// <summary>
                /// To get mail data from app.config
                /// string email = ConfigurationManager.AppSettings.Get("email");
                /// string password = ConfigurationManager.AppSettings.Get("password");
                /// string filePath = ConfigurationManager.AppSettings.Get("filePath");
                /// int port = Convert.ToInt32(ConfigurationManager.AppSettings.Get("port"));
                /// string path = filePath;
                /// string connectionString = ConfigurationManager.AppSettings.Get("connection");
                /// string format = "yyyy-MM-dd HH:mm:ss";
                /// int emailsToRetrive = Convert.ToInt32(ConfigurationManager.AppSettings.Get("emailToRetrive"));
                /// </summary>
                //Way to get mail data from DB
                string MailAddress = db.WebMailSettings.Where(z => z.KeyID == (Int64)CommonEnums.MailSettings.MailAddress).FirstOrDefault().KeyValue;
                string MailPassword = db.WebMailSettings.Where(z => z.KeyID == (Int64)CommonEnums.MailSettings.MailPassword).FirstOrDefault().KeyValue;
                string Port = db.WebMailSettings.Where(z => z.KeyID == (Int64)CommonEnums.MailSettings.Port).FirstOrDefault().KeyValue;
                string IMAP = db.WebMailSettings.Where(z => z.KeyID == (Int64)CommonEnums.MailSettings.IMAP).FirstOrDefault().KeyValue;
                string ExceptionPath = db.WebMailSettings.Where(z => z.KeyID == (Int64)CommonEnums.MailSettings.ExceptionLogFilePath)
                        .FirstOrDefault().KeyValue;
                // numbers of unread mails u want to get
                string EmailToRetrive = db.WebMailSettings.Where(z => z.KeyID == (Int64)CommonEnums.MailSettings.EmailToRetrive)
                        .FirstOrDefault().KeyValue;
                string format = "yyyy-MM-dd HH:mm:ss";
                // Gmail IMAP4 server is "imap.gmail.com"
                MailServer oServer = new MailServer(IMAP,
                                    MailAddress,
                                    MailPassword,
                                    ServerProtocol.Imap4);
                // Enable SSL connection.
                oServer.SSLConnection = true;

                // Set 993 SSL port
                oServer.Port = int.Parse(Port);

                MailClient oClient = new MailClient("TryIt");
                oClient.Connect(oServer);
                // retrieve unread/new email only
                oClient.GetMailInfosParam.Reset();
                oClient.GetMailInfosParam.GetMailInfosOptions = GetMailInfosOptionType.NewOnly;
                MailInfo[] infos = oClient.GetMailInfos();
                infos = infos.Take(int.Parse(EmailToRetrive)).ToArray();
                Thread.Sleep(500);
                for (int i = 0; i < infos.Length; i++)
                {
                    MailInfo info = infos[i];

                    // Receive email from IMAP4 server
                    Mail oMail = oClient.GetMail(info);
                    //saving message into database
                    WebmailMessages message = new WebmailMessages();
                    message.Body = oMail.HtmlBody;
                    message.ToEmail = (oMail.To.Count() > 0) ? oMail.To.FirstOrDefault().Address : "Not Set";
                    message.Subject = oMail.Subject.Replace("(Trial Version)", "");
                    message.FromEmail = oMail.From.Address;
                    message.FromName = oMail.From.Name;
                    message.ReceviedDate = DateTime.Parse(oMail.ReceivedDate.ToString(format));
                    message.UIDS = info.UIDL;
                    messagesBLL.AddModel(message, out long mailID);
                    if (oMail.Attachments.Any())
                    {
                        foreach (var att in oMail.Attachments)
                        {
                            string extention = string.Empty;
                            var extentions = att.Name.Split('.');
                            if (extentions.Length > 1)
                            {
                                extention = extentions[1];
                            }
                            else
                            {
                                extention = extentions[0];
                            }
                            WebMailAttachments attachment = new WebMailAttachments();
                            attachment.Attachments_Data = att.Content;
                            attachment.Attachments_MIME = att.ContentType;
                            attachment.Attachments_Extension = extention;
                            attachment.Attachments_FileName = att.Name;
                            attachment.WebMailInfoMessageID = mailID;
                            attachmentsBLL.AddModel(attachment);
                        }
                    }
                    if (!info.Read)
                    {
                        oClient.MarkAsRead(info, true);
                    }
                }
            }
            catch (Exception ex)
            {
                string ExceptionPath = db.WebMailSettings.Where(z => z.KeyID == (Int64)CommonEnums.MailSettings.ExceptionLogFilePath)
                .FirstOrDefault().KeyValue;
                string ExceptionMessage = "Date : " + DateTime.Now.ToString() + " \t" + "Message : " + ex.Message;
                if (ex.InnerException != null)
                {
                    ExceptionMessage = ExceptionMessage + " \n Inner Exception : " + ex.InnerException.Message;
                }
                new WriteToFile().WriteExceptionToFile(ExceptionMessage, ExceptionPath, "GetMailException");

            }
        }
    }
}
