namespace Desenvolve.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

[PrimaryKey(nameof(CodigoUsuario), nameof(CodigoEquipe))]
public class MUsuarioEquipe
{
	[Column]
	private int CodigoUsuario {get; set;}

	[ForeignKey("CodigoUsuario")]
	public MUsuario Usuario {get; set;}

	[Column]
	private int CodigoEquipe {get; set;}

	[ForeignKey("CodigoEquipe")]
	public MEquipe Equipe {get; set;}

	public bool Lider {get; set;}

	public bool Admin {get; set;}
	
	public MUsuarioEquipe()
	{
		this.Usuario = new MUsuario();
		this.Equipe = new MEquipe();
	}

	public MUsuarioEquipe(MUsuario usuario, MEquipe equipe, bool lider = false, bool admin = false)
	{
		this.Usuario = usuario;
		this.Equipe = equipe;
		this.Lider = lider;
		this.Admin = admin;
	}
}
