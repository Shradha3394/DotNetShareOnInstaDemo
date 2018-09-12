using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ShareOnInstaDemoApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public void Authenticate()
        {
            var client_id = ConfigurationManager.AppSettings["instagram.clientid"].ToString();
            var redirect_uri = ConfigurationManager.AppSettings["instagram.redirecturi"].ToString();
            Response.Redirect("https://api.instagram.com/oauth/authorize/?client_id=" + client_id + "&redirect_uri=" + redirect_uri + "&response_type=code");
        }

        public void Share()
        {
            #region Get access token
            var code = Request.QueryString["code"];
            NameValueCollection parameters = new NameValueCollection();
            parameters.Add("client_id", ConfigurationManager.AppSettings["instagram.clientid"].ToString());
            parameters.Add("client_secret", ConfigurationManager.AppSettings["instagram.clientsecret"].ToString());
            parameters.Add("grant_type", "authorization_code");
            parameters.Add("redirect_uri", ConfigurationManager.AppSettings["instagram.redirecturi"].ToString());
            parameters.Add("code", code);
            WebClient client = new WebClient();
            var result = client.UploadValues("https://api.instagram.com/oauth/access_token", "POST", parameters);

            #endregion

            // TODO: Write code to share
            var imagePath = @"E:\Shradha\download.jpg";
            FileStream fileStream = null;
            using (fileStream = System.IO.File.OpenRead(imagePath))
            {
                MemoryStream memStream = new MemoryStream();
                memStream.SetLength(fileStream.Length);
                fileStream.Read(memStream.GetBuffer(), 0, (int)fileStream.Length);
            }

            var data = new
            {
                upload_id = DateTime.Now.ToString(),
                photo = fileStream,
                media_type = "1"
            };
            var json = JsonConvert.SerializeObject(data);
            var cli = new WebClient();
            cli.Headers[HttpRequestHeader.ContentType] = "application/json";
            string response1 = cli.UploadString("https://www.instagram.com/create/upload/photo/", json);

            //var baseAddress = "https://www.instagram.com/create/upload/photo/";

            //var http = (HttpWebRequest)WebRequest.Create(new Uri(baseAddress));
            //http.Accept = "application/json";
            //http.ContentType = "application/json";
            //http.Method = "POST";

            //string parsedContent = JsonConvert.SerializeObject(data);
            //ASCIIEncoding encoding = new ASCIIEncoding();
            //Byte[] bytes = encoding.GetBytes(parsedContent);

            //Stream newStream = http.GetRequestStream();
            //newStream.Write(bytes, 0, bytes.Length);
            //newStream.Close();

            //var response1 = http.GetResponse();

            //var stream = response1.GetResponseStream();
            //var sr = new StreamReader(stream);
            //var content = sr.ReadToEnd();
        }
    }
}