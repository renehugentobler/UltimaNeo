using System;
using System.IO;
using System.IO.Compression;
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
        public Boolean isCompressed { get; set; }
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

        static byte[] Compress(byte[] raw)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                using (GZipStream gzip = new GZipStream(memory,
                CompressionMode.Compress, true))
                {
                    gzip.Write(raw, 0, raw.Length);
                }
                return memory.ToArray();
            }
        }

        static byte[] Decompress(byte[] gzip)
        {
            using (GZipStream stream = new GZipStream(new MemoryStream(gzip), CompressionMode.Decompress))
            {
                const int size = 4096;
                byte[] buffer = new byte[size];
                using (MemoryStream memory = new MemoryStream())
                {
                    int count = 0;
                    do
                    {
                        count = stream.Read(buffer, 0, size);
                        if (count > 0)
                        {
                            memory.Write(buffer, 0, count);
                        }
                    }
                    while (count > 0);
                    return memory.ToArray();
                }
            }
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


        public Boolean encode() { return encode(false); }
        public Boolean encode(Boolean isCompressed)
        {
            data = string.Format("{0,-8}{1:u}{2}{3}{4}", cmd, DateTime.Now, this.source, this.target, this.data);
            byte[] plainTextBytes = System.Text.Encoding.UTF8.GetBytes(data);
            if (isCompressed)
            {
                plainTextBytes = Compress(plainTextBytes);
            }
            payload = System.Convert.ToBase64String(plainTextBytes);
            hashCode = GetMd5Hash(MD5.Create(), payload);
            msg = String.Format("{0}{1}", hashCode, payload);
            return true;
        }
        public Boolean decode() { return decode(false); }

        public Boolean decode(Boolean isCompressed)
        {
            byte[] base64EncodedBytes = System.Convert.FromBase64String(payload);
            if (isCompressed)
            {
                base64EncodedBytes = Decompress(base64EncodedBytes);
            }
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
