using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Bson;
using MyCookBookWebApplication.Models;

namespace MyCookBookWebApplication.Pages
{
    public class RecentRecipesModel : MyCookBookModel {
	    
	    public List<Recipe> Recipes;
	    private readonly int numOfRecent = 10;

        public async Task OnGet(string id) {
	        if (id == null) {
		        id = HttpContext.Session.GetString("userId");
	        }
	        else {
				HttpContext.Session.SetString("userId", id);
			}
			User = new User(id);
	        Recipes = Inf.GetRecentUserRecipes(User, numOfRecent);
		}

	}
}