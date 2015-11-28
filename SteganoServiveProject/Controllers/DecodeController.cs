using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.WindowsAzure.Mobile.Service;
using System.Net.Http.Headers;
using System.Drawing;
using System.IO;

namespace SteganoServiveProject.Controllers
{
    public class DecodeController : ApiController
    {
        public ApiServices Services { get; set; }
        Stream message = new System.IO.MemoryStream(System.Text.Encoding.ASCII.GetBytes("This is a default message"));
        Bitmap image = null;
        Stream key = new MemoryStream(System.Text.Encoding.ASCII.GetBytes("Avi Mathur"));

        // GET api/Decode
        public HttpResponseMessage Get()
        {
            HttpResponseMessage testHtml = new HttpResponseMessage(HttpStatusCode.OK);
            testHtml.Content = new StringContent(Properties.Resources.DecodeHTML);
            testHtml.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
            return testHtml;
        }

        public async System.Threading.Tasks.Task<string> Post()
        {
            MyMultipartFormDataStreamProvider streamProvider = new MyMultipartFormDataStreamProvider();
            streamProvider = await Request.Content.ReadAsMultipartAsync(streamProvider);
            image = streamProvider.image;
            CryptUtility.ExtractMessageFromBitmap(ref image, key, ref message);
            return System.Text.Encoding.ASCII.GetString(CryptUtility.ReadFully(message));
        }

    }
}
