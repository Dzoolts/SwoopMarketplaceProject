using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SwoopMarketplaceProject.Models;
using System.ComponentModel.DataAnnotations;

namespace SwoopMarketplaceProject.Pages.Listings;

public class ListingsCreateModel : PageModel
{
    private readonly SwoopContext _db;
    public ListingsCreateModel(SwoopContext db) => _db = db;

    public IList<Category> Categories { get; private set; } = new List<Category>();

    public class InputModel
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        public decimal Price { get; set; }

        [Required(ErrorMessage = "Please select a category.")]
        public long? CategoryId { get; set; }

        public string Condition { get; set; } = "used";
        public string? Location { get; set; }

        [Required]
        public string Description { get; set; } = string.Empty;
    }

    [BindProperty]
    public InputModel Input { get; set; } = new();

    public async Task OnGetAsync()
    {
        Categories = await _db.Categories.OrderBy(c => c.Name).ToListAsync();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        // re-populate categories for redisplay if validation fails
        Categories = await _db.Categories.OrderBy(c => c.Name).ToListAsync();

        if (!ModelState.IsValid)
        {
            return Page();
        }

        // server-side sanity: ensure category exists
        if (Input.CategoryId == null || !await _db.Categories.AnyAsync(c => c.Id == Input.CategoryId.Value))
        {
            ModelState.AddModelError("Input.CategoryId", "Please select a valid category.");
            return Page();
        }

        var listing = new Listing
        {
            Title = Input.Title,
            Description = Input.Description,
            Price = Input.Price,
            CategoryId = Input.CategoryId,
            Condition = Input.Condition,
            Location = Input.Location,
            Status = "active",
            UserId = 1, // TODO: replace with current user id from auth
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _db.Listings.Add(listing);
        await _db.SaveChangesAsync();

        return RedirectToPage("/Listings/Details", new { id = listing.Id });
    }
}