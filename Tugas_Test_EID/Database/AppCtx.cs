using Microsoft.EntityFrameworkCore;
namespace Tugas_Test_EID
{
    public class AppCtx : DbContext
    {
        public AppCtx(DbContextOptions<AppCtx> opt) : base(opt) { }
        
        // Dibawah ini model dari Database
        public virtual DbSet<DB_Item_Inventory> db_inventory {get; set;}
        public virtual DbSet<DB_Item_Trx> db_transaction {get; set; }
        public virtual DbSet<DB_User_Login> db_user {get; set;}
    }
}