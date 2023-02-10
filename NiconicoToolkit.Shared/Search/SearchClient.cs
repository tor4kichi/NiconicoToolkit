using System;
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
            User = new User.UserSearchSubClient(context, defaultOptions);
            List = new List.ListSearchSubClient(context, defaultOptions);
        }

        public Video.VideoSearchSubClient Video { get; }
        public User.UserSearchSubClient User { get; }
        public List.ListSearchSubClient List { get; }
    }
}
