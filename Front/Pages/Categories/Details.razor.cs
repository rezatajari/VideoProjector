using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using Front.DTOs.Category;

namespace Front.Pages.Categories
{
    public partial class Details
    {
        private CategoryDetailsDto? _model = new();
        [Parameter]
        public int CategoryId { get; set; }
        [Inject]
        private HttpClient Http { get; set; }
        private bool _isLoading = true;
        private string _errorMessage = string.Empty;


        protected override async Task OnInitializedAsync()
        {
            try
            {
                _isLoading = true;
                _model = await Http.GetFromJsonAsync<CategoryDetailsDto>(
                    requestUri: $"api/Categories/Details/{CategoryId}");
            }
            catch (Exception ex)
            {
                _errorMessage = "Failed to load category details";
            }
            finally
            {
                _isLoading = false;
            }
        }
    }
}
