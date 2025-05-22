using Fuxikarte.Backend.Models;
using Fuxikarte.Backend.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Fuxikarte.Backend.DTOs;
using AutoMapper;

namespace Fuxikarte.Backend.Services
{
    public class UserService
    {
        private readonly AppDbContext _context;
        private readonly PasswordHasher<User> _passwordHasher;
        private readonly IMapper _mapper;

        public UserService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<User>();
            _mapper = mapper;
        }

        public async Task CreateUser(User user, string password)
        {
            user.Password = _passwordHasher.HashPassword(user, password);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<User>> GetAllUsers() => await _context.Users.ToListAsync();

        public async Task<User?> GetUserById(int id) => await _context.Users.FindAsync(id);

        public async Task<bool> UpdateUser(int id, UserUpdateDTO model)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            user.Username = model.Username ?? user.Username;
            if (!string.IsNullOrWhiteSpace(model.Password))
            {
                user.Password = _passwordHasher.HashPassword(user, model.Password);
            }
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}