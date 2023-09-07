using Refit;
using System.Net.Http;
using SdkClient.Dto;

namespace SdkClient
{
    public interface IApiRefit
    {
        [Get("/api/products")]
        public Task<HttpResponseMessage> GetAll(int? offset, int? limit);

        [Get("/api/products/{id}")]
        public Task<ProductResponse> GetOne(Guid id);

        [Post("/api/products")]
        public Task<ProductResponse> Save([Body] ProductRequest req);

        [Put("/api/products/{id}")]
        public Task<ProductResponse> Modify([Body] ProductUpdate req, Guid id);

        [Delete("/api/products/{id}")]
        public Task Delete(Guid id);
    }
}