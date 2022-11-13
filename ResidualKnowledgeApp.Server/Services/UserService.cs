using ResidualKnowledgeApp.Server.Repositories;

namespace ResidualKnowledgeApp.Server.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _usersRepository;

        public UserService(IUserRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        public async Task<User> CreateUserAsync(User user)
        {
            var userId = await _usersRepository.AddAsync(user);
            return user;
        }

        public async Task DeleteUserAsync(int userId)
        {
            await _usersRepository.DeleteAsync(userId);
        }

        public async Task<bool> DoesUserExist(int userId)
        {
            var user = await _usersRepository.FindAsync(u => u.Id == userId);
            return user != null;
        }

        public async Task<User> GetUserAsync(int userId)
        {
            var user = await _usersRepository.GetAsync(userId);
            return user;
        }

        public async Task UpdateUserAsync(int userId, object update)
        {
            await _usersRepository.UpdateAsync(userId, update);
        }
        
        public async Task<User> GetProjectUserAsync(int projectId)
        {
            var user = await _usersRepository.GetProjectUserAsync(projectId);
            return user;
        }
    }
}