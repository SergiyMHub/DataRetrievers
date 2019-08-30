using System;
using System.Collections.Generic;

namespace DataRetrievers
{
    public class DataPage<TProjection>
    {
        public IEnumerable<TProjection> Data {get;set;}
        public long TotalRecords {get;set;}
        
    }
}