namespace Desenvolve.Models;

using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

[PrimaryKey(nameof(CodigoUsuario), nameof(CodigoEquipe))]
public class MUsuarioEquipe
{
	public enum TipoCargo
	{
		Membro,
		Administrador,
		Lider
	}

	[Column]
	private int CodigoUsuario {get; set;}

	[ForeignKey("CodigoUsuario")]
	public MUsuario Usuario {get; set;}

	[Column]
	private int CodigoEquipe {get; set;}

	[ForeignKey("CodigoEquipe")]
	public MEquipe Equipe {get; set;}

	public TipoCargo Cargo {get; set;}

	public MUsuarioEquipe()
	{
		this.Usuario = new MUsuario();
		this.Equipe = new MEquipe();
	}

	public MUsuarioEquipe(MUsuario usuario, MEquipe equipe, TipoCargo cargo)
	{
		this.Usuario = usuario;
		this.Equipe = equipe;
		this.Cargo = cargo;
	}
}
