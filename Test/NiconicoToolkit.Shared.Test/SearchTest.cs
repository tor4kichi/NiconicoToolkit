using Microsoft.VisualStudio.TestTools.UnitTesting;
using NiconicoToolkit.Live;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NiconicoToolkit.UWP.Test.Tests
{
    [TestClass]
    public sealed class SearchTest
    {
        [TestInitialize]
        public async Task Initialize()
        {
            var creationResult = await AccountTestHelper.CreateNiconicoContextAndLogInWithTestAccountAsync();
            _context = creationResult.niconicoContext;
            _searchClient = _context.Search;
        }

        NiconicoContext _context;
        Search.SearchClient _searchClient;


        [TestMethod]
        [DataRow("モンハン")]
        public async Task VideoKeywordSearchAsync(string keyword)
        {
            var res = await _searchClient.Video.VideoSearchAsync(keyword);

            Assert.IsTrue(res.Meta.IsSuccess);

            Assert.IsNotNull(res.Data);
            Assert.IsNotNull(res.Data.Items);

            if (res.Data.Items.Any())
            {
                var item = res.Data.Items[0];

                Assert.IsNotNull(item);
                Assert.IsNotNull(item.Title);
                Assert.IsNotNull(item.Owner);
            }
        }

        [TestMethod]
        [DataRow("アニメ")]
        public async Task VideoTagSearchAsync(string keyword)
        {
            var res = await _searchClient.Video.VideoSearchAsync(keyword, true);

            Assert.IsTrue(res.Meta.IsSuccess);

            Assert.IsNotNull(res.Data);
            Assert.IsNotNull(res.Data.Items);

            if (res.Data.Items.Any())
            {
                var item = res.Data.Items[0];

                Assert.IsNotNull(item);
                Assert.IsNotNull(item.Title);
                Assert.IsNotNull(item.Owner);
            }
        }

        [TestMethod]
        [DataRow("ニコニコ")]
        public async Task UserSearchAsync(string keyword)
        {
            var res = await _searchClient.User.UserSearchAsync(keyword);

            Assert.IsTrue(res.Meta.IsSuccess);

            Assert.IsNotNull(res.Data);
            Assert.IsNotNull(res.Data.Items);

            if (res.Data.Items.Any())
            {
                var item = res.Data.Items[0];

                Assert.IsNotNull(item);
                Assert.IsNotNull(item.Id);
                Assert.IsNotNull(item.Nickname);
            }
        }
    }
}
