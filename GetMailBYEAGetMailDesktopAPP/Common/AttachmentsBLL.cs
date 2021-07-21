using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using GetMailBYEAGetMailDesktopAPP.Model;

namespace GetMailBYEAGetMailDesktopAPP.Common
{
    class AttachmentsBLL
    {
        GetMailEntities db = new GetMailEntities();
        public void AddModel(WebMailAttachments model)
        {
            db.WebMailAttachments.Add(model);
            try
            {
                db.SaveChanges();

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
                new WriteToFile().WriteExceptionToFile(ExceptionMessage, ExceptionPath, "AddWebMailAttachmentValidationException");
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
                new WriteToFile().WriteExceptionToFile(ExceptionMessage, ExceptionPath, "AddWebMailAttachmentException");

            }
        }
    }
}
