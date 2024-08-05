using System.Collections.Generic;
using System;
using System.Text.Json.Serialization;

namespace NiconicoToolkit.Video.Watch;

public class NicoVideoWatchApiResponse : ResponseWithData<NicoVideoWatchApiResponse.NicoVideoWatchApiData>
{
    // Root myDeserializedClass = JsonSerializer.Deserialize<Root>(myJsonResponse);
    public class AdditionalParams
    {
        [JsonPropertyName("videoId")]
        public string VideoId { get; set; }

        [JsonPropertyName("videoDuration")]
        public int? VideoDuration { get; set; }

        [JsonPropertyName("isAdultRatingNG")]
        public bool? IsAdultRatingNG { get; set; }

        [JsonPropertyName("isAuthenticationRequired")]
        public bool? IsAuthenticationRequired { get; set; }

        [JsonPropertyName("isR18")]
        public bool? IsR18 { get; set; }

        [JsonPropertyName("nicosid")]
        public string Nicosid { get; set; }

        [JsonPropertyName("lang")]
        public string Lang { get; set; }

        [JsonPropertyName("watchTrackId")]
        public string WatchTrackId { get; set; }

        [JsonPropertyName("linearType")]
        public string LinearType { get; set; }

        [JsonPropertyName("adIdx")]
        public int? AdIdx { get; set; }

        [JsonPropertyName("skipType")]
        public int? SkipType { get; set; }

        [JsonPropertyName("skippableType")]
        public int? SkippableType { get; set; }

        [JsonPropertyName("pod")]
        public int? Pod { get; set; }
    }

    public class Admission
    {
        [JsonPropertyName("isEnabled")]
        public bool? IsEnabled { get; set; }
    }

    public class Audio
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("isAvailable")]
        public bool? IsAvailable { get; set; }

        [JsonPropertyName("bitRate")]
        public int? BitRate { get; set; }

        [JsonPropertyName("samplingRate")]
        public int? SamplingRate { get; set; }

        [JsonPropertyName("integratedLoudness")]
        public double? IntegratedLoudness { get; set; }

        [JsonPropertyName("truePeak")]
        public double? TruePeak { get; set; }

        [JsonPropertyName("qualityLevel")]
        public int? QualityLevel { get; set; }

        [JsonPropertyName("loudnessCollection")]
        public List<LoudnessCollection> LoudnessCollection { get; set; }
    }

    public class Author
    {
        [JsonPropertyName("@type")]
        public string Type { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }
    }

    public class Client
    {
        [JsonPropertyName("nicosid")]
        public string Nicosid { get; set; }

        [JsonPropertyName("watchId")]
        public string WatchId { get; set; }

        [JsonPropertyName("watchTrackId")]
        public string WatchTrackId { get; set; }
    }

    public class Comment
    {
        [JsonPropertyName("server")]
        public Server Server { get; set; }

        [JsonPropertyName("keys")]
        public Keys Keys { get; set; }

        [JsonPropertyName("layers")]
        public List<Layer> Layers { get; set; }

        [JsonPropertyName("threads")]
        public List<Thread> Threads { get; set; }

        [JsonPropertyName("ng")]
        public Ng Ng { get; set; }

        [JsonPropertyName("isAttentionRequired")]
        public bool? IsAttentionRequired { get; set; }

        [JsonPropertyName("nvComment")]
        public NvComment NvComment { get; set; }

        [JsonPropertyName("isDefaultInvisible")]
        public bool? IsDefaultInvisible { get; set; }
    }

    public class Commons
    {
        [JsonPropertyName("hasContentTree")]
        public bool? HasContentTree { get; set; }
    }

    public class Content
    {
        [JsonPropertyName("player_type")]
        public string PlayerType { get; set; }

        [JsonPropertyName("genre")]
        public string Genre { get; set; }

        [JsonPropertyName("content_type")]
        public string ContentType { get; set; }
    }

    public class ContinuationBenefit
    {
        [JsonPropertyName("isEnabled")]
        public bool? IsEnabled { get; set; }
    }

    public class Count
    {
        [JsonPropertyName("view")]
        public int? View { get; set; }

        [JsonPropertyName("comment")]
        public int? Comment { get; set; }

        [JsonPropertyName("mylist")]
        public int? Mylist { get; set; }

        [JsonPropertyName("like")]
        public int? Like { get; set; }
    }

    public class NicoVideoWatchApiData
    {
        [JsonPropertyName("metadata")]
        public Metadata Metadata { get; set; }

        [JsonPropertyName("response")]
        public Response Response { get; set; }
    }

    public class Domand
    {
        [JsonPropertyName("videos")]
        public List<Video> Videos { get; set; }

        [JsonPropertyName("audios")]
        public List<Audio> Audios { get; set; }

        [JsonPropertyName("isStoryboardAvailable")]
        public bool? IsStoryboardAvailable { get; set; }

        [JsonPropertyName("accessRightKey")]
        public string AccessRightKey { get; set; }
    }

    public class EasyComment
    {
        [JsonPropertyName("phrases")]
        public List<Phrase> Phrases { get; set; }
    }

    public class Edit
    {
        [JsonPropertyName("isEditable")]
        public bool? IsEditable { get; set; }

        [JsonPropertyName("uneditableReason")]
        public string UneditableReason { get; set; }

        [JsonPropertyName("editKey")]
        public object EditKey { get; set; }
    }

    public class External
    {
        [JsonPropertyName("commons")]
        public Commons Commons { get; set; }

        [JsonPropertyName("ichiba")]
        public Ichiba Ichiba { get; set; }
    }

    public class Genre
    {
        [JsonPropertyName("key")]
        public string Key { get; set; }

        [JsonPropertyName("label")]
        public string Label { get; set; }

        [JsonPropertyName("isImmoral")]
        public bool? IsImmoral { get; set; }

        [JsonPropertyName("isDisabled")]
        public bool? IsDisabled { get; set; }

        [JsonPropertyName("isNotSet")]
        public bool? IsNotSet { get; set; }
    }

    public class Ichiba
    {
        [JsonPropertyName("isEnabled")]
        public bool? IsEnabled { get; set; }
    }

    public class Information
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }
    }

    public class InteractionStatistic
    {
        [JsonPropertyName("@type")]
        public string Type { get; set; }

        [JsonPropertyName("interactionType")]
        public string InteractionType { get; set; }

        [JsonPropertyName("userInteractionCount")]
        public int? UserInteractionCount { get; set; }
    }

    public class Item
    {
        [JsonPropertyName("@id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public class Item2
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("isCategory")]
        public bool? IsCategory { get; set; }

        [JsonPropertyName("isCategoryCandidate")]
        public bool? IsCategoryCandidate { get; set; }

        [JsonPropertyName("isNicodicArticleExists")]
        public bool? IsNicodicArticleExists { get; set; }

        [JsonPropertyName("isLocked")]
        public bool? IsLocked { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("timingMs")]
        public object TimingMs { get; set; }

        [JsonPropertyName("additionalParams")]
        public AdditionalParams AdditionalParams { get; set; }
    }

    public class ItemListElement
    {
        [JsonPropertyName("@type")]
        public string Type { get; set; }

        [JsonPropertyName("position")]
        public int? Position { get; set; }

        [JsonPropertyName("item")]
        public Item Item { get; set; }
    }

    public class JsonLd
    {
        [JsonPropertyName("@context")]
        public string Context { get; set; }

        [JsonPropertyName("@type")]
        public string Type { get; set; }

        [JsonPropertyName("@id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("caption")]
        public string Caption { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("duration")]
        public string Duration { get; set; }

        [JsonPropertyName("uploadDate")]
        public DateTime? UploadDate { get; set; }

        [JsonPropertyName("embedUrl")]
        public string EmbedUrl { get; set; }

        [JsonPropertyName("interactionStatistic")]
        public List<InteractionStatistic> InteractionStatistic { get; set; }

        [JsonPropertyName("thumbnail")]
        public List<Thumbnail> Thumbnail { get; set; }

        [JsonPropertyName("thumbnailUrl")]
        public List<string> ThumbnailUrl { get; set; }

        [JsonPropertyName("requiresSubscription")]
        public bool? RequiresSubscription { get; set; }

        [JsonPropertyName("isAccessibleForFree")]
        public bool? IsAccessibleForFree { get; set; }

        [JsonPropertyName("commentCount")]
        public int? CommentCount { get; set; }

        [JsonPropertyName("keywords")]
        public string Keywords { get; set; }

        [JsonPropertyName("playerType")]
        public string PlayerType { get; set; }

        [JsonPropertyName("provider")]
        public Provider Provider { get; set; }

        [JsonPropertyName("author")]
        public Author Author { get; set; }

        [JsonPropertyName("itemListElement")]
        public List<ItemListElement> ItemListElement { get; set; }
    }

    public class Keys
    {
        [JsonPropertyName("userKey")]
        public string UserKey { get; set; }
    }

    public class Layer
    {
        [JsonPropertyName("index")]
        public int? Index { get; set; }

        [JsonPropertyName("isTranslucent")]
        public bool? IsTranslucent { get; set; }

        [JsonPropertyName("threadIds")]
        public List<ThreadId> ThreadIds { get; set; }
    }

    public class LinkTag
    {
        [JsonPropertyName("rel")]
        public string Rel { get; set; }

        [JsonPropertyName("href")]
        public string Href { get; set; }

        [JsonPropertyName("attrs")]
        public object Attrs { get; set; }
    }

    public class LoudnessCollection
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("value")]
        public double? Value { get; set; }
    }

    public class Marquee
    {
        [JsonPropertyName("isDisabled")]
        public bool? IsDisabled { get; set; }

        [JsonPropertyName("tagRelatedLead")]
        public object TagRelatedLead { get; set; }
    }

    public class Media
    {
        [JsonPropertyName("domand")]
        public Domand Domand { get; set; }

        [JsonPropertyName("delivery")]
        public Delivery Delivery { get; set; }
    }

    public partial class Delivery
    {
        [JsonPropertyName("recipeId")]
        public string RecipeId { get; set; }

        [JsonPropertyName("encryption")]
        public Encryption Encryption { get; set; }

        [JsonPropertyName("movie")]
        public Movie Movie { get; set; }

        [JsonPropertyName("storyboard")]
        public object Storyboard { get; set; }

        [JsonPropertyName("trackingId")]
        public string TrackingId { get; set; }
    }


    public class Encryption
    {
        [JsonPropertyName("encryptedKey")]
        public string EncryptedKey { get; set; }

        [JsonPropertyName("keyUri")]
        public string KeyUri { get; set; }
    }

    public partial class Movie
    {
        [JsonPropertyName("contentId")]
        public string ContentId { get; set; }

        [JsonPropertyName("audios")]
        public Audio[] Audios { get; set; }

        [JsonPropertyName("videos")]
        public Video[] Videos { get; set; }

        [JsonPropertyName("session")]
        public Session Session { get; set; }
    }

    public partial class Session
    {
        [JsonPropertyName("recipeId")]
        public string RecipeId { get; set; }

        [JsonPropertyName("playerId")]
        public string PlayerId { get; set; }

        [JsonPropertyName("videos")]
        public string[] Videos { get; set; }

        [JsonPropertyName("audios")]
        public string[] Audios { get; set; }

        [JsonPropertyName("movies")]
        public object[] Movies { get; set; }

        [JsonPropertyName("protocols")]
        public string[] Protocols { get; set; }

        [JsonPropertyName("authTypes")]
        public AuthTypes AuthTypes { get; set; }

        [JsonPropertyName("serviceUserId")]
        public string ServiceUserId { get; set; }

        [JsonPropertyName("token")]
        public string Token { get; set; }

        [JsonPropertyName("signature")]
        public string Signature { get; set; }

        [JsonPropertyName("contentId")]
        public string ContentId { get; set; }

        [JsonPropertyName("heartbeatLifetime")]
        public long HeartbeatLifetime { get; set; }

        [JsonPropertyName("contentKeyTimeout")]
        public long ContentKeyTimeout { get; set; }

        [JsonPropertyName("priority")]
        public double Priority { get; set; }

        [JsonPropertyName("transferPresets")]
        public object[] TransferPresets { get; set; }

        [JsonPropertyName("urls")]
        public Url[] Urls { get; set; }
    }


    public partial class AuthTypes
    {
        [JsonPropertyName("http")]
        public string Http { get; set; }

        [JsonPropertyName("hls")]
        public string Hls { get; set; }
    }

    public partial class Url
    {
        [JsonPropertyName("url")]
        public Uri UrlUrl { get; set; }

        [JsonPropertyName("isWellKnownPort")]
        public bool IsWellKnownPort { get; set; }

        [JsonPropertyName("isSsl")]
        public bool IsSsl { get; set; }

        public string UrlUnsafe => "https://api.dmc.nico/api/sessions";
    }

    public class Meta
    {
        [JsonPropertyName("status")]
        public int? Status { get; set; }

        [JsonPropertyName("code")]
        public string Code { get; set; }
    }

    public class Metadata
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("linkTags")]
        public List<LinkTag> LinkTags { get; set; }

        [JsonPropertyName("metaTags")]
        public List<MetaTag> MetaTags { get; set; }

        [JsonPropertyName("jsonLds")]
        public List<JsonLd> JsonLds { get; set; }
    }

    public class MetaTag
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("content")]
        public string Content { get; set; }

        [JsonPropertyName("property")]
        public string Property { get; set; }
    }

    public class Ng
    {
        [JsonPropertyName("ngScore")]
        public NgScore NgScore { get; set; }

        [JsonPropertyName("channel")]
        public List<object> Channel { get; set; }

        [JsonPropertyName("owner")]
        public List<object> Owner { get; set; }

        [JsonPropertyName("viewer")]
        public object Viewer { get; set; }
    }

    public class NgScore
    {
        [JsonPropertyName("isDisabled")]
        public bool? IsDisabled { get; set; }
    }

    public class Nicodic
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("viewTitle")]
        public string ViewTitle { get; set; }

        [JsonPropertyName("summary")]
        public string Summary { get; set; }

        [JsonPropertyName("link")]
        public string Link { get; set; }
    }

    public class Niconico
    {
        [JsonPropertyName("user")]
        public User User { get; set; }

        [JsonPropertyName("content")]
        public Content Content { get; set; }
    }

    public class NvComment
    {
        [JsonPropertyName("threadKey")]
        public string ThreadKey { get; set; }

        [JsonPropertyName("server")]
        public string Server { get; set; }

        [JsonPropertyName("params")]
        public Params Params { get; set; }
    }

    public class VideoOwner
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("nickname")]
        public string Nickname { get; set; }

        [JsonPropertyName("iconUrl")]
        public string IconUrl { get; set; }

        [JsonPropertyName("channel")]
        public VideoOwnerChannel Channel { get; set; }

        [JsonPropertyName("live")]
        public object Live { get; set; }

        [JsonPropertyName("isVideosPublic")]
        public bool? IsVideosPublic { get; set; }

        [JsonPropertyName("isMylistsPublic")]
        public bool? IsMylistsPublic { get; set; }

        [JsonPropertyName("videoLiveNotice")]
        public object VideoLiveNotice { get; set; }

        [JsonPropertyName("viewer")]
        public OwnerViewer Viewer { get; set; }
    }

    public partial class OwnerViewer
    {
        [JsonPropertyName("isFollowing")]
        public bool IsFollowing { get; set; }
    }

    public partial class VideoOwnerChannel
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("url")]
        public Uri Url { get; set; }
    }

    public class Params
    {
        [JsonPropertyName("targets")]
        public List<Target> Targets { get; set; }

        [JsonPropertyName("language")]
        public string Language { get; set; }
    }

    public class Payment
    {
        [JsonPropertyName("video")]
        public PaymentVideo Video { get; set; }

        [JsonPropertyName("preview")]
        public Preview Preview { get; set; }
    }

    public partial class PaymentVideo
    {
        [JsonPropertyName("isPpv")]
        public bool IsPpv { get; set; }

        [JsonPropertyName("isAdmission")]
        public bool IsAdmission { get; set; }

        [JsonPropertyName("isPremium")]
        public bool IsPremium { get; set; }

        [JsonPropertyName("watchableUserType")]
        public string WatchableUserType { get; set; }

        [JsonPropertyName("commentableUserType")]
        public string CommentableUserType { get; set; }
    }

    public class PcWatchPage
    {
        [JsonPropertyName("tagRelatedBanner")]
        public object TagRelatedBanner { get; set; }

        [JsonPropertyName("videoEnd")]
        public VideoEnd VideoEnd { get; set; }

        [JsonPropertyName("showOwnerMenu")]
        public bool? ShowOwnerMenu { get; set; }

        [JsonPropertyName("showOwnerThreadCoEditingLink")]
        public bool? ShowOwnerThreadCoEditingLink { get; set; }

        [JsonPropertyName("showMymemoryEditingLink")]
        public bool? ShowMymemoryEditingLink { get; set; }
    }

    public class Phrase
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }

        [JsonPropertyName("nicodic")]
        public Nicodic Nicodic { get; set; }
    }

    public class Player
    {
        [JsonPropertyName("initialPlayback")]
        public object InitialPlayback { get; set; }

        [JsonPropertyName("comment")]
        public Comment Comment { get; set; }

        [JsonPropertyName("layerMode")]
        public int? LayerMode { get; set; }
    }

    public class Ppv
    {
        [JsonPropertyName("isEnabled")]
        public bool? IsEnabled { get; set; }
    }

    public class Premium
    {
        [JsonPropertyName("isEnabled")]
        public bool? IsEnabled { get; set; }
    }

    public class Preview
    {
        [JsonPropertyName("ppv")]
        public Ppv Ppv { get; set; }

        [JsonPropertyName("admission")]
        public Admission Admission { get; set; }

        [JsonPropertyName("continuationBenefit")]
        public ContinuationBenefit ContinuationBenefit { get; set; }

        [JsonPropertyName("premium")]
        public Premium Premium { get; set; }
    }

    public class Provider
    {
        [JsonPropertyName("@type")]
        public string Type { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public class Ranking
    {
        [JsonPropertyName("genre")]
        public object Genre { get; set; }

        [JsonPropertyName("popularTag")]
        public List<object> PopularTag { get; set; }
    }

    public class Rating
    {
        [JsonPropertyName("isAdult")]
        public bool? IsAdult { get; set; }
    }

    public partial class Channel
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("isOfficialAnime")]
        public bool IsOfficialAnime { get; set; }

        [JsonPropertyName("isDisplayAdBanner")]
        public bool IsDisplayAdBanner { get; set; }

        [JsonPropertyName("thumbnail")]
        public ChannelThumbnail Thumbnail { get; set; }

        [JsonPropertyName("viewer")]
        public ChannelViewer Viewer { get; set; }
    }

    public partial class ChannelThumbnail
    {
        [JsonPropertyName("url")]
        public Uri Url { get; set; }

        [JsonPropertyName("smallUrl")]
        public Uri SmallUrl { get; set; }
    }

    public partial class ChannelViewer
    {
        [JsonPropertyName("follow")]
        public Follow Follow { get; set; }
    }

    public partial class Follow
    {
        [JsonPropertyName("isFollowed")]
        public bool IsFollowed { get; set; }

        [JsonPropertyName("isBookmarked")]
        public bool IsBookmarked { get; set; }

        [JsonPropertyName("token")]
        public string Token { get; set; }

        [JsonPropertyName("tokenTimestamp")]
        public int TokenTimestamp { get; set; }
    }

    public class Response
    {
        [JsonPropertyName("ads")]
        public object Ads { get; set; }

        [JsonPropertyName("category")]
        public object Category { get; set; }

        [JsonPropertyName("channel")]
        public Channel Channel { get; set; }

        [JsonPropertyName("client")]
        public Client Client { get; set; }

        [JsonPropertyName("comment")]
        public Comment Comment { get; set; }

        [JsonPropertyName("easyComment")]
        public EasyComment EasyComment { get; set; }

        [JsonPropertyName("external")]
        public External External { get; set; }

        [JsonPropertyName("genre")]
        public Genre Genre { get; set; }

        [JsonPropertyName("marquee")]
        public Marquee Marquee { get; set; }

        [JsonPropertyName("media")]
        public Media Media { get; set; }

        [JsonPropertyName("okReason")]
        public string OkReason { get; set; }

        [JsonPropertyName("owner")]
        public VideoOwner Owner { get; set; }

        [JsonPropertyName("payment")]
        public Payment Payment { get; set; }

        [JsonPropertyName("pcWatchPage")]
        public PcWatchPage PcWatchPage { get; set; }

        [JsonPropertyName("player")]
        public Player Player { get; set; }

        //[JsonPropertyName("ppv")]
        //public object Ppv { get; set; }

        [JsonPropertyName("ranking")]
        public Ranking Ranking { get; set; }

        [JsonPropertyName("series")]
        public WatchApiSeries Series { get; set; }

        //[JsonPropertyName("smartphone")]
        //public object Smartphone { get; set; }

        [JsonPropertyName("system")]
        public System System { get; set; }

        [JsonPropertyName("tag")]
        public Tag Tag { get; set; }

        [JsonPropertyName("video")]
        public Video2 Video { get; set; }

        [JsonPropertyName("videoAds")]
        public VideoAds VideoAds { get; set; }

        [JsonPropertyName("videoLive")]
        public object VideoLive { get; set; }

        [JsonPropertyName("viewer")]
        public VideoViewer Viewer { get; set; }

        [JsonPropertyName("waku")]
        public Waku Waku { get; set; }
    }

    public partial class VideoViewer
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("nickname")]
        public string Nickname { get; set; }

        [JsonPropertyName("isPremium")]
        public bool IsPremium { get; set; }

        [JsonPropertyName("existence")]
        public Existence Existence { get; set; }
    }

    public partial class Existence
    {
        [JsonPropertyName("age")]
        public int Age { get; set; }

        [JsonPropertyName("prefecture")]
        public string Prefecture { get; set; }

        [JsonPropertyName("sex")]
        public string Sex { get; set; }
    }

    public partial class WatchApiSeries
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("thumbnailUrl")]
        public Uri ThumbnailUrl { get; set; }

        [JsonPropertyName("video")]
        public SeriesVideo Video { get; set; }
    }

    public partial class SeriesVideo
    {
        [JsonPropertyName("prev")]
        public NvapiVideoItem Prev { get; set; }

        [JsonPropertyName("next")]
        public NvapiVideoItem Next { get; set; }

        [JsonPropertyName("first")]
        public NvapiVideoItem First { get; set; }
    }


    public class Server
    {
        [JsonPropertyName("url")]
        public string Url { get; set; }
    }

    public class System
    {
        [JsonPropertyName("serverTime")]
        public DateTime? ServerTime { get; set; }

        [JsonPropertyName("isPeakTime")]
        public bool? IsPeakTime { get; set; }

        [JsonPropertyName("isStellaAlive")]
        public bool? IsStellaAlive { get; set; }
    }

    public class Tag
    {
        [JsonPropertyName("items")]
        public List<Item> Items { get; set; }

        [JsonPropertyName("hasR18Tag")]
        public bool? HasR18Tag { get; set; }

        [JsonPropertyName("isPublishedNicoscript")]
        public bool? IsPublishedNicoscript { get; set; }

        [JsonPropertyName("edit")]
        public Edit Edit { get; set; }

        [JsonPropertyName("viewer")]
        public object Viewer { get; set; }
    }

    public class Target
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("fork")]
        public string Fork { get; set; }
    }

    public class Thread
    {
        [JsonPropertyName("id")]
        public int? Id { get; set; }

        [JsonPropertyName("fork")]
        public int? Fork { get; set; }

        [JsonPropertyName("forkLabel")]
        public string ForkLabel { get; set; }

        [JsonPropertyName("videoId")]
        public string VideoId { get; set; }

        [JsonPropertyName("isActive")]
        public bool? IsActive { get; set; }

        [JsonPropertyName("isDefaultPostTarget")]
        public bool? IsDefaultPostTarget { get; set; }

        [JsonPropertyName("isEasyCommentPostTarget")]
        public bool? IsEasyCommentPostTarget { get; set; }

        [JsonPropertyName("isLeafRequired")]
        public bool? IsLeafRequired { get; set; }

        [JsonPropertyName("isOwnerThread")]
        public bool? IsOwnerThread { get; set; }

        [JsonPropertyName("isThreadkeyRequired")]
        public bool? IsThreadkeyRequired { get; set; }

        [JsonPropertyName("threadkey")]
        public object Threadkey { get; set; }

        [JsonPropertyName("is184Forced")]
        public bool? Is184Forced { get; set; }

        [JsonPropertyName("hasNicoscript")]
        public bool? HasNicoscript { get; set; }

        [JsonPropertyName("label")]
        public string Label { get; set; }

        [JsonPropertyName("postkeyStatus")]
        public int? PostkeyStatus { get; set; }

        [JsonPropertyName("server")]
        public string Server { get; set; }
    }

    public class ThreadId
    {
        [JsonPropertyName("id")]
        public int? Id { get; set; }

        [JsonPropertyName("fork")]
        public int? Fork { get; set; }

        [JsonPropertyName("forkLabel")]
        public string ForkLabel { get; set; }
    }

    public class Thumbnail
    {
        [JsonPropertyName("@type")]
        public string Type { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("width")]
        public int? Width { get; set; }

        [JsonPropertyName("height")]
        public int? Height { get; set; }

        [JsonPropertyName("middleUrl")]
        public string MiddleUrl { get; set; }

        [JsonPropertyName("largeUrl")]
        public string LargeUrl { get; set; }

        [JsonPropertyName("player")]
        public string Player { get; set; }

        [JsonPropertyName("ogp")]
        public string Ogp { get; set; }
    }

    public class User
    {
        [JsonPropertyName("login_status")]
        public string LoginStatus { get; set; }
    }

    public class Video
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("isAvailable")]
        public bool? IsAvailable { get; set; }

        [JsonPropertyName("label")]
        public string Label { get; set; }

        [JsonPropertyName("bitRate")]
        public int? BitRate { get; set; }

        [JsonPropertyName("width")]
        public int? Width { get; set; }

        [JsonPropertyName("height")]
        public int? Height { get; set; }

        [JsonPropertyName("qualityLevel")]
        public int? QualityLevel { get; set; }

        [JsonPropertyName("recommendedHighestAudioQualityLevel")]
        public int? RecommendedHighestAudioQualityLevel { get; set; }
    }

    public class Video2
    {
        [JsonPropertyName("isPpv")]
        public bool? IsPpv { get; set; }

        [JsonPropertyName("isAdmission")]
        public bool? IsAdmission { get; set; }

        [JsonPropertyName("isContinuationBenefit")]
        public bool? IsContinuationBenefit { get; set; }

        [JsonPropertyName("isPremium")]
        public bool? IsPremium { get; set; }

        [JsonPropertyName("watchableUserType")]
        public string WatchableUserType { get; set; }

        [JsonPropertyName("commentableUserType")]
        public string CommentableUserType { get; set; }

        [JsonPropertyName("billingType")]
        public string BillingType { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("count")]
        public Count Count { get; set; }

        [JsonPropertyName("duration")]
        public int Duration { get; set; }

        [JsonPropertyName("thumbnail")]
        public Thumbnail Thumbnail { get; set; }

        [JsonPropertyName("rating")]
        public Rating Rating { get; set; }

        [JsonPropertyName("registeredAt")]
        public DateTime? RegisteredAt { get; set; }

        [JsonPropertyName("isPrivate")]
        public bool? IsPrivate { get; set; }

        [JsonPropertyName("isDeleted")]
        public bool? IsDeleted { get; set; }

        [JsonPropertyName("isNoBanner")]
        public bool? IsNoBanner { get; set; }

        [JsonPropertyName("isAuthenticationRequired")]
        public bool? IsAuthenticationRequired { get; set; }

        [JsonPropertyName("isEmbedPlayerAllowed")]
        public bool? IsEmbedPlayerAllowed { get; set; }

        [JsonPropertyName("isGiftAllowed")]
        public bool? IsGiftAllowed { get; set; }

        [JsonPropertyName("viewer")]
        public object Viewer { get; set; }

        [JsonPropertyName("watchableUserTypeForPayment")]
        public string WatchableUserTypeForPayment { get; set; }

        [JsonPropertyName("commentableUserTypeForPayment")]
        public string CommentableUserTypeForPayment { get; set; }

        [JsonPropertyName("9d091f87")]
        public bool? _9d091f87 { get; set; }
    }

    public class VideoAds
    {
        [JsonPropertyName("additionalParams")]
        public AdditionalParams AdditionalParams { get; set; }

        [JsonPropertyName("items")]
        public List<Item> Items { get; set; }

        [JsonPropertyName("reason")]
        public string Reason { get; set; }
    }

    public class VideoEnd
    {
        [JsonPropertyName("bannerIn")]
        public object BannerIn { get; set; }

        [JsonPropertyName("overlay")]
        public object Overlay { get; set; }
    }

    public class Waku
    {
        [JsonPropertyName("information")]
        public Information Information { get; set; }

        [JsonPropertyName("bgImages")]
        public List<object> BgImages { get; set; }

        [JsonPropertyName("addContents")]
        public object AddContents { get; set; }

        [JsonPropertyName("addVideo")]
        public object AddVideo { get; set; }

        [JsonPropertyName("tagRelatedBanner")]
        public object TagRelatedBanner { get; set; }

        [JsonPropertyName("tagRelatedMarquee")]
        public object TagRelatedMarquee { get; set; }
    }


}




public class RequireActionForGuestWatchPageResponse
{
    public RequireActionForGuestWatchPageResponse(VideoDataForGuest videoData, TagsForGuest tags)
    {
        VideoData = videoData;
        Tags = tags;
    }

    public VideoDataForGuest VideoData { get; }
    public TagsForGuest Tags { get; }
}
