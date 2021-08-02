using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace NiconicoToolkit.SnapshotSearch
{
    public sealed class SnapshotApiVersion
    {
        [JsonPropertyName("last_modified")]
        public DateTimeOffset LastModified { get; init; }
    }
}
