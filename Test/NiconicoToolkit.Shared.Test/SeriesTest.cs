using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiconicoToolkit.UWP.Test.Tests
{
    [TestClass]
    public sealed class SeriesTest
    {
        private NiconicoContext _context;

        [TestInitialize]
        public void Initialize()
        {
            _context = new NiconicoContext(AccountTestHelper.Site);
            _context.SetupDefaultRequestHeaders();
        }


        [TestMethod]
        [DataRow("76015")] // ユーザーのシリーズ
        [DataRow("225544")] // chのシリーズ
        public async Task GetSeriesVideoAsync(string seriesId)
        {
            var res = await _context.Series.GetUserSeriesVideosAsync(seriesId);
            Assert.IsNotNull(res.Data?.Items);

            foreach (var video in res.Data.Items.Take(3))
            {
                Assert.IsNotNull(video.Video.Id, "video.Id is null");
                Assert.IsNotNull(video.Video.Title, "video.Title is null");
                Assert.AreNotEqual(TimeSpan.Zero, video.Video.Duration, "video.Duration is TimeSpan.Zero");
            }

            Assert.IsNotNull(res.Data.Detail.Title);
        }


        [TestMethod]
        [DataRow(53842185)] // ユーザー
        [DataRow(225544)] // chのシリーズ
        public async Task GetUserSeriesAsync(int userId)
        {
            var res = await _context.Series.GetUserSeriesAsync(userId);
            Assert.IsTrue(res.IsSuccess);

            Assert.IsNotNull(res.Data);
            Assert.IsNotNull(res.Data.Items);
        }
    }
}
