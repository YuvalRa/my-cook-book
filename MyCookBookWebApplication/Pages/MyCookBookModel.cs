using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyCookBookWebApplication.Models;

namespace MyCookBookWebApplication.Pages {
	public class MyCookBookModel : PageModel {

		[BindProperty]
		public User User { get; set; }
		public Infrastructure.Infrastructure Inf = new Infrastructure.Infrastructure();

		public async Task<IActionResult> OnPostSearch(string searchStr) {
			try {
				HttpContext.Session.SetString("searchStr", searchStr);
				return RedirectToPage("SearchResults");
			} catch (Exception) {
				return RedirectToPage("ErrorPage");
			}
		}
	}
}
