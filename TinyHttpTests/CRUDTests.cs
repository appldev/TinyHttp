using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TinyHttp;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace TinyHttpTests
{
    public class GeoIPModel
    {
        // {"ip":"79.142.76.176","country_code":"SE","country_name":"Sweden","region_code":"AB","region_name":"Stockholm","city":"Stockholm","zip_code":"173 11",
        // "time_zone":"Europe/Stockholm","latitude":59.3333,"longitude":18.05,"metro_code":0}
        public string ip { get; set; }
        public string country_code { get; set; }
        public string country_name { get; set; }
        public string region_code { get; set; }
        public string region_name { get; set; }
        public string city { get; set; }
        public string zip_code { get; set; }
        public string time_zone { get; set; }
        public decimal latitude { get; set; }
        public decimal longitude { get; set; }
        public int metro_code { get; set; }
    }

    [TestClass]
    public class CRUDTests
    {
        [TestMethod]
        public void TestGeoIP()
        {
            HttpResponse<GeoIPModel> response = TinyHttp.HttpClient.Execute<GeoIPModel>(string.Format("http://freegeoip.net/json/{0}", "79.142.76.176"), HttpClient.RequestTypes.GET);
            Assert.IsTrue(response.ResponseStatus == System.Net.HttpStatusCode.OK);
            Console.WriteLine(response.ResponseData.country_name);
        }


        public static readonly string BaseUrl = "http://jsonplaceholder.typicode.com";

        [TestMethod]
        public async Task TestCreate()
        {
            dynamic data = new DataObject();
            data.title = "Test title";
            data.body = "Test body";
            data.userId = 1;
            HttpResponse<DataObject> response = await HttpClient.GetCreateRequest(BaseUrl + "/posts")
                .WithContent<object>(HttpClient.RequestContentTypes.Json,(object)data)
                .ExecuteAsync<DataObject>();

            Assert.IsTrue(response.ResponseStatus == System.Net.HttpStatusCode.Created);
            data = response.ResponseData;
            Assert.IsTrue(data.id > 0);
            Console.WriteLine("Created data with id: {0}", data.id);
            Console.WriteLine(DataObject.ToJson(response.ResponseData, true));
        }

        [TestMethod]
        public async Task TestUpdate()
        {
            dynamic data = new DataObject();
            data.title = "Test title";
            data.body = "Test body";
            data.userId = 1;
            data.id = 22;
            HttpResponse<DataObject> response = await HttpClient.GetUpdateRequest(BaseUrl + "/posts/22")
                .WithContent<object>(HttpClient.RequestContentTypes.Json, (object)data)
                .ExecuteAsync<DataObject>();

            Assert.IsTrue(response.ResponseStatus == System.Net.HttpStatusCode.OK);
            dynamic result = response.ResponseData;
            Assert.IsTrue(data.id == result.id);
            Console.WriteLine(DataObject.ToJson(response.ResponseData, true));
        }

        [TestMethod]
        public async Task TestDelete()
        {
            HttpResponse<DataObject> response = await HttpClient.GetDeleteRequest(BaseUrl + "/posts/5")
                .WithContent(HttpClient.RequestContentTypes.Json)
                .ExecuteAsync<DataObject>();

            Assert.IsTrue(response.ResponseStatus == System.Net.HttpStatusCode.OK);
            Console.WriteLine(DataObject.ToJson(response.ResponseData, true));
        }

        [TestMethod]
        public async Task TestRead()
        {
            HttpResponse<List<DataObject>> response = await HttpClient.GetReadRequest(BaseUrl + "/posts")
                .WithContent(HttpClient.RequestContentTypes.Json)
                .ExecuteAsync<List<DataObject>>();

            Assert.IsTrue(response.ResponseStatus == System.Net.HttpStatusCode.OK);
            Console.WriteLine(DataObject.ToJson(response.ResponseData, true));
        }

        [TestMethod]
        public async Task TestReadOne()
        {
            HttpResponse<DataObject> response = await HttpClient.GetReadRequest(BaseUrl + "/posts/3")
                .WithContent(HttpClient.RequestContentTypes.Json)
                .ExecuteAsync<DataObject>();

            Assert.IsTrue(response.ResponseStatus == System.Net.HttpStatusCode.OK);
            Console.WriteLine(DataObject.ToJson(response.ResponseData, true));
        }

        [TestMethod]
        public async Task TestReadFilter()
        {
            HttpResponse<List<DataObject>> response = await HttpClient.GetReadRequest(BaseUrl + "/posts",
                new NameValueCollection()
                {
                    {"userId","1" }
                })
                .WithContent(HttpClient.RequestContentTypes.Json)
                .ExecuteAsync<List<DataObject>>();

            Assert.IsTrue(response.ResponseStatus == System.Net.HttpStatusCode.OK);
            Console.WriteLine(DataObject.ToJson(response.ResponseData, true));
        }
    }
}
