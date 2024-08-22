using CommunityToolkit.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NiconicoToolkit.FollowingsActivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiconicoToolkit.Tests;

[TestClass]
public sealed class FollowingsActivityTest
{
    NiconicoContext _context;


    [TestInitialize]
    public async Task Initialize()
    {
        (_context, _, _, _) = await AccountTestHelper.CreateNiconicoContextAndLogInWithTestAccountAsync();
    }

    [TestMethod]
    [DataRow(ActivityType.Publish)]
    [DataRow(ActivityType.Video)]
    [DataRow(ActivityType.Live)]
    [DataRow(ActivityType.All)]
    public async Task GetFollowingsActivityAsync(ActivityType type)
    {
        var res = await _context.FollowingsActivity.GetFollowingsActivityAsync(type, null, null);

        Assert.IsTrue(res.IsOk);

        foreach (var item in res.Activities.Take(3))
        {
            Assert.IsNotNull(item.Id);
            Assert.IsNotNull(item.Label.Text);
            Assert.IsNotNull(item.ThumbnailUrl);
            Assert.IsNotNull(item.Content);
            Assert.IsNotNull(item.Kind);
            Assert.AreNotEqual(default(DateTime), item.CreatedAt);
        }
    }

    [TestMethod]    
    public async Task GetFollowingsActivityAsync_NextCursor()
    {
        var type = ActivityType.Publish;
        var res = await _context.FollowingsActivity.GetFollowingsActivityAsync(type, null, null);
        await Task.Delay(3000);
        var res2 = await _context.FollowingsActivity.GetFollowingsActivityAsync(type, null, nextCursor: res.NextCursor);

        Guard.IsNotEqualTo(res.NextCursor, res2.NextCursor);
        Assert.IsTrue(res2.IsOk);
    }


    [TestMethod]
    public async Task GetFollowingsActivityAsync_FilteringWithActor()
    {
        var type = ActivityType.Publish;
        var res = await _context.FollowingsActivity.GetFollowingsActivityAsync(type, null, null);
        await Task.Delay(3000);
        var actor = new NiconicoActivityActor(ActivityActorType.User, res.Activities[0].Actor.Id);
        var res2 = await _context.FollowingsActivity.GetFollowingsActivityAsync(type, actor, nextCursor: res.NextCursor);

        Assert.IsTrue(res2.IsOk);
        foreach (var activity in res2.Activities.Take(3))
        {
            Assert.AreEqual(activity.Actor.Id, actor.ActorId.ToString());
        }
    }
}
