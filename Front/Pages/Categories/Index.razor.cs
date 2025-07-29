

using System.Net.Http.Json;
using Front.DTOs.Category;
using Microsoft.AspNetCore.Components;

namespace Front.Pages.Categories
{
    public partial class Index
    {
        [Inject]
        private HttpClient Http { get; set; }   

        private List<CategoryDto>? _categories= [];

        private bool _isLoading = true;
        private string _errorMessage = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            await LoadCategories();
        }

        private async Task LoadCategories()
        {
            try
            {
                _isLoading = true;
               
                _categories = await Http.GetFromJsonAsync<List<CategoryDto>>("http://localhost:61028/api/categories/list");
            }
            catch (Exception ex)
            {
                _errorMessage = "Failed to load categories";
            }
            finally
            {
                _isLoading = false;
            }
        }
    }
}
