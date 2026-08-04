using Microsoft.VisualStudio.TestTools.UnitTesting;
using NiconicoToolkit.Video;
using NiconicoToolkit.Ranking.Video;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NiconicoToolkit.Rss.Video;
using System.Diagnostics;
using CommunityToolkit.Diagnostics;

namespace NiconicoToolkit.Tests
{
    [TestClass]
    public sealed class VideoTest
    {
        [TestInitialize]
        public void Initialize()
        {
            _context = new NiconicoContext(AccountTestHelper.Site);
            _context.SetupDefaultRequestHeaders();
            _videoClient = _context.Video;
        }

        NiconicoContext _context;
        VideoClient _videoClient;



    }
}
