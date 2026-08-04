using System.Collections.Generic;

namespace NiconicoToolkit.Ranking.Video;

public static class VideoRankingConstants
{
    public const string NiconicoRankingGenreDomain = "https://www.nicovideo.jp/ranking/genre/";

    public const int MaxPage = 10;
    public const int MaxPageWithTag = 3;
    public const int MaxPageHotTopic = 3;
    public const int MaxPageHotTopicWithKey = 1;

    public const int ItemsCountPerPage = 100;

    public static readonly RankingTerm[] AllRankingTerms = new[]
    {
            RankingTerm.Hour,
            RankingTerm.Day,
            RankingTerm.Week,
            RankingTerm.Month,
            RankingTerm.Total
    };


    public static readonly RankingTerm[] HotTopicAccepteRankingTerms = new[]
    {
            RankingTerm.Hour,
            RankingTerm.Day
    };

    public static readonly RankingTerm[] GenreWithTagAccepteRankingTerms = new[]
    {
            RankingTerm.Hour,
            RankingTerm.Day
    };        
}


public sealed record GenreInfo(string Id, string Name);

public static class RankingGenreConstants
{
    public static readonly GenreInfo All = new("e9uj2uks", "総合");
    public static readonly GenreInfo Game = new("4eet3ca4", "ゲーム");
    public static readonly GenreInfo Anime = new("zc49b03a", "アニメ");
    public static readonly GenreInfo Vocaloid = new("dshv5do5", "ボカロ");
    public static readonly GenreInfo VoiceSynthesis = new("wnm2mhv0", "音声合成実況・解説・劇場");
    public static readonly GenreInfo Entertainment = new("8kjl94d9", "エンタメ");
    public static readonly GenreInfo Music = new("wq76qdin", "音楽");
    public static readonly GenreInfo Utattemita = new("1ya6bnqd", "歌ってみた");
    public static readonly GenreInfo Odottemita = new("6yuf530c", "踊ってみた");
    public static readonly GenreInfo Ensouttemita = new("6r5jr8nd", "演奏してみた");
    public static readonly GenreInfo CommentaryCourse = new("v6wdx6p5", "解説・講座");
    public static readonly GenreInfo Cooking = new("lq8d5918", "料理");
    public static readonly GenreInfo TravelOutdoor = new("k1libcse", "旅行・アウトドア");
    public static readonly GenreInfo Nature = new("24aa8fkw", "自然");
    public static readonly GenreInfo Vehicle = new("3d8zlls9", "乗り物");
    public static readonly GenreInfo TechnologyCraft = new("n46kcz9u", "技術・工作");
    public static readonly GenreInfo SocietyPoliticsNews = new("lzicx0y6", "社会・政治・時事");
    public static readonly GenreInfo Mmd = new("p1acxuoz", "MMD");
    public static readonly GenreInfo VTuber = new("6mkdo4xd", "VTuber");
    public static readonly GenreInfo Radio = new("oxzi6bje", "ラジオ");
    public static readonly GenreInfo Sports = new("4w3p65pf", "スポーツ");
    public static readonly GenreInfo Animal = new("ne72lua2", "動物");
    public static readonly GenreInfo Other = new("ramuboyn", "その他");

    /// <summary>
    /// 全ジャンルのリスト
    /// </summary>
    public static IReadOnlyList<GenreInfo> AllGenres { get; } = new[]
    {
        All, Game, Anime, Vocaloid, VoiceSynthesis, Entertainment,
        Music, Utattemita, Odottemita, Ensouttemita, CommentaryCourse,
        Cooking, TravelOutdoor, Nature, Vehicle, TechnologyCraft,
        SocietyPoliticsNews, Mmd, VTuber, Radio, Sports, Animal, Other
    };
}
