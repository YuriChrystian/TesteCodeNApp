using Microsoft.EntityFrameworkCore;
using OficinaAPI.Models;

namespace OficinaAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){
        }
        public DbSet<OrcamentoModel> Orcamentos { get; set; }
        public DbSet<ItemModel> Itens { get; set; }
    }
}
