using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SwoopMarketplaceProject.Models;

namespace SwoopMarketplaceProject.Pages.Listings;

public class ListingsDetailsModel : PageModel
{
    private readonly SwoopContext _db;
    public ListingsDetailsModel(SwoopContext db) => _db = db;

    public Listing? Listing { get; private set; }
    public string? MainImage { get; private set; }
    public List<ListingImage> OtherImages { get; private set; } = new();

    public async Task<IActionResult> OnGetAsync(long id)
    {
        Listing = await _db.Listings
            .Include(l => l.ListingImages)
            .Include(l => l.User)
            .Include(l => l.Category)
            .FirstOrDefaultAsync(l => l.Id == id && l.Status == "active");

        if (Listing == null) return NotFound();

        MainImage = Listing.ListingImages?.FirstOrDefault(i => i.IsPrimary == true)?.ImageUrl
                    ?? Listing.ListingImages?.FirstOrDefault()?.ImageUrl;

        OtherImages = Listing.ListingImages?.Where(i => i.ImageUrl != MainImage).ToList() ?? new List<ListingImage>();

        return Page();
    }
}