using ApiGrpc.Model;
using GrpcProduct;

namespace ApiGrpc.Services
{
    public class ProductsService
    {
        private readonly List<Product> products;  
        public ProductsService()
        {
            products = new List<Product>();
        }

        public PaginationResponse GetAll(PaginationArgs args)
        {
            List<Product> productsActive = this.products.Where(x => x.Status).OrderBy(x => x.Id).Skip(args.Offset).Take(args.Limit == 0 ? 10 : args.Limit).ToList();
            int totalItems = products.Where(x => x.Status).OrderBy(x => x.Id).Skip(args.Offset).Take(args.Limit == 0 ? 10 : args.Limit).Count();
            PaginationResponse pageResponse = new PaginationResponse()
            {
                TotalItems = totalItems,
            };
            List<ProductsResponse> productsResponse = productsActive.Select(x => new ProductsResponse
            {
                Id = x.Id.ToString(),
                Name = x.Name,
                Price = x.Price,
                Stock = x.Stock
            }).ToList();
            pageResponse.Products.AddRange(productsResponse);
            return pageResponse;
        }

        public ProductsResponse GetOne(ProductPk pk)
        {
            Product? product = this.products.FirstOrDefault(x => x.Id.ToString() == pk.Id && x.Status);
            if (product == null) return new ProductsResponse();
            else return new ProductsResponse()
            {
                Id = product.Id.ToString(),
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock
            };
        }

        public void Save(Product product)
        {
            products.Add(product);
        }

        public ProductsResponse Update(ProductUpdate productUpdate)
        {
            Product? productToModify = this.products.FirstOrDefault(x => x.Status && x.Id.ToString() == productUpdate.Id);
            int index = this.products.IndexOf(productToModify);
            if (productToModify == null) return new ProductsResponse();
            productToModify.Name = productUpdate.Req.Name != null && productUpdate.Req.Name != productToModify.Name ? productUpdate.Req.Name : productToModify.Name;
            productToModify.Price = productUpdate.Req.Price != 0 && productToModify.Price != productUpdate.Req.Price ? productUpdate.Req.Price : productToModify.Price;
            productToModify.Stock = productUpdate.Req.Stock != 0 && productToModify.Stock != productUpdate.Req.Stock ? productUpdate.Req.Stock : productToModify.Stock;
            productToModify.UpdatedAt = DateTime.Now;
            this.products[index] = productToModify;
            return new ProductsResponse()
            {
                Id = productToModify.Id.ToString(),
                Name = productToModify.Name,
                Price = productToModify.Price,
                Stock = productToModify.Stock
            };
        }

        public void Delete(ProductPk pk)
        {
            Product? product = this.products.FirstOrDefault(x => x.Id.ToString() == pk.Id && x.Status);
            int index = this.products.FindIndex(x => x.Id.ToString() == pk.Id && x.Status);
            if (product == null) return;
            product.Status = false;
            this.products[index] = product;
        }
    }
}
