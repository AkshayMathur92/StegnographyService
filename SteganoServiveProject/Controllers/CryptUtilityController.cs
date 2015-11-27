using System.Drawing;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.WindowsAzure.Mobile.Service;
using System.Web.Http.Results;
using System.Net.Http.Headers;
using System.Collections.Specialized;
using System.Threading.Tasks;
using System.Drawing.Imaging;

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
        public async Task<String> Post()
        {
            Services.Log.Info("Hello from custom controller!");
            MyMultipartFormDataStreamProvider streamProvider = new MyMultipartFormDataStreamProvider();
            streamProvider = await Request.Content.ReadAsMultipartAsync(streamProvider);
            method = streamProvider.FormData.Get("\"method\"");
            message = new MemoryStream(System.Text.Encoding.ASCII.GetBytes(streamProvider.FormData.Get("\"msg\"")));
            //image  = new Bitmap(streamProvider.FileData[0].LocalFileName);
            //image = new Bitmap("C:\\Users\\amathur\\Downloads\\BodyPart.jpg");
            image = streamProvider.image;
            //image.Save("C:\\Users\\amathur\\Downloads\\myCryptimage.jpg");
            if (method.Equals("encode"))
            {
                //TODO
                CryptUtility.HideMessageInBitmap(message, ref image, key, useGrayScale);
                image.Save("C:\\Users\\amathur\\Downloads\\myCryptimage.png",ImageFormat.Png);
            }
            else
            {
                //TODO
                CryptUtility.ExtractMessageFromBitmap(ref image, key, ref message);
            }
            String final = System.Text.Encoding.ASCII.GetString(ReadFully(message));
            return final;

        }
        public static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        public class CryptUtility
        {

            /// <summary>Hides a message in a bitmap</summary>
            /// <param name="messageStream">The message to hide</param>
            /// <param name="bitmap">The carrier bitmap</param>
            /// <param name="keyStream">The key to use</param>
            public static void HideMessageInBitmap(Stream messageStream, ref Bitmap bitmap, Stream keyStream, bool useGrayscale)
            {
                HideOrExtract(ref messageStream, ref bitmap, keyStream, false, useGrayscale);
                messageStream.Seek(0, SeekOrigin.Begin);
            }

            /// <summary>Extracts an hidden message from a bitmap</summary>
            /// <param name="bitmap">The carrier bitmap</param>
            /// <param name="keyStream">The key used for hiding the message</param>
            /// <param name="messageStream">Empty stream to receive the message</param>
            public static void ExtractMessageFromBitmap(ref Bitmap bitmap, Stream keyStream, ref Stream messageStream)
            {
                HideOrExtract(ref messageStream,ref bitmap, keyStream, true, false);
                messageStream.Seek(0, SeekOrigin.Begin);
            }

            /// <summary>Stepts through the pixels of a bitmap using a key pattern and hides or extracts a message</summary>
            /// <param name="messageStream">If exctract is false, the message to hide - otherwise an empty stream to receive the extracted message</param>
            /// <param name="bitmap">The carrier bitmap</param>
            /// <param name="keyStream">The key specifying the unchanged pixels between two hidden bytes</param>
            /// <param name="extract">Extract a hidden message (true), or hide a message in a clean carrier bitmap (false)</param>
            private static void HideOrExtract(ref Stream messageStream, ref Bitmap bitmap, Stream keyStream, bool extract, bool useGrayscale)
            {
                //Current count of pixels between two hidden message-bytes
                //Changes with every hidden byte according to the key
                int currentStepWidth = 0;
                //Current byte in the key stream - normal direction
                byte currentKeyByte;
                //Current byte in the key stream - reverse direction
                byte currentReverseKeyByte;
                //current position in the key stream
                long keyPosition;

                //maximum X and Y position
                int bitmapWidth = bitmap.Width - 1;
                int bitmapHeight = bitmap.Height - 1;

                //Color component to hide the next byte in (0-R, 1-G, 2-B)
                //Rotates with every hidden byte
                int currentColorComponent = 0;

                //Stores the color of a pixel
                Color pixelColor;

                //Length of the message
                Int32 messageLength;

                if (extract)
                {
                    //Read the length of the hidden message from the first pixel
                    pixelColor = bitmap.GetPixel(0, 0);
                    messageLength = (pixelColor.R << 2) + (pixelColor.G << 1) + pixelColor.B;
                    messageStream = new MemoryStream(messageLength);
                }
                else
                {

                    messageLength = (Int32)messageStream.Length;

                    if (messageStream.Length >= 16777215)
                    { //The message is too long
                        String exceptionMessage = "The message is too long, only 16777215 bytes are allowed.";
                        throw new Exception(exceptionMessage);
                    }

                    //Check size of the carrier image

                    //Pixels available
                    long countPixels = (bitmapWidth * bitmapHeight) - 1;
                    //Pixels required - start with one pixel for length of message
                    long countRequiredPixels = 1;
                    //add up the gaps between used pixels (sum up all the bytes of the key)
                    while ((keyStream.Position < keyStream.Length) && (keyStream.Position < messageLength))
                    {
                        countRequiredPixels += keyStream.ReadByte();
                    }
                    //If the key is shorter than the message, it will be repeated again and again
                    //Multiply with count of key periods
                    countRequiredPixels *= (long)System.Math.Ceiling(((float)messageStream.Length / (float)keyStream.Length));

                    if (countRequiredPixels > countPixels)
                    { //The bitmap is too small
                        String exceptionMessage = "The image is too small for this message and key. " + countRequiredPixels + " pixels are required.";
                        throw new Exception(exceptionMessage);
                    }

                    //Write length of the bitmap into the first pixel
                    int colorValue = messageLength;
                    int red = colorValue >> 2;
                    colorValue -= red << 2;
                    int green = colorValue >> 1;
                    int blue = colorValue - (green << 1);
                    pixelColor = Color.FromArgb(red, green, blue);
                    bitmap.SetPixel(0, 0, pixelColor);
                }

                //Reset the streams
                keyStream.Seek(0, SeekOrigin.Begin);
                messageStream.Seek(0, SeekOrigin.Begin);

                //Current position in the carrier bitmap
                //Start with 1, because (0,0) contains the message's length
                Point pixelPosition = new Point(1, 0);

                //Loop over the message and hide each byte
                for (int messageIndex = 0; messageIndex < messageLength; messageIndex++)
                {

                    //repeat the key, if it is shorter than the message
                    if (keyStream.Position == keyStream.Length)
                    {
                        keyStream.Seek(0, SeekOrigin.Begin);
                    }
                    //Get the next pixel-count from the key, use "1" if it's 0
                    currentKeyByte = (byte)keyStream.ReadByte();
                    currentStepWidth = (currentKeyByte == 0) ? (byte)1 : currentKeyByte;

                    //jump to reverse-read position and read from the end of the stream
                    keyPosition = keyStream.Position;
                    keyStream.Seek(-keyPosition, SeekOrigin.End);
                    currentReverseKeyByte = (byte)keyStream.ReadByte();
                    //jump back to normal read position
                    keyStream.Seek(keyPosition, SeekOrigin.Begin);

                    //Perform line breaks, if current step is wider than the image
                    while (currentStepWidth > bitmapWidth)
                    {
                        currentStepWidth -= bitmapWidth;
                        pixelPosition.Y++;
                    }

                    //Move X-position
                    if ((bitmapWidth - pixelPosition.X) < currentStepWidth)
                    {
                        pixelPosition.X = currentStepWidth - (bitmapWidth - pixelPosition.X);
                        pixelPosition.Y++;
                    }
                    else
                    {
                        pixelPosition.X += currentStepWidth;
                    }

                    //Get color of the "clean" pixel
                    pixelColor = bitmap.GetPixel(pixelPosition.X, pixelPosition.Y);

                    if (extract)
                    {
                        //Extract the hidden message-byte from the color
                        byte foundByte = (byte)(currentReverseKeyByte ^ GetColorComponent(pixelColor, currentColorComponent));
                        messageStream.WriteByte(foundByte);
                        //Rotate color components
                        currentColorComponent = (currentColorComponent == 2) ? 0 : (currentColorComponent + 1);

                    }
                    else
                    {
                        //To add a bit of confusion, xor the byte with a byte read from the keyStream
                        int currentByte = messageStream.ReadByte() ^ currentReverseKeyByte;

                        if (useGrayscale)
                        {
                            pixelColor = Color.FromArgb(currentByte, currentByte, currentByte);
                        }
                        else
                        {
                            //Change one component of the color to the message-byte
                            SetColorComponent(ref pixelColor, currentColorComponent, currentByte);
                            //Rotate color components
                            currentColorComponent = (currentColorComponent == 2) ? 0 : (currentColorComponent + 1);
                        }
                        bitmap.SetPixel(pixelPosition.X, pixelPosition.Y, pixelColor);
                    }
                }

                //the stream will be closed by the calling method
                //bitmap = null;
                //keyStream = null;
            }

            /// <summary>Return one component of a color</summary>
            /// <param name="pixelColor">The Color</param>
            /// <param name="colorComponent">The component to return (0-R, 1-G, 2-B)</param>
            /// <returns>The requested component</returns>
            private static byte GetColorComponent(Color pixelColor, int colorComponent)
            {
                byte returnValue = 0;
                switch (colorComponent)
                {
                    case 0:
                        returnValue = pixelColor.R;
                        break;
                    case 1:
                        returnValue = pixelColor.G;
                        break;
                    case 2:
                        returnValue = pixelColor.B;
                        break;
                }
                return returnValue;
            }

            /// <summary>Changees one component of a color</summary>
            /// <param name="pixelColor">The Color</param>
            /// <param name="colorComponent">The component to change (0-R, 1-G, 2-B)</param>
            /// <param name="newValue">New value of the component</param>
            private static void SetColorComponent(ref Color pixelColor, int colorComponent, int newValue)
            {
                switch (colorComponent)
                {
                    case 0:
                        pixelColor = Color.FromArgb(newValue, pixelColor.G, pixelColor.B);
                        break;
                    case 1:
                        pixelColor = Color.FromArgb(pixelColor.R, newValue, pixelColor.B);
                        break;
                    case 2:
                        pixelColor = Color.FromArgb(pixelColor.R, pixelColor.G, newValue);
                        break;
                }
            }

            private static String UnTrimColorString(String color, int desiredLength)
            {
                int difference = desiredLength - color.Length;
                if (difference > 0)
                {
                    color = new String('0', difference) + color;
                }
                return color;
            }

        }
    }

}
