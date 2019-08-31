using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DataRetrievers
{
    public interface IDataRetriver<TProjection>
    {
        Task<DataPage<TProjection>> RetrieveAsync(IEnumerable<Expression<Func<TProjection, bool>>> predicates, IEnumerable<Sorting> sorting, uint skip = 0, uint take = 1);
    }

}