using System.Net.Http.Json;
using Front.DTOs;
using Front.DTOs.Product;
using Front.DTOs.ShoppingCart;
using Front.Services;
using Microsoft.AspNetCore.Components;

namespace Front.Pages.Products
{
    public partial class List
    {

        private List<ProductListDto>? _model = [];
        private ProductSearchDto _searchModel = new();

        [Inject]
        private HttpClient Http { get; set; }

        [Inject]
        private ShoppingCartService CartService { get; set; }

        private bool _isLoading = true;
        private string _errorMessage=string.Empty;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                _isLoading = true;
                _model = await Http.GetFromJsonAsync<List<ProductListDto>>(
                    requestUri: "api/Product/List");
            }
            catch (Exception ex)
            {
                _errorMessage = "An error occurred while loading the product list: ";
            }
            finally
            {
                _isLoading = false;
            }
        }
        private async Task SearchProducts()
        {
            try
            {
                _errorMessage = null;
                _isLoading = true;
                var result = await Http.PostAsJsonAsync(requestUri: "api/Product/Search",value:_searchModel);

                var response = await result.Content.ReadFromJsonAsync<GeneralResponse<List<ProductListDto>>>();
                if (!response.IsSuccess)
                    _errorMessage = "Search failed. Server returned an error.";

                _model = response.Data;
            }
            catch (Exception ex)
            {
                _errorMessage = "An error occurred while loading the product search: ";
            }
            finally
            {
                _isLoading = false;
            }
        }

        private void AddToCart(ProductListDto product)
        {
            var item = new ShoppingCartItemDto
            {
                ProductId = product.ProductId,
                ProductName = product.Name,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                Quantity = 1
            };

            CartService.AddItem(item);
        }
    }
}
