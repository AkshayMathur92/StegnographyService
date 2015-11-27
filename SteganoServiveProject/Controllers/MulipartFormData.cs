using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http.Formatting.Internal;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;

namespace System.Net.Http
{
    /// <summary>
    /// A <see cref="MultipartStreamProvider"/> implementation suited for use with HTML file uploads for writing file 
    /// content to a <see cref="FileStream"/>. The stream provider looks at the <b>Content-Disposition</b> header 
    /// field and determines an output <see cref="Stream"/> based on the presence of a <b>filename</b> parameter.
    /// If a <b>filename</b> parameter is present in the <b>Content-Disposition</b> header field then the body 
    /// part is written to a <see cref="FileStream"/>, otherwise it is written to a <see cref="MemoryStream"/>.
    /// This makes it convenient to process MIME Multipart HTML Form data which is a combination of form 
    /// data and file content.
    /// </summary>
    public class MyMultipartFormDataStreamProvider : MultipartStreamProvider
    {
        private NameValueCollection _formData = new NameValueCollection();

        // Set of indexes of which HttpContents we designate as form data
        private Collection<bool> _isFormData = new Collection<bool>();

        //For Image
        public Bitmap image { get; set; } = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="MultipartFormDataStreamProvider"/> class.
        /// </summary>
        /// <param name="rootPath">The root path where the content of MIME multipart body parts are written to.</param>
        public MyMultipartFormDataStreamProvider()
        {
        }
        /// <summary>
        /// Gets a <see cref="NameValueCollection"/> of form data passed as part of the multipart form data.
        /// </summary>
        public NameValueCollection FormData
        {
            get { return _formData; }
        }

        /// <summary>
        /// This body part stream provider examines the headers provided by the MIME multipart parser
        /// and decides whether it should return a file stream or a memory stream for the body part to be 
        /// written to.
        /// </summary>
        /// <param name="parent">The parent MIME multipart HttpContent instance.</param>
        /// <param name="headers">Header fields describing the body part</param>
        /// <returns>The <see cref="Stream"/> instance where the message body part is written to.</returns>
        public override Stream GetStream(HttpContent parent, HttpContentHeaders headers)
        {
            if (parent == null)
            {
                throw new System.ArgumentException("Parent cannot be null");
            }

            if (headers == null)
            {
                throw new System.ArgumentException("Headers cannot be null");
            }

            // For form data, Content-Disposition header is a requirement
            ContentDispositionHeaderValue contentDisposition = headers.ContentDisposition;
            if (contentDisposition != null)
            {
                // If we have a file name then write contents out to temporary file. Otherwise just write to MemoryStream
                if (!String.IsNullOrEmpty(contentDisposition.FileName))
                {
                    // We won't post process files as form data
                    _isFormData.Add(false);
                     //return base:GetStream(parent, headers);
                    return new MemoryStream();
                }

                // We will post process this as form data
                _isFormData.Add(true);

                // If no filename parameter was found in the Content-Disposition header then return a memory stream.
                return new MemoryStream();
            }

            // If no Content-Disposition header was present.
            throw new System.InvalidOperationException("Content-Disposition");
        }

        /// <summary>
        /// Read the non-file contents as form data.
        /// </summary>
        /// <returns></returns>
        public override async Task ExecutePostProcessingAsync()
        {
            // Find instances of HttpContent for which we created a memory stream and read them asynchronously
            // to get the string content and then add that as form data
            for (int index = 0; index < Contents.Count; index++)
            {
                if (_isFormData[index])
                {
                    HttpContent formContent = Contents[index];
                    // Extract name from Content-Disposition header. We know from earlier that the header is present.
                    ContentDispositionHeaderValue contentDisposition = formContent.Headers.ContentDisposition;
                    string formFieldName = contentDisposition.Name ?? String.Empty;

                    // Read the contents as string data and add to form data
                    string formFieldValue = await formContent.ReadAsStringAsync();
                    FormData.Add(formFieldName, formFieldValue);
                }
                else
                {
                    //we know from earlier that it is not form data but imagedata
                    HttpContent imageContent = Contents[index];
                    byte[] imagedata = await imageContent.ReadAsByteArrayAsync();
                    image = new Bitmap(new MemoryStream(imagedata));
                }
            }
        }
    }
}