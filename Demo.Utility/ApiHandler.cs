
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Demo.Models
{
    public class ApiHandler
    {
        public static ProjectModel GetProjects()
        {


            try
            {
                using (WebClient webClient = new WebClient())
                {
                    webClient.BaseAddress = "https://media.castingworkbook.com";
                    var json = webClient.DownloadString("/playmedia/upwork-backend.json");
                    var list = JsonConvert.DeserializeObject<ProjectModel>(json);
                    return list;
                }
            }
            catch (WebException ex)
            {
                throw ex;
            }
          
        }
    }
}
