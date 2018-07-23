using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chat.API.Data.DAL;
using Chat.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Chat.API.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(User user)
        {
            if (user == null) throw new ArgumentNullException();

            await _context.users.AddAsync(user);
            await _context.SaveChangesAsync();

            await Task.CompletedTask;

        }

        public User GetUserById(int id)
        {
            return _context.users.FirstOrDefault(x => x.Id == id); 
        }

        public User GetUserByUsername(string username)
        {
            return _context.users.FirstOrDefault(x => x.Username == username);
        }
    }
}
