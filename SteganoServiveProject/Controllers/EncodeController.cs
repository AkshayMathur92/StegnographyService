using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.WindowsAzure.Mobile.Service;
using System.IO;
using System.Drawing;
using System.Net.Http.Headers;

namespace SteganoServiveProject.Controllers
{
    public class EncodeController : ApiController
    {
        public ApiServices Services { get; set; }
        bool useGrayScale = false;
        Stream message = new MemoryStream(System.Text.Encoding.ASCII.GetBytes("This is a default message"));
        Bitmap image = null;
        Stream key = new MemoryStream(System.Text.Encoding.ASCII.GetBytes("Avi Mathur"));
        String method = null;
        // GET api/Encode
        public HttpResponseMessage Get()
        {
            HttpResponseMessage testHtml = new HttpResponseMessage(HttpStatusCode.OK);
            testHtml.Content = new StringContent(Properties.Resources.EncodeHTML);
            testHtml.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
            return testHtml;
        }
        public async System.Threading.Tasks.Task<HttpResponseMessage> Post()
        {
            MyMultipartFormDataStreamProvider streamProvider = new MyMultipartFormDataStreamProvider();
            streamProvider = await Request.Content.ReadAsMultipartAsync(streamProvider);
            image = streamProvider.image;
            message = new MemoryStream(System.Text.Encoding.ASCII.GetBytes(streamProvider.FormData.Get("message")));
            CryptUtility.HideMessageInBitmap(message, ref image, key, useGrayScale);
            
            HttpResponseMessage imageHttp = new HttpResponseMessage();
            imageHttp.Content = new ByteArrayContent(CryptUtility.ImageToByte2(image));
            return imageHttp;
        }
    }
}
