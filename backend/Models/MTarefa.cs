namespace Desenvolve.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

public class MTarefa : IValidatableObject
{
	public enum NivelComplexidade
	{
		NaoEstabelecido,
		Simples,
		Medio,
		Complexo
	}

	[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int Codigo {get; set;}

	[Required(ErrorMessage = "O nome da tarefa é obrigatório")]
	[MaxLength(64)]
	public string Nome {get; set;}

	[MaxLength(512)]
	public string? Descricao {get; set;}

	public NivelComplexidade Complexidade {get; set;}

	public MProjeto Projeto {get; set;}

	public MUsuario Responsavel {get; set;}

	public DateTime? Inicio {get; set;}

	public DateTime? Finalizado {get; set;}

	public DateTime? Prazo {get; set;}

	public MTarefa()
	{
		this.Nome = "";
		this.Projeto = new MProjeto();
		this.Responsavel = new MUsuario();
	}

	public MTarefa(string nome, string? descricao, NivelComplexidade complexidade, MProjeto projeto, MUsuario responsavel, DateTime? prazo = null)
	{
		this.Nome = nome;
		this.Descricao = descricao;
		this.Complexidade = complexidade;
		this.Projeto = projeto;
		this.Responsavel = responsavel;
		this.Prazo = prazo;
	}

	public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
	{
		List<ValidationResult> errosValidacao = new List<ValidationResult>();

		Validator.TryValidateProperty(this.Nome, new ValidationContext(this, null, null) { MemberName = "Nome" }, errosValidacao);

		if (Inicio != null && Finalizado != null && Inicio > Finalizado)
			errosValidacao.Add(new ValidationResult("A data de início não pode ser posterior à data de finalização", ["Inicio", "Finalizado"]));

		if (Projeto.Equipe.Membros.All(usuario => usuario != Responsavel))
			errosValidacao.Add(new ValidationResult("O responsável pela tarefa deve ser membro da equipe do projeto", ["Responsavel"]));

		return errosValidacao;
	}
}
