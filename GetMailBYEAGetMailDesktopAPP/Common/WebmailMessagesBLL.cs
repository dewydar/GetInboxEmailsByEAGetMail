using GetMailBYEAGetMailDesktopAPP.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetMailBYEAGetMailDesktopAPP.Common
{
    class WebmailMessagesBLL
    {
        private readonly GetMailEntities db = new GetMailEntities();

        public void AddModel(WebmailMessages model,out long mailID)
        {
            mailID = 0;
            db.Entry(model).State = System.Data.Entity.EntityState.Added;
            try
            {
                db.SaveChanges();
                mailID = model.WebmailMessagesID;
            }
            catch (DbEntityValidationException ex)
            {
                string ExceptionPath = db.WebMailSettings.Where(z => z.KeyID == (Int64)CommonEnums.MailSettings.ExceptionLogFilePath)
                    .FirstOrDefault().KeyValue;
                string ExceptionMessage = "Date : " + DateTime.Now.ToString() + " \t" + "Message : " + ex.Message;
                if (ex.InnerException != null)
                {
                    ExceptionMessage = ExceptionMessage + " \n Inner Exception : " + ex.InnerException.Message;
                }
                new WriteToFile().WriteExceptionToFile(ExceptionMessage, ExceptionPath, "AddEmailInfoValidationException");
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
                new WriteToFile().WriteExceptionToFile(ExceptionMessage, ExceptionPath, "AddEmailInfoException");

            }
        }
    }
}
