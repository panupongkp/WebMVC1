using Microsoft.EntityFrameworkCore;
using WebMVC1.Models;

namespace WebMVC1.DBContext
{
    public class FWContext : DbContext
    {
        public FWContext(DbContextOptions options) : base(options) { }
     
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MailMergeEntity>().ToTable("MailMerge","dbo").HasKey(x => x.id);
            modelBuilder.Entity<TextEditorEntity>().ToTable("TextEditor", "dbo").HasKey(x => x.id);
            //modelBuilder.Entity<TestEntity>().ToTable("Test", "dbo");
        }

        public DbSet<MailMergeEntity> mailMergeEntities { get; set; }
        public DbSet<TextEditorEntity> textEditorEntities { get; set; }
        //public DbSet<TestEntity> testEntities { get; set; }
    }
}
