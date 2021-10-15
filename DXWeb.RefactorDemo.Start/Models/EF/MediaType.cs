using System;
using System.Collections.Generic;

#nullable disable

namespace DXWeb.RefactorDemo.Models.EF
{
    public partial class MediaType
    {
        public MediaType()
        {
            Tracks = new HashSet<Track>();
        }

        public int MediaTypeId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Track> Tracks { get; set; }
    }
}
