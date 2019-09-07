using System;
using MongoDB.Bson;

namespace MyCookBookWebApplication.Models {
    public class Comment {
		public ObjectId Id;
		public ObjectId RecipeId;
        public string CommentText;
        public DateTime Date;

        public Comment(string comment) {
	        IsValidComment(comment);
	        CommentText = comment;
	        Date = DateTime.Now.Date;
        }

		public Comment(string comment, ObjectId recipeId) {
            IsValidComment(comment);
            CommentText = comment;
            Date = DateTime.Now.Date;
            RecipeId = recipeId;
        }

		public Comment() { }

        private void IsValidComment(string str) {
            if (String.IsNullOrEmpty(str)) {
                throw new Exception("The Comment Is Empty");
            }
        }
    }
}