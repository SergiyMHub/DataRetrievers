using DataRetrievers.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataRetrievers
{
    public class InMemoryDataRetriever<TProjection> : DataRetriever<TProjection>
    {
        private IEnumerable<TProjection> _items;

        public InMemoryDataRetriever(IEnumerable<TProjection> items)
        {
            Guard.ArgumentNotNull(items, nameof(items));
            Guard.ArgumentHasNoNulls(items, nameof(items));

            _items = items;
        }

        protected override IQueryable<TProjection> CreateBaseQuery() => _items.AsQueryable();
    }
}
