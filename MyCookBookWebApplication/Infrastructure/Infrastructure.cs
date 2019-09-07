using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using MyCookBookWebApplication.Models;

namespace MyCookBookWebApplication.Infrastructure {
    public class Infrastructure {
	    private IMongoDatabase _db;

        public Infrastructure() {
	        MongoClient dbClient = new MongoClient("mongodb://127.0.0.1:27017");
	        _db = dbClient.GetDatabase("CookBookDB");
        }
		
		#region add methods 

		public void AddUser(User user) {
			IMongoCollection<BsonDocument> usersCollection = _db.GetCollection<BsonDocument>("users and passwords");
			//check if the user name exists 
			User u = GetUser(user.Name);
			if (u != null) {
				throw new Exception("The user name is already exists");
			}

			BsonDocument userDoc = new BsonDocument {
				new BsonElement("_id", user.Id),
				new BsonElement("Name", user.Name),
				new BsonElement("Password", user.Password)
			};
			usersCollection.InsertOne(userDoc);
		}

		public void AddRecipe(Recipe recipe, User user) {
			try {
				IMongoCollection<BsonDocument> recipesCollection = _db.GetCollection<BsonDocument>("recipes");
				BsonDocument recipeDoc = new BsonDocument{
					new BsonElement("_id", recipe.Id),
					new BsonElement("Title", recipe.Title),
					new BsonElement("Ingredients", recipe.Ingredients),
					new BsonElement("Preparation", recipe.Preparation),
					new BsonElement("Rating", recipe.Rating),
					new BsonElement("Date", recipe.Date),
					new BsonElement("Tags", new BsonArray(recipe.Tags)),
					new BsonElement("Link", recipe.Link),
					new BsonElement("UserId", user.Id)
				};

				recipesCollection.InsertOne(recipeDoc);

				IMongoCollection<BsonDocument> usersRecipesCollection = _db.GetCollection<BsonDocument>("users recipes");
				BsonDocument userRecipeDoc = new BsonDocument{
					new BsonElement("UserId", user.Id),
					new BsonElement("RecipeId", recipe.Id),
					new BsonElement("RecipeTitle", recipe.Title)
				};

				usersRecipesCollection.InsertOne(userRecipeDoc);
			}
			catch (Exception) {
				throw new Exception("Failed to add the recipe to the DB");
			}
		}

		public void AddComment(Comment comment) {
			try {
				IMongoCollection<BsonDocument> commentsCollection = _db.GetCollection<BsonDocument>("comments");
				BsonDocument commentDoc = new BsonDocument {
					new BsonElement("RecipeId", comment.RecipeId),
					new BsonElement("CommentText", comment.CommentText),
					new BsonElement("Date", comment.Date)
				};
				commentsCollection.InsertOne(commentDoc);
			}
			catch (Exception) {
				throw new Exception("Failed to add the comment to the DB");
			}
		}

		public void AddTag(Recipe recipe, string newTag) {
			try {
				newTag = newTag.Trim();
				IMongoCollection<BsonDocument> recipesCollection = _db.GetCollection<BsonDocument>("recipes");
				recipesCollection.FindOneAndUpdate(
					"{ _id: ObjectId(\"" + recipe.Id + "\")}",
					"{ $push: { Tags: '" + newTag + "' } }"
				);
			}
			catch (Exception) {
				throw new Exception("Failed to add the tag to the DB");
			}
		}

		#endregion

		#region get methods

		public Recipe GetRecipe(string recipeId) {
			IMongoCollection<Recipe> recipes = _db.GetCollection<Recipe>("recipes");
			return recipes.Find("{ _id: ObjectId(\"" + recipeId + "\")}").ToList()[0];
		}

		public List<Comment> GetComments(string recipeId) {
			IMongoCollection<Comment> commentsCollection = _db.GetCollection<Comment>("comments");
			return commentsCollection.Find("{ RecipeId: ObjectId(\"" + recipeId + "\")}").ToList();
		}

		public List<Recipe> GetRecentUserRecipes(User user, int numOfRecipes) {
			IMongoCollection<Recipe> recipesCollection = _db.GetCollection<Recipe>("recipes");
			return recipesCollection.Find(
				"{ UserId: ObjectId(\"" + user.Id + "\")}").
				Sort("{ Date : -1 }").
				Limit(numOfRecipes).
				ToList();
		}

		public string[] GetAllUserTags(User user) {
			IMongoCollection<Recipe> recipesCollection = _db.GetCollection<Recipe>("recipes");
			List<Recipe> recipesList = recipesCollection.Find(
				"{Tags: {$ne:null} , UserId: ObjectId(\"" + user.Id + "\")}"
				).ToList();
			HashSet<string> tags = new HashSet<string>();
			foreach (Recipe recipe in recipesList) {
				foreach (string tag in recipe.Tags) {
					tags.Add(tag);
				}
			}
			string[] tagsArray = tags.ToArray();
			Array.Sort(tagsArray);
			return tagsArray;
		}

		public List<Recipe> GetTagsRecipes(string tagName, User user) {
			IMongoCollection<Recipe> recipesCollection = _db.GetCollection<Recipe>("recipes");
			return recipesCollection.Find("{Tags: \"" + tagName+ "\" , UserId: ObjectId(\"" + user.Id + "\")}").ToList();
		}

		public List<Recipe> GetRecipeByPartialTitle(string partialTitle, User user) {
			IMongoCollection<Recipe> recipesCollection = _db.GetCollection<Recipe>("recipes");
			return recipesCollection.Find("{Title:/" + partialTitle + "/ , UserId: ObjectId(\"" + user.Id + "\")}").ToList();
		}

		public User GetUser(string userName) {
			IMongoCollection<User> usersCollection = _db.GetCollection<User>("users and passwords");
			List<User> users = usersCollection.Find("{Name: '" + userName + "' }").ToList();
			return users.Count == 1 ? users[0] : null;
		}

		public bool IsUserNameExists(string userName) {
			return GetUser(userName) != null;
		}

		#endregion

		#region edit methods

		public void EditRecipe(Recipe recipe) {
			try {
				IMongoCollection<BsonDocument> recipesCollection = _db.GetCollection<BsonDocument>("recipes");
				recipesCollection.FindOneAndUpdate(
					"{ _id: ObjectId(\"" + recipe.Id + "\")}",
					"{ $set: { Title: '" + recipe.Title + "' , Ingredients : '" + recipe.Ingredients + "' , " +
					"Preparation : '" + recipe.Preparation + "', " +
					"Link : '" + recipe.Link + "' } }"
				);
			} catch (Exception) {
				throw new Exception("Failed to edit the recipe");
			}

		}

		public void EditComments(List<Comment> comments, string[] editedCommentsText) {
			try {
				IMongoCollection<Comment> commentsCollection = _db.GetCollection<Comment>("comments");
				for (int i = 0; i < comments.Count; i++) {
					if (
						String.IsNullOrEmpty(editedCommentsText[i])
					) {
						DeleteComment(comments[i]);
					} 
					else if (!comments[i].CommentText.Equals(
						editedCommentsText[i])
					) {
						commentsCollection.FindOneAndUpdate(
							"{ _id: ObjectId(\"" + comments[i].Id + "\")}",
							"{ $set: { CommentText: '" + editedCommentsText[i] + "' } }"
						);
					}
				}
			} 
			catch (Exception) {
				throw new Exception("Failed to edit the comment");
			}
		}

		public void EditRating(Recipe recipe, int newRating) {
			try {
				IMongoCollection<BsonDocument> recipesCollection = _db.GetCollection<BsonDocument>("recipes");
				recipesCollection.FindOneAndUpdate(
					"{ _id: ObjectId(\"" + recipe.Id + "\")}",
					"{ $set: { Rating : " + newRating + " } }"
				);
			}
			catch (Exception) {
				throw new Exception("Failed to edit the rating");
			}
		}

		#endregion

		#region delete methods

		public void DeleteRecipe(Recipe recipe) {
			try {
				IMongoCollection<BsonDocument> recipesCollection = _db.GetCollection<BsonDocument>("recipes");
				recipesCollection.DeleteOne("{ _id: ObjectId(\"" + recipe.Id + "\")}");
				DeleteAllRecipeComments(recipe);
				DeleteRecipeFromUser(recipe);
			}
			catch (Exception) {
				throw new Exception("Failed to delete the recipe");
			}
		}

		public void DeleteComment(Comment comment) {
			try {
				IMongoCollection<Comment> commentsCollection = _db.GetCollection<Comment>("comments");
				commentsCollection.DeleteOne("{ _id: ObjectId(\"" + comment.Id + "\")}");
			} catch (Exception) {
				throw new Exception("Failed to delete the comment");
			}
		}

		public void DeleteTag(Recipe recipe, string[] tagsToDelete) {
			try {
				IMongoCollection<BsonDocument> recipesCollection = _db.GetCollection<BsonDocument>("recipes");
				foreach (string tagToDelete in tagsToDelete) {
					recipesCollection.UpdateOne(
						"{ _id: ObjectId(\"" + recipe.Id + "\")}",
						"{ $pull: { Tags: '" + tagToDelete + "' } }");
					recipe.DeleteTag(tagToDelete);
				}
			} catch (Exception) {
				throw new Exception("Failed to delete the tag");
			}
		}

		private void DeleteAllRecipeComments(Recipe recipe) {
			try {
				IMongoCollection<Comment> commentsCollection = _db.GetCollection<Comment>("comments");
				commentsCollection.DeleteOne("{ RecipeId : ObjectId(\"" + recipe.Id + "\")}");
			}
			catch (Exception) {
				throw new Exception("Failed to delete all the recipe comments");
			}
		}

		private void DeleteRecipeFromUser(Recipe recipe) {
			try {
				IMongoCollection<BsonDocument> usersRecipes = _db.GetCollection<BsonDocument>("users recipes");
				usersRecipes.DeleteOne("{ RecipeId : ObjectId(\"" + recipe.Id + "\")}");
			}
			catch (Exception) {
				throw new Exception("Failed to delete the recipe from the user");
			}
		}

		#endregion

	}
}