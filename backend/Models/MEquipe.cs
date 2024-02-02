namespace Desenvolve.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

public class MEquipe
{
	[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int Codigo {get; set;}

	[MaxLength(64)]
	public string Nome {get; set;}

	public ISet<MUsuarioEquipe> UsuarioEquipes {get; set;}

	[NotMapped]
	public IEnumerable<MUsuario> Membros
	{
		get { return UsuarioEquipes.Select(usuarioEquipe => usuarioEquipe.Usuario); }
	}

	[NotMapped]
	public IEnumerable<MUsuario> Administradores
	{
		get { return UsuarioEquipes.Where(usuarioEquipe => usuarioEquipe.Admin).Select(usuarioEquipe => usuarioEquipe.Usuario); }
	}

	[NotMapped]
	// Método de validação a partir da linha 57
	[ValidacaoUnicoLiderEquipe(ErrorMessage = "É permito apenas um líder por equipe.")]
	public MUsuario? Lider
	{
		get { return UsuarioEquipes.FirstOrDefault(usuarioEquipe => usuarioEquipe.Lider)?.Usuario; }
	}

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
}

// Verifica a quantidade de líderes em uma equipe
[AttributeUsage(AttributeTargets.Property)]
public class ValidacaoUnicoLiderEquipe : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
		var equipe = (MEquipe)validationContext.ObjectInstance;

		if(equipe != null)
		{
			var ContadorLider = equipe.UsuarioEquipes.Count(usuarioEquipe => usuarioEquipe.Lider);

			if (ContadorLider > 1)
			{
				return new ValidationResult(ErrorMessage);
			}
		}

        return ValidationResult.Success;
    }
}