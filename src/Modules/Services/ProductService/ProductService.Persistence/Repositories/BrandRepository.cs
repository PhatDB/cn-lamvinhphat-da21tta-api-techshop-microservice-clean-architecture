using ProductService.Domain.Abstractions.Repositories;
using ProductService.Domain.Entities;

namespace ProductService.Persistence.Repositories
{
    public class BrandRepository : GenericRepository<Brand>, IBrandRepository
    {
        private readonly ApplicationDbContext _context;

        public BrandRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}