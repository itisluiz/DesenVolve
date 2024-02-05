namespace Desenvolve.Contexts;

using Desenvolve.Models;
using Desenvolve.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;

public class CTXDesenvolve : DbContext
{
	public DbSet<MLogExcecao> LogsExcecao { get; set; } = null!;
	public DbSet<MUsuario> Usuarios { get; set; } = null!;
	public DbSet<MEquipe> Equipes { get; set; } = null!;
	public DbSet<MUsuarioEquipe> UsuarioEquipes { get; set; } = null!;
	public DbSet<MProjeto> Projetos { get; set; } = null!;
	public DbSet<MTarefa> Tarefas { get; set; } = null!;

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		string server = AppSettings.Valor<string>("DBConnection:Server");
		string database = AppSettings.Valor<string>("DBConnection:Database");
		
		optionsBuilder.UseSqlServer($"Server={server};Database={database};Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;");

		base.OnConfiguring(optionsBuilder);
	}

	protected void OnModelCreating(EntityTypeBuilder<MUsuario> usuarioBuilder)
	{
		usuarioBuilder.HasAlternateKey(usuario => usuario.Email);
		usuarioBuilder.Property(typeof(string), "SenhaHash").HasColumnName("SenhaHash");
	}

	// A ideia é colocar o autoinclude somente para entidades de "relacionamento" ou majoritariamente "fracas"
	protected void OnModelCreating(EntityTypeBuilder<MUsuarioEquipe> usuarioEquipeBuilder)
	{
		usuarioEquipeBuilder.Navigation(usuarioEquipe => usuarioEquipe.Equipe).AutoInclude();
		usuarioEquipeBuilder.Navigation(usuarioEquipe => usuarioEquipe.Usuario).AutoInclude();
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		OnModelCreating(modelBuilder.Entity<MUsuario>());
		OnModelCreating(modelBuilder.Entity<MUsuarioEquipe>());

		base.OnModelCreating(modelBuilder);	
	}

	public override int SaveChanges()
	{
		foreach (EntityEntry entidade in ChangeTracker.Entries())
		{
			// Se implementa a interface IValidatableObject e está sendo adicionado ou modificado, valida (Comportamento EF6?)
			if (entidade.Entity is IValidatableObject objetoValidavel && (entidade.State == EntityState.Added || entidade.State == EntityState.Modified))
			{
				IEnumerable<ValidationResult> errosValidacao = objetoValidavel.Validate(new ValidationContext(entidade.Entity));

				if (errosValidacao.Any())
					throw new ValidationException(errosValidacao.First(), null, null);
			}
		}

		return base.SaveChanges();
	}

}
