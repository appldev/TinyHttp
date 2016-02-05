using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TinyHttp;
using System.Collections.Specialized;
using System.Threading.Tasks;
using System.Text;
using System.IO;

namespace TinyHttpTests
{
    [TestClass]
    public class ProductUploadTests
    {
        [TestMethod]
        public async Task UploadFromFile()
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "brontorestapi.txt");
            string file = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "brontoproducts.xml");
            if (!File.Exists(path))
            {
                Console.WriteLine("The file {0} must exist with the clientid and clientsecret in line 1 and 2", path);
                return;
            }
            else if (!File.Exists(file))
            {
                Console.WriteLine("The file {0} must exist with the bronto products", file);
                return;
            }


            string[] lines = File.ReadAllLines(path);
            
            string clientId = lines[0];
            string clientSecret = lines[1];

            HttpResponse<OAuthAccessToken> result = await TinyHttp.HttpClient.OAuthClientLoginAsync("https://auth.bronto.com/oauth2/token", clientId, clientSecret);
            OAuthAccessToken Auth = result.ResponseData;
            System.Net.ServicePointManager.Expect100Continue = false;
            FileUpload fu = new FileUpload(System.IO.File.ReadAllText(file, Encoding.UTF8),string.Format("brontoproducts{0}.xml",DateTime.Now.Ticks.ToString()), "text/xml", "feed", Encoding.UTF8);
            NameValueCollection nvc = new NameValueCollection();
            nvc.Add("catalogId", "1311");

            HttpResponse<string> import = await HttpClient.ExecuteAsync<string>(
                HttpClient.CreateFormDataRequest("https://rest.bronto.com/products/public/feed_import",
                nvc, new System.Collections.Generic.List<FileUpload>() { fu }, Auth)
                );
            System.Net.ServicePointManager.Expect100Continue = true;
            string transactionid = import.ResponseData;
            Console.WriteLine("UUID: {0}", import.ResponseData);



        }
    }
}
