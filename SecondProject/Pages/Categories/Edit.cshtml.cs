using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SecondProject.Data;
using SecondProject.Model;

namespace SecondProject.Pages.Categories;
[BindProperties]
public class EditModel : PageModel
{
    private readonly ApplicationDbContext _db;

    public Category Category { get; set; }
    public EditModel(ApplicationDbContext db)
    {
        _db= db;
    }
    public void OnGet(int? id)
    {
        Category = _db.Category.Find(id);
    }

    public async Task<IActionResult> OnPost()
    {
        if (Category.Name == Category.DisplayOrder.ToString())
        {
            ModelState.AddModelError("Category.DisplayOrder","The Display Order can't be same as the name");
        }
        if (ModelState.IsValid)
        {
             _db.Category.Update(Category);
            _db.SaveChanges();
            TempData["success"] = "Category Updated Sucessfully!";
            return RedirectToPage("Index");
        }
        return Page();
    }
}
