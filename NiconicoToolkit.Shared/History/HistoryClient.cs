using NiconicoToolkit.Activity.VideoWatchHistory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiconicoToolkit.Activity;

public sealed class HistoryClient
{
    private readonly NiconicoContext _context;

    public HistoryClient(NiconicoContext context, System.Text.Json.JsonSerializerOptions defaultOptions)
    {
        _context = context;
        VideoWachHistory = new VideoWatchHisotrySubClient(context, defaultOptions);
    }

    public VideoWatchHisotrySubClient VideoWachHistory { get; }
}
