using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace NiconicoToolkit.Video.Watch;

public sealed class WatchJsonResponse : ResponseWithMeta
{
    [JsonPropertyName("data")]
    public DmcWatchApiData Data { get; init; }
}
