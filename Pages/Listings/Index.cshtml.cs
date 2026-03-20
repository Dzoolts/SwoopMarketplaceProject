using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SwoopMarketplaceProject.Models;

namespace SwoopMarketplaceProject.Pages.Listings;

public class ListingsIndexModel : PageModel
{
    private readonly SwoopContext _db;
    public ListingsIndexModel(SwoopContext db) => _db = db;

    public IList<Listing> Listings { get; private set; } = new List<Listing>();

    [BindProperty(SupportsGet = true)]
    public string? Q { get; set; }

    [BindProperty(SupportsGet = true)]
    public long? CategoryId { get; set; }

    public async Task OnGetAsync()
    {
        var query = _db.Listings
            .Where(l => l.Status == "active")
            .Include(l => l.ListingImages)
            .Include(l => l.User)
            .Include(l => l.Category)
            .AsQueryable();

        if (CategoryId.HasValue)
            query = query.Where(l => l.CategoryId == CategoryId.Value);

        if (!string.IsNullOrWhiteSpace(Q))
            query = query.Where(l => l.Title.Contains(Q) || l.Description.Contains(Q));

        Listings = await query.OrderByDescending(l => l.CreatedAt).Take(100).ToListAsync();
    }
}