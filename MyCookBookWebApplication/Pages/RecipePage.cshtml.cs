using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MyCookBookWebApplication.Models;

namespace MyCookBookWebApplication.Pages
{
    public class RecipePageModel : MyCookBookModel {
		
	    [BindProperty]
	    public Recipe Recipe { get; set; }
	    [BindProperty]
	    public List<Comment> Comments { get; set; }
		[BindProperty]
	    public string[] Ingredients { get; set; }
		[BindProperty]
	    public string[] Preparation { get; set; }

		public async Task OnGet(string id) {
			try {
				if (id == null) {
					id = HttpContext.Session.GetString("recipeId");
				}
				InitializeAll(id);
			} catch (Exception) {
				HttpContext.Session.SetString("errorMsg", "שגיאה בטעינת המתכון");
				RedirectToPage("ErrorPage");
			}
		}

		public async Task<IActionResult> OnPostChangeRating(int rating, string id) {
			try {
				InitializeAll(id);
				Inf.EditRating(Recipe, rating);
				Recipe.ChangeRating(rating);
				return Page();
			} catch (Exception) {
				HttpContext.Session.SetString("errorMsg", "שגיאה בעריכת הדירוג");
				return RedirectToPage("ErrorPage");
			}
		}

		public async Task<IActionResult> OnPostDeleteTag(string id) {
			try {
				InitializeAll(id);
				bool res = Request.Form.TryGetValue("tag", out var tagsToDelete);
				if (res) {
					Inf.DeleteTag(Recipe, tagsToDelete);
				}
				return Page();
			} catch (Exception) {
				HttpContext.Session.SetString("errorMsg", "שגיאה במחיקת הטאג");
				return RedirectToPage("ErrorPage");
			}
		}

		public async Task<IActionResult> OnPostDeleteRecipe(string id) {
			try {
				InitializeAll(id);
				HttpContext.Session.SetString("userId", Recipe.UserId.ToString());
				Inf.DeleteRecipe(Recipe);
				return RedirectToPage("RecentRecipes");
			} catch (Exception) {
				HttpContext.Session.SetString("errorMsg", "שגיאה במחיקת המתכון");
				return RedirectToPage("ErrorPage");
			}
		}

		public async Task<IActionResult> OnPostAddTag(string newTag, string id) {
			try {
				InitializeAll(id);
				Inf.AddTag(Recipe, newTag);
				Recipe.AddTag(newTag);
				return Page();
			} catch (Exception) {
				HttpContext.Session.SetString("errorMsg", "שגיאה בהוספת הטאג");
				return RedirectToPage("ErrorPage");
			}
		}

		public async Task<IActionResult> OnPostAddComment(string newComment, string id) {
			try {
				InitializeAll(id);
				Comment newCom = new Comment(newComment, new ObjectId(id));
				Inf.AddComment(newCom);
				Comments.Add(newCom);
				return Page();
			} catch (Exception) {
				HttpContext.Session.SetString("errorMsg", "שגיאה בהוספת התגובה");
				return RedirectToPage("ErrorPage");
			}
		}

		public async Task<IActionResult> OnPostEditRecipe(string id) {
			try {
				InitializeAll(id);
				HttpContext.Session.SetString("recipeId", Recipe.Id.ToString());
				HttpContext.Session.SetString("userId", Recipe.UserId.ToString());
				return RedirectToPage("EditRecipe");
			} catch (Exception) {
				return RedirectToPage("ErrorPage");
			}
		}

		public async Task<IActionResult> OnPostTag(string id) {
			try {
				HttpContext.Session.SetString("tagName", id);
				return RedirectToPage("TagsRecipes");
			} catch (Exception) {
				HttpContext.Session.SetString("errorMsg", "שגיאה בהוספת טאג");
				return RedirectToPage("ErrorPage");
			}
		}

		private void InitializeAll(string recipeId) {
			Recipe = Inf.GetRecipe(recipeId);
			Comments = Inf.GetComments(recipeId);
			Ingredients = Recipe.Ingredients.Split(new[] { '\n' });
			Preparation = Recipe.Preparation.Split(new[] { '\n' });
		}
	}
}