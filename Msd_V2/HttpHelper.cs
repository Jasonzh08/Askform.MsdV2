using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Msd_V2
{
    public class HttpHelper
    {
        static Setting setting = new Setting();
        public static string HttpGet(string url,string accessToken)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Headers.Add("Authorization", setting.ConstString + " " + accessToken);
            request.Method = "GET";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);
            string content = reader.ReadToEnd();
            return content;
        }


        public static string HttpPost(string url, Dictionary<string, string> parameterData, string accessToken)
        { 
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Headers.Add("Authorization", setting.ConstString + " " + accessToken);
            request.Method = "POST";
            string postData = ParameterDataEncode(parameterData);
            byte[] bytes = Encoding.UTF8.GetBytes(postData);
            int length = bytes.Length;
            request.ContentLength = length;
            request.ContentType = "application/x-www-form-urlencoded";
            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(bytes, 0, bytes.Length);
            }
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);
            string content = reader.ReadToEnd();
            return content;
        }

        /// <summary>
        /// 字符串作为参数
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        public static string HttpPost(string url, string postData)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            //request.Headers.Add("Authorization", setting.ConstString + " " + setting.AccessToken);
            request.Method = "POST";
            byte[] bytes = Encoding.UTF8.GetBytes(postData);
            int length = bytes.Length;
            request.ContentLength = length;
            request.ContentType = "application/x-www-form-urlencoded";
            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(bytes, 0, bytes.Length);
            }
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);
            string content = reader.ReadToEnd();
            return content;
        }


        /// <summary> 
        /// 请求数据编码
        /// </summary>
        /// <param name="parameterData">请求参数字典</param>
        public static string ParameterDataEncode(Dictionary<string, string> parameterData)
        {
            StringBuilder builder = new StringBuilder(parameterData.Count * 128);
            foreach (var kv in parameterData)
            {
                builder.Append(kv.Key).Append("=").Append(HttpUtility.UrlEncode(kv.Value)).Append("&");
                //builder.Append(kv.Key).Append("=").Append(kv.Value).Append("&");
            }
            return builder.ToString(0, builder.Length - 1);
        }
    }
}
