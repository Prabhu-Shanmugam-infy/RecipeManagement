using Microsoft.Net.Http.Headers;
using RecipeManagement.Contracts;
using System.Text.Json;
using System.Text;
using System.Net.Http;
using RecipeManagement.Models;
using RecipeManagement.Entities;
using Microsoft.EntityFrameworkCore;

namespace RecipeManagement
{
    public class RecipeManagementService
    {
        private readonly HttpClient _httpClient;
        private bool IsAuthSet;

        public RecipeManagementService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<RegisterResponse> Register(RegisterRequest request)
        {
            using StringContent jsonContent = new(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/register", jsonContent);
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadFromJsonAsync<RegisterResponse>().Result;
            }
            else
            {
                return null;
            }

        }

        public async Task<LoginResponse> Login(LoginRequest request)
        {
            using StringContent jsonContent = new(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/login", jsonContent);
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadFromJsonAsync<LoginResponse>().Result;
            }
            else
            {
                return null;
            }

        }

        public async Task<List<CategoryModel>> GetAllCategoriesAsync()
        {
            var response = await _httpClient.GetAsync("api/category");
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadFromJsonAsync<List<CategoryModel>>().Result;
            }
            else
            {
                return null;
            }

        }

        public async Task<RecipeModel> CreateRecipeAsync(RecipeModel request)
        {
            using StringContent jsonContent = new(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/recipe", jsonContent);
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadFromJsonAsync<RecipeModel>().Result;
            }
            else
            {
                return null;
            }
        }

        public async Task<RecipeModel> UpdateRecipeAsync(RecipeModel request)
        {
            using StringContent jsonContent = new(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync("api/recipe/" + request.Id, jsonContent);
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadFromJsonAsync<RecipeModel>().Result;
            }
            else
            {
                return null;
            }
        }

        public async Task<BaseResponse> DeleteRecipeAsync(int id)
        {

            var response = await _httpClient.DeleteAsync("api/recipe/" + id);
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadFromJsonAsync<BaseResponse>().Result;
            }
            else
            {
                return null;
            }
        }

        public async Task<RecipeModel> GetRecipeByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync("api/recipe/" + id);
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadFromJsonAsync<RecipeModel>().Result;
            }
            else
            {
                return null;
            }
        }

        public async Task<List<RecipeModel>> GetAllRecipesAsync()
        {
            var response = await _httpClient.GetAsync("api/recipe");
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadFromJsonAsync<List<RecipeModel>>().Result;
            }
            else
            {
                return null;
            }

        }

        public async Task<List<RecipeModel>> SearchRecipesAsync(string query)
        {
            var response = await _httpClient.GetAsync("api/recipe/search/" + query);
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadFromJsonAsync<List<RecipeModel>>().Result;
            }
            else
            {
                return null;
            }

        }

        public async Task<List<RecipeModel>> FilterRecipeAsync(FilterModel request)
        {
            using StringContent jsonContent = new(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/recipe/filter", jsonContent);
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadFromJsonAsync<List<RecipeModel>>().Result;
            }
            else
            {
                return null;
            }
        }

        public async Task<List<UserModel>> GetAllUsersAsync()
        {
            var response = await _httpClient.GetAsync("api/user");
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadFromJsonAsync<List<UserModel>>().Result;
            }
            else
            {
                return null;
            }

        }

        public async Task<UserModel> GetUserByIdAsync(int userId)
        {
            var response = await _httpClient.GetAsync("api/user/" + userId);
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadFromJsonAsync<UserModel>().Result;
            }
            else
            {
                return null;
            }

        }

        public async Task<UserModel> CreateUserAsync(UserModel request)
        {
            using StringContent jsonContent = new(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/user", jsonContent);
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadFromJsonAsync<UserModel>().Result;
            }
            else
            {
                return null;
            }
        }

        public async Task<UserModel> UpdateUserAsync(UserModel request)
        {
            using StringContent jsonContent = new(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync("api/user/" + request.Id, jsonContent);
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadFromJsonAsync<UserModel>().Result;
            }
            else
            {
                return null;
            }
        }

        public async Task<BaseResponse> DeleteUserAsync(int id)
        {
            
            var response = await _httpClient.DeleteAsync("api/user/" + id);
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadFromJsonAsync<BaseResponse>().Result;
            }
            else
            {
                return null;
            }
        }

    }
}
