using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TodoTask.Models;

namespace TodoTask.Data
{
    public class TodoTaskContext : DbContext
    {
        public TodoTaskContext (DbContextOptions<TodoTaskContext> options)
            : base(options)
        {
        }

        public DbSet<TodoTask.Models.Todo> Todo { get; set; }
    }
}
