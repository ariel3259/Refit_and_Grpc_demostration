using GrpcProduct;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

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
            int off = offset ?? 0;
            int lim = limit ?? 10;
            PaginationProducts response = await _client.GetAllAsync(new PaginationArgs
            {
                Limit = off,
                Offset = lim,
            });
            //Adding x total count header
            Response.Headers.Add("x-total-count", response.TotalItems.ToString());

            //Adding link header
            StringBuilder linkBuilder = new();
            int next = off + lim;
            if (off > 0) linkBuilder.Append($"</clien/products?offset={off - lim}&limit={lim}>; rel=\"previous\" ,");
            if (next < response.TotalItems) linkBuilder.Append($"/client/products?offset={next}&limit={lim}; rel=\"next\"");
            Response.Headers.Add("Link", linkBuilder.ToString());
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
            ProductsHandler response = await _client.SaveAsync(request);
            if (response.IsError)
            {
                string message = "";
                Console.WriteLine(response.Errors.Count);
                foreach (string error in response.Errors)
                {
                    message += $"{error}.";
                }
                return Problem(detail: message, statusCode: 400);
            }
            return Created("/client/products", response.Product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] ProductRequest product, [FromRoute(Name = "id")] string id)
        {
            ProductsHandler res = await _client.UpdateAsync(new ProductUpdate()
            {
                Product = product,
                Id = id
            });
            if (res.IsError) return Problem(detail: res.Errors[0], statusCode: 400);
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
