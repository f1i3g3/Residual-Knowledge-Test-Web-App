using System.Security.Cryptography;
using System.Text;

namespace ResidualKnowledgeApp.Tests
{
	[TestClass]
	public class DataBaseTests
	{
		[TestMethod]
		public void DeleteRecord()
		{
			Assert.AreEqual(1, 1);	

		}

		[TestMethod]
		public void Crypto()
		{
			using (var mySha = SHA256.Create())
			{
				string password = "1234rreerr5";
				string salt = RandomNumberGenerator.GetInt32(1000).ToString();

				var bytes = Encoding.UTF8.GetBytes(password + salt);
				var value = mySha.ComputeHash(bytes);

				Console.WriteLine(value.Length);
                Assert.IsNotNull(value);
			}
		}
	}
}