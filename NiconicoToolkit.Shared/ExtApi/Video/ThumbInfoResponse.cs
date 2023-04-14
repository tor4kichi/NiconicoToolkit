#nullable enable
using AngleSharp.Dom;
using CommunityToolkit.Diagnostics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace NiconicoToolkit.ExtApi.Video;


[XmlRoot(ElementName = "nicovideo_thumb_response")]
public class ThumbInfoResponse
{

    [XmlElement(ElementName = "thumb")]
    public ThumbInfoData Data { get; set; }

    [XmlElement(ElementName = "error")]
    public Error? Error { get; set; }

    [XmlAttribute(AttributeName = "status")]
    public string Status { get; set; }


    public bool IsOK => Status.Equals("ok", StringComparison.Ordinal);
}

[XmlRoot(ElementName = "tag")]
public class Tag
{
    [XmlAttribute(AttributeName = "lock")]
    public int Lock { get; set; }

    [XmlText]
    public string Text { get; set; }


    public bool IsLocked => Lock == 1;
}

[XmlRoot(ElementName = "tags")]
public class Tags
{
    [XmlElement(ElementName = "tag")]
    public List<Tag> Tag { get; set; }

    [XmlAttribute(AttributeName = "domain")]
    public string Domain { get; set; }
}

[XmlRoot(ElementName = "thumb")]
public class ThumbInfoData
{

    [XmlElement(ElementName = "video_id")]
    public string VideoId { get; set; }

    [XmlElement(ElementName = "title")]
    public string Title { get; set; }

    [XmlElement(ElementName = "description")]
    public string Description { get; set; }

    [XmlElement(ElementName = "thumbnail_url")]
    public string ThumbnailUrlNotHttps { get; set; }

    [XmlIgnore]
    string? _thumbnailUrl;
    
    [XmlIgnore]
    public string ThumbnailUrl => _thumbnailUrl ??= ThumbnailUrlNotHttps.Replace("http://", "https://");

    [XmlElement(ElementName = "first_retrieve")]
    public DateTime PostAt { get; set; }

    [XmlElement(ElementName = "length")]
    public string LengthRaw { get; set; }

    TimeSpan? _length;
    public TimeSpan Length => _length ??= LengthRaw.ToTimeSpan();

    [XmlElement(ElementName = "view_counter")]
    public int ViewCount { get; set; }

    [XmlElement(ElementName = "comment_num")]
    public int CommentCount { get; set; }

    [XmlElement(ElementName = "mylist_counter")]
    public int MylistCount { get; set; }

    [XmlElement(ElementName = "last_res_body")]
    public string LastResBodyRaw { get; set; }

    public string[] ParseLastComments()
    {
        return LastResBodyRaw.Split(' ');
    }

    [XmlElement(ElementName = "watch_url")]
    public string WatchUrl { get; set; }

    [XmlElement(ElementName = "thumb_type")]
    public string ThumbType { get; set; }

    [XmlElement(ElementName = "embeddable")]
    public int Embeddable { get; set; }

    [XmlElement(ElementName = "no_live_play")]
    public int NoLivePlay { get; set; }

    [XmlElement(ElementName = "tags")]
    public Tags? Tags { get; set; }

    [XmlElement(ElementName = "genre")]
    public string Genre { get; set; }



    [XmlElement(ElementName = "ch_id")]
    public int ChId { get; set; }

    [XmlElement(ElementName = "ch_name")]
    public string? ChName { get; set; }

    [XmlElement(ElementName = "ch_icon_url")]
    public string? ChIconUrl { get; set; }




    [XmlElement(ElementName = "user_id")]
    public int UserId { get; set; }

    [XmlElement(ElementName = "user_nickname")]
    public string? UserNickname { get; set; }

    [XmlElement(ElementName = "user_icon_url")]
    public string? UserIconUrl { get; set; }

    public (VideoProviderType ProviderType, int Id, string Name, string IconUrl) GetProviderData()
    {
        if (UserId > 0)
        {
            Guard.IsNotNull(UserNickname, nameof(UserNickname));
            Guard.IsNotNull(UserIconUrl, nameof(UserIconUrl));

            return (VideoProviderType.User, UserId, UserNickname, UserIconUrl);
        }
        else if (ChId > 0)
        {
            Guard.IsNotNull(ChName, nameof(ChId));
            Guard.IsNotNull(ChIconUrl, nameof(ChIconUrl));

            return (VideoProviderType.Channel, ChId, ChName, ChIconUrl);
        }        
        else
        {
            throw new InvalidOperationException();
        }
    }
}

public enum VideoProviderType
{
    User,
    Channel
}

[XmlRoot(ElementName = "error")]
public class Error
{
    [XmlElement(ElementName = "code")]
    public string Code { get; set; }

    [XmlElement(ElementName = "description")]
    public string Description { get; set; }

    ThumbInfoErrorCode? _errorCode;
    public ThumbInfoErrorCode ErrorCode => _errorCode ??= Enum.Parse<ThumbInfoErrorCode>(Code);
}

public enum ThumbInfoErrorCode
{
    NOT_FOUND,
    COMMUNITY,
    DELETED
}
