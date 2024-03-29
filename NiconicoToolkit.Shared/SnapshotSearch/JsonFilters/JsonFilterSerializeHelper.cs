﻿using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NiconicoToolkit.SnapshotSearch.JsonFilters
{

    public static class JsonFilterSerializeHelper
    {
		public static JsonSerializerOptions SerializerOptions { get; } = new JsonSerializerOptions() 
		{
			DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
			Converters = 
			{
				new JsonFilterDataJsonConverter(),
			}
		};

		public static JsonSerializerOptions SerializerOptionsOnWriterValue { get; } = new JsonSerializerOptions()
		{
			DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
		};
	}
}
