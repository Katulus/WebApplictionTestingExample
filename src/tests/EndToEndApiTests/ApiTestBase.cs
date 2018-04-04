using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace EndToEndApiTests
{
    public abstract class ApiTestBase
    {
        // Set to URL of running application server instance
        private const string BaseUrl = "http://localhost:5000";

        protected HttpWebResponse Post(string url, string data)
        {
            HttpWebRequest request = WebRequest.CreateHttp(BaseUrl + url);
            request.Method = "POST";
            request.ContentType = "application/json";

            using (var requestStream = request.GetRequestStream())
            using (var requestWriter = new StreamWriter(requestStream))
            {
                requestWriter.Write(data);
            }

            return (HttpWebResponse)request.GetResponse();
        }

        protected HttpWebResponse Get(string url)
        {
            HttpWebRequest request = WebRequest.CreateHttp(BaseUrl + url);
            return (HttpWebResponse)request.GetResponse();
        }

        protected async Task<string> GetResponseData(HttpWebResponse response)
        {
            using (var responseStream = response.GetResponseStream())
            using (var streamReader = new StreamReader(responseStream))
            {
                return await streamReader.ReadToEndAsync();
            }
        }
    }
}