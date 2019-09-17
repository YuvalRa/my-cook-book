using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyCookBookWebApplication.Models;

namespace MyCookBookWebApplication.Pages {
    public class IndexModel : PageModel {
	    public User User;
        public bool failedTologin = false;
		Infrastructure.Infrastructure inf = new Infrastructure.Infrastructure();

	    public void OnGet() { }

        public async Task<IActionResult> OnPostRegister() {
			string email = Request.Form["email"];
			string password = Request.Form["psw"];
			string passRepeat = Request.Form["psw-repeat"];
			
			if (password.Equals(passRepeat) && !inf.IsUserNameExists(email)) {
				User = new User(email, password);
				inf.AddUser(User);
				HttpContext.Session.SetString("userId", User.Id.ToString());
				return RedirectToPage("RecentRecipes");
			}

            failedTologin = true;
			return Page();
        }

        public async Task<IActionResult> OnPostLogin() {
	        string email = Request.Form["email"];
	        string password = Request.Form["psw"];
		
			User = User.GetUser(email, password);
			if (User != null) {
				HttpContext.Session.SetString("userId", User.Id.ToString());
				return RedirectToPage("RecentRecipes");
			}

            failedTologin = true;
			return Page();
		}
	}
}