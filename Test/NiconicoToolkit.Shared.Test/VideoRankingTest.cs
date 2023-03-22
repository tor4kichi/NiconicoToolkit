using Microsoft.VisualStudio.TestTools.UnitTesting;
using NiconicoToolkit.Ranking.Video;
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
        public async Task FailGetPopularGenreTagsOnAllAsync()
        {
            await Assert.ThrowsExceptionAsync<Exception>(async () => 
            {
                var res = await _context.Video.Ranking.GetPopularTagAsync(RankingGenre.All);
                if (res.IsSuccess is false)
                {
                    throw new Exception(res.Meta.ErrorCode);
                }
            });
        }

        [TestMethod]
        [DataRow(nameof(RankingGenre.Anime))]
        public async Task GetPopularGenreTagsAsync(string genre)
        {
            var res = await _context.Video.Ranking.GetPopularTagAsync(Enum.Parse<RankingGenre>(genre));

            Assert.IsTrue(res.Meta.IsSuccess);

            if (res.Data?.Tags.Any() ?? false)
            {
                var tag = res.Data.Tags[0];
                Assert.IsNotNull(tag);
            }
        }

        [TestMethod]        
        public async Task GetHotTopicTagsVideoRankingAsync()
        {
            var res = await _context.Video.Ranking.GetHotTopicAsync();

            Assert.IsTrue(res.Meta.IsSuccess);

            if (res.Data?.HotTopics.Any() ?? false)
            {
                var topic = res.Data.HotTopics[0];                
            }
        }


        [TestMethod]
        [DataRow(nameof(RankingGenre.All))]
        [DataRow(nameof(RankingGenre.Animal))]
        [DataRow(nameof(RankingGenre.HotTopic))]
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
