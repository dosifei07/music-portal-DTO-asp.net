using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MusicPortal.DataAccess.Data;
using MusicPortal.DataAccess.Models;
using MusicPortal.DataAccess.Repositories.Interfaces;

namespace MusicPortal.DataAccess.Repositories.Implementations
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context)
        {
        }

        public override async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users
                .Include(u => u.Roles)
                .Include(u => u.ArtistProfile)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public override async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.Include(u => u.Roles).ToListAsync();
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<IEnumerable<User>> GetPendingUsersAsync()
        {
            return await _context.Users
                .Include(u => u.Roles)
                .Where(u => u.IsApproved == false)
                .ToListAsync();
        }

        public async Task<bool> AnyUsersExistAsync()
        {
            return await _context.Users.AnyAsync();
        }

        public async Task<PagedResult<User>> GetAllUsersAsync(int page = 1, int pageSize = 20)
        {
            page = page < 1 ? 1 : page;
            pageSize = pageSize < 1 ? 20 : pageSize;

            var query = _context.Users.OrderBy(u => u.Username);
            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Include(u => u.Roles)
                .ToListAsync();

            return new PagedResult<User> { Items = items, TotalCount = totalCount, Page = page, PageSize = pageSize };
        }
        public async Task<bool> ApproveUserAsync(User user, IEnumerable<Role> roles)
        {
            try
            {
                user.Roles.Clear();
                foreach (var role in roles) user.Roles.Add(role);
                user.IsApproved = true;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public async Task<bool> SetRolesAsync(User user, IEnumerable<Role> roles)
        {
            try
            {
                user.Roles.Clear();
                foreach (var role in roles) user.Roles.Add(role);

                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }
    }
}