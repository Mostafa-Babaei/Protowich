using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.S3.Dtos
{
    public class S3Options
    {
        public string ServiceUrl { get; set; } = "";
        public string AccessKey { get; set; } = "";
        public string SecretKey { get; set; } = "";
        public string Bucket { get; set; } = "";
        public bool UseSsl { get; set; } = true;
        public bool ForcePathStyle { get; set; } = true;
        public string FolderPrefix { get; set; } = "Files";
    }
}
