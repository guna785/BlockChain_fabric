using BL.service;
using DAL.Models;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace BL.Helper
{
    public class BlHelper
    {
        private IGenericBL<otp> _otps;
        private IGenericBL<email> _email;
        public BlHelper(IGenericBL<otp> otps, IGenericBL<email> email)
        {
            _otps = otps;
            _email = email;
        }
        public async Task<bool> GenerateOtp(string uid, string toemail)
        {
            var op = _otps.AsQueryable().OrderByDescending(x => x.CreatedAt).Where(x => x.userId == ObjectId.Parse(uid) && x.status == "active").FirstOrDefault();
            Random generator = new Random();
            String r = generator.Next(0, 1000000).ToString("D6");
            if (op == null)
            {
                await _otps.InsertOneAsync(new otp()
                {
                    CreatedAt = DateTime.Now,
                    otpvalue = r,
                    status = "active",
                    userId = ObjectId.Parse(uid)
                });
                var res = EmailConfig.SendEmail("Confidential Mail", "One Time Password Is " + r + " Please don't Share it to any one", toemail, _email);


            }
            else
            {
                op.status = "deactive";
                await _otps.ReplaceOneAsync(op);
                await _otps.InsertOneAsync(new otp()
                {
                    CreatedAt = DateTime.Now,
                    otpvalue = r,
                    status = "active",
                    userId = ObjectId.Parse(uid)
                });
                var res = EmailConfig.SendEmail("Confidential Mail", "One Time Password Is " + r + " Please don't Share it to any one", toemail, _email);

            }
            return true;
        }
        public async Task<string?> VerifyOtp(string otpval)
        {
            var op = _otps.AsQueryable().Where(x => x.status == "active" && x.otpvalue.Equals(otpval)).FirstOrDefault();
            if (op != null)
            {
                op.status = "finished";
                await _otps.ReplaceOneAsync(op);
                return op.userId.ToString();
            }
            return null;
        }

    }
    public static class EmailConfig
    {
        public async static Task<string> SendEmail(string subject, string msg, string toemail, IGenericBL<email> email)
        {
            var e = email.AsQueryable().FirstOrDefault();
            if (e != null)
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(e.smtpserver);
                mail.From = new MailAddress(e.emailaddress);
                mail.To.Add(toemail);

                mail.Subject = subject;
                mail.Body = msg;

                SmtpServer.Port = 587;

                SmtpServer.Credentials = new System.Net.NetworkCredential(e.emailaddress, e.password);
                SmtpServer.EnableSsl = true;
                ServicePointManager.ServerCertificateValidationCallback =
                delegate (object es, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                {
                    return true;
                };
                try
                {
                    await SmtpServer.SendMailAsync(mail);
                    return "Ok";
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
            return "Email No Configured";
        }

    }
    public static class SMSConfig
    {
        public static string SendSms(string msg)
        {
            return "sent";
        }
    }
    public class DateConfig
    {
        public static string getMonthStingfromNumber(int month)
        {
            string[] monthAbbrev = CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedMonthNames;
            month--;
            return monthAbbrev[month];
        }
        public static int getMonthIntfromNumber(string month)
        {
            string[] monthAbbrev = CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedMonthNames;
            var index = Array.IndexOf(monthAbbrev, month) + 1;
            return index;
        }
    }
    public class defaultImage
    {
        public static string img = "";
    }


}
