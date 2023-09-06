using ApiRefit.Model;
using SdkClient.Dto;
namespace ApiRefit.Services
{
    public class ProductService
    {
        private readonly List<Product> products ;
        public ProductService()
        {
            products = new List<Product>() ;
        }

        public PageProducts GetAll(int? offset, int? limit)
        {
            List<Product> productsActive = this.products.Where(x => x.Status).OrderBy(x => x.Id).Skip(offset ?? 0).Take(limit ?? 10).ToList();
            int totalItems = products.Where(x => x.Status).OrderBy(x => x.Id).Count();
            List<ProductResponse> productsResponse = productsActive.Select(x => new ProductResponse
            {
                Id = x.Id,
                Name = x.Name,
                Price = x.Price,
                Stock = x.Stock
            }).ToList();
            return new PageProducts()
            {
                TotalItems = totalItems,
                Products = productsResponse
            };
        }

        public ProductResponse? GetOne(Guid pk)
        {
            Product? product = this.products.FirstOrDefault(x => x.Id == pk && x.Status);
            if (product == null) return null;
            else return new ProductResponse()
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock
            };
        }

        public ProductResponse Save(ProductRequest req)
        {
            Product product = new()
            {
                Name = req.Name,
                Price = req.Price,
                Stock = req.Stock
            };
            products.Add(product);
            Console.WriteLine(products.Count);
            return new()
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock
            };
        }

        public ProductResponse? Update(ProductUpdate productUpdate, Guid id)
        {
            Product? productToModify = this.products.FirstOrDefault(x => x.Status && x.Id == id);
            int index = this.products.IndexOf(productToModify);
            if (productToModify == null) return null;
            productToModify.Name = productUpdate.Name != null && productUpdate.Name != productToModify.Name ? productUpdate.Name : productToModify.Name;
            productToModify.Price = productUpdate.Price != 0 && productToModify.Price != productUpdate.Price ? (int)productUpdate.Price : productToModify.Price;
            productToModify.Stock = productUpdate.Stock != 0 && productToModify.Stock != productUpdate.Stock ? (int)productUpdate.Stock : productToModify.Stock;
            productToModify.UpdatedAt = DateTime.Now;
            this.products[index] = productToModify;
            return new ProductResponse()
            {
                Id = productToModify.Id,
                Name = productToModify.Name,
                Price = productToModify.Price,
                Stock = productToModify.Stock
            };
        }

        public void Delete(Guid pk)
        {
            Product? product = this.products.FirstOrDefault(x => x.Id == pk && x.Status);
            int index = this.products.FindIndex(x => x.Id == pk && x.Status);
            if (product == null) return;
            product.Status = false;
            this.products[index] = product;
        }
    }
}
