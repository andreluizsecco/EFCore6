using Microsoft.EntityFrameworkCore;

public class Program
{
    static void Main(string[] args)
    {
        using (var db = new LivrosContext())
        {
            if (!db.Livros.Any())
            {
                db.Livros.Add(new Livro { Titulo = "Domain-Driven Design: Tackling Complexity in the Heart of Software", Autor = "Eric Evans", AnoPublicacao = 2003 });
                db.Livros.Add(new Livro { Titulo = "Agile Principles, Patterns, and Practices in C#", Autor = "Robert C. Martin", AnoPublicacao = 2006 });
                db.Livros.Add(new Livro { Titulo = "Clean Code: A Handbook of Agile Software Craftsmanship", Autor = "Robert C. Martin", AnoPublicacao = 2008 });
                db.Livros.Add(new Livro { Titulo = "Implementing Domain-Driven Design", Autor = "Vaughn Vernon", AnoPublicacao = 2013 });
                db.Livros.Add(new Livro { Titulo = "Patterns, Principles, and Practices of Domain-Driven Design", Autor = "Scott Millet", AnoPublicacao = 2015 });
                db.Livros.Add(new Livro { Titulo = "Refactoring: Improving the Design of Existing Code", Autor = "Martin Fowler", AnoPublicacao = 2012 });

                db.SaveChanges();

                Console.WriteLine("****** ALTERANDO DADOS ******");
                ExecutarAlteracoes(db);
                Console.WriteLine();
            }

            Console.WriteLine("****** CONSULTANDO DADOS DIRETAMENTE NA TABELA ******");
            ConsultaDadosTabela(db);
            Console.WriteLine();

            System.Console.WriteLine("****** CONSULTA DADOS HISTÓRICOS (TemporalAll) DO LIVRO DO VAUGH VERNON ******");
            ConsultaDadosHistoricosTemporalAll(db);
            Console.WriteLine();

            System.Console.WriteLine("****** CONSULTA DADOS HISTÓRICOS (TemporalFromTo) DO LIVRO DO VAUGH VERNON ******");
            ConsultaDadosHistoricosTemporalFromTo(db);
            Console.WriteLine();

            System.Console.WriteLine("****** CONSULTA DADOS HISTÓRICOS (Between) DO LIVRO DO VAUGH VERNON ******");
            ConsultaDadosHistoricosTemporalBetween(db);
            Console.WriteLine();

            System.Console.WriteLine("****** CONSULTA DADOS HISTÓRICOS (ContainedIn) DO LIVRO DO VAUGH VERNON ******");
            ConsultaDadosHistoricosTemporalContainedIn(db);
            Console.WriteLine();

            System.Console.WriteLine("****** EXCLUINDO E RESTAURANDO LIVRO EXCLUÍDO ******");
            ExcluindoERestaurandoLivro(db);
            Console.WriteLine();
        }
    }

    private static void ExcluindoERestaurandoLivro(LivrosContext db)
    {
        var livro = db.Livros.Single(p => p.Autor.Equals("Vaughn Vernon"));
        db.Remove(livro);
        db.SaveChanges();
        var data = DateTime.Now.AddSeconds(-1).ToUniversalTime();
        Console.WriteLine($"Recuperando livro com a data: {data}");

        var livroDoHistorico = db
            .Livros
            .TemporalAsOf(data)
            .Single(e => e.Autor == "Vaughn Vernon");
        livroDoHistorico.LivroId = 0;

        db.Add(livroDoHistorico);
        db.SaveChanges();
        ConsultaDadosHistoricosTemporalAll(db);
    }

    private static void ExibirMensagemSaida(string titulo, int ano, DateTime inicioValidade, DateTime fimValidade) =>
        Console.WriteLine($"Livro '{titulo}' com o ano de publicação '{ano}' de {inicioValidade} até {fimValidade}");

    private static void ConsultaDadosHistoricosTemporalAll(LivrosContext db)
    {
        var historico = db
            .Livros
            .TemporalAll()
            .Where(p => p.Autor == "Vaughn Vernon")
            .OrderBy(e => EF.Property<DateTime>(e, "InicioValidade"))
            .Select(
                p => new
                {
                    Livro = p,
                    InicioValidade = EF.Property<DateTime>(p, "InicioValidade"),
                    FimValidade = EF.Property<DateTime>(p, "FimValidade")
                })
            .ToList();

        foreach (var registro in historico)
            ExibirMensagemSaida(registro.Livro.Titulo, registro.Livro.AnoPublicacao, registro.InicioValidade, registro.FimValidade);
    }

    private static void ConsultaDadosHistoricosTemporalFromTo(LivrosContext db)
    {
        var from = DateTime.Now.AddSeconds(-5).ToUniversalTime();
        var to = DateTime.Now.AddSeconds(3).ToUniversalTime();
        Console.WriteLine($"Consultando de {from} até {to}");

        var historico = db
            .Livros
            .TemporalFromTo(from, to)
            .Where(p => p.Autor == "Vaughn Vernon")
            .OrderBy(e => EF.Property<DateTime>(e, "InicioValidade"))
            .Select(
                p => new
                {
                    Livro = p,
                    InicioValidade = EF.Property<DateTime>(p, "InicioValidade"),
                    FimValidade = EF.Property<DateTime>(p, "FimValidade")
                })
            .ToList();

        foreach (var registro in historico)
            ExibirMensagemSaida(registro.Livro.Titulo, registro.Livro.AnoPublicacao, registro.InicioValidade, registro.FimValidade);
    }

    private static void ConsultaDadosHistoricosTemporalBetween(LivrosContext db)
    {
        var from = DateTime.Now.AddSeconds(-5).ToUniversalTime();
        var to = DateTime.Now.AddSeconds(3).ToUniversalTime();
        Console.WriteLine($"Consultando entre {from} e {to}");

        var historico = db
            .Livros
            .TemporalBetween(from, to)
            .Where(p => p.Autor == "Vaughn Vernon")
            .OrderBy(e => EF.Property<DateTime>(e, "InicioValidade"))
            .Select(
                p => new
                {
                    Livro = p,
                    InicioValidade = EF.Property<DateTime>(p, "InicioValidade"),
                    FimValidade = EF.Property<DateTime>(p, "FimValidade")
                })
            .ToList();

        foreach (var registro in historico)
            ExibirMensagemSaida(registro.Livro.Titulo, registro.Livro.AnoPublicacao, registro.InicioValidade, registro.FimValidade);
    }

    private static void ConsultaDadosHistoricosTemporalContainedIn(LivrosContext db)
    {
        var from = DateTime.Now.AddSeconds(-10).ToUniversalTime();
        var to = DateTime.Now.ToUniversalTime();
        Console.WriteLine($"Consultando dados contidos entre {from} até {to}");

        var historico = db
            .Livros
            .TemporalContainedIn(from, to)
            .Where(p => p.Autor == "Vaughn Vernon")
            .OrderBy(e => EF.Property<DateTime>(e, "InicioValidade"))
            .Select(
                p => new
                {
                    Livro = p,
                    InicioValidade = EF.Property<DateTime>(p, "InicioValidade"),
                    FimValidade = EF.Property<DateTime>(p, "FimValidade")
                })
            .ToList();

        foreach (var registro in historico)
            ExibirMensagemSaida(registro.Livro.Titulo, registro.Livro.AnoPublicacao, registro.InicioValidade, registro.FimValidade);
    }

    private static void ConsultaDadosTabela(LivrosContext db)
    {
        var livros = db.Livros.ToList();
        foreach (var livro in livros)
        {
            var livroEntry = db.Entry(livro);
            var inicioValidade = livroEntry.Property<DateTime>("InicioValidade").CurrentValue;
            var fimValidade = livroEntry.Property<DateTime>("FimValidade").CurrentValue;

            Console.WriteLine($"Livro '{livro.Titulo}' válido desde {inicioValidade} to {fimValidade}");
        }
    }

    private static void ExecutarAlteracoes(LivrosContext db)
    {
        var livro = db.Livros.Single(p => p.Autor.Equals("Eric Evans"));
        livro.AnoPublicacao = 2004;
        db.SaveChanges();
        Thread.Sleep(2000);
        livro.AnoPublicacao = 2003;
        db.SaveChanges();

        var livro2 = db.Livros.Single(p => p.Autor.Equals("Vaughn Vernon"));
        livro2.AnoPublicacao = 2014;
        db.SaveChanges();
        Thread.Sleep(5000);
        livro2.AnoPublicacao = 2012;
        db.SaveChanges();
        Thread.Sleep(7000);
        livro2.AnoPublicacao = 2013;
        db.SaveChanges();
        Thread.Sleep(2000);
    }
}

public class LivrosContext : DbContext
{
    public DbSet<Livro> Livros { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            @"Server=(localdb)\mssqllocaldb;Database=EFCore6;Trusted_Connection=True;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Livro>()
            .ToTable("Livro", t => t.IsTemporal(t =>
            {
                t.HasPeriodStart("InicioValidade");
                t.HasPeriodEnd("FimValidade");
                t.UseHistoryTable("LivroHistorico");
            }));

        modelBuilder
            .Entity<Livro>()
            .Property(p => p.Titulo)
            .HasColumnType("varchar(200)");

        modelBuilder
            .Entity<Livro>()
            .Property(p => p.Autor)
            .HasColumnType("varchar(100)");
    }
}

public class Livro
{
    public int LivroId { get; set; }
    public string Titulo { get; set; }
    public string Autor { get; set; }
    public int AnoPublicacao { get; set; }
}
