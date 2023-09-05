using ApiGrpc.Model;
using ApiGrpc.Services;
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

        public override async Task<PaginationResponse> GetAll(PaginationArgs args, ServerCallContext serverCallContext)
        {
            await Task.Delay(100);
           return _service.GetAll(args);
        }

        public override async Task<ProductsResponse> GetOne(ProductPk pk, ServerCallContext serverCallContext)
        {
            await Task.Delay(100);
            return _service.GetOne(pk);
        }

        public override async Task<ProductsResponse> Save(ProductRequest productRequest, ServerCallContext serverCallContext)
        {
            await Task.Delay(100);
            Product product = new Product()
            {
                Name = productRequest.Name,
                Price = productRequest.Price,
                Stock = productRequest.Stock
            };
            _service.Save(product);
            return new ProductsResponse()
            {
                Id = product.Id.ToString(),
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock
            };
        }

        public override async Task<ProductsResponse> Update(ProductUpdate productUpdate, ServerCallContext serverCallContext)
        {
            await Task.Delay(100);
            return _service.Update(productUpdate);
        }

        public override async Task<GrpcProduct.Void> Delete(ProductPk pk, ServerCallContext serverCallContext)
        {
            await Task.Delay(100);
            _service.Delete(pk);
            return new GrpcProduct.Void();
        }
    }
}
