using System;
using System.Security.Cryptography;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using apiTestPro.Class;
using System.Configuration;

namespace apiTestPro
{

    public class YLHttpHelperMc
    {
        public static string _BaseUrl = ConfigurationSettings.AppSettings["BaseUrl"];
        public HttpClient _httpClient { get; set; }
        public dynamic DynamicParas { get; set; }
        public dynamic CommonConf { get; set; }
        public dynamic Request { get; set; }
        public dynamic ConstParams { get; set; }
        public dynamic OrgInfo { get; set; }

        public YLHttpHelperMc(dynamic obj)
        {
            string BaseUrl = _BaseUrl;
            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromMinutes(10);
            _httpClient.BaseAddress = new Uri(BaseUrl);
        }

        public string HttpPost(McHttpDto dto,string JsonStr)
        {
            DateTimeOffset currDate = DateTimeOffset.Now;
            dto.Date = currDate;

            ConstructParas(dto);
            string signature = ToBase64(dto);

            _httpClient.DefaultRequestHeaders.Date = currDate;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
               "IWOP", dto.AccessId + ":" + signature);

            string json = JsonStr;
            var input = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
            string result= _httpClient.PostAsJsonAsync(dto.ParasStr, input).Result.Content.ReadAsStringAsync().Result;
            return result;
        }

        private McHttpDto ConstructParas(McHttpDto dto)
        {
            dto.ParasStr = dto.apiStr.Replace("@version@", dto.CurrVersion.ToString());
            return dto;
        }

        private string ToBase64(McHttpDto dto)
        {
           string text = dto.Verb + "\n" + dto.ContentType + "\n" + dto.Date.ToString("r") + "\n" + dto.BaseUrl + dto.ParasStr;
            HMACSHA1 myHMACSHA1 = new HMACSHA1(Encoding.UTF8.GetBytes(dto.SecrectKey));
            byte[] byteText = myHMACSHA1.ComputeHash(Encoding.UTF8.GetBytes(text));
            return System.Convert.ToBase64String(byteText);
        }

    }
}
