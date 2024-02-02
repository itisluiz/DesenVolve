namespace Desenvolve.Contexts;

using Desenvolve.Models;
using Desenvolve.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class CTXDesenvolve : DbContext
{
	public DbSet<MUsuario> Usuarios { get; set; } = null!;
	public DbSet<MEquipe> Equipes { get; set; } = null!;
	public DbSet<MUsuarioEquipe> UsuarioEquipes { get; set; } = null!;
	public DbSet<MProjeto> Projetos { get; set; } = null!;
	public DbSet<MTarefa> Tarefas { get; set; } = null!;

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		IConfigurationSection dbConnection = AppSettings.Section("DBConnection");
		string server = dbConnection.GetValue<string>("Server") ?? @".\";
		string database = dbConnection.GetValue<string>("Database") ?? throw new ArgumentNullException("Database name is null");
		
		optionsBuilder.UseSqlServer($"Server={server};Database={database};Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;");
	}

	protected void OnModelCreating(EntityTypeBuilder<MUsuario> usuarioBuilder)
	{
		usuarioBuilder.HasAlternateKey(usuario => usuario.Email);
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		OnModelCreating(modelBuilder.Entity<MUsuario>());	
		base.OnModelCreating(modelBuilder);	
	}

}
