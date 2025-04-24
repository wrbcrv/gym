using Api.Data;
using Api.Models;
using Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories
{
    public class ActivityRepository : IActivityRepository
    {
        private readonly AppDbContext _context;

        public ActivityRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Activity?> GetByIdAsync(int id)
        {
            return await _context.Activities
                .Include(a => a.User)
                .Include(a => a.Group)
                .Include(a => a.Comments)
                    .ThenInclude(c => c.User)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Activity>> GetAllByGroupIdAsync(int groupId, int pageNumber, int pageSize)
        {
            return await _context.Activities
                .Include(a => a.User)
                .Include(a => a.Comments)
                    .ThenInclude(c => c.User)
                .Where(a => a.GroupId == groupId)
                .OrderByDescending(a => a.Date)
                .ThenByDescending(a => a.StartTime)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task AddAsync(Activity activity)
        {
            _context.Activities.Add(activity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(int groupId, Activity activity)
        {
            var existing = await _context.Activities
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == activity.Id && a.GroupId == groupId);

            if (existing == null) return false;

            _context.Entry(activity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int activityId, int groupId)
        {
            var activity = await _context.Activities
                .FirstOrDefaultAsync(a => a.Id == activityId && a.GroupId == groupId);

            if (activity == null) return false;

            _context.Activities.Remove(activity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task AddCommentAsync(Comment comment)
        {
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<(int UserId, string? FullName, int Count)>> GetUserActivityCountsByGroupAsync(int groupId)
        {
            return await _context.Activities
                .Where(a => a.GroupId == groupId)
                .GroupBy(a => new { a.UserId, a.User.FullName })
                .Select(g => new ValueTuple<int, string?, int>(
                    g.Key.UserId,
                    g.Key.FullName,
                    g.Count()
                ))
                .ToListAsync();
        }
    }
}
