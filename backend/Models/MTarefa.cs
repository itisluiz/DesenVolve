namespace Desenvolve.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

public class MTarefa
{
	public enum NivelComplexidade
	{
		Simples,
		Medio,
		Complexo
	}

	[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int Codigo {get; set;}

	[MaxLength(64)]
	public string Nome {get; set;}

    [MaxLength(512)]
	public string Descricao {get; set;}

	public NivelComplexidade Complexidade {get; set;}

	public MProjeto Projeto {get; set;}

    public MUsuario Responsavel {get; set;}

	// "A fazer", "fazendo", "feito" inferidos pelas datas nulas ou não
	public DateTime? Inicio {get; set;}

    public DateTime? Finalizado {get; set;}

	public DateTime? Prazo {get; set;}

	public MTarefa()
	{
		this.Nome = "";
        this.Descricao = "";
		this.Projeto = new MProjeto();
		this.Responsavel = new MUsuario();
	}

	public MTarefa(string nome, string descricao, NivelComplexidade complexidade, MProjeto projeto, MUsuario responsavel, DateTime? prazo = null)
	{
		this.Nome = nome;
        this.Descricao = descricao;
		this.Complexidade = complexidade;
		this.Projeto = new MProjeto();
		this.Responsavel = new MUsuario();
		this.Prazo = prazo;
	}
}