using Newtonsoft.Json;
using SME_WEB_ApiManagement.Models;
using System.Net;

namespace SME_WEB_ApiManagement.DAO
{

    public class ErrorApiDAO
    {
        private readonly IConfiguration _configuration;
        protected static string APIpath;

        public ErrorApiDAO(IConfiguration configuration)
        {
            _configuration = configuration;

        }

        public static ViewErroApiModels GetErrorApiBySearch(TErrorApiLogModels vm, string apipath = null, string flagCount = null, int currentpage = 0, int PageSize = 0, string TokenStr = null)
        {

            ViewErroApiModels vme = new ViewErroApiModels();
            try
            {
                APIpath = apipath + "TErrorApiLog/GetErrorApi";
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(APIpath);
                httpWebRequest.ContentType = "application/json";
                //  httpWebRequest.Headers.Add("Authorization", "Bearer " + TokenStr);
                httpWebRequest.Method = "POST";
                List<TErrorApiLogModels> Llist = new List<TErrorApiLogModels>();
                try
                {
                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        TErrorApiLogModels Req; // กำหนด Type ชัดเจน

                        if (vm != null)
                        {
                            Req = vm;
                        }
                        else
                        {
                            Req = new TErrorApiLogModels();
                        }

                        if (flagCount != "Y")
                        {
                            Req.rowOFFSet = (currentpage - 1) * PageSize;
                            Req.rowFetch = PageSize;
                        }
                        else
                        {
                            Req.rowOFFSet = 0;
                            Req.rowFetch = 0;

                        }
                        var json = JsonConvert.SerializeObject(Req, Formatting.Indented);


                        streamWriter.Write(json);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }
                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {

                        var result = streamReader.ReadToEnd();
                        vme = JsonConvert.DeserializeObject<ViewErroApiModels>(result);
                        return vme;

                    }

                }
                catch (Exception ex)
                {
                    return new ViewErroApiModels();
                }
            }
            catch (Exception ex) { return null; }


        }
        public static int? UpsertSystem(MSystemModels vm = null, string apipath = null, string TokenStr = null)
        {


            APIpath = apipath + "MSystem/UpsertSystem";
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(APIpath);
            httpWebRequest.ContentType = "application/json";
            //  httpWebRequest.Headers.Add("Authorization", "Bearer " + TokenStr);
            httpWebRequest.Method = "POST";
            int? Llist = 0;
            try
            {
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    var Req = vm;


                    var json = JsonConvert.SerializeObject(Req, Formatting.Indented);

                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {

                    var result = streamReader.ReadToEnd();
                    Llist = JsonConvert.DeserializeObject<int?>(result);
                    return Llist;

                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static string? DeleteSystem(string id = null, string apipath = null, string TokenStr = null)
        {


            APIpath = apipath + "MSystem/" + id;
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(APIpath);
            httpWebRequest.ContentType = "application/json";
            //  httpWebRequest.Headers.Add("Authorization", "Bearer " + TokenStr);
            httpWebRequest.Method = "DELETE";

            try
            {
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {

                    var result = streamReader.ReadToEnd();
                    string Llist = JsonConvert.DeserializeObject<string>(result);
                    return Llist;

                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static int? UpsertOrg(MOrganizationModels vm = null, string apipath = null, string TokenStr = null)
        {


            APIpath = apipath + "MOrganization/UpsertOrg";
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(APIpath);
            httpWebRequest.ContentType = "application/json";
            //  httpWebRequest.Headers.Add("Authorization", "Bearer " + TokenStr);
            httpWebRequest.Method = "POST";
            int? Llist = 0;
            try
            {
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    var Req = vm;


                    var json = JsonConvert.SerializeObject(Req, Formatting.Indented);

                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {

                    var result = streamReader.ReadToEnd();
                    Llist = JsonConvert.DeserializeObject<int?>(result);
                    return Llist;

                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static List<MOrganizationModels> GetOrgBySeach(MOrganizationModels vm, string apipath = null, string flagCount = null, int currentpage = 0, int PageSize = 0, string TokenStr = null)
        {


            APIpath = apipath + "MOrganization/GetOrgBySeach";
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(APIpath);
            httpWebRequest.ContentType = "application/json";
            //  httpWebRequest.Headers.Add("Authorization", "Bearer " + TokenStr);
            httpWebRequest.Method = "POST";
            List<MOrganizationModels> Llist = new List<MOrganizationModels>();
            try
            {
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    MOrganizationModels Req; // กำหนด Type ชัดเจน
                    if (vm != null)
                    {
                        Req = vm;
                    }
                    else
                    {
                        Req = new MOrganizationModels();
                    }
                    if (flagCount != "Y")
                    {
                        Req.rowOFFSet = (currentpage - 1) * PageSize;
                        Req.rowFetch = PageSize;
                    }
                    else
                    {
                        Req.rowOFFSet = 0;
                        Req.rowFetch = 0;

                    }
                    var json = JsonConvert.SerializeObject(Req, Formatting.Indented);


                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {

                    var result = streamReader.ReadToEnd();
                    Llist = JsonConvert.DeserializeObject<List<MOrganizationModels>>(result);
                    return Llist;

                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static string? DeleteOrg(string id = null, string apipath = null, string TokenStr = null)
        {


            APIpath = apipath + "MOrganization/" + id;
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(APIpath);
            httpWebRequest.ContentType = "application/json";
            //  httpWebRequest.Headers.Add("Authorization", "Bearer " + TokenStr);
            httpWebRequest.Method = "DELETE";

            try
            {
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {

                    var result = streamReader.ReadToEnd();
                    string Llist = JsonConvert.DeserializeObject<string>(result);
                    return Llist;

                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
