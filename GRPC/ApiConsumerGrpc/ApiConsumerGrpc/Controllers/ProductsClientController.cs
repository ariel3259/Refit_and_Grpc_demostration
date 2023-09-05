using GrpcProduct;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace ApiConsumerGrpc.Controllers
{
    [ApiController]
    [Route("/client/products")]
    public class ProductsClientController: ControllerBase
    {
        private readonly ProductService.ProductServiceClient _client;

        public ProductsClientController(ProductService.ProductServiceClient client)
        {
            _client = client;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery(Name = "offset")] int? offset,[FromQuery(Name = "limit")] int? limit)
        {
            PaginationResponse response = await _client.GetAllAsync(new PaginationArgs
            {
                Limit = limit ?? 10,
                Offset = offset ?? 0,
            });
            Response.Headers.Add("x-total-count", response.TotalItems.ToString());
            return Ok(response.Products.ToList());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne([FromRoute(Name = "id")]string id)
        {
            ProductsResponse response = await _client.GetOneAsync(new ProductPk()
            {
                Id = id
            });
            if (response.Name == null) return NoContent();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] ProductRequest request)
        {
            ProductsResponse response = await _client.SaveAsync(request);
            return Created("/client/products", response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] ProductRequest product, [FromRoute(Name = "id")] string id)
        {
            ProductsResponse res = await _client.UpdateAsync(new ProductUpdate()
            {
                Req = product,
                Id = id
            });
            if (res.Name == "") return NoContent();
            return Ok(res);
        }

        [HttpDelete("{id}")]

        public async Task<IActionResult> Delete([FromRoute(Name = "id")] string id)
        {
            await _client.DeleteAsync( new ProductPk()
            {
                Id = id
            });
            return NoContent();
        }
    }
}
