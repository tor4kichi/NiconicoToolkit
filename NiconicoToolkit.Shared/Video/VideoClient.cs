using NiconicoToolkit.Ranking.Video;
using NiconicoToolkit.Mylist;
using NiconicoToolkit.Video.Watch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using NiconicoToolkit.Video.Watch.NV_Comment;

namespace NiconicoToolkit.Video
{
    public sealed class VideoClient
    {
        private readonly NiconicoContext _context;

        private readonly JsonSerializerOptions _option;

        public VideoRankingSubClient Ranking { get; }
        public VideoWatchSubClient VideoWatch { get; }
        public NvCommentSubClient NvComment { get; }

        internal VideoClient(NiconicoContext context, JsonSerializerOptions defaultOptions)
        {
            _option = defaultOptions;
            _context = context;
            Ranking = new VideoRankingSubClient(context, _option);
            VideoWatch = new VideoWatchSubClient(context, _option);
            NvComment = new NvCommentSubClient(context, _option);
        }
    }

}

