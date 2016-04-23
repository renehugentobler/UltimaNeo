using System;
using System.Security.Cryptography;
using System.Text;

namespace UltimaNeo.Tools
{
    public class WebSocketMessage
    {
        string cmd { get; set; }
        DateTime timestamp { get; set; }
        Guid source { get; set; }
        Guid target { get; set; }
        string payload { get; set; }
        string data { get; set; }
        string hashCode { get; set; }
        public Boolean isValid { get; protected set; }
        public string errormsg { get; protected set; }
        public string msg { get; protected set; }


        static string GetMd5Hash(MD5 md5Hash, string input)
        {
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++) { sBuilder.Append(data[i].ToString("x2")); }
            return sBuilder.ToString();
        }

        static string GetMd5Hash(MD5 md5Hash, byte[] input)
        {
            byte[] data = md5Hash.ComputeHash(input);
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++) { sBuilder.Append(data[i].ToString("x2")); }
            return sBuilder.ToString();
        }

        public WebSocketMessage(string msg)
        {
            isValid = true;
            if (msg.Length < 32)
            {
                isValid = false;
                errormsg = "Invalid Length";
            }
            else
            {
                hashCode = msg.Substring(0, 32);
                payload = msg.Remove(0,32);
                isValid = (hashCode == GetMd5Hash(MD5.Create(), payload));
                if (!isValid) { errormsg = "Invalid Hash"; }
            }
        }


        public Boolean encode()
        {
            data = string.Format("{0,-8}{1:u}{2}{3}{4}", cmd, DateTime.Now, this.source, this.target, this.data);
            byte[] plainTextBytes = System.Text.Encoding.UTF8.GetBytes(data);
            payload = System.Convert.ToBase64String(plainTextBytes);
            hashCode = GetMd5Hash(MD5.Create(), payload);
            msg = String.Format("{0}{1}", hashCode, payload);
            return true;
        }

        public Boolean decode()
        {
            var base64EncodedBytes = System.Convert.FromBase64String(payload);
            data = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
            return true;
        }

        public WebSocketMessage(string cmd, Guid source, Guid target, string data)
        {
            isValid = true;
            this.cmd = cmd;
            this.source = source;
            this.target = target;
            this.data = data;
        }

    }
}
