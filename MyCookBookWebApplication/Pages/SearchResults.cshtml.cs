using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MyCookBookWebApplication.Models;

namespace MyCookBookWebApplication.Pages
{
    public class SearchResultsModel : MyCookBookModel {

	    public List<Recipe> Recipes;
	    public string searchStr;

        public async Task OnGet(string str) {
			try {
				if (str == null) {
					str = HttpContext.Session.GetString("searchStr");
				}
				searchStr = str;
				string userId = HttpContext.Session.GetString("userId");
				User = new User(userId);
				Recipes = Inf.GetRecipeByPartialTitle(searchStr, User);
			} catch (Exception) {
				RedirectToPage("ErrorPage");
			}
        }
    }
}