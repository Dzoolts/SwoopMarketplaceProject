using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SwoopMarketplaceProject.Models;

public class IndexModel : PageModel
{
    private readonly SwoopContext _db;
    public IndexModel(SwoopContext db) => _db = db;

    public IList<Listing> Featured { get; private set; } = new List<Listing>();
    public IList<Category> Categories { get; private set; } = new List<Category>();

    public async Task OnGetAsync()
    {
        Categories = await _db.Categories.OrderBy(c => c.Name).ToListAsync();
        Featured = await _db.Listings
            .Where(l => l.Status == "active")
            .Include(l => l.ListingImages)
            .Include(l => l.User)
            .Include(l => l.Category)
            .OrderByDescending(l => l.CreatedAt)
            .Take(12)
            .ToListAsync();
    }
}