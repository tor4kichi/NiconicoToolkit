using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiconicoToolkit.UWP.Test.Tests
{
    [TestClass]
    public sealed class VideoRankingTest
    {
        [TestInitialize]
        public void Initialize()
        {
            _context = new NiconicoContext(AccountTestHelper.Site);
            _context.SetupDefaultRequestHeaders();
        }

        NiconicoContext _context;

        [TestMethod]
        [DataRow("all")]
        public async Task GetVideoRankingAsync(string genre)
        {
            var res = await _context.Video.Ranking.GetRankingAsync(Enum.Parse<Ranking.Video.RankingGenre>(genre));

            Assert.IsTrue(res.Meta.IsSuccess);

            Assert.IsNotNull(res.Data);
            Assert.IsNotNull(res.Data.Items);

            if (res.Data.Items.Any())
            {
                var mylist = res.Data.Items[0];

                Assert.IsNotNull(mylist.Id);
                Assert.IsNotNull(mylist.Owner);
            }
        }
    }
}
