using ApiRefit.Services;
using ApiRefit.Validators;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using SdkClient.Dto;
using System.Text;

namespace ApiRefit.Controllers
{
    [ApiController]
    [Route("/api/products")]
    public class ProductsController: ControllerBase
    {
        private readonly ProductService _service; 
        public ProductsController(ProductService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAll([FromQuery(Name = "offset")] int? offset, [FromQuery(Name = "limit")] int? limit)
        {
            PageProducts page = _service.GetAll(offset, limit);
            Response.Headers.Add("x-total-count", page.TotalItems.ToString());
            int off = offset ?? 0;
            int lim = limit ?? 10;
            int next = off + lim;
            StringBuilder linkBuilder = new();
            if (next < page.TotalItems) linkBuilder.Append($"</api/products?offset={next}&limit={lim}>; rel=\"next\", ");
            if (off > 0) linkBuilder.Append($"</api/products?offset={off - lim}&limit={lim}>; rel=\"previous\"");
            Response.Headers.Add("Link", linkBuilder.ToString());
            return Ok(page.Products);
        }

        [HttpGet("{id}")]
        public IActionResult GetOne([FromRoute(Name = "id")] Guid id)
        {
            ProductResponse? response = _service.GetOne(id);
            if (response == null) return NoContent();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] ProductRequest req)
        {
            ProductRequestValidator reqValidation = new();
            ValidationResult result = await reqValidation.ValidateAsync(req);
            if (!result.IsValid)
            {
                string message = "";
                foreach (ValidationFailure error in result.Errors)
                {
                    message += error.ErrorMessage;
                }
                return Problem(detail: message, statusCode: 400);
            }
            ProductResponse response = _service.Save(req);
            return Created("/api/products", response);
        }

        [HttpPut("{id}")]
        public IActionResult Update([FromBody] ProductUpdate productUpdate, [FromRoute(Name = "id")] Guid id)
        {
            ProductResponse? response = _service.Update(productUpdate, id);
            if (response == null) return Problem(detail: "El producto no existe", statusCode: 400);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute(Name = "id")] Guid id)
        {
            _service.Delete(id);
            return NoContent();
        }

    }
}
