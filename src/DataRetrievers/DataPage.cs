using DataRetrievers.Internal;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataRetrievers
{
    public class DataPage<TProjection>
    {
        public DataPage(IEnumerable<TProjection> data, ulong count)
        {
            Guard.ArgumentNotNull(data, nameof(data));
            Guard.ArgumentHasNoNulls(data, nameof(data));

            this.Data = data;
            this.TotalRecords = count;
        }

        public IEnumerable<TProjection> Data { get; set; } = Enumerable.Empty<TProjection>();
        public ulong TotalRecords { get; set; } = 0;
        
    }
}