using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using MyCookBookWebApplication.Models;

namespace MyCookBookWebApplication.Pages
{
    public class EditRecipeModel : MyCookBookModel {

	    [BindProperty]
		public Recipe Recipe { get; set; }
		[BindProperty]
		public List<Comment> Comments { get; set; }
		
        public async Task OnGet(string id) {
	        if (id == null) {
		        id = HttpContext.Session.GetString("recipeId");
	        }
			InitializeAll(id);
        }

        public async Task<IActionResult> OnPostSave(string id) {
			try {
				InitializeAll(id);
				HttpContext.Session.SetString("recipeId", Recipe.Id.ToString());
				HttpContext.Session.SetString("userId", Recipe.UserId.ToString());

				string title = Request.Form["name"];
				string ingredients = Request.Form["ing"];
				string preparation = Request.Form["prep"];
				string link = Request.Form["link"];

				Recipe.ChangeTitle(title);
				Recipe.ChangeIngredients(ingredients);
				Recipe.ChangePreparationInstructions(preparation);
				Recipe.ChangeLink(link);

				Inf.EditRecipe(Recipe);

				if (Request.Form.TryGetValue("comments", out var commentsText)) {
					Inf.EditComments(Comments, commentsText);
				}

				return RedirectToPage("RecipePage");
			} catch (Exception) {
				HttpContext.Session.SetString("errorMsg", "שגיאה בעריכת המתכון");
				return RedirectToPage("ErrorPage");
			}
        }

		private void InitializeAll(string recipeId) {
	        Recipe = Inf.GetRecipe(recipeId);
	        Comments = Inf.GetComments(recipeId);
        }
	}
}