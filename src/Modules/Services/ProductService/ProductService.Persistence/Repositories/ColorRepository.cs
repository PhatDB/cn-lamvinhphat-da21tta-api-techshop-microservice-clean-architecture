using BuildingBlocks.Error;
using BuildingBlocks.Results;
using Microsoft.EntityFrameworkCore;
using ProductService.Domain.Abstractions.Repositories;
using ProductService.Domain.Entities;

namespace ProductService.Persistence.Repositories
{
    public class ColorRepository : GenericRepository<Color>, IColorRepository
    {
        private readonly ApplicationDbContext _context;

        public ColorRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Result<Color>> GetColorByNameAsync(
            string colorName, CancellationToken cancellationToken)
        {
            Color? color =
                await _context.Colors.FirstOrDefaultAsync(c => c.Name == colorName,
                    cancellationToken);

            return color is null
                ? Result.Failure<Color>(Error.NotFound("Color.NotFound",
                    $"Color '{colorName}' not found."))
                : Result.Success(color);
        }
    }
}