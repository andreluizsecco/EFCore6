using Microsoft.EntityFrameworkCore;

public class Program
{
    static void Main(string[] args) { }
}

public class PessoaContext : DbContext
{
    public DbSet<Pessoa> Pessoas { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            @"Server=(localdb)\mssqllocaldb;Database=EFCore6;Trusted_Connection=True;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Pessoa>()
            .ToTable("Pessoa");

        modelBuilder
            .Entity<Pessoa>()
            .Property(p => p.Nome)
            .HasColumnType("varchar(100)");

        modelBuilder
            .Entity<Pessoa>()
            .Property(p => p.Sobrenome)
            .HasColumnType("varchar(100)");

        modelBuilder
            .Entity<Pessoa>()
            .Property(p => p.DataNascimento)
            .HasColumnType("date");
    }
}

public class Pessoa
{
    public int ID { get; set; }
    public string Nome { get; set; }
    public string Sobrenome { get; set; }
    public DateTime DataNascimento { get; set; }
}