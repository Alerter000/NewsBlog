using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsBlog.Models
{
    public static class ConverterFileToBytes
    {
        public static byte[] Convert(HttpPostedFileBase upload)
        {
            string fileName = System.IO.Path.GetFileName(upload.FileName);
            byte[] file = new byte[upload.ContentLength];
            upload.InputStream.Read(file, 0, upload.ContentLength);
            return file;
        }
    }
}