using System.Net.Http.Json;
using Front.DTOs.Product;
using Microsoft.AspNetCore.Components;

namespace Front.Pages.Products
{
    public partial class List
    {

        private List<ProductListDto>? _model = new();
        [Inject]
        private HttpClient Http { get; set; }

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
    }
}
