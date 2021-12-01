using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

public class Program
{
    static void Main(string[] args) { }
}

public class EmpresaContext : DbContext
{
    public DbSet<Empresa> Empresas { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            @"Server=(localdb)\mssqllocaldb;Database=EFCore6;Trusted_Connection=True;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Empresa>()
            .ToTable("Empresa");

        modelBuilder
            .Entity<Empresa>()
            .Property(p => p.NomeEmpresarial)
            .IsUnicode(false)
            .HasMaxLength(150);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        //Definir por padrão que todas as strings serão mapeadas para varchar(500), se no houverem mapeamentos específicos
        configurationBuilder
            .Properties<string>()
            .AreUnicode(false)
            .HaveMaxLength(500);

        //DateTime pode ser convertido para int64 no banco de dados
        configurationBuilder
            .Properties<DateTime>()
            .HaveConversion<long>();

        //Mapeamento padrão para bool utilizando conversores pré-construidos
        configurationBuilder
            .Properties<bool>()
            .HaveConversion<BoolToZeroOneConverter<int>>();

        //Ignorar propriedades que sejam de determinado tipo
        configurationBuilder
            .IgnoreAny<DomainEvent>();

        //Mapeando todos os enums de determinado tipo para string
        configurationBuilder
            .Properties<EmpresaTipo>()
            .HaveConversion<string>()
            .AreUnicode(false)
            .HaveMaxLength(10);
    }

    public class Empresa
    {
        public int ID { get; set; }
        public string NomeEmpresarial { get; set; }
        public string NomeFantasia { get; set; }
        public DateTime DataAbertura { get; set; }
        public EmpresaTipo Tipo { get; set; }
        public bool Ativo { get; set; }

        public IEnumerable<DomainEvent> Eventos { get; set; }
    }

    public class DomainEvent
    {
        public DateTime Data { get; set; }
    }

    public enum EmpresaTipo
    {
        MEI = 1,
        ME = 2,
        Ltda = 3,
        SA = 4
    }
}