namespace Desenvolve.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class MProjeto : IValidatableObject
{
	[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int Codigo {get; set;}
	
	[Required(ErrorMessage = "O nome do projeto é obrigatório")]
	[MaxLength(64)]
	public string Nome {get; set;}

	public MEquipe Equipe {get; set;}

	public ISet<MTarefa> Tarefas {get; set;}

	// O início do projeto é a data de início da tarefa mais antiga
	public DateTime? Inicio
	{
		get
		{
			IEnumerable<MTarefa> tarefasComInicio = Tarefas.Where(tarefa => tarefa.Inicio != null);
			if (!tarefasComInicio.Any())
				return null;

			return tarefasComInicio.Min(tarefa => tarefa.Inicio);
		}
	}

	// O final do projeto é a data de finalização da última tarefa, desde que todas as tarefas estejam finalizadas
	public DateTime? Finalizado
	{
		get
		{
			if (Tarefas.Any(tarefa => tarefa.Finalizado == null))
				return null;

			return Tarefas.Max(tarefa => tarefa.Finalizado);
		}
	}

	// O prazo do projeto é a data de prazo com o maior prazo
	public DateTime? Prazo
	{
		get
		{
			IEnumerable<MTarefa> tarefasComPrazo = Tarefas.Where(tarefa => tarefa.Prazo != null);
			if (!tarefasComPrazo.Any())
				return null;

			return tarefasComPrazo.Max(tarefa => tarefa.Prazo);
		}
	}

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

	public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
	{
		List<ValidationResult> errosValidacao = new List<ValidationResult>();

		Validator.TryValidateProperty(this.Nome, new ValidationContext(this, null, null) { MemberName = "Nome" }, errosValidacao);

		return errosValidacao;
	}

}
