using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apiTestPro.Class
{
    public class McHttpDto
    {
        public string BaseUrl { get; set; }

        public string AccessId { get; set; }

        public string SecrectKey { get; set; }

        public string Verb { get; set; }

        public string ContentType { get; set; }

        public string ParasStr { get; set; }

        public string apiStr { get; set; }

        public DateTimeOffset Date { get; set; }

        public long CurrVersion { get; set; }

        public string MapStr { get; set; }
    }

    public class ResultDto
    {
        public bool success { get; set; }
        public string result { get; set; }
        public string error { get; set; }
    }
}
