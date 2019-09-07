using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyCookBookWebApplication.Models;

namespace MyCookBookWebApplication.Pages {
	public class AddRecipeModel : MyCookBookModel {

		[BindProperty]
		public Recipe Recipe { get; set; }
		[BindProperty]
		public Comment Comment { get; set; }
		
		public void OnGet(string id) {
			if (id == null) {
				id = HttpContext.Session.GetString("userId");
			}

			User = new User(id);
		}

		public async Task<IActionResult> OnPostSave(string id) {
			try {
				User = new User(id);
				string title = Request.Form["name"];
				string ingredients = Request.Form["ing"];
				string preparation = Request.Form["prep"];
				string link = Request.Form["link"];
				string tags = Request.Form["tags"];
				int rating = Int32.Parse(Request.Form["rating"]);
				string commentText = Request.Form["comments"];
				List<string> tagsList = null;
				if (!tags.Equals("")) {
					tagsList = tags.Split(", ").ToList();
				}

				Recipe = new Recipe(title, ingredients, preparation, rating, tagsList, link, User.Id);
				Inf.AddRecipe(Recipe, User);
				if (!String.IsNullOrEmpty(commentText)) {
					Comment = new Comment(commentText, Recipe.Id);
					Inf.AddComment(Comment);
				}

				HttpContext.Session.SetString("recipeId", Recipe.Id.ToString());
				HttpContext.Session.SetString("userId", id);

				return RedirectToPage("RecipePage");
			} catch (Exception) {
				HttpContext.Session.SetString("errorMsg", "שגיאה בהוספת המתכון");
				return RedirectToPage("ErrorPage");
			}
		}
	}
}