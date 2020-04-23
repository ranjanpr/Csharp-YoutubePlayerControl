using CefSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using YoutubePlayerLib.Properties;

namespace YoutubePlayerLib.Cef
{
    /// <summary>
    /// Shamlessly copypasted from CEFSharp example project.
    /// </summary> 

    internal class CefSharpSchemeHandler : IResourceHandler
    {
        private static readonly IDictionary<string, string> ResourceDictionary;

        private string mimeType;
        private MemoryStream stream;

        static CefSharpSchemeHandler()
        {
            ResourceDictionary = new Dictionary<string, string>
            {
                { "/CefPlayer.html", Resources.CefPlayer},
                { "/CefPlayer.js", Resources.CefPlayer1},
            };
        }

        bool IResourceHandler.ProcessRequest(IRequest request, ICallback callback)
        {
            // The 'host' portion is entirely ignored by this scheme handler.
            var uri = new Uri(request.Url);
            var fileName = uri.AbsolutePath;

            if (string.Equals(fileName, "/PostDataTest.html", StringComparison.OrdinalIgnoreCase))
            {
                var postDataElement = request.PostData.Elements.FirstOrDefault();
                stream = ResourceHandler.GetMemoryStream("Post Data: " + (postDataElement == null ? "null" : postDataElement.GetBody()), Encoding.UTF8);
                mimeType = "text/html";
                callback.Continue();
                return true;
            }

            if (string.Equals(fileName, "/PostDataAjaxTest.html", StringComparison.OrdinalIgnoreCase))
            {
                var postData = request.PostData;
                if (postData == null)
                {
                    stream = ResourceHandler.GetMemoryStream("Post Data: null", Encoding.UTF8);
                    mimeType = "text/html";
                    callback.Continue();
                }
                else
                {
                    var postDataElement = postData.Elements.FirstOrDefault();
                    stream = ResourceHandler.GetMemoryStream("Post Data: " + (postDataElement == null ? "null" : postDataElement.GetBody()), Encoding.UTF8);
                    mimeType = "text/html";
                    callback.Continue();
                }

                return true;
            }

            if (string.Equals(fileName, "/EmptyResponseFilterTest.html", StringComparison.OrdinalIgnoreCase))
            {
                stream = null;
                mimeType = "text/html";
                callback.Continue();

                return true;
            }

            string resource;
            if (ResourceDictionary.TryGetValue(fileName, out resource) && !string.IsNullOrEmpty(resource))
            {
                Task.Run(() =>
                {
                    using (callback)
                    {
                        var bytes = Encoding.UTF8.GetBytes(resource);
                        stream = new MemoryStream(bytes);

                        var fileExtension = Path.GetExtension(fileName);
                        mimeType = ResourceHandler.GetMimeType(fileExtension);

                        callback.Continue();
                    }
                });

                return true;
            }
            else
            {
                callback.Dispose();
            }

            return false;
        }


        void IResourceHandler.GetResponseHeaders(IResponse response, out long responseLength, out string redirectUrl)
        {
            responseLength = stream == null ? 0 : stream.Length;
            redirectUrl = null;

            response.StatusCode = (int)HttpStatusCode.OK;
            response.StatusText = "OK";
            response.MimeType = mimeType;
        }

        bool IResourceHandler.ReadResponse(Stream dataOut, out int bytesRead, ICallback callback)
        {
            //Dispose the callback as it's an unmanaged resource, we don't need it in this case
            callback.Dispose();

            if (stream == null)
            {
                bytesRead = 0;
                return false;
            }

            //Data out represents an underlying buffer (typically 32kb in size).
            var buffer = new byte[dataOut.Length];
            bytesRead = stream.Read(buffer, 0, buffer.Length);

            dataOut.Write(buffer, 0, buffer.Length);

            return bytesRead > 0;
        }

        bool IResourceHandler.CanGetCookie(CefSharp.Cookie cookie)
        {
            return true;
        }

        bool IResourceHandler.CanSetCookie(CefSharp.Cookie cookie)
        {
            return true;
        }

        void IResourceHandler.Cancel()
        {

        }

        void IDisposable.Dispose()
        {

        }
    }

}
