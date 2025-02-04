using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MemCache.Contracts;
using MemCache.ProductService.Data;

namespace MemCache.ProductService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly DataDbContext _dbContext;
        private readonly IPublishEndpoint _publishEndpoint;
        public ProductsController(DataDbContext dbContext, IPublishEndpoint publishEndpoint)
        {
            _dbContext = dbContext;
            _publishEndpoint = publishEndpoint;
        }

        [HttpGet]
        public IActionResult Get()
        {
            Thread.Sleep(3000);
            return Ok(_dbContext.Products.Include(c => c.Category).Select(p => new
            {
                p.Id,
                p.Name,
                p.Description,
                CategoryName = p.Category.Name,
                p.Price,
                p.Discount,
                p.Quantity
            }));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] Guid id)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (product is null)
                return NotFound("محصول یافت نشد.");

            if (product.Quantity <= 0)
                return BadRequest("محصول موجود نیست.");

            product.Quantity--;
            await _dbContext.SaveChangesAsync();

            await _publishEndpoint.Publish(new ProductUpdated(id.ToString()));

            return Ok($"موجودی محصول {product.Name} کاهش یافت.");
        }
    }
}
