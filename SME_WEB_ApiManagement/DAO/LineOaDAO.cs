using Newtonsoft.Json;
using SME_WEB_ApiManagement.Models;
using System.Net;
using System.Reflection;

namespace SME_WEB_ApiManagement.DAO
{
    public class LineOaDAO
    {
        private readonly IConfiguration _configuration;
        protected static string APIpath;
        public LineOaDAO(IConfiguration configuration)
        {
            _configuration = configuration;

        }

        public static async Task<int> InsrtLineOaEmp(EmployeeLineOAModels lh, string apipath = null, string TokenStr = null)
        {
            int resultIns = 0;
            EmployeeLineOAModels ldata = new EmployeeLineOAModels();
            try
            {
                APIpath = apipath + "TEmployeeLineOa";
             
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(APIpath);
                //var refreshtoken = refreshToken();
                //  httpWebRequest.Headers.Add("Authorization", "Bearer " + TokenStr);

                httpWebRequest.ContentType = "application/json";

                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    var Req = lh;


                    var json = JsonConvert.SerializeObject(Req, Formatting.Indented);

                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                var response = (HttpWebResponse)httpWebRequest.GetResponse();

                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                    ldata = JsonConvert.DeserializeObject<EmployeeLineOAModels>(result);

                    if (ldata !=null)
                    {
                        resultIns = 1;
                    }

                }

                return resultIns;
            }
            catch (Exception ex)

            { throw new Exception("Error in CreatePopup: " + ex.Message); }
        }



    }
}
