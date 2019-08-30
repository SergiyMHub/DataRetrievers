using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DataRetrievers
{
    public interface IDataRetriver<TProjection>
    {
        Task<DataPage<TProjection>> RetrieveAsync(IEnumerable<Expression<Func<TProjection, bool>>> predicates, IEnumerable<Sorting> sorting, long skip = 0, int take = 1);
    }

}