namespace Desenvolve.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

public class MProjeto
{
	[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int Codigo {get; set;}

	[MaxLength(64)]
	public string Nome {get; set;}

	public MEquipe Equipe {get; set;}

	public ISet<MTarefa> Tarefas {get; set;}

	public MProjeto()
	{
		this.Nome = "";
		this.Equipe = new MEquipe();
		this.Tarefas = new HashSet<MTarefa>();
	}

	public MProjeto(string nome, MEquipe equipe)
	{
		this.Nome = nome;
		this.Equipe = equipe;
		this.Tarefas = new HashSet<MTarefa>();
	}

}