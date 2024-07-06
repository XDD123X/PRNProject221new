using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace ProjectPRN221.Core
{
	public class EmailService
	{
		public static Task Send(string email, string subject, string message)
		{
			// create message
			var mail = "hieundhe170151@fpt.edu.vn";
			var pw = "yzwp kzqa npcp fihc";

			var client = new SmtpClient("smtp.gmail.com", 587)
			{
				EnableSsl = true,
				Credentials = new NetworkCredential(mail, pw)
			};

			return client.SendMailAsync(
				new MailMessage(
							from: mail,
							to: email,
							subject: subject,
							message
					)
			);
		}
	}
	}
