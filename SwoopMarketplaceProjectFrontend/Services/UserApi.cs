using SwoopMarketplaceProjectFrontend.Dtos;

namespace SwoopMarketplaceProjectFrontend.Services
{
    public class UserApi
    {
        private readonly IHttpClientFactory _f;

        public UserApi(IHttpClientFactory f) => _f = f;

        // Return actual UserDto list (fixed)
        public async Task<List<UserDto>> GetAllAsync()
            => await _f.CreateClient("SwoopApi")
                .GetFromJsonAsync<List<UserDto>>("api/Users") ?? new();

        public async Task<UserDto?> GetByAzonAsync(int azon)
            => await _f.CreateClient("SwoopApi")
                .GetFromJsonAsync<UserDto>($"api/Users/{azon}");

        // New helper: find user by email via the Users endpoint
        public async Task<UserDto?> GetByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return null;
            var client = _f.CreateClient("SwoopApi");
            try
            {
                var all = await client.GetFromJsonAsync<List<UserDto>>("api/Users") ?? new();
                return all.FirstOrDefault(u => string.Equals(u.Email, email, StringComparison.OrdinalIgnoreCase));
            }
            catch
            {
                return null;
            }
        }

        public async Task CreateAsync(UserDto dto)
        {
            var r = await _f.CreateClient("SwoopApi").PostAsJsonAsync("api/Users", dto);
            r.EnsureSuccessStatusCode();
        }

        public async Task UpdateAsync(int azon, UserDto dto)
        {
            var r = await _f.CreateClient("SwoopApi").PutAsJsonAsync($"api/Users/{azon}", dto);
            r.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(int azon)
        {
            var r = await _f.CreateClient("SwoopApi").DeleteAsync($"api/Users/{azon}");
            r.EnsureSuccessStatusCode();
        }
    }
}
