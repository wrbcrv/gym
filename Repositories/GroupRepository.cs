using Api.Data;
using Api.Models;
using Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories
{
    public class GroupRepository : IGroupRepository
    {
        private readonly AppDbContext _context;

        public GroupRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Group>> GetAllAsync(int userId)
        {
            return await _context.Groups
                .Include(g => g.Members)
                .Where(g => g.Members.Any(m => m.Id == userId))
                .ToListAsync();
        }

        public async Task<Group?> GetByIdAsync(int id)
        {
            var group = await _context.Groups
                .Include(g => g.Members)
                .Include(g => g.Activities)
                    .ThenInclude(a => a.User)
                .FirstOrDefaultAsync(g => g.Id == id);

            return group;
        }

        public async Task AddAsync(Group group)
        {
            _context.Groups.Add(group);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Group group)
        {
            _context.Entry(group).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var group = await _context.Groups.FindAsync(id);
            if (group != null)
            {
                _context.Groups.Remove(group);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Group?> GetByIdWithMembersAsync(int id)
        {
            return await _context.Groups
                .Include(g => g.Members)
                .FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<bool> IsUserInGroupAsync(int groupId, int userId)
        {
            return await _context.Groups
                .AnyAsync(g => g.Id == groupId && g.Members.Any(m => m.Id == userId));
        }
    }
}
