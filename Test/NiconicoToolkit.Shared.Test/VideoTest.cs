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

namespace NiconicoToolkit.UWP.Test.Tests
{
    [TestClass]
    public sealed class VideoTest
    {
        [TestInitialize]
        public void Initialize()
        {
            _context = new NiconicoContext(AccountTestHelper.Site);
            _videoClient = _context.Video;
        }

        NiconicoContext _context;
        VideoClient _videoClient;


        #region Ranking

        [TestMethod]
        [DataRow(RankingGenre.All)]
        [DataRow(RankingGenre.HotTopic)]
        [DataRow(RankingGenre.Anime)]
        public async Task GetRankingTagsAsync(RankingGenre genre)
        {
            var res = await _context.Video.Ranking.GetGenrePickedTagAsync(genre);            
            foreach (var tag in res)
            {
                Assert.IsTrue(!string.IsNullOrWhiteSpace(tag.DisplayName));
                Assert.IsNotNull(tag.Tag);
            }

            if (genre is not RankingGenre.All)
            {
                Assert.AreEqual(res.Count(x => x.IsDefaultTag), 1);
            }
        }



        [TestMethod]
        [DataRow(RankingGenre.All)]
        [DataRow(RankingGenre.HotTopic)]
        [DataRow(RankingGenre.Anime)]
        public async Task GetRankingRssItemsAsync(RankingGenre genre)
        {
            var res = await _context.Video.Ranking.GetRankingRssAsync(genre);

            Assert.IsTrue(res.IsOK);
            Assert.IsTrue(res.Items.Any());

            foreach (var item in res.Items.Take(3))
            {
                Assert.IsNotNull(item.Description);
                Assert.IsNotNull(item.RawTitle);
                
                var moreData = item.GetMoreData();

                Assert.AreNotEqual(default(TimeSpan), moreData.Length);
                Assert.IsTrue(!string.IsNullOrWhiteSpace(moreData.Title));
                Assert.IsTrue(!string.IsNullOrWhiteSpace(moreData.ThumbnailUrl));
                Assert.IsTrue(moreData.WatchCount > 0);
                Assert.AreNotEqual(default(DateTime), moreData.PostedAt);
            }
        }


        [TestMethod]
        [DataRow(RankingGenre.HotTopic)]
        [DataRow(RankingGenre.Anime)]
        public async Task GetRankingRssItemsWithTagAsync(RankingGenre genre)
        {
            var tagsRes = await _context.Video.Ranking.GetGenrePickedTagAsync(genre);

            var tag = tagsRes.Skip(1).FirstOrDefault();
            var res = await _context.Video.Ranking.GetRankingRssAsync(genre, tag.Tag);

            Assert.IsTrue(res.IsOK);
            Assert.IsTrue(res.Items.Any());

            foreach (var item in res.Items.Take(3))
            {
                Assert.IsNotNull(item.Description);
                Assert.IsNotNull(item.RawTitle);

                var moreData = item.GetMoreData();

                Assert.AreNotEqual(default(TimeSpan), moreData.Length);
                Assert.IsTrue(!string.IsNullOrWhiteSpace(moreData.Title));
                Assert.IsTrue(!string.IsNullOrWhiteSpace(moreData.ThumbnailUrl));
                Assert.IsTrue(moreData.WatchCount > 0);
                Assert.AreNotEqual(default(DateTime), moreData.PostedAt);
            }
        }

        #endregion Ranking



        #region Web Ranking


        [TestMethod]
        [DataRow(RankingGenre.All)]
        [DataRow(RankingGenre.HotTopic)]
        [DataRow(RankingGenre.Other)]
        public async Task GetRankingItemsFromWebRankingPageAsync(RankingGenre genre)
        {
            var res = await _context.Video.Ranking.GetRankingFromWebAsync(genre);

            Guard.IsTrue(res.IsSuccess, nameof(res.IsSuccess));
            Guard.IsTrue(res.Items.Any(), nameof(res.Items));

            foreach (var item in res.Items.Take(3))
            {
                Guard.IsNotNullOrEmpty(item.VideoId, nameof(item.VideoId));
                Guard.IsNotNullOrEmpty(item.OwnerId, nameof(item.OwnerId));
                if (item.IsSensitiveContent is false)
                {
                    Guard.IsNotNull(item.Description);
                    Guard.IsNotEqualTo(item.Duration, default(TimeSpan));
                    Guard.IsTrue(!string.IsNullOrWhiteSpace(item.Title));
                    Guard.IsTrue(!string.IsNullOrWhiteSpace(item.Thumbnail));
                    Guard.IsTrue(item.ViewCount > 0);
                    Guard.IsNotEqualTo(item.RegisteredAt, default(DateTime));
                }
            }
        }

        [TestMethod]
        [DataRow(RankingGenre.HotTopic)]
        [DataRow(RankingGenre.Anime)]
        public async Task GetRankingItemsWithTagFromWebRankingPageAsync(RankingGenre genre)
        {
            var tagsRes = await _context.Video.Ranking.GetGenrePickedTagAsync(genre);

            var tag = tagsRes.Skip(1).FirstOrDefault();
            var res = await _context.Video.Ranking.GetRankingFromWebAsync(genre, tag.Tag);

            Assert.IsTrue(res.IsSuccess);
            Assert.IsTrue(res.Items.Any());

            foreach (var item in res.Items.Take(3))
            {
                Assert.IsNotNull(item.Description);

                Assert.AreNotEqual(default(TimeSpan), item.Duration);
                Assert.IsTrue(!string.IsNullOrWhiteSpace(item.Title));
                Assert.IsTrue(!string.IsNullOrWhiteSpace(item.Thumbnail));
                Assert.IsTrue(item.ViewCount > 0);
                Assert.AreNotEqual(default(DateTime), item.RegisteredAt);
            }
        }
        #endregion

    }
}
