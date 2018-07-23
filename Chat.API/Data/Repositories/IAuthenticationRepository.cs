using System.Threading.Tasks;
using Chat.API.Models;


namespace Chat.API.Data.Repositories
{
  
    public interface IUserRepository
    {
        User GetUserById(int id);
        Task AddAsync(User user);
    }
}