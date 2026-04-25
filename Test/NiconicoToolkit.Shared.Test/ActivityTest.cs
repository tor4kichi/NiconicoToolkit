using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiconicoToolkit.Tests
{
    [TestClass]
    public sealed class ActivityTest
    {
        NiconicoContext _context;


        [TestInitialize]
        public async Task Initialize()
        {
            (_context, _, _, _) = await AccountTestHelper.CreateNiconicoContextAndLogInWithTestAccountAsync();
        }

        [TestMethod]
        public async Task GetVideoWatchHitoryAsync()
        {
            var res = await _context.History.VideoWachHistory.GetWatchHistoryAsync(6);

            var res2 = await _context.History.VideoWachHistory.GetWatchHistoryAsync(6, res);
            Assert.IsTrue(res.Meta.IsSuccess);

            if (res.Data.Items.Count > 0)
            {
                var item = res.Data.Items[0];
                Assert.IsNotNull(item.Video);
                Assert.IsNotNull(item.IsMaybeLikeUserItem);
                Assert.AreNotEqual(item.ViewedAt, default(DateTimeOffset));
            }

            Assert.AreNotEqual(res.Data.Items[0].Video.Id, res2.Data.Items[0].Video.Id);


        }

        [TestMethod]
        public async Task GetShortVideoWatchHitoryAsync()
        {
            var res = await _context.History.VideoWachHistory.GetShortVideoWatchHistoryAsync(6);

            Assert.IsTrue(res.Meta.IsSuccess);

            if (res.Data.Items.Count > 0)
            {
                var item = res.Data.Items[0];
                Assert.IsNotNull(item.Video);
                Assert.IsNotNull(item.IsMaybeLikeUserItem);
                Assert.AreNotEqual(item.ViewedAt, default(DateTimeOffset));
            }
        }
    }
}
