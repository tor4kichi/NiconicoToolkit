using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace NiconicoToolkit.Video.Watch.Domand;

public class DomandHlsAccessRightResponseData
{
    [JsonPropertyName("contentUrl")]
    public string ContentUrl { get; set; }

    [JsonPropertyName("createTime")]
    public DateTime? CreateTime { get; set; }

    [JsonPropertyName("expireTime")]
    public DateTime? ExpireTime { get; set; }
}

public sealed class DomandHlsAccessRightResponse : ResponseWithData<DomandHlsAccessRightResponseData>
{

}
