namespace Desenvolve.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

public class MEquipe : IValidatableObject
{
	[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int Codigo {get; set;}

	[MinLength(2, ErrorMessage = "O nome deve possuir no mínimo 2 caracteres")]
	[MaxLength(64)]
	public string Nome {get; set;}

	[JsonIgnore]
	public ISet<MUsuarioEquipe> UsuarioEquipes {get; set;}
	
	[NotMapped]
	[JsonIgnore]
	public IEnumerable<MUsuario> Membros
	{
		get { return UsuarioEquipes.Select(usuarioEquipe => usuarioEquipe.Usuario); }
	}

	[NotMapped]
	[JsonIgnore]
	public IEnumerable<MUsuario> Administradores
	{
		get
		{
			return UsuarioEquipes.Where(usuarioEquipe => 
				usuarioEquipe.Cargo >= MUsuarioEquipe.TipoCargo.Administrador).Select(usuarioEquipe => usuarioEquipe.Usuario);
		}
	}

	[NotMapped]
	[JsonIgnore]
	public MUsuario Lider
	{
		get { return UsuarioEquipes.First(usuarioEquipe => usuarioEquipe.Cargo == MUsuarioEquipe.TipoCargo.Lider).Usuario; }
	}

	// Para retorno na API
	[NotMapped]
	public IEnumerable<object> MembrosEquipe
	{
		get
		{
			return UsuarioEquipes.Select(usuarioEquipe => new
			{
				usuarioEquipe.Usuario,
				Cargo = new
				{
					Codigo = usuarioEquipe.Cargo,
					Nome = usuarioEquipe.Cargo.ToString()
				}
			});
		}
	}

	[JsonIgnore]
	public ISet<MProjeto> Projetos {get; set;}

	public MEquipe()
	{
		this.Nome = "";
		this.UsuarioEquipes = new HashSet<MUsuarioEquipe>();
		this.Projetos = new HashSet<MProjeto>();
	}

	public MEquipe(string nome)
	{
		this.Nome = nome;
		this.UsuarioEquipes = new HashSet<MUsuarioEquipe>();
		this.Projetos = new HashSet<MProjeto>();
	}

	public void AdicionarMembro(MUsuario usuario, MUsuarioEquipe.TipoCargo cargo)
	{
		if (cargo == MUsuarioEquipe.TipoCargo.Lider && UsuarioEquipes.Any(usuarioEquipe => usuarioEquipe.Cargo == MUsuarioEquipe.TipoCargo.Lider))
			throw new ArgumentException("A equipe já possui um líder");

		if (Membros.Contains(usuario))
			throw new ArgumentException("O usuário já é membro da equipe");

		UsuarioEquipes.Add(new MUsuarioEquipe(usuario, this, cargo));
	}

	public void RemoverMembro(MUsuario usuario)
	{
		MUsuarioEquipe? usuarioEquipe = UsuarioEquipes.FirstOrDefault(usuarioEquipe => usuarioEquipe.Usuario == usuario);

		if (usuarioEquipe == null)
			throw new ArgumentException("O usuário não é membro da equipe");

		if (usuarioEquipe.Cargo == MUsuarioEquipe.TipoCargo.Lider)
			throw new ArgumentException("Não é possível remover o líder da equipe");

		UsuarioEquipes.Remove(usuarioEquipe);
	}

	public void AlterarCargo(MUsuario usuario, MUsuarioEquipe.TipoCargo cargo)
	{
		MUsuarioEquipe? usuarioEquipe = UsuarioEquipes.FirstOrDefault(usuarioEquipe => usuarioEquipe.Usuario == usuario);

		if (usuarioEquipe == null || usuarioEquipe.Cargo == MUsuarioEquipe.TipoCargo.Lider)
			throw new ArgumentException("Não é possível alterar o cargo do líder da equipe");

		// Para tornar alguém líder, é necessário remover o cargo de líder do líder atual
		if (cargo == MUsuarioEquipe.TipoCargo.Lider)
		{
			MUsuarioEquipe liderAtual = UsuarioEquipes.First(usuarioEquipe => usuarioEquipe.Cargo == MUsuarioEquipe.TipoCargo.Lider);
			liderAtual.Cargo = MUsuarioEquipe.TipoCargo.Administrador;
		}

		usuarioEquipe.Cargo = cargo;
	}

	public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
	{
		List<ValidationResult> errosValidacao = new List<ValidationResult>();

		Validator.TryValidateProperty(this.Nome, new ValidationContext(this, null, null) { MemberName = "Nome" }, errosValidacao);

		int quantidadeLideres = UsuarioEquipes.Where(usuarioEquipe => usuarioEquipe.Cargo == MUsuarioEquipe.TipoCargo.Lider).Count();
	
		if (quantidadeLideres != 1)
			errosValidacao.Add(new ValidationResult("A equipe deve possuir exatamente um líder", ["UsuarioEquipes"]));

		return errosValidacao;
	}
}
