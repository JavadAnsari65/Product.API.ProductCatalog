using System.Linq.Expressions;

namespace Product.API.ProductCatalog.Extensions.SearchClasses
{
    public class EntityFilterService<T> where T : class
    {
        private readonly IQueryable<T> _query;
        public EntityFilterService(IQueryable<T> query)
        {
            _query = query;
        }

        public IQueryable<T> ApplyFilter(Expression<Func<T, bool>> filter)
        {
            return _query.Where(filter);
        }
    }
}
