using ApiGrpc.Model;
using ApiGrpc.Services;
using ApiGrpc.Validators;
using FluentValidation.Results;
using Grpc.Core;
using GrpcProduct;

namespace ApiGrpc.GrpcServices
{
    public class ProductsServiceGrpc: ProductService.ProductServiceBase
    {
        private readonly ProductsService _service;

        public ProductsServiceGrpc(ProductsService service)
        {
            _service = service;
        }

        public override async Task<PaginationProducts> GetAll(PaginationArgs args, ServerCallContext serverCallContext)
        {
            await Task.Delay(100);
           return _service.GetAll(args);
        }

        public override async Task<ProductsResponse> GetOne(ProductPk pk, ServerCallContext serverCallContext)
        {
            await Task.Delay(100);
            return _service.GetOne(pk);
        }

        public override async Task<ProductsHandler> Save(ProductRequest productRequest, ServerCallContext serverCallContext)
        {
            ProductRequestValidator validator = new();
            ValidationResult result = await validator.ValidateAsync(productRequest);
            if (!result.IsValid)
            {
                ProductsHandler handler = new()
                {
                    Product = null,
                    IsError = true
                };
                List<string> messages = result.Errors.Select(x => x.ErrorMessage).ToList();
                handler.Errors.AddRange(messages);
                return handler;
            }
            Product product = new Product()
            {
                Name = productRequest.Name,
                Price = productRequest.Price,
                Stock = productRequest.Stock
            };
            _service.Save(product);
            ProductsResponse productResponse = new ProductsResponse()
            {
                Id = product.Id.ToString(),
                 Name = product.Name,
                Price = product.Price,
                Stock = product.Stock
            };
            return new ProductsHandler()
            {
                Product = productResponse,
                IsError = false
            } ;
        }

        public override async Task<ProductsHandler> Update(ProductUpdate productUpdate, ServerCallContext serverCallContext)
        {
            await Task.Delay(100);
            ProductsResponse productResponse = _service.Update(productUpdate);
            ProductsHandler handler = new();
            if (productResponse == null)
            {
                handler.IsError = true;
                handler.Errors.Add("El producto seleccionado no existe");
            }
            else
            {
                handler.IsError = false;
                handler.Product = productResponse;
            }  
            return handler;
        }

        public override async Task<GrpcProduct.Void> Delete(ProductPk pk, ServerCallContext serverCallContext)
        {
            await Task.Delay(100);
            _service.Delete(pk);
            return new GrpcProduct.Void();
        }
    }
}
