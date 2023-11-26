using Blazored.LocalStorage;
using HappyWarehouse.BusinessLogic.DTOs.Common;
using HappyWarehouse.Shared.Common;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace HappyWarehouse.Client.Services
{
    public class ApiClientService
    {
        public HttpClient _httpClient;
        public NavigationManager _navigationManager;
        public ILocalStorageService _localStorageService;

        public ApiClientService(NavigationManager navigationManager, ILocalStorageService localStorageService, HttpClient httpClient)
        {
            _navigationManager = navigationManager;
            _localStorageService = localStorageService;
            _httpClient = httpClient;
        }

        public async Task<List<T>> GetList<T>(string url)
        {
            try
            {
                string token = await _localStorageService.GetItemAsStringAsync("token");
                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                var response = await _httpClient.GetAsync($"{_navigationManager.BaseUri}api/{url}");

                response.EnsureSuccessStatusCode();

                var responseString = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<T>>(responseString);

                return result;
            }
            catch (Exception ex)
            {
                return new List<T>();
            }
        }

        public async Task<PaginatedList<T>> GetPagedResult<T>(int? pageNumber, string url, int? warehouseId = null)
        {
            try
            {
                string token = await _localStorageService.GetItemAsStringAsync("token");
                var request = new HttpRequestMessage()
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"{_navigationManager.BaseUri}api/{url}?pageNumber={pageNumber}{(warehouseId != null ? "&warehouseId=" + warehouseId : "")}")
                };
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.SendAsync(request);

                response.EnsureSuccessStatusCode();

                var responseString = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<PaginatedList<T>>(responseString);

                return result;
            }
            catch (Exception ex)
            {
                return new PaginatedList<T>();
            }
        }

        public async Task<QueryResult<bool>> Add(object payload, string url)
        {
            try
            {
                var stringData = JsonConvert.SerializeObject(payload);
                string token = await _localStorageService.GetItemAsStringAsync("token");

                var request = new HttpRequestMessage()
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri($"{_navigationManager.BaseUri}api/{url}"),
                    Content = new StringContent(stringData, System.Text.Encoding.UTF8, "application/json")
                };
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var responseString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<QueryResult<bool>>(responseString);
            }
            catch (Exception ex)
            {
                return new QueryResult<bool>()
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<T> PostAsync<T>(object payload, string url)
        {
            var stringData = JsonConvert.SerializeObject(payload);
            string token = await _localStorageService.GetItemAsStringAsync("token");

            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{_navigationManager.BaseUri}api/{url}"),
                Content = new StringContent(stringData, System.Text.Encoding.UTF8, "application/json")
            };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(request);
            var responseString = await response.Content.ReadAsStringAsync();

            response.EnsureSuccessStatusCode();

            return JsonConvert.DeserializeObject<T>(responseString);
        }

        public async Task<QueryResult<bool>> Update(object payload, string url)
        {
            try
            {
                var stringData = JsonConvert.SerializeObject(payload);
                string token = await _localStorageService.GetItemAsStringAsync("token");

                var request = new HttpRequestMessage()
                {
                    Method = HttpMethod.Put,
                    RequestUri = new Uri($"{_navigationManager.BaseUri}api/{url}"),
                    Content = new StringContent(stringData, System.Text.Encoding.UTF8, "application/json")
                };
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var responseString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<QueryResult<bool>>(responseString);
            }
            catch (Exception ex)
            {
                return new QueryResult<bool>()
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<QueryResult<bool>> Delete(int id, string url)
        {
            try
            {
                string token = await _localStorageService.GetItemAsStringAsync("token");

                var request = new HttpRequestMessage()
                {
                    Method = HttpMethod.Delete,
                    RequestUri = new Uri($"{_navigationManager.BaseUri}api/{url}/{id}")
                };
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var responseString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<QueryResult<bool>>(responseString);
            }
            catch (Exception ex)
            {
                return new QueryResult<bool>()
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }
    }
}
