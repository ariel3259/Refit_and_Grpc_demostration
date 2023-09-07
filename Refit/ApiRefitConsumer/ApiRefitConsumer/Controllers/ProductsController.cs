using Microsoft.AspNetCore.Mvc;
using Refit;
using SdkClient;
using SdkClient.Dto;

namespace ApiRefitConsumer.Controllers
{
    [ApiController]
    [Route("/client/products")]
    public class ProductsController: ControllerBase
    {
        private readonly IApiRefit _client;

        public ProductsController(IApiRefit client)
        {
            _client = client;

        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery(Name = "offset")] int? offset, [FromQuery(Name = "limit")] int? limit)
        {
                HttpResponseMessage response = await this._client.GetAll(offset, limit);
                if  (!response.IsSuccessStatusCode) return Problem(statusCode: (int)response.StatusCode);
                List<ProductResponse>? products = await response.Content.ReadFromJsonAsync<List<ProductResponse>>();
                string? link = response.Headers.GetValues("link").FirstOrDefault();
                string? xTotalCount = response.Headers.GetValues("x-total-count").FirstOrDefault();
                Response.Headers.Add("Link", link);
                Response.Headers.Add("x-total-count", xTotalCount);
                return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] ProductRequest request)
        {
            try
            {
                ProductResponse response = await _client.Save(request);
                return Created("/client/product", response);
            } catch(ApiException ex)
            {
                return Problem(detail: ex.Message, statusCode: (int)ex.StatusCode);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] ProductUpdate request, [FromRoute(Name = "id")] Guid id)
        {
            try
            {
                ProductResponse response = await _client.Modify(request, id);
                return Ok(response);
            }
            catch (ApiException ex)
            {
                return Problem(detail: ex.Message, statusCode: (int)ex.StatusCode);
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute(Name = "id")] Guid id)
        {
            try
            {
                _client.Delete(id);
                return NoContent();
            } catch (ApiException ex)
            {
                return Problem(detail: ex.Message, statusCode: (int)ex.StatusCode);
            }
        }
    }
}
