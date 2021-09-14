using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Core.Dto;
using Core.Services.Base;
using DAL;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MimeKit;
using InvalidDataException = Core.Services.Base.InvalidDataException;

namespace Core.Services
{
    public class MailService : IService
    {
        private readonly IConfiguration _configuration;
        private readonly ModelContext _context;
        private readonly HttpContext _httpContext;

        public MailService(ModelContext context, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _context = context;
            _httpContext = httpContextAccessor.HttpContext;
            _configuration = configuration;
        }

        public string SendEmailConfirmMail(string email, string username)
        {
            var token = Guid.NewGuid().ToString();
            var host = $"{_httpContext.Request.Scheme}://{_httpContext.Request.Host.Value}/confirm-email/{token}";
            SendMail(new MailRequest
            {
                To = email,
                Subject = "Please confirm your email",
                Body = $"Hello, {username} please use <a href=\"{host}\">this link</a> to confirm your email"
            });
            return token;
        }

        private void SendMail(MailRequest request)
        {
            var name = _configuration["MailSettings:Name"];
            var address = _configuration["MailSettings:Mail"];

            var body = new BodyBuilder
            {
                HtmlBody = request.Body
            };
            var email = new MimeMessage
            {
                Sender = new MailboxAddress(name, address),
                From = { new MailboxAddress(name, address) },
                To = { MailboxAddress.Parse(request.To) },
                Subject = request.Subject,
                Body = body.ToMessageBody()
            };

            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            try
            {
                smtp.Connect(_configuration["MailSettings:Host"], int.Parse(_configuration["MailSettings:Port"]),
                    SecureSocketOptions.StartTls);
                smtp.Authenticate(address, _configuration["MailSettings:Password"]);
                smtp.Send(email);
            }
            catch (Exception e)
            {
                Directory.CreateDirectory(ResolveMailDirectory());
                var filename = $"{DateTime.Now:yyyy-MM-dd-hhmmss}_{Guid.NewGuid()}.eml";
                email.WriteTo(Path.Join(ResolveMailDirectory(), filename));
                throw new InvalidDataException(e)
                {
                    ["email"] = new[] { "Invalid email address" }
                };
            }

            smtp.Disconnect(true);
        }

        private string ResolveMailDirectory()
        {
            return Path.Join(
                _configuration["FileStorage:BasePath"] ??
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Mails");
        }
    }
}