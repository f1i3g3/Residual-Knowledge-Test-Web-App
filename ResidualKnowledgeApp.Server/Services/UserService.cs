using ResidualKnowledgeApp.Server.Repositories;

namespace ResidualKnowledgeApp.Server.Services
{
	public class UserService : IUserService
	{
		private readonly IUserRepository _usersRepository;
		private readonly ILogger<UserService> _logger;

		public UserService(IUserRepository usersRepository, ILogger<UserService> logger)
		{
			_usersRepository = usersRepository;
			_logger = logger;
		}

		public async Task<User> CreateUserAsync(User user)
		{
			_logger.LogInformation("Creating user");
			var userId = await _usersRepository.AddAsync(user);

			_logger.LogInformation("Done!");
			return user;
		}

		public async Task DeleteUserAsync(int userId)
		{
			_logger.LogInformation("Deleting user...");
			await _usersRepository.DeleteAsync(userId);
			_logger.LogInformation("Done!");
		}

		public async Task<bool> DoesUserExist(int userId)
		{
			var user = await _usersRepository.FindAsync(u => u.Id == userId);
			return user != null;
		}

		public async Task<User> GetUserAsync(int userId)
		{
			_logger.LogInformation("Getting user...");
			var user = await _usersRepository.GetAsync(userId);

			_logger.LogInformation("Returning...");
			return user;
		}

		public async Task UpdateUserAsync(int userId, object update)
		{
			_logger.LogInformation("Updating user...");
			await _usersRepository.UpdateAsync(userId, update);
			_logger.LogInformation("Done!");
		}
		
		/*
		public async Task<User> GetProjectUserAsync(int projectId)
		{
			_logger.LogInformation("Getting user project...");
			var user = await _usersRepository.GetProjectUserAsync(projectId);

			_logger.LogInformation("Returning...");
			return user;
		}
		*/
	}
}