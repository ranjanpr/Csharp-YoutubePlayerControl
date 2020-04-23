using CefSharp;

namespace YoutubePlayerLib.Cef
{

    /// <summary>
    /// Shamelessly copypasted from CEFSharp example project.
    /// </summary>
    class CefSharpSchemeHandlerFactory : ISchemeHandlerFactory
    {
        public const string SchemeName = "custom";

        public IResourceHandler Create(IBrowser browser, IFrame frame, string schemeName, IRequest request)
        {
            if (schemeName == SchemeName && request.Url.EndsWith("CefSharp.Core.xml", System.StringComparison.OrdinalIgnoreCase))
            {
                //Display the debug.log file in the browser
                return ResourceHandler.FromFilePath("CefSharp.Core.xml", ".xml");
            }
            return new CefSharpSchemeHandler();
        }
    }

}
