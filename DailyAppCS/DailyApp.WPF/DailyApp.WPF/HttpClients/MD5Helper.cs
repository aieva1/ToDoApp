using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace DailyApp.WPF.HttpClients
{
    internal class MD5Helper
    {
        public static string GetMd5(string content)
        {
            using (var md5 = MD5.Create())
            {
                byte[] hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(content));
                return string.Join("", hashBytes.Select(x => x.ToString("x2")));
            }
        }
    }
}
