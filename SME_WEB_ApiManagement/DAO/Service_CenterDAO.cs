using Newtonsoft.Json;
using SME_WEB_ApiManagement.Models;
using System.Data;
using System.Globalization;
using System.Net;
using System.Net.Mail;

namespace SME_WEB_ApiManagement.DAO
{
    public class Service_CenterDAO
    {
        private readonly IConfiguration _configuration;
        protected static string APIpath;

        public Service_CenterDAO(IConfiguration configuration)
        {
            _configuration = configuration;

        }
        public static vDropdownDTO GetLookups(string Ltype = null, string apipath = null, string TokenStr = null)
        {


            APIpath = apipath + "Dropdown/LookUp/" + Ltype;
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(APIpath);
            httpWebRequest.ContentType = "application/json";
          //  httpWebRequest.Headers.Add("Authorization", "Bearer " + TokenStr);
            httpWebRequest.Method = "GET";
            vDropdownDTO Llist = new vDropdownDTO();
            try
            {
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {

                    var result = streamReader.ReadToEnd();
                    Llist = JsonConvert.DeserializeObject<vDropdownDTO>(result);
                    return Llist;

                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static vDropdownDTO GetDropdownOrganization( string apipath = null, string TokenStr = null)
        {


            APIpath = apipath + "Dropdown/GetDropdownOrganization";
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(APIpath);
            httpWebRequest.ContentType = "application/json";
          //  httpWebRequest.Headers.Add("Authorization", "Bearer " + TokenStr);
            httpWebRequest.Method = "GET";
            vDropdownDTO Llist = new vDropdownDTO();
            try
            {
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {

                    var result = streamReader.ReadToEnd();
                    Llist = JsonConvert.DeserializeObject<vDropdownDTO>(result);
                    return Llist;

                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static vDropdownDTO GetDropdownOrganizationWithOutData(List<MRegisterModels> Models,string apipath = null, string TokenStr = null)
        {
            var xdata = new List<MOrganizationModels>();
            if (Models.Count > 0)
            {
                foreach (var item in Models)
                {
                    MOrganizationModels x = new MOrganizationModels();

                    x.OrganizationCode = item.OrganizationCode;

                    xdata.Add(x);
                }
            }
            try
            {
                APIpath = apipath + "Dropdown/GetDropdownOrganizationWithOutData";
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(APIpath);
                httpWebRequest.ContentType = "application/json";
                //  httpWebRequest.Headers.Add("Authorization", "Bearer " + TokenStr);
                httpWebRequest.Method = "POST";
                vDropdownDTO Llist = new vDropdownDTO();
                try
                {
                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        var Req = new List<MOrganizationModels>(); // Correctly initialize as a list

                        if (xdata != null)
                        {
                            Req = xdata; // Assign the list directly
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
                        Llist = JsonConvert.DeserializeObject<vDropdownDTO>(result);
                        return Llist;
                    }

                }
                catch (Exception ex)
                {
                    return new vDropdownDTO();
                }
            }
            catch (Exception ex) { return null; }
       
        }
        public static vDropdownDTO GetDropdownSystem(string apipath = null, string TokenStr = null)
        {


            APIpath = apipath + "Dropdown/GetDropdownSystem";
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(APIpath);
            httpWebRequest.ContentType = "application/json";
         //   httpWebRequest.Headers.Add("Authorization", "Bearer " + TokenStr);
            httpWebRequest.Method = "GET";
            vDropdownDTO Llist = new vDropdownDTO();
            try
            {
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {

                    var result = streamReader.ReadToEnd();
                    Llist = JsonConvert.DeserializeObject<vDropdownDTO>(result);
                    return Llist;

                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static string ThaiBahtText(string strNumber, bool IsTrillion = false)
        {
            string BahtText = "";
            string strTrillion = "";
            string[] strThaiNumber = { "ศูนย์", "หนึ่ง", "สอง", "สาม", "สี่", "ห้า", "หก", "เจ็ด", "แปด", "เก้า", "สิบ" };
            string[] strThaiPos = { "", "สิบ", "ร้อย", "พัน", "หมื่น", "แสน", "ล้าน" };

            decimal decNumber = 0;
            decimal.TryParse(strNumber, out decNumber);

            if (decNumber == 0)
            {
                return "ศูนย์บาทถ้วน";
            }

            strNumber = decNumber.ToString("0.00");
            string strInteger = strNumber.Split('.')[0];
            string strSatang = strNumber.Split('.')[1];

            if (strInteger.Length > 13)
                throw new Exception("รองรับตัวเลขได้เพียง ล้านล้าน เท่านั้น!");

            bool _IsTrillion = strInteger.Length > 7;
            if (_IsTrillion)
            {
                strTrillion = strInteger.Substring(0, strInteger.Length - 6);
                BahtText = ThaiBahtText(strTrillion, _IsTrillion);
                strInteger = strInteger.Substring(strTrillion.Length);
            }

            int strLength = strInteger.Length;
            for (int i = 0; i < strInteger.Length; i++)
            {
                string number = strInteger.Substring(i, 1);
                if (number != "0")
                {
                    if (i == strLength - 1 && number == "1" && strLength != 1)
                    {
                        BahtText += "เอ็ด";
                    }
                    else if (i == strLength - 2 && number == "2" && strLength != 1)
                    {
                        BahtText += "ยี่";
                    }
                    else if (i != strLength - 2 || number != "1")
                    {
                        BahtText += strThaiNumber[int.Parse(number)];
                    }

                    BahtText += strThaiPos[(strLength - i) - 1];
                }
            }

            if (IsTrillion)
            {
                return BahtText + "ล้าน";
            }

            if (strInteger != "0")
            {
                BahtText += "บาท";
            }

            if (strSatang == "00")
            {
                BahtText += "ถ้วน";
            }
            else
            {
                strLength = strSatang.Length;
                for (int i = 0; i < strSatang.Length; i++)
                {
                    string number = strSatang.Substring(i, 1);
                    if (number != "0")
                    {
                        if (i == strLength - 1 && number == "1" && strSatang[0].ToString() != "0")
                        {
                            BahtText += "เอ็ด";
                        }
                        else if (i == strLength - 2 && number == "2" && strSatang[0].ToString() != "0")
                        {
                            BahtText += "ยี่";
                        }
                        else if (i != strLength - 2 || number != "1")
                        {
                            BahtText += strThaiNumber[int.Parse(number)];
                        }

                        BahtText += strThaiPos[(strLength - i) - 1];
                    }
                }

                BahtText += "สตางค์";
            }

            return BahtText;
        }

        public static PagingModel LoadPagingViewModel(int rowcount, int? currentpage, int PageSize)
        {
            PagingModel page = new PagingModel();

            double dPageSize = PageSize;
            double totalRow;
            totalRow = rowcount;

            page.TotalPage = Math.Ceiling(totalRow / dPageSize); // แสดงค่าหน้าทั้งหมด
            page.CurrentPageNumber = (currentpage == 0) ? 1 : currentpage;
            page.PageSize = PageSize;
            page.TotalRows = rowcount;

            return page;
        }

        public static MMailModels GetMailAll(string apipath = null, string TokenStr = null, string code = null)
        {

            APIpath = apipath + "ServiceCommon/GetMailAll/" + code;
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(APIpath);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Headers.Add("Authorization", "Bearer " + TokenStr);
            httpWebRequest.Method = "GET";
            MMailModels Llist = new MMailModels();
            try
            {
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {

                    var result = streamReader.ReadToEnd();
                    Llist = JsonConvert.DeserializeObject<MMailModels>(result);
                    return Llist;

                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static bool sendmail(MMailModels GetBodymail, string toEmail, MailSettingModels xmailSetting)
        {
            try
            {
                using (MailMessage mail = new MailMessage())
                {

                    mail.From = new MailAddress(xmailSetting.EmailAdminfrom, xmailSetting.EmailAdminfromName);

                    mail.To.Add(toEmail);
                    mail.CC.Add(toEmail);
                    mail.Subject = GetBodymail.MailSubject;
                    mail.Body = GetBodymail.MailDetail;
                    mail.IsBodyHtml = true;

                    using (SmtpClient smtp = new SmtpClient(xmailSetting.Host, int.Parse(xmailSetting.Port)))
                    {
                        smtp.Credentials = new NetworkCredential(xmailSetting.EmailAdminfrom, xmailSetting.EmailPassword);
                        smtp.EnableSsl = true;
                        smtp.Send(mail);

                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }


        //public static int InsertImportFile(string apipath, string tokenStr, TFileUploadModels models)
        //{
        //    int ret = 0;
        //    APIpath = apipath + "ImportFile/CreateImportFile";

        //    HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(APIpath);
        //    ////var refreshtoken = refreshToken();
        //    httpWebRequest.Headers.Add("Authorization", "Bearer " + tokenStr);

        //    httpWebRequest.ContentType = "application/json";

        //    httpWebRequest.Method = "POST";

        //    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
        //    {
        //        var json = JsonConvert.SerializeObject(models, Formatting.Indented);

        //        streamWriter.Write(json);
        //        streamWriter.Flush();
        //        streamWriter.Close();
        //    }

        //    var response = (HttpWebResponse)httpWebRequest.GetResponse();

        //    using (var streamReader = new StreamReader(response.GetResponseStream()))
        //    {
        //        var result = streamReader.ReadToEnd();
        //        ret = JsonConvert.DeserializeObject<int>(result);


        //    }
        //    return ret;
        //}
        //public static List<TFileUploadModels> CheckImportFile(TFileUploadModels models, string apipath = null, string TokenStr = null)
        //{
        //    List<TFileUploadModels> ret = new List<TFileUploadModels>();

        //    APIpath = apipath + "ImportFile/GetChexkFileImport";

        //    HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(APIpath);
        //    ////var refreshtoken = refreshToken();
        //    httpWebRequest.Headers.Add("Authorization", "Bearer " + TokenStr);

        //    httpWebRequest.ContentType = "application/json";

        //    httpWebRequest.Method = "POST";

        //    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
        //    {
        //        var json = JsonConvert.SerializeObject(models, Formatting.Indented);

        //        streamWriter.Write(json);
        //        streamWriter.Flush();
        //        streamWriter.Close();
        //    }

        //    var response = (HttpWebResponse)httpWebRequest.GetResponse();

        //    using (var streamReader = new StreamReader(response.GetResponseStream()))
        //    {
        //        var result = streamReader.ReadToEnd();
        //        ret = JsonConvert.DeserializeObject<List<TFileUploadModels>>(result);


        //    }
        //    return ret;
        //}


        //public static List<TFileUploadModels> GetFileImportAll(string apipath, string tokenStr, TFileUploadModels models)
        //{

        //    List<TFileUploadModels> Llist = new List<TFileUploadModels>();

        //    APIpath = apipath + "ImportFile/GetFileImportAll";
        //    HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(APIpath);
        //    httpWebRequest.Headers.Add("Authorization", "Bearer " + tokenStr);

        //    httpWebRequest.ContentType = "application/json";

        //    httpWebRequest.Method = "POST";


        //    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
        //    {
        //        var Req = new TFileUploadModels();

        //        if (models != null)
        //        {
        //            Req = models;
        //        }


        //        var json = JsonConvert.SerializeObject(Req, Formatting.Indented);

        //        streamWriter.Write(json);
        //        streamWriter.Flush();
        //        streamWriter.Close();


        //    }
        //    var response = (HttpWebResponse)httpWebRequest.GetResponse();

        //    using (var streamReader = new StreamReader(response.GetResponseStream()))
        //    {
        //        var result = streamReader.ReadToEnd();

        //        Llist = JsonConvert.DeserializeObject<List<TFileUploadModels>>(result);

        //    }
        //    return Llist;

        //}

        public static string GetGenerateCode(string apipath = null, string TokenStr = null)
        {
            string reCode = "";
            APIpath = apipath + "Request/GetGenerateCode";
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(APIpath);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Headers.Add("Authorization", "Bearer " + TokenStr);
            httpWebRequest.Method = "GET";

            try
            {
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {

                    var result = streamReader.ReadToEnd();
                    reCode = JsonConvert.DeserializeObject<string>(result);
                    return reCode;

                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public static string ConvertdDigittimeinttoString(string xHr, string xMin)
        {
            string Hr = "";
            string mm = "";
            string HHmm = "";

            if (xHr.Length == 1)
            {
                Hr = "0" + xHr;
            }
            else
            {
                Hr = xHr;
            }
            if (xMin.Length == 1)
            {
                mm = "0" + xMin;
            }
            else
            {
                mm = xMin;
            }
            HHmm = Hr + ":" + mm;
            return HHmm;

        }
        public static TimeOnly ConverttimeinttoString(string xHr, string xMin)
        {
            TimeOnly timeOnly = new TimeOnly();
            try
            {
                // แยกชั่วโมงและนาทีจาก xtime
                int hours = int.Parse(xHr);
                int minutes = int.Parse(xMin);
                // แปลงนาทีเกินเป็นชั่วโมงเพิ่มเติม
                hours += minutes / 60;       // เพิ่มชั่วโมงจากนาทีที่เกิน
                minutes = minutes % 60;       // คำนวณนาทีที่เหลือ
                string sethour;               // สร้าง TimeOnly โดยใช้ชั่วโมงและนาที
                string setMinus;

                if (hours.ToString().Length == 1)
                {
                    sethour = "0" + hours.ToString();
                }
                else
                {
                    sethour = hours.ToString();
                }

                //set minus
                if (minutes.ToString().Length == 1)
                {
                    setMinus = "0" + minutes.ToString();
                }
                else
                {
                    setMinus = minutes.ToString();
                }

                DateTime dateTime = DateTime.ParseExact(sethour + ":" + setMinus.ToString(), "HH:mm", null);
                timeOnly = new TimeOnly(dateTime.Hour, dateTime.Minute);


            }
            catch (Exception ex)
            {

            }
            return timeOnly;

        }
        public static int ConvertToMinutes(string time)
        {
            // แยกเวลาเป็นชั่วโมงและนาทีโดยใช้ :
            string[] parts = time.Split(':');

            // แปลงชั่วโมงและนาทีจาก string เป็น int
            int hours = int.Parse(parts[0]);
            int minutes = int.Parse(parts[1]);

            // คำนวณนาทีทั้งหมด
            return (hours * 60) + minutes;
        }

        public static int ConvertMinutesToHouse(string time)
        {
            // แยกเวลาเป็นชั่วโมงและนาทีโดยใช้ :
            string[] parts = time.Split(':');

            // แปลงชั่วโมงและนาทีจาก string เป็น int
            int hours = int.Parse(parts[0]);
            int minutes = int.Parse(parts[1]);

            // คำนวณนาทีทั้งหมด
            return (hours * 60) + minutes;
        }
        public static string genDateOffset(string dataAction)
        {
            CultureInfo englishCulture = new CultureInfo("en-US");
            string formattedDate;
            string input = dataAction;
            DateTime dateTime = DateTime.ParseExact(input, "dd/MM/yyyy HH:mm", englishCulture);

            // กำหนด Time Zone Offset เป็น +07:00
            DateTimeOffset dateTimeOffset = new DateTimeOffset(dateTime, TimeSpan.FromHours(7));

            // แปลงวันที่เป็นรูปแบบที่ต้องการ
            formattedDate = dateTimeOffset.ToString("yyyy-MM-ddTHH:mm:ss.fffK", englishCulture);
            return formattedDate;
        }
    }
}
