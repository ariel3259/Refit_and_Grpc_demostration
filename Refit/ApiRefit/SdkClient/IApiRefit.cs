using Refit;
using SdkClient.Dto;

namespace SdkClient
{
    public interface IApiRefit
    {
        [Get("/api/products")]
        public Task<List<ProductResponse>> GetAll(int? offset, int? limit);

        [Get("/api/products/{id}")]
        public Task<ProductResponse> GetOne(Guid id);

        [Post("/api/products")]
        public Task<ProductResponse> Save([Body] ProductRequest req);

        [Put("/api/products/{id}")]
        public Task<ProductResponse> Modify([Body] ProductRequest req, Guid id);

        [Delete("/api/products/{id}")]
        public Task Delete(Guid id);
    }
}