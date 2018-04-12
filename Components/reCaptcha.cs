using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;

namespace VF.ANB.Modules
{
    public class ResponseObj
    {
        public string success { get; set; }
    }


    public class reCaptcha
    {
        //protected static string ReCaptcha_Key = "6LckmAwTAAAAADJZs55WDAfintK8G9RUHZdg1EAh";
        //protected static string ReCaptcha_Secret = "6LckmAwTAAAAAERQCzobwpPwbqNcLt7_eXSTAJTg";

        protected static string ReCaptcha_Key = ConfigurationManager.AppSettings["reCaptchaKey"];//"6LeikiIUAAAAAGH3Vgvh-OOFrfEe5iD17f8xeAsn";
        protected static string ReCaptcha_Secret = ConfigurationManager.AppSettings["reCaptchaSecret"];//"6LeikiIUAAAAAIJTbH9crFYgk1ZkFAfia7Zn9mL5";

        [WebMethod]
        public  bool VerifyCaptcha(string response)
        {
            bool valid = false;
            try
            {
                string url = "https://www.google.com/recaptcha/api/siteverify?secret=" + ReCaptcha_Secret + "&response=" + response;
                string resp = (new WebClient()).DownloadString(url);
                JavaScriptSerializer js = new JavaScriptSerializer();
                ResponseObj data = js.Deserialize<ResponseObj>(resp);// Deserialize Json
                valid = Convert.ToBoolean(data.success);
            }
            catch
            {
                valid = false;
            }
            
                return valid;
          
          
        }
    }
}