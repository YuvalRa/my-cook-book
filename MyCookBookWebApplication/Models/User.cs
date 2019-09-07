using System;
using System.Security.Cryptography;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MyCookBookWebApplication.Models {
	public class User {

		[BsonId]
		public ObjectId Id;
		public string Name;
		public string Password;

		public User(string userName, string userPassword) {
			Name = userName;
			Password = GetHashString(userPassword);
			Id = ObjectId.GenerateNewId();
		}

		public User(ObjectId id) {
			Id = id;
		}

		public User(string id) {
			Id = new ObjectId(id);
		}

		public User() { }

		public static User GetUser(string email, string pass) {
			Infrastructure.Infrastructure inf = new Infrastructure.Infrastructure();
			User user = inf.GetUser(email);
			bool correctPass = false;
			if (user != null) {
				correctPass = GetHashString(pass).Equals(user.Password);
			}
			return correctPass ? user : null;
		}

		private static byte[] GetHash(string inputString) {
			HashAlgorithm algorithm = SHA256.Create();
			return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
		}

		private static string GetHashString(string inputString) {
			StringBuilder sb = new StringBuilder();
			foreach (byte b in GetHash(inputString))
				sb.Append(b.ToString("X2"));
			return sb.ToString();
		}

	}
}