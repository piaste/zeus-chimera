using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chimera.Catalog.Api.Models.Requests
{
    public class FetchPagedRequest
    {
        public int StartRowIndex { get; set; }

        public int MaximumRows { get; set; }
    }
}
