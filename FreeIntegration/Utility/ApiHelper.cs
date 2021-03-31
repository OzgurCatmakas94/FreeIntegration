using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace FreeIntegration.Utility
{
    public static class ApiHelper
    {
        public static async Task<string> PostAsync(string url, string headerToken, object contentObj)
        {
            try
            {
                //API Request
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                //httpWebRequest.Headers.Add("Authorization", "Bearer " + headerToken);
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = JsonConvert.SerializeObject(contentObj);
                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }
                var httpResponse =await Task.Run(()=> (HttpWebResponse)httpWebRequest.GetResponse()).ConfigureAwait(true);
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result1 = streamReader.ReadToEnd();

                }
                return httpResponse.StatusCode.ToString();
            }
            catch (Exception ex)
            {
                return "ERROR";
            }

        }
    }
}
