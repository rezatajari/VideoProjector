using System.Net.Http.Json;
using Front.DTOs.Product;
using Microsoft.AspNetCore.Components;

namespace Front.Pages.Products
{
    public partial class Details
    {
        private  ProductDetailsDto? _model = new();
        [Parameter] 
        public int ProductId { get; set; }
        [Inject]
        private HttpClient Http { get; set; }

        private bool _isLoading = true;
        private string _errorMessage=string.Empty;
        protected override async Task OnInitializedAsync()
        {
            try
            {
                _isLoading = true;
                _model = await Http.GetFromJsonAsync<ProductDetailsDto>(
                    requestUri: $"api/Product/Details/{ProductId}");
            }
            catch (Exception ex)
            {
                _errorMessage = "An error occurred while loading the product details: ";
            }
            finally
            {
                _isLoading= false;
            }
        }
    }
}
