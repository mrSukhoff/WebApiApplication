using System.Data.Entity;

namespace WebApiApplication.Models
{
    public class RecordContext : DbContext
    {
        public DbSet<Record> Records { get; set; }

        public RecordContext()
            : base("RecordsConnection")
        {
        }


    }
}