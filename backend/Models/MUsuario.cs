namespace Desenvolve.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

public class MUsuario : IValidatableObject
{
	[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int Codigo {get; set;}

	[MinLength(2, ErrorMessage = "O nome deve possuir no mínimo 2 caracteres")]
	[MaxLength(64)]
	public string Nome {get; set;}

	[MinLength(2, ErrorMessage = "O sobrenome deve possuir no mínimo 2 caracteres")]
	[MaxLength(64)]
	public string Sobrenome {get; set;}

	// Email possuí unique constraint
	[EmailAddress(ErrorMessage = "O email informado não é válido")]
	[MaxLength(256)]
	public string Email {get; set;}

	[Column]
	[StringLength(84)]
	private string SenhaHash {get; set;}

	// TamanhoSenha só existe para validação, só é não-nulo quando a senha é atribuida e seu valor
	// não é persistido no banco de dados
	[NotMapped]
	private int? TamanhoSenha {get; set;}

	[NotMapped]
	public string Senha { set { SenhaHash = HashSenha(value); } }

	[JsonIgnore]
	public ISet<MUsuarioEquipe> UsuarioEquipes {get; set;}

	[NotMapped]
	[JsonIgnore]
	public IEnumerable<MEquipe> Equipes { get { return UsuarioEquipes.Select(usuarioEquipe => usuarioEquipe.Equipe); } }

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

	public bool TestarSenha(string senha)
	{
		PasswordHasher<MUsuario> passwordHasher = new PasswordHasher<MUsuario>();
		return passwordHasher.VerifyHashedPassword(this, SenhaHash, senha) != PasswordVerificationResult.Failed;
	}

	private string HashSenha(string senha)
	{
		this.TamanhoSenha = senha.Length;
		PasswordHasher<MUsuario> passwordHasher = new PasswordHasher<MUsuario>();
		return passwordHasher.HashPassword(this, senha);
	}

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
		List<ValidationResult> errosValidacao = new List<ValidationResult>();

		Validator.TryValidateProperty(this.Nome, new ValidationContext(this, null, null) { MemberName = "Nome" }, errosValidacao);
		Validator.TryValidateProperty(this.Sobrenome, new ValidationContext(this, null, null) { MemberName = "Sobrenome" }, errosValidacao);
		Validator.TryValidateProperty(this.Email, new ValidationContext(this, null, null) { MemberName = "Email" }, errosValidacao);

		if (this.TamanhoSenha != null && this.TamanhoSenha < 6)
			errosValidacao.Add(new ValidationResult("A senha deve possuir no mínimo 6 caracteres", ["Senha"]));

		return errosValidacao;
    }
}
