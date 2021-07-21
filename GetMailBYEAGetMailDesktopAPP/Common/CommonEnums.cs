using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetMailBYEAGetMailDesktopAPP.Common
{
    class CommonEnums
    {
        /// <summary>
        /// here we create enum to get mail information from table WebMailSettings by key ID
        /// </summary>
        public enum MailSettings
        {
            MailAddress = 1,
            MailPassword = 2,
            IMAP = 3,
            Port = 4,
            ExceptionLogFilePath = 5,
            EmailToRetrive =6
        }
    }
}
