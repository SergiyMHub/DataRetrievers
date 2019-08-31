using DataRetrievers.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataRetrievers
{
    public abstract class DataRetriever<TProjection> : IDataRetriver<TProjection>
    {
        public async Task<DataPage<TProjection>> RetrieveAsync(IEnumerable<Expression<Func<TProjection, bool>>> predicates, IEnumerable<Sorting> sorting, ulong skip = 0, uint take = 1)
        {
            Guard.ArgumentNotNull(predicates, nameof(predicates));
            Guard.ArgumentHasNoNulls(predicates, nameof(predicates));
            GuardPredicatesContaineOnlyAllowedOperators(predicates);

            Guard.ArgumentNotNull(sorting, nameof(sorting));
            Guard.ArgumentHasNoNulls(sorting, nameof(sorting));
            GuardSortingShouldContainOnlyPropertyAccessors(sorting);

            var query = CreateBaseQuery();
            var filteredQuery = ApplyFiltering(query, predicates);
            var sortedQuery = ApplySorting(filteredQuery, sorting);
            var framedQuery = ApplyFraming(sortedQuery, take, skip);

            var count = filteredQuery.LongCount();
            var rawDataPage = framedQuery.ToArray();
            var dataPage = await TransformProjectionsAsync(rawDataPage);

            return new DataPage<TProjection>(dataPage, (ulong)count);
        }

        private IQueryable<TProjection> ApplyFraming(IQueryable<TProjection> sortedQuery, uint take, ulong skip)
        {
            return sortedQuery;
        }

        private IQueryable<TProjection> ApplySorting(IQueryable<TProjection> filteredQuery, IEnumerable<Sorting> sorting)
        {
            var result = filteredQuery;
            var wasSorted = false;
            foreach (var item in sorting)
            {
                var paramExpr = Expression.Parameter(typeof(TProjection), "p");
                var propertyExpr = Expression.Property(paramExpr, item.FieldName);
                var sortKeyExpr = Expression.Lambda(propertyExpr, paramExpr);
                var sortMethodName = "";

                if (item.IsDescending)
                {
                    sortMethodName = wasSorted ? nameof(Queryable.ThenByDescending) : nameof(Queryable.OrderByDescending);
                }
                else
                {
                    sortMethodName = wasSorted ? nameof(Queryable.ThenBy) : nameof(Queryable.OrderBy);
                }

                var expr = Expression.Call(
                    typeof(Queryable),
                    sortMethodName,
                    new[] { typeof(TProjection), propertyExpr.Type },
                    result.Expression,
                    Expression.Quote(sortKeyExpr));

                result = result.Provider.CreateQuery<TProjection>(expr);

                wasSorted = true;
            }

            return result;
        }

        private IQueryable<TProjection> ApplyFiltering(IQueryable<TProjection> query, IEnumerable<Expression<Func<TProjection, bool>>> predicates)
        {
            var result = query;
            foreach (var filter in predicates)
            {
                result = result.Where(filter);
            }

            return result;
        }

        protected virtual async Task<IEnumerable<TProjection>> TransformProjectionsAsync(IEnumerable<TProjection> rawDataPage)
        {
            return rawDataPage;
        }

        protected abstract IQueryable<TProjection> CreateBaseQuery();
        /*{
            return Enumerable.Empty<TInternalProjection>().AsQueryable();
        }*/

        private void GuardPredicatesContaineOnlyAllowedOperators(IEnumerable<Expression<Func<TProjection, bool>>> predicates)
        {
            //not implemented yet
        }

        private void GuardSortingShouldContainOnlyPropertyAccessors(IEnumerable<Sorting> sorting)
        {
            var idx = 0;
            foreach (var sort in sorting)
            {
                if (!Check.HasProperty<TProjection>(sort.FieldName))
                {
                    throw new ArgumentException($"Projection does not have property {sort.FieldName}. Sorting [{idx}]");
                }
                idx++;
            }
        }
    }
}
