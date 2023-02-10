﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace NiconicoToolkit.Search
{
    public sealed class SearchClient
    {
        private readonly NiconicoContext _context;

        private readonly JsonSerializerOptions _option;

        public SearchClient(NiconicoContext context, JsonSerializerOptions defaultOptions)
        {
            _option = defaultOptions;
            _context = context;
            Video = new Video.VideoSearchSubClient(context, defaultOptions);
        }

        public Video.VideoSearchSubClient Video { get; }
    }
}