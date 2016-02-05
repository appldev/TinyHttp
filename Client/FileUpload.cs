using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyHttp
{
    public class FileUpload
    {
        public FileUpload()
        {

        }

        public FileUpload(string filePath, string contentType, string parameterName)
        {
            FileName = System.IO.Path.GetFileName(filePath);
            ContentType = contentType;
            ParameterName = parameterName;
            Content = System.IO.File.ReadAllBytes(filePath);
        }

        public FileUpload(string content, string fileName, string contentType, string parameterName)
            : this(content, fileName,contentType,parameterName, Encoding.UTF8)
        {
            
        }

        public FileUpload(string content, string fileName, string contentType, string parameterName, Encoding encoding)
        {
            FileName = fileName;
            ContentType = contentType;
            ParameterName = parameterName;
            Content = encoding.GetBytes(content);
        }



        public string FileName { get; set; }
        public string ContentType { get; set; }
        public string ParameterName { get; set; }
        public byte[] Content { get; set; }
    }
}
