using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;

namespace Stargazer.Common.Email
{
    /// <summary>
    /// email发送客户端
    /// </summary>
    public class EmailClient
    {
        /// <summary>
        /// 发送email
        /// </summary>
        /// <param name="smtpServer">SMTP服务器</param>
        /// <param name="useSsl">是否启用SSL加密</param>
        /// <param name="userName">登录账号</param>
        /// <param name="password">登录密码</param>
        /// <param name="nickName">发件人昵称</param>
        /// <param name="fromMail">发件人</param>
        /// <param name="toMail">收件人</param>
        /// <param name="subject">主题</param>
        /// <param name="body">内容</param>
        /// <returns></returns>
        public static void SendMail(
            string smtpServer,
            int port,
            bool useSsl,
            string userName,
            string password,
            string nickName,
            string fromMail,
            string toMail,
            string subject,
            string body)
        {
            var message = new MimeMessage();
            //发信人
            message.From.Add(new MailboxAddress(nickName, fromMail));
            //收信人
            message.To.Add(new MailboxAddress("", toMail));
            //标题
            message.Subject = subject;
            var textPart = new TextPart(TextFormat.Html)
            {
                Text = body
            };
            
            var multipart = new Multipart("mixed");
            //添加正文内容
            multipart.Add(textPart);
            message.Body = multipart;

            using (var client = new SmtpClient())
            {
                client.Connect(smtpServer, port, useSsl);
                client.Authenticate(userName, password);
                client.Send(message);
                client.Disconnect(true);
            }
        }
    }
}