using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyCookBookWebApplication.Models;

namespace MyCookBookWebApplication.Pages
{
    public class TagsRecipesModel : MyCookBookModel {

		[BindProperty]
		public string TagName { get; set; }
		[BindProperty]
		public List<Recipe> Recipes { get; set; }

		public async Task OnGet(string id) {
			try {
				if (id == null) {
					TagName = HttpContext.Session.GetString("tagName");
				} else {
					TagName = id;
					HttpContext.Session.SetString("tagName", id);
				}
				User = new User(HttpContext.Session.GetString("userId"));
				Recipes = Inf.GetTagsRecipes(TagName, User);
			} catch (Exception) {
				RedirectToPage("ErrorPage");
			}
		}
    }
}