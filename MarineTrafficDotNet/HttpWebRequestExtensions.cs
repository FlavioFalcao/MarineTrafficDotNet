using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace MarineTrafficDotNet
{
    /// <summary>
    /// Extensions methods for the HttpWebRequest class to make life a little easier
    /// </summary>
    public static class HttpWebRequestExtensions
    {
        /// <summary>
        /// The character encoding for the request's bytes
        /// </summary>
        private static readonly Encoding _Encode = System.Text.Encoding.GetEncoding("utf-8");

        /// <summary>
        /// Sets the data for the given request (set the GetRequestStream stream)
        /// NOTE: If the data is null or empty, it will not be set
        /// </summary>
        /// <param name="request"></param>
        /// <param name="data"></param>
        public static void SetRequestData(this HttpWebRequest request, string data)
        {
            if (string.IsNullOrEmpty(data))
                return;

            // Create a byte array of the data we want to send  
            byte[] byteData = UTF8Encoding.UTF8.GetBytes(data);

            // Set the content length in the request headers  
            request.ContentLength = byteData.Length;

            // Write data  
            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(byteData, 0, byteData.Length);
            }
        }

        /// <summary>
        /// Gets the response of this request as a simple string
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string GetResponseString(this HttpWebRequest request)
        {
            // Read the string back as a string, performing all necessary disposal automatically
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    using (StreamReader readStream = new StreamReader(responseStream, _Encode))
                    {
                        return readStream.ReadToEnd();
                    }
                }
            }
        }

        /// <summary>
        /// Loads the request with data and gets the response as a string as one action
        /// </summary>
        /// <param name="request"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string SetRequestDataAndGetResponseString(this HttpWebRequest request, string data = null)
        {
            request.SetRequestData(data);
            return request.GetResponseString();
        }

        /// <summary>
        /// Creates a new request object to the given address using the given verb, setting reasonable defaults for the request
        /// Just set your headers and you're ready to request
        /// </summary>
        /// <param name="address"></param>
        /// <param name="verb"></param>
        /// <returns></returns>
        public static HttpWebRequest Create(string address, string verb)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(address);

            request.Method = verb;

            request.SendChunked = false;
            request.AllowWriteStreamBuffering = true;
            request.AllowAutoRedirect = true;

            return request;
        }
    }
}
