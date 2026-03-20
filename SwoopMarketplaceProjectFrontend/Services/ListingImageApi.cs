using System.Net.Http.Headers;
using System.Net.Http.Json;
using SwoopMarketplaceProjectFrontend.Dtos;

namespace SwoopMarketplaceProjectFrontend.Services
{
    public class ListingImageApi
    {
        private readonly IHttpClientFactory _f;
        private readonly AuthSession _auth;

        public ListingImageApi(IHttpClientFactory f, AuthSession auth) { _f = f; _auth = auth; }

        public async Task<List<ListingImageDto>> GetAllAsync()
            => await _f.CreateClient("SwoopApi")
                .GetFromJsonAsync<List<ListingImageDto>>("api/ListingImages") ?? new();

        public async Task<ListingImageDto?> GetByAzonAsync(int azon)
            => await _f.CreateClient("SwoopApi")
                .GetFromJsonAsync<ListingImageDto>($"api/ListingImages/{azon}");

        public async Task CreateAsync(ListingImageDto dto)
        {
            var client = _f.CreateClient("SwoopApi");

            using var req = new HttpRequestMessage(HttpMethod.Post, "api/ListingImages")
            {
                Content = JsonContent.Create(dto)
            };

            var token = _auth.GetToken();
            if (!string.IsNullOrWhiteSpace(token))
                req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var r = await client.SendAsync(req);
            r.EnsureSuccessStatusCode();
        }


        public async Task UpdateAsync(int azon, ListingImageDto dto)

        {

            var r = await _f.CreateClient("SwoopApi")

            .PutAsJsonAsync($"api/ListingImages/{azon}", dto);

            r.EnsureSuccessStatusCode();

        }


        public async Task DeleteAsync(int azon)

        {

            var r = await _f.CreateClient("SwoopApi")

            .DeleteAsync($"api/ListingImages/{azon}");

            r.EnsureSuccessStatusCode();

        }
    }
}
