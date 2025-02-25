using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace NiconicoToolkit.Live.WatchSession.ToClientMessage;


[JsonSourceGenerationOptions()]
[JsonSerializable(typeof(Error_WatchSessionToClientMessage))]
[JsonSerializable(typeof(Seat_WatchSessionToClientMessage))]
[JsonSerializable(typeof(Akashic_WatchSessionToClientMessage))]
[JsonSerializable(typeof(Stream_WatchSessionToClientMessage))]
[JsonSerializable(typeof(MessageServer_WatchSessionToClientMessage))]
[JsonSerializable(typeof(ServerTime_WatchSessionToClientMessage))]
[JsonSerializable(typeof(Statistics_WatchSessionToClientMessage))]
[JsonSerializable(typeof(Schedule_WatchSessionToClientMessage))]
[JsonSerializable(typeof(Ping_WatchSessionToClientMessage))]
[JsonSerializable(typeof(Disconnect_WatchSessionToClientMessage))]
[JsonSerializable(typeof(Reconnect_WatchSessionToClientMessage))]
[JsonSerializable(typeof(PostCommentResult_WatchSessionToClientMessage))]
public sealed partial class WatchSessionToClientMessageSourceGenerationContext : JsonSerializerContext
{

}

public abstract class WatchServerToClientMessage
{

}

public sealed class Error_WatchSessionToClientMessage : WatchServerToClientMessage
{
    [JsonPropertyName("code")]
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public ErrorMessageType Code { get; set; }
}

public sealed class Seat_WatchSessionToClientMessage : WatchServerToClientMessage
{
    [JsonPropertyName("keepIntervalSec")]
    public int KeepIntervalSec { get; set; }
}

public sealed class Akashic_WatchSessionToClientMessage : WatchServerToClientMessage
{
    [JsonPropertyName("status")]
    public AkashicStatus Status { get; set; }

    [JsonPropertyName("playId")]
    public string PlayId { get; set; }

    [JsonPropertyName("token")]
    public string Token { get; set; }

    [JsonPropertyName("playerId")]
    public string PlayerId { get; set; }

    [JsonPropertyName("contentUrl")]
    public string ContentUrl { get; set; }

    [JsonPropertyName("logServerUrl")]
    public string LogServerUrl { get; set; }
}


public sealed class Stream_WatchSessionToClientMessage : WatchServerToClientMessage
{
    [JsonPropertyName("uri")]
    public string Uri { get; set; }

    [JsonPropertyName("syncUri")]
    public string SyncUri { get; set; }

    [JsonPropertyName("quality")]
    public string Quality{ get; set; }

    [JsonPropertyName("availableQualities")]
    public string[] AvailableQualities { get; set; }

    [JsonPropertyName("protocol")]
    public string Protocol { get; set; }
}


//public sealed class Room_WatchSessionToClientMessage : WatchServerToClientMessage
//{
//    [JsonPropertyName("messageServer")]
//    public MessageServer MessageServer { get; set; }

//    [JsonPropertyName("name")]
//    public string Name { get; set; }

//    [JsonPropertyName("threadId")]
//    public string ThreadId { get; set; }

//    [JsonPropertyName("isFirst")]
//    public bool IsFirst { get; set; }

//    [JsonPropertyName("waybackkey")]
//    [Obsolete("※ 部屋統合後はキーなしで取得できるようにするため空になります")]
//    public string waybackkey { get; set; }

//    [JsonPropertyName("yourPostKey")]
//    [Obsolete("※ 部屋統合までは不要のため空になります")]
//    public string YourPostKey { get; set; }
//}

public sealed class MessageServer_WatchSessionToClientMessage : WatchServerToClientMessage
{
    [JsonPropertyName("viewUri")]
    public Uri ViewUri { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("vposBaseTime")]
    public DateTimeOffset VposBaseTime { get; set; }
}

//public sealed class Rooms_WatchSessionToClientMessage : WatchServerToClientMessage
//{
//    [JsonPropertyName("rooms")]
//    public Room_WatchSessionToClientMessage[] Rooms { get; set; }
//}

public sealed class ServerTime_WatchSessionToClientMessage : WatchServerToClientMessage
{
    [JsonPropertyName("currentMs")]
    public DateTime CurrentTime { get; set; }
}


public sealed class Statistics_WatchSessionToClientMessage : WatchServerToClientMessage
{
    [JsonPropertyName("viewers")]
    public int? Viewers { get; set; }

    [JsonPropertyName("comments")]
    public int? comments { get; set; }

    [JsonPropertyName("adPoints")]
    public int? adPoints { get; set; }

    [JsonPropertyName("giftPoints")]
    public int? giftPoints { get; set; }
}


public sealed class Schedule_WatchSessionToClientMessage : WatchServerToClientMessage
{
    [JsonPropertyName("begin")]
    public DateTime Begin { get; set; }

    [JsonPropertyName("end")]
    public DateTime End { get; set; }
}

public sealed class Ping_WatchSessionToClientMessage : WatchServerToClientMessage
{
    
}

public sealed class Disconnect_WatchSessionToClientMessage : WatchServerToClientMessage
{
    [JsonPropertyName("reason")]
    public DisconnectReasonType Reason { get; set; }
}


public sealed class Reconnect_WatchSessionToClientMessage : WatchServerToClientMessage
{
    [JsonPropertyName("audienceToken")]
    public string AudienceToken { get; set; }

    [JsonPropertyName("waitTimeSec")]
    public int WaitTimeSec { get; set; }
}

public sealed class PostCommentResult_WatchSessionToClientMessage : WatchServerToClientMessage
{
    [JsonPropertyName("chat")]
    public PostCommentResultChat Chat { get; set; }
}

public sealed class PostCommentResultChat
{
    [JsonPropertyName("mail")]
    public string Mail { get; set; }

    [JsonPropertyName("anonymity")]
    public int Anonymity { get; set; }

    [JsonPropertyName("content")]
    public string Content { get; set; }

    [JsonPropertyName("restricted")]
    public bool Restricted { get; set; }
}
