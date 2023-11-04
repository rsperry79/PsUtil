using System;
using System.Net;
using System.Net.Mail;

namespace Utilities.Common.TokenHelpers
{
    /// <summary>
    /// Defines the <see cref="SendEmail" />
    /// </summary>
    public class SendEmail
    {
        /// <summary>
        /// Defines the settings
        /// </summary>
        private readonly SmtpSettings settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="SendEmail"/> class.
        /// </summary>
        public SendEmail()
        {
            this.settings = new SmtpSettings
            {
                Host = "smtp-mail.outlook.com",
                Port = 587,
                Username = "TuringNotifier@outlook.com",
                Code = "exxact@1",
            };
        }

        /// <summary>
        /// The Send
        /// </summary>
        /// <param name="to">The to<see cref="string"/></param>
        /// <param name="subject">The subject<see cref="string"/></param>
        /// <param name="body">The body<see cref="string"/></param>
        /// <returns>The <see cref="bool"/></returns>
        public bool Send(string to, string subject, string body)
        {
            // TODO: Validate arguments
            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(this.settings.Username);
                    mail.To.Add(to);
                    mail.Subject = subject;
                    mail.Body = body;
                    mail.ReplyToList.Add(new MailAddress(this.settings.Username));

                    SmtpClient smtp = new SmtpClient(this.settings.Host, this.settings.Port)
                    {
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(this.settings.Username, this.settings.Code),
                        Timeout = 60000, // 60 seconds
                        EnableSsl = true // Outlook.com and Gmail require SSL
                    };
                    smtp.Send(mail);
                    smtp.Dispose();

                    // email was accepted by the SMTP server
                    return true;
                }
            }
            catch (Exception)
            {
                // TODO: Log the exception message
                return false;
            }
        }
    }
}
