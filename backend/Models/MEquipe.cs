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