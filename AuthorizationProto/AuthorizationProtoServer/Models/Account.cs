using System.ComponentModel.DataAnnotations;

namespace AuthorizationPrototype.Models
{
	public class Account
	{
		public int ContactId { get; set; }

		// user ID from AspNetUser table.
		public string? OwnerID { get; set; } // Added

		public string? FirstName { get; set; }
		public string? LastName { get; set; }

		[DataType(DataType.EmailAddress), Required]
		public string? Email { get; set; }

		[Required]
		public int? PasswordField { get; set; }

		public ContactStatus Status { get; set; } // Added
	}

	public enum AccountStatus // Added
	{
		Submitted,
		Approved,
		Rejected
	}
}
