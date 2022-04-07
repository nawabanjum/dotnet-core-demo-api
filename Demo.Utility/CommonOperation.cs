using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;


namespace Demo.Utility
{
    public static class CommonOperation
    {
        public static string Post(string url, Object data)
        {
            try
            {
                WebClient client = new WebClient();
                string jsonData = data != null ? JsonConvert.SerializeObject(data) : "";
                client.Headers.Add("content-type", "application/json");
               
                string response = client.UploadString(url, "POST", jsonData);
                return response;
            }
            catch (WebException ex)
            {
                String responseFromServer = ex.Message.ToString() + " ";
                if (ex.Response != null)
                {
                    using (WebResponse response = ex.Response)
                    {
                        Stream dataRs = response.GetResponseStream();
                        using (StreamReader reader = new StreamReader(dataRs))
                        {
                            responseFromServer += reader.ReadToEnd();
                        }
                    }
                }

                return responseFromServer;
            }
        }


        public static string AddData(string url, object data, string method = "POST")
        {


            //  url += AppConstant.SessionSignature + "=" + HttpContext.Current.Session[AppConstant.SessionSignature] + "&_=" + DateTime.Now.ToFileTime();

            using (var client = new WebClient())
            {
                string responseText = "";
                string jsonData;
                try
                {
                    client.Encoding = Encoding.UTF8;
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";

                    var obj = JsonConvert.SerializeObject(data);
                    // obj = "[" + obj + "]";

                    jsonData = client.UploadString(url, method, obj);
                    return jsonData;
                }
                catch (WebException exception)
                {

                    if (exception.Response != null)
                    {
                        var responseStream = exception.Response.GetResponseStream();

                        if (responseStream != null)
                        {
                            using (var reader = new StreamReader(responseStream))
                            {
                                responseText = reader.ReadToEnd();
                            }
                        }
                    }
                }
                return responseText;


            }
        }
        public static string GetData(string baseurl,string url,string method="POST")
        {
            using (var client = new WebClient())
            {

                try
                {
                    client.BaseAddress = baseurl;
                    var json = client.DownloadString(url);

                    //dynamic item = JsonConvert.DeserializeObject<object>(json);
                   
                    return json;

                }
                catch (Exception ex)
                {
                    throw;
                }
            }

        }

      
      }
}