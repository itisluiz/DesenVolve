namespace Desenvolve.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

public class MUsuario
{
	[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int Codigo {get; set;}

	[MaxLength(64)]
	public string Nome {get; set;}

	[MaxLength(64)]
	public string Sobrenome {get; set;}

	// Email possuí unique constraint
	[MaxLength(256)]
	public string Email {get; set;}

	[Column]
	[MaxLength(128)]
	public string SenhaHash {get; set;}

	[NotMapped]
	public string Senha { set { SenhaHash = HashSenha(value); } }

	public ISet<MUsuarioEquipe> UsuarioEquipes {get; set;}

	[NotMapped]
	public IEnumerable<MEquipe> Equipes
	{
		get { return UsuarioEquipes.Select(usuarioEquipe => usuarioEquipe.Equipe); }
	}

	public MUsuario()
	{
		this.Nome = "";
		this.Sobrenome = "";
		this.Email = "";
		this.SenhaHash = "";
		this.UsuarioEquipes = new HashSet<MUsuarioEquipe>();
	}

	public MUsuario(string nome, string sobrenome, string email, string senha)
	{
		this.Nome = nome;
		this.Sobrenome = sobrenome;
		this.Email = email;
		this.SenhaHash = HashSenha(senha);
		this.UsuarioEquipes = new HashSet<MUsuarioEquipe>();
	}

	public bool VerificarSenha(string senha)
	{
		PasswordHasher<MUsuario> passwordHasher = new PasswordHasher<MUsuario>();
		return passwordHasher.VerifyHashedPassword(this, SenhaHash, senha) == PasswordVerificationResult.Success;
	}

	private string HashSenha(string senha)
	{
		PasswordHasher<MUsuario> passwordHasher = new PasswordHasher<MUsuario>();
		return passwordHasher.HashPassword(this, senha);
	}
}