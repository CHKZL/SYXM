using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HTTPget获取海康威视图像
{
    class Program
    {

        static string Nonce = "";
        static string cnonce = "fedc16f6d6f18f52";
        static string response = "";
        static void Main(string[] args)
        {
            
            while(true)
            {
                Console.Clear();
                Console.WriteLine("按任意键继续");
                Console.ReadKey();
                Console.WriteLine("当前Nonce值:" + Nonce + "\n当前cnonce值:" + cnonce + "\n当前response值:" + response);
                try
                {

                    string A1 = "Digest username=\"admin\",realm=\"DS-2CD2T45D-I8\",nonce=\"" + Nonce + "\",uri=\"/Streaming/channels/1/picture\",cnonce=\"" + cnonce + "\",nc=00000001,response=\"" + response + "\",qop=\"auth\"";


                    CreateGetHttpResponse("http://192.168.0.102/Streaming/channels/1/picture", 3000, DefaultUserAgent, A1);
                    Console.WriteLine("GET exit");
                    Console.WriteLine("按任意键继续");
                    Console.ReadKey();
                    //HttpGetMath("http://192.168.0.102/Streaming/channels/1/picture", "");
                }
                catch (WebException ex)
                {
                    //获取HTTP401报文内容
                    string AA = ex.Response.Headers.ToString();
                    AA = AA.Replace("\", nonce=\"", "#");
                    AA = AA.Replace("\", stale=", "#");
                    string[] get401ms = AA.Split('#');
                    string g401 = get401ms[1];
                    Console.WriteLine("nonce="+ g401);
                    //string g401 = "524551794e305647526a70684f574d32596a67794e773d3d";
                    
                    string A1 = GetMd5Str32("admin:DS-2CD2T45D-I8:admin12345"); ;
                    string A2 = GetMd5Str32("GET:/Streaming/channels/1/picture");
                    string HD = GetMd5Str32(A1 + ":" + g401 + ":00000001:fedc16f6d6f18f52:auth:" + A2);
                   

                    Nonce = g401;
                    response = HD;

                    string GET401 = "Digest username=\"admin\",realm=\"DS-2CD2T45D-I8\",nonce=\"" + Nonce + "\",uri=\"/Streaming/channels/1/picture\",cnonce=\"" + cnonce + "\",nc=00000001,response=\"" + response + "\",qop=\"auth\"";
                    try
                    {
                        CreateGetHttpResponse("http://192.168.0.102/Streaming/channels/1/picture", 3000, DefaultUserAgent, GET401);
                        Console.WriteLine("GET exit");
                        Console.WriteLine("按任意键继续");
                        Console.ReadKey();
                    }
                    catch (Exception eexx)
                    {

                        Console.WriteLine(eexx.ToString());
                        Console.ReadKey();
                    }

                    
                    //label1.Text = ex.ToString();
                }
            }
           


        }

        private static string GetMd5Str32(string text)
        {
            try
            {
                using (MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider())
                {
                    byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(text));
                    StringBuilder sBuilder = new StringBuilder();
                    for (int i = 0; i < data.Length; i++)
                    {
                        sBuilder.Append(data[i].ToString("x2"));
                    }
                    return sBuilder.ToString().ToLower();
                }
            }
            catch { return null; }
        }



        private static readonly string DefaultUserAgent = @"Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; Touch; rv:11.0) like Gecko";
        public static HttpWebResponse CreateGetHttpResponse(string url, int? timeout, string userAgent, string A)//, CookieCollection cookies)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "GET";
            request.UserAgent = DefaultUserAgent;
            if (!string.IsNullOrEmpty(userAgent))
            {
                request.UserAgent = userAgent;
            }
            if (timeout.HasValue)
            {
                request.Timeout = timeout.Value;
            }

            request.PreAuthenticate = true;
            request.Headers.Add(HttpRequestHeader.Authorization, A);
            //if (cookies != null)
            {
                //request.CookieContainer = new CookieContainer();
                //request.CookieContainer.Add(cookies);
            }
            return request.GetResponse() as HttpWebResponse;
        }

    }
}
