using System;
using System.Linq;

namespace DataRetrievers
{
    public interface IQueryParameters<TProjection>
    {
        IQueryable<TProjection> ApplyToQuery(IQueryable<TProjection> queryable);
    } 
}