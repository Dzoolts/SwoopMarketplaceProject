using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SwoopMarketplaceProjectFrontend.Dtos;
using SwoopMarketplaceProjectFrontend.Services;

namespace SwoopMarketplaceProjectFrontend.Pages.Reports
{
    public class CreateModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly AuthSession _authSession;

        public CreateModel(IHttpClientFactory httpClientFactory, AuthSession authSession)
        {
            _httpClient = httpClientFactory.CreateClient("SwoopApi");
            _authSession = authSession;
        }

        [BindProperty]
        public ReportDto Report { get; set; } = new();

        public bool Success { get; set; }

        // 👉 GET (amikor megnyitod az oldalt)
        public void OnGet(int listingId)
        {
            Report.ListingId = listingId;
        }

        // 👉 POST (amikor elküldöd a formot)
        public async Task<IActionResult> OnPostAsync()
        {
            // token hozzáadása
            var token = _authSession.GetToken();
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }

            var json = JsonSerializer.Serialize(Report);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/reports", content);

            if (response.IsSuccessStatusCode)
            {
                Success = true;

                // opcionális: ürítjük a mezőt
                Report.Description = string.Empty;

                return Page();
            }

            ModelState.AddModelError(string.Empty, "Hiba történt a jelentés küldésekor.");
            return Page();
        }
    }
}