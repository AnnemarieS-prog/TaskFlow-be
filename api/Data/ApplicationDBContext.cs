using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Data;

public class ApplicationDBContext : DbContext
{
    public DbSet<MyTask> Tasks { get; set; }
    public DbSet<TaskList> TaskLists { get; set; }

    public ApplicationDBContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
    {
        
    }
}
