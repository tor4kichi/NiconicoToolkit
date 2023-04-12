using System;
using System.Threading.Tasks;
using System.Threading;
using System.Text.Json;
using System.IO;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using System.Net;
using System.Xml.Serialization;
#if WINDOWS_UWP
using Windows.Web.Http;
#else
using System.Net.Http;
using System.Net.Http.Headers;
#endif

namespace NiconicoToolkit
{
    internal static class HttpContentExtensions
    {
#if WINDOWS_UWP
        public static async Task<T> ReadJsonAsAsync<T>(this IHttpContent httpContent, JsonSerializerOptions options = null, CancellationToken ct = default)
        {
            using (var inputStream = await httpContent.ReadAsInputStreamAsync())
            using (var stream = inputStream.AsStreamForRead())
            {
                return await JsonSerializer.DeserializeAsync<T>(stream, options, ct);
            }
        }
#else
        public static async Task<T> ReadJsonAsAsync<T>(this HttpContent httpContent, JsonSerializerOptions options = null, CancellationToken ct = default)
        {
            using (var stream = await httpContent.ReadAsStreamAsync())
            {
                return await JsonSerializer.DeserializeAsync<T>(stream, options, ct);
            }
        }
#endif


#if WINDOWS_UWP
        public static async Task<T> ReadXmlAsAsync<T>(this IHttpContent httpContent, CancellationToken ct = default)
        {
            using (var inputStream = await httpContent.ReadAsInputStreamAsync())
            using (var stream = inputStream.AsStreamForRead())
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(stream);            
            }
        }
#else
        public static async Task<T> ReadXmlAsAsync<T>(this HttpContent httpContent, CancellationToken ct = default)
        {
            using (var stream = await httpContent.ReadAsStreamAsync())
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(stream);                
            }
        }
#endif

#if WINDOWS_UWP
        public static async Task<T> ReadHtmlDocumentActionAsync<T>(this IHttpContent httpContent, Func<IHtmlDocument, T> genereterDelegate, HtmlParser parser = null)
        {
            HtmlParser htmlParser = parser ?? new HtmlParser();
            using (var contentStream = await httpContent.ReadAsInputStreamAsync())
            using (var stream = contentStream.AsStreamForRead())
            using (var document = await htmlParser.ParseDocumentAsync(stream))
            {
                return genereterDelegate(document);
            }
        }
#else
        public static async Task<T> ReadHtmlDocumentActionAsync<T>(this HttpContent httpContent, Func<IHtmlDocument, T> genereterDelegate, HtmlParser parser = null)
        {
            HtmlParser htmlParser = parser ?? new HtmlParser();
            using (var stream = await httpContent.ReadAsStreamAsync())
            using (var document = await htmlParser.ParseDocumentAsync(stream))
            {
                return genereterDelegate(document);
            }
        }
#endif
    }

}
