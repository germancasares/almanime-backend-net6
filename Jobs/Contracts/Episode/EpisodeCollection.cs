﻿using System.Collections.Generic;

namespace Jobs.Contracts.Episode
{
    public class EpisodeCollection
    {
        public List<EpisodeDataModel> Data { get; set; }
        public Meta Meta { get; set; }
        public Links Links { get; set; }
    }
}
