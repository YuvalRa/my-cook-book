using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MyCookBookWebApplication.Models {
	public class Recipe {
		[BsonId]
		public ObjectId Id;
		public string Title;
		public string Ingredients;
		public string Preparation;
		public int Rating;
		public List<string> Tags;
		public DateTime Date;
		public string Link;
		public ObjectId UserId;

		public Recipe() {
			Title = null;
			Ingredients = null;
			Preparation = null;
			Rating = -1;
			Tags = new List<string>();
			Date = DateTime.Now.Date;
			Link = null;
			Id = ObjectId.GenerateNewId();
		}

		public Recipe(
			string title,
			string ingredients,
			string preparation,
			int rating,
			List<string> tags,
			string link,
			ObjectId userId
		) {
			CreateNewRecipe(title, ingredients, preparation, rating, tags, link, userId);
		}

		private void CreateNewRecipe(
			string title,
			string ingredients,
			string preparation,
			int rating,
			List<string> tags,
			string link,
			ObjectId userId
		) {
			CheckTheTitle(title);
			CheckTheIngredients(ingredients);
			CheckThePreparationInstructions(preparation);

			if (tags == null) {
				tags = new List<string>();
			} else if (tags.Count > 0) {
				CheckTheTags(tags);
			}

			if (!CheckTheRating(rating)) {
				rating = 0;
			}

			Title = title;
			Ingredients = ingredients;
			Preparation = preparation;
			Rating = rating;
			Tags = tags;
			Date = DateTime.Now.Date;
			Link = link;
			Id = ObjectId.GenerateNewId();
			UserId = userId;
		}

		#region validation checks methods

		private void CheckTheTitle(string str) {
			if (String.IsNullOrEmpty(str)) {
				throw new Exception("The Title Is Missing");
			}
		}

		private void CheckTheIngredients(string str) {
			if (String.IsNullOrEmpty(str)) {
				throw new Exception("The Ingredients Are Missing");
			}
		}

		private void CheckThePreparationInstructions(string str) {
			if (String.IsNullOrEmpty(str)) {
				throw new Exception("The Preparation Instructions Are Missing");
			}
		}

		private bool CheckTheRating(int rating) {
			return rating >= 0 && rating <= 5;
		}

		private void CheckTheTag(string tag) {
			if (String.IsNullOrEmpty(tag)) {
				throw new Exception("The Tag Is Empty");
			}
		}

		private void CheckTheTags(List<string> tags) {
			foreach (string tag in tags) {
				CheckTheTag(tag);
			}
		}

		#endregion

		#region updates methods

		public void ChangeTitle(string newTitle) {
			CheckTheTitle(newTitle);
			Title = newTitle;
		}

		public void ChangeIngredients(string newIngredients) {
			CheckTheIngredients(newIngredients);
			Ingredients = newIngredients;
		}

		public void ChangePreparationInstructions(string newPreparationInstructions) {
			CheckThePreparationInstructions(newPreparationInstructions);
			Preparation = newPreparationInstructions;
		}

		public bool ChangeRating(int newRating) {
			if (CheckTheRating(newRating)) {
				Rating = newRating;
				return true;
			}
			return false;
		}

		public void AddTag(string tag) {
			CheckTheTag(tag);
			Tags.Add(tag);
		}

		public void DeleteTag(string tagToDelete) {
			Tags.Remove(tagToDelete);
		}

		public void ChangeLink(string newLink) {
			Link = newLink;
		}

		#endregion
	}
}