using Microsoft.EntityFrameworkCore;

namespace BookSeparation
{
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options): base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("BookSeparation"); // 指定使用内存数据库
        }

        public DbSet<Todo> Todos { get; set; }
    }


    public class Todo
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsCompleted { get; set; }
    }
}
