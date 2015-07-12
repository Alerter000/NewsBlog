using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace NewsBlog.Models
{
    public static class MD5Hash
    {
        public static string GetMD5Hash(string str)
        {
            System.Security.Cryptography.MD5 hash = System.Security.Cryptography.MD5.Create();
            byte[] data = hash.ComputeHash(Encoding.Default.GetBytes(str));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

        //public static Guid ToGuid(int value)
        //{
        //    byte[] bytes = new byte[16];
        //    BitConverter.GetBytes(value).CopyTo(bytes, 0);
        //    return new Guid(bytes);
        //}
    }
}