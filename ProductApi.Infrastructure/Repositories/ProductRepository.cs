using eCommerce.SharedLibrary.Responses;
using ProductApi.Application.Interfaces;
using ProductApi.Domain.Entities;
using System.Linq.Expressions;

namespace ProductApi.Infrastructure.Repositories
{
    public class ProductRepository : IProduct
    {
        public Task<Response> CreateAsync(Product entity)
        {
            throw new NotImplementedException();
        }

        public Task<Response> DeleteAsync(Product entity)
        {
            throw new NotImplementedException();
        }

        public Task<Product> FindByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> GetAllAsync(Product entity)
        {
            throw new NotImplementedException();
        }

        public Task<Product> GetByAsync(Expression<Func<Product, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<Response> UpdateAsync(Product entity)
        {
            throw new NotImplementedException();
        }
    }
}
