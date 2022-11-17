namespace ResidualKnowledgeApp.Server.Services
{
	public interface IUserService
	{
		Task<User> CreateUserAsync(User user);

		Task DeleteUserAsync(int userId);
		
		Task<bool> DoesUserExist(int userId);
		
		Task<User> GetUserAsync(int userId);
		
		Task UpdateUserAsync(int userId, object update);

        Task<bool> AuthorizeUser(int userId, int hash);

        // Task<User> GetProjectUserAsync(int projectId);
    }
}