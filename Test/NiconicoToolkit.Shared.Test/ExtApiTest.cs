using CommunityToolkit.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NiconicoToolkit;
using NiconicoToolkit.ExtApi.Video;
using NiconicoToolkit.Tests;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NiconicoToolkit.Tests;

[TestClass]
public class ExtApiTest
{
    NiconicoContext _context;
    [TestInitialize]
    public void InitializeTest()
    {
        _context = new NiconicoContext(AccountTestHelper.Site);
    }
    [TestMethod]
    [DataRow("so42043182")]
    [DataRow("sm42050014")]
    public async Task GetThumbInfoAsync(string videoId)
    {
        var res = await _context.ExtApiClient.GetVideoInfoAsync(videoId);
        Guard.IsTrue(res.IsOK, nameof(res.IsOK));
        Guard.IsNotNull(res.Data, nameof(res.Data));
        Guard.IsNull(res.Error, nameof(res.Error));
        Guard.IsNotNullOrWhiteSpace(res.Data.Title, nameof(res.Data.Title));

        var niconicoId = new NiconicoId(videoId);
        var (providerType, providerId, providerName, providerIconUrl) = res.Data.GetProviderData();
        
        if (videoId.StartsWith(ContentIdHelper.VideoIdPrefixForChannel))
        {
            Guard.IsTrue(providerType is VideoProviderType.Channel, "provider type not VideoProviderType.Channel (videoId is start with 'ch')");
        }
        else if (videoId.StartsWith(ContentIdHelper.VideoIdPrefixForUser))
        {
            Guard.IsTrue(providerType is VideoProviderType.User, "provider type not VideoProviderType.User (videoId is start with 'sm')");
        }

        Guard.IsGreaterThan(providerId, 0, nameof(providerId));
        Guard.IsNotNullOrWhiteSpace(providerName, nameof(providerName));
        Guard.IsNotNullOrEmpty(providerIconUrl, nameof(providerIconUrl));
    }
    
    //[TestMethod]
    [DataRow("so42043182")]
    [DataRow("sm42050014")]
    public async Task GetThumbInfoAsync_Fail(string videoId)
    {

    }
}
