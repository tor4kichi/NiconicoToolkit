using CommunityToolkit.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NiconicoToolkit.Ranking.Video;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiconicoToolkit.Tests
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
                var res = await _context.Video.Ranking.GetRankingAsync("ignore");
                if (res.IsSuccess is false)
                {
                    throw new Exception(res.Meta.ErrorCode);
                }
            });
        }


        [TestMethod]
        public async Task GetVideoRankingAsync()
        {
            var res = await _context.Video.Ranking.GetRankingAsync(RankingGenreConstants.All);

            Guard.IsTrue(res.Meta.IsSuccess);

            Guard.IsNotNull(res.Data);
            Guard.IsNotNull(res.Data.Response.GetTeibanRanking);

            if (res.Data.Response.GetTeibanRanking.Data.Items.Any())
            {
                var video = res.Data.Response.GetTeibanRanking.Data.Items[0];

                Guard.IsNotNull(video.Id);
                Guard.IsNotNull(video.Owner);
            }
        }


        [TestMethod]
        public async Task GetVideoRankingAndGenreAndTagAsync()
        {
            var res = await _context.Video.Ranking.GetRankingAsync(RankingGenreConstants.Game);

            Guard.IsTrue(res.Meta.IsSuccess);

            Guard.IsNotNull(res.Data);
            Guard.IsNotNull(res.Data.Response.GetTeibanRanking);

            if (res.Data.Response.GetTeibanRanking.Data.Items.Any())
            {
                var video = res.Data.Response.GetTeibanRanking.Data.Items[0];

                Guard.IsNotNull(video.Id);
                Guard.IsNotNull(video.Owner);
            }

            var tags = res.Data.Response.GetTeibanRankingFeaturedKeyAndTrendTags.Data.TrendTags;
            Guard.HasSizeNotEqualTo(tags, 0);
            var tagRes = await _context.Video.Ranking.GetRankingAsync(RankingGenreConstants.Game, tag: tags[0], pageCount: 2);
            Guard.IsNotNullOrEmpty(tagRes.Data.Response.GetTeibanRanking.Data.Tag);
        }
    }
}
