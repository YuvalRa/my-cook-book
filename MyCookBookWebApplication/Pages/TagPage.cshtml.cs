using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyCookBookWebApplication.Models;

namespace MyCookBookWebApplication.Pages
{
    public class TagPageModel : MyCookBookModel {
		[BindProperty]
	    public string[] TagsList { get; set; }

		public async Task OnGet(string id) {
			try {
				if (id == null) {
					id = HttpContext.Session.GetString("userId");
				} else {
					HttpContext.Session.SetString("userId", id);
				}
				User = new User(id);
				TagsList = Inf.GetAllUserTags(User);
			} catch (Exception) {
				RedirectToPage("ErrorPage");
			}
	    }
		
		public async Task<IActionResult> OnPostTag(string id) {
			HttpContext.Session.SetString("tagName", id);
			return RedirectToPage("TagsRecipes");
		}
	}
}