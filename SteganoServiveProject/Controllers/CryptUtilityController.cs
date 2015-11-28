using System.Drawing;
using System.IO;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.WindowsAzure.Mobile.Service;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SteganoServiveProject.Controllers
{
    public class CryptUtilityController : ApiController
    {
        public ApiServices Services { get; set; }
        bool useGrayScale = false;
        Stream message = new MemoryStream(System.Text.Encoding.ASCII.GetBytes("This is a default message"));
        Bitmap image = null;
        Stream key = new MemoryStream(System.Text.Encoding.ASCII.GetBytes("Avi Mathur"));
        String method = null;
        // GET api/CryptUtility
        public HttpResponseMessage Get()
        {
            
            HttpResponseMessage testHtml = new HttpResponseMessage(HttpStatusCode.OK);
            testHtml.Content = new StringContent(Properties.Resources.TestHTML);
            testHtml.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
            return testHtml;
        }
        // POST api/CryptUtility
        public async Task<HttpResponseMessage> Post()
        {
            HttpResponseMessage returnHTML = new HttpResponseMessage(HttpStatusCode.OK);
            //Services.Log.Info("Hello from custom controller!");
            
            MyMultipartFormDataStreamProvider streamProvider = new MyMultipartFormDataStreamProvider();
            streamProvider = await Request.Content.ReadAsMultipartAsync(streamProvider);
            method = streamProvider.FormData.Get("method");
            message = new MemoryStream(System.Text.Encoding.ASCII.GetBytes(streamProvider.FormData.Get("message")));
            image = streamProvider.image;
            if (method.Equals("encode"))
            {
                //TODO
                CryptUtility.HideMessageInBitmap(message, ref image, key, useGrayScale);
                returnHTML.Content = new ByteArrayContent(CryptUtility.ImageToByte2(image));
                returnHTML.Content.Headers.ContentType = new MediaTypeHeaderValue("image/png");
                returnHTML.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("inline");
                returnHTML.Content.Headers.ContentDisposition.FileName = "SecretImage.png";

                //image.Save("C:\\Users\\amathur\\Downloads\\myCryptimage.png",ImageFormat.Png);
            }
            else
            {
                //TODO
                CryptUtility.ExtractMessageFromBitmap(ref image, key, ref message);
                returnHTML.Content = new StringContent(System.Text.Encoding.ASCII.GetString(CryptUtility.ReadFully(message)));
                returnHTML.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
                returnHTML.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("inline");
            }
            return returnHTML;

        }        
    }

}
