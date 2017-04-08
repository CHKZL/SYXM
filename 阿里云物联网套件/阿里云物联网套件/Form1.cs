using System;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Windows.Forms;
using Aliyun.Acs.Iot;
using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Profile;
using Aliyun.Acs.Iot.Model.V20160530;
using System.Collections.Generic;

namespace 阿里云物联网套件
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            
            InitializeComponent();
        }

        string Format = "XML";
        string Version = "2016-05-30";
        string SignatureMethod = "HMAC-SHA1";
        string SignatureNonce = "";
        string SignatureVersion = "1.0";
        string AccessKeyId = "LTAIPuIjzVlI5M5C";
        string Timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"); 
        string RegionId = "cn-hangzhou";
        string Action = "GetCats";
        string ProductKey = "1000102539";
        string DeviceName = "4GGG";
        string RpcContent = "OK";
        string TimeOut = "5000";
        string accessKey = "LTAIPuIjzVlI5M5C";
        string accessSecret = "dDzgBEk3TvEEetiyFf2TRf797xuHlp";
        string RootId = "0";

        /// <summary>
        /// 获取Url编码,小写
        /// </summary>
        /// <param name="str">需要编码的Url</param>
        /// <returns></returns>
        public string UrlEncodeXX(string str)
        {
            StringBuilder builder = new StringBuilder();
            foreach (char c in str)
            {
                if (HttpUtility.UrlEncode(c.ToString()).Length > 1)
                {
                    builder.Append(HttpUtility.UrlEncode(c.ToString()));
                }
                else
                {
                    builder.Append(c);
                }
            }
            return builder.ToString();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Random R = new Random();
            SignatureNonce = Guid.NewGuid().ToString(); ;
            try
            {
                Timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");
                //string U = UrlEncode("Format") + "=" + UrlEncode(Format)+"&"+ UrlEncode("Version") + "=" + UrlEncode(Version) +"&" + UrlEncode("AccessKeyId") + "=" + UrlEncode(AccessKeyId) + "&" + UrlEncode("SignatureMethod") + "=" + UrlEncode(SignatureMethod) + "&" + UrlEncode("Timestamp") + "=" + UrlEncode(Timestamp) + "&" + UrlEncode("SignatureVersion") + "=" + UrlEncode(SignatureVersion) + "&" + UrlEncode("SignatureNonce") + "=" + UrlEncode(SignatureNonce) + "&" + UrlEncode("RegionId") + "=" + UrlEncode(RegionId) + "&" + UrlEncode("Action") + "=" + UrlEncode(Action) + "&" + UrlEncode("ProductKey") + "=" + UrlEncode(ProductKey) + "&" + UrlEncode("DeviceName") + "=" + UrlEncode(DeviceName) + "&" + UrlEncode("RpcContent") + "=" + UrlEncode(RpcContent) + "&" + UrlEncode("TimeOut") + "=" + UrlEncode(TimeOut);

                //string U = UrlEncode("AccessKeyId=" + AccessKeyId + "&Action=" + Action + "&Format=" + Format + "&RegionId=" + RegionId + "&ProductKey=" + ProductKey + "&DeviceName=" + DeviceName + "&RpcContent=" + RpcContent + "&TimeOut=" + TimeOut + "&SignatureMethod=" + SignatureMethod + "&SignatureNonce=" + SignatureNonce + "&SignatureVersion=" + SignatureVersion + "&Timestamp=" + Timestamp + "&Version=" + Version);
                
                string U = UrlEncode("AccessKeyId=" + AccessKeyId + "&Action=" + Action + "&Format=" + Format + "&RegionId=" + RegionId + "&RootId=" + RootId + "&SignatureMethod=" + SignatureMethod + "&SignatureNonce=" + SignatureNonce + "&SignatureVersion=" + SignatureVersion + "&Timestamp=" + UrlEncode(Timestamp) + "&Version=" + Version);
                //string U = UrlEncode("AccessKeyId=" + AccessKeyId + "&Action=" + Action + "&Format=" + Format + "&RegionId=" + RegionId + "&ProductKey=" + ProductKey + "&DeviceName=" + DeviceName + "&RpcContent=" + Base64(RpcContent) + "&TimeOut=" + TimeOut + "&SignatureMethod=" + SignatureMethod + "&SignatureNonce=" + SignatureNonce + "&SignatureVersion=" + SignatureVersion + "&Timestamp=" + UrlEncode(Timestamp) + "&Version=" + Version);
                string UU = "GET&%2F&" + U;
                string UUU = HMACSHA1(UU, accessSecret+"&");
                //UrlEncode
                //string OUTU = "Version="+ Version+ "&Action="+ Action + "&Format="+ Format+ "&ProductKey="+ ProductKey+ "&DeviceName=" + UrlEncode(DeviceName) + "&RpcContent="+UrlEncode(Base64(RpcContent))+ "&TimeOut=" + UrlEncode(TimeOut) + "&Timestamp=" + UrlEncode(Timestamp) + "&SignatureMethod=" + UrlEncode(SignatureMethod) + "&SignatureVersion=" + UrlEncode(SignatureVersion) + "&SignatureNonce=" + UrlEncode(SignatureNonce) + "&AccessKeyId=" + UrlEncode(AccessKeyId) + "&RegionId="+UrlEncode(RegionId)+ "&Signature="+UrlEncode(UUU);
                string OUTU = "Version=" + Version + "&Action=" + Action + "&Format=" + Format + "&RootId=" + UrlEncodeXX(RootId) + "&Timestamp=" + UrlEncodeXX(Timestamp) + "&SignatureMethod=" + UrlEncodeXX(SignatureMethod) + "&SignatureVersion=" + UrlEncodeXX(SignatureVersion) + "&SignatureNonce=" + UrlEncodeXX(SignatureNonce) + "&AccessKeyId=" + UrlEncodeXX(AccessKeyId) + "&RegionId=" + UrlEncode(RegionId) + "&Signature=" + UrlEncodeXX(UUU);
                //string OUTU = "Version=" + Version + "&Action=" + Action + "&Format=" + Format + "&ProductKey=" + UrlEncodeXX(ProductKey) + "&DeviceName=" + UrlEncodeXX(DeviceName) + "&RpcContent=" + UrlEncodeXX(Base64(RpcContent)) + "&TimeOut=" + UrlEncodeXX(TimeOut) + "&Timestamp=" + UrlEncodeXX(Timestamp) + "&SignatureMethod=" + UrlEncodeXX(SignatureMethod) + "&SignatureVersion=" + UrlEncodeXX(SignatureVersion) + "&SignatureNonce=" + UrlEncodeXX(SignatureNonce) + "&AccessKeyId=" + UrlEncodeXX(AccessKeyId) + "&RegionId=" + UrlEncode(RegionId) + "&Signature=" + UrlEncodeXX(UUU);
                ///string UUUU = Base64(UUU);
                /// string OUTU = U + "&" + UrlEncode("Signature") + "=" + UrlEncode(UUUU);
                // label1.Text = OUTU;
                CreateGetHttpResponse("http://iot.aliyuncs.com/?" + OUTU, 3000, DefaultUserAgent);
            }
            catch (Exception ex)
            {
                label1.Text = ex.ToString();
            } 
        }

        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="str">需要加密的字符串</param>
        /// <returns></returns>
        public string Base64(string str)
        {
            return System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(str));
        }

        ///// <summary>
        ///// 进行MHAC-SHA1加密
        ///// </summary>
        ///// <param name="str">需要加密的字符串</param>
        ///// <param name="key">加密密钥</param>
        ///// <returns></returns>
        //public string HMACSHA1(string str, string key)
        //{
        //    HMACSHA1 hmacsha1 = new HMACSHA1();
        //    hmacsha1.Key = Encoding.UTF8.GetBytes(key);
        //    byte[] dataBuffer = Encoding.UTF8.GetBytes(str);
        //    byte[] hashBytes = hmacsha1.ComputeHash(dataBuffer);
        //    return Convert.ToBase64String(hashBytes);
        //}


        /// <summary>
        /// 获取Url编码,大写
        /// </summary>
        /// <param name="str">需要编码的Url</param>
        /// <returns></returns>
        public string UrlEncode(string str)
        {
            StringBuilder builder = new StringBuilder();
            foreach (char c in str)
            {
                if (HttpUtility.UrlEncode(c.ToString()).Length > 1)
                {
                    builder.Append(HttpUtility.UrlEncode(c.ToString()).ToUpper());
                }
                else
                {
                    builder.Append(c);
                }
            }
            return builder.ToString();
        }

        private static readonly string DefaultUserAgent = @"Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; Touch; rv:11.0) like Gecko";
        public static HttpWebResponse CreateGetHttpResponse(string url, int? timeout, string userAgent)//, string A)//, CookieCollection cookies)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "GET";
            //request.UserAgent = DefaultUserAgent;
            if (!string.IsNullOrEmpty(userAgent))
            {
                request.UserAgent = userAgent;
            }
            if (timeout.HasValue)
            {
                request.Timeout = timeout.Value;
            }

            //request.PreAuthenticate = true;
            //request.Headers.Add(HttpRequestHeader.Authorization, A);
            //if (cookies != null)
            {
                //request.CookieContainer = new CookieContainer();
                //request.CookieContainer.Add(cookies);
            }
            return request.GetResponse() as HttpWebResponse;
        }

        /// <summary>
        /// 进行MHAC-SHA1加密
        /// </summary>
        /// <param source="str">需要加密的字符串</param>
        /// <param accessSecret="key">加密密钥</param>
        /// <returns></returns>
        public static string HMACSHA1(String source, String accessSecret)
        {
            using (var algorithm = KeyedHashAlgorithm.Create("HMACSHA1"))
            {
                algorithm.Key = Encoding.UTF8.GetBytes(accessSecret.ToCharArray());
                return Convert.ToBase64String(algorithm.ComputeHash(Encoding.UTF8.GetBytes(source.ToCharArray())));
            }
        }


        /// <summary>
        /// HMAC-SHA1
        /// </summary>
        /// <param name="secret">密钥</param>
        /// <param name="strOrgData">源文</param>
        /// <returns></returns>
        public static string HmacSha1Sign( string strOrgData, string secret)
        {
            HMACSHA1 hmacsha1 = new HMACSHA1();
            hmacsha1.Key = Encoding.UTF8.GetBytes(secret);
            byte[] dataBuffer = Encoding.UTF8.GetBytes(strOrgData);
            byte[] hashBytes = hmacsha1.ComputeHash(dataBuffer);
            return Convert.ToBase64String(hashBytes);
        }
        private void button2_Click(object sender, EventArgs e)
        {

            //string U = UrlEncode("Format=XML&SignatureMethod=HMAC-SHA1&Topic.1=%2F60027911%2Ftopic1&Timestamp=2016-05-05T03%3A03%3A28Z&Action=Sub&AccessKeyId=testId&SubCallback=http%3A%2F%2Flocalhost%3A18080%2Fmock%2Fconsumer&RegionId=cn-hangzhou&SignatureNonce=947519ce-68ee-4546-8508-69e0338d3568&AppKey=123&Version=2016-01-04&SignatureVersion=1.0").Replace("%3D","=").Replace("%26","&");

            //string UU = "GET&%2F&AccessKeyId%3DtestId&Action%3DSub&AppKey%3D123&" + U;
            //string UUU = HMACSHA1(UU, "test&");
            //string UUUU = Base64(UUU);
            ////string OUTU = "Format=" + Format + "&Version=" + Version + "&SignatureMethod=" + SignatureMethod + "&SignatureNonce=" + SignatureNonce + "&SignatureVersion=" + SignatureVersion + "&AccessKeyId=" + AccessKeyId + "&Timestamp=" + Timestamp + "&RegionId=" + RegionId+ "&Signature="+UrlEncode(UUUU);
            //string OUTU = U + "&" + UrlEncode("Signature") + "=" + UrlEncode(UUUU);
            //label1.Text = UUUU;
            //label1.Text = Base64(HMACSHA1("GET&%2F&AccessKeyId%3DtestId&Action%3DSub&AppKey%3D123&Format%3DXML&RegionId%3Dcn-hangzhou&SignatureMethod%3DHMAC-SHA1&SignatureNonce%3D085c2857-4cbf-4fe2-82ca-2ac00062c7d4&SignatureVersion%3D1.0&SubCallback%3Dhttp%253A%252F%252Flocalhost%253A18080%252Fmock%252Fconsumer&Timestamp%3D2016-05-05T03%253A02%253A18Z&Topic.1%3D%252F60027911%252Ftopic1&Version%3D2016-01-04", "test&"));
            label1.Text = HMACSHA1("GET&%2F&AccessKeyId%3DLTAIPuIjzVlI5M5C%26Action%3DRevertRpc%26DeviceName%3Dtest1%26Format%3DXML%26ProductKey%3D1000102539%26RegionId%3Dcn-hangzhou%26RpcContent%3DQVNE%26SignatureMethod%3DHMAC-SHA1%26SignatureNonce%3Dd166b671-46a1-48fa-bad5-28c76f56397d%26SignatureVersion%3D1.0%26TimeOut%3D5000%26Timestamp%3D2017-03-13T07%253A12%253A43Z%26Version%3D2016-05-30", accessSecret+"&");
            //vBz5BwUdebR0lGtrLySmjRv/izs=


        }


        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                IClientProfile profile = DefaultProfile.GetProfile("cn-hangzhou", accessKey, accessSecret);
                DefaultAcsClient client = new DefaultAcsClient(profile);//初始化SDK客户端
                QueryDeviceByNameRequest request = new QueryDeviceByNameRequest();
                request.ProductKey= ProductKey;
                request.DeviceName= DeviceName;
                QueryDeviceByNameResponse response = null;
                try
                {
                    var resp = client.GetAcsResponse(request);//.AcsResponse(req);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                if (response != null)
                {
                    MessageBox.Show("Response requestId:" + response.RequestId + " isSuccess:" + response.Success + " Error:" + response.ErrorMessage);
                }
            }
            catch (Exception ex)
            {

                label1.Text = ex.ToString();
            }
          
        }

       

        private void button4_Click(object sender, EventArgs e)
        {

            IClientProfile profile = DefaultProfile.GetProfile("cn-hangzhou", accessKey, accessSecret);
             DefaultAcsClient client = new DefaultAcsClient(profile);//初始化SDK客户端

            RevertRpcRequest rpcRequest = new RevertRpcRequest();
            rpcRequest.DeviceName = "test1";//设备接入时候得到ID
            rpcRequest.ProductKey = long.Parse(ProductKey);//设备接入时候填写的ProductKey
            rpcRequest.TimeOut = 5000; //超时时间，单位毫秒.如果超过这个时间设备没反应则返回"TIMEOUT"
            rpcRequest.RpcContent = "QVNE";//推送给设备的数据.数据要求二进制数据做一次BASE64编码.(示例里面是"helloworld"编码后的值)
            RevertRpcResponse rpcResponse = client.GetAcsResponse(rpcRequest);
            MessageBox.Show(rpcResponse.ResponseContent);//得到设备返回的数据信息.
            MessageBox.Show(rpcResponse.RpcCode);//对应的响应码( TIMEOUT/SUCCESS/OFFLINE等)












            //BatchGetDeviceStateRequest request = new BatchGetDeviceStateRequest();
            //request.ProductKey = ProductKey;
            //List<string> devices = new List<string>();
            //devices.Add(DeviceName);
            //request.DeviceNames.Add(DeviceName);
            //BatchGetDeviceStateResponse response = null;
            //try
            //{
            //    resp = client.getAcsResponse(req);

            //}
            //catch (Exception ex)
            //{
            //    //ex.printStackTrace();
            //    MessageBox.Show(ex.ToString());
            //}
            //if (response != null)
            //{
            //    MessageBox.Show("Response requestId:" + response.RequestId + " isSuccess:" + response.Success + " Error:" + response.ErrorMessage);
            //}
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            Random R = new Random();
            SignatureNonce = Guid.NewGuid().ToString(); ;
            try
            {
                Timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");
                string U = UrlEncode("AccessKeyId=" + AccessKeyId + "&Action=" + "RevertRpc" + "&DeviceName=" + "test" + "&Format=" + Format + "&ProductKey=" + ProductKey + "&RegionId=" + RegionId + "&RpcContent=" + "QVNE" + "&SignatureMethod=" + SignatureMethod + "&SignatureNonce=" + SignatureNonce + "&SignatureVersion=" + SignatureVersion + "&TimeOut=" + TimeOut + "&Timestamp=" + UrlEncode(Timestamp) + "&Version=" + Version);
                string UU = "GET&%2F&" + U;
                string UUU = HMACSHA1(UU, accessSecret + "&");
                string OUTU = "Version=" + Version + "&Action=" + "RevertRpc" + "&Format=" + Format + "&DeviceName=" + UrlEncodeXX("test") + "&ProductKey=" + UrlEncodeXX(ProductKey) + "&TimeOut=" + UrlEncodeXX(TimeOut) + "&RpcContent=" + "QVNE" + "&Timestamp=" + UrlEncodeXX(Timestamp) + "&SignatureMethod=" + UrlEncodeXX(SignatureMethod) + "&SignatureVersion=" + UrlEncodeXX(SignatureVersion) + "&SignatureNonce=" + UrlEncodeXX(SignatureNonce) + "&AccessKeyId=" + UrlEncodeXX(AccessKeyId) + "&RegionId=" + UrlEncodeXX(RegionId) + "&Signature=" + UrlEncodeXX(UUU);

                CreateGetHttpResponse("http://iot.aliyuncs.com/?" + OUTU, 3000, DefaultUserAgent);
            }
            catch (Exception ex)
            {
                label1.Text = ex.ToString();
            }
        }
    }
}
