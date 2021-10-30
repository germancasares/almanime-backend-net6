using System.Collections.Generic;

namespace Jobs.Contracts.Anime
{
    public class AnimeCollection
    {
        public List<AnimeDataModel> Data { get; set; }
        public Meta Meta { get; set; }
        public Links Links { get; set; }
    }
}
