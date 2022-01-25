using ResidualKnowledgeTestApp.Shared;
using ResidualKnowledgeTestApp.Shared.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ResidualKnowledgeTestApp.Server.Services
{
    public interface IUserService
    {
        Task<User> CreateUserAsync(User user);

        Task DeleteUserAsync(int userId);
        
        Task<bool> DoesUserExist(int userId);
        
        Task<User> GetUserAsync(int userId);
        
        Task UpdateUserAsync(int userId, object update);
        
        Task<User> GetProjectUserAsync(int projectId);
    }
}