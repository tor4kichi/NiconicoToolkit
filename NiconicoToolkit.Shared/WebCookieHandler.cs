using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


#if false
namespace NiconicoToolkit;
using Microsoft.Web.WebView2;
internal sealed class WebView2SetCookieHandler : DelegatingHandler
{
    private readonly WebView2 _webView2;

    public WebView2SetCookieHandler(WebView2 webView2, HttpMessageHandler innerHandler)
        : base(innerHandler)
    {
        _webView2 = webView2;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (request.RequestUri != null)
        {
            var cookieHeader = await this.GetCookieHeaderAsync(request.RequestUri).ConfigureAwait(false);

            if (!string.IsNullOrEmpty(cookieHeader))
            {
                request.Headers.Add("Cookie", cookieHeader);
            }
        }

        return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
    }

    private async Task<string> GetCookieHeaderAsync(Uri uri)
    {
        CookieContainer cookieContainer;

        if (_webView2.CheckAccess())
        {
            cookieContainer = await this.GetCookieContainerAsync(uri).ConfigureAwait(false);
        }
        else
        {
            var cookieContainerTask = _webView2.Dispatcher.InvokeAsync(() => this.GetCookieContainerAsync(uri)).Task.Unwrap();
            cookieContainer = await cookieContainerTask.ConfigureAwait(false);
        }

        return cookieContainer.GetCookieHeader(uri);
    }

    private async Task<CookieContainer> GetCookieContainerAsync(Uri uri)
    {
        var cookies = await _webView2.CoreWebView2.CookieManager.GetCookiesAsync(uri.ToString()).ConfigureAwait(false);
        var cookieContainer = new CookieContainer();

        foreach (var cookie in cookies)
        {
            cookieContainer.Add(cookie.ToSystemNetCookie());
        }

        return cookieContainer;
    }
}
#endif
