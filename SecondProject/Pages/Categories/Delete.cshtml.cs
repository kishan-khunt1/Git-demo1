using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SecondProject.Data;
using SecondProject.Model;

namespace SecondProject.Pages.Categories;
[BindProperties]
public class DeleteModel : PageModel
{
    private readonly ApplicationDbContext _db;

    public Category Category { get; set; }
    public DeleteModel(ApplicationDbContext db)
    {
        _db = db;
    }
    public void OnGet(int? id)
    {
        Category = _db.Category.Find(id);
    }

    public async Task<IActionResult> OnPost()
    {

        var deletefromdb = _db.Category.Find(Category.Id);
        if (deletefromdb != null)
        {
            _db.Category.Remove(deletefromdb);
            await _db.SaveChangesAsync();
            TempData["success"]= "Category Deleted Sucessfully!";
            return RedirectToPage("Index");

        }
        return Page();
    }
}

