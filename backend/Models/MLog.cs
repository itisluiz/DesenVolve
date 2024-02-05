namespace Desenvolve.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

public class MLog
{
	public enum TipoLog
	{
		Informacao,
		Excecao
	}
	
	[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int Codigo {get; set;}
	
	public TipoLog Tipo {get; set;}

	[MaxLength(3072)]
	public string Mensagem {get; set;}

	public DateTime Data {get; set;}

	private MLog()
	{
		this.Mensagem = "";
		this.Data = DateTime.Now;
	}

	public MLog(Exception excecao) : this()
	{
		StringBuilder mensagem = new StringBuilder();

		// Obtém a mensagem e tipo de todas as exceções internas
		Exception? excecaoInterna = excecao;
		for (int i = 0; i < 4 && excecaoInterna != null; ++i, excecaoInterna = excecaoInterna.InnerException)
			mensagem.AppendLine($"[Excecão {i}] {excecaoInterna.GetType().Name}: {excecaoInterna.Message}");

		string? namespaceProjeto = typeof(MLog).Namespace?.Split('.', 2)[0];

		// Filtra a call stack para mostrar apenas as chamadas do projeto
		if (excecao.StackTrace != null && namespaceProjeto != null)
		{
			foreach (string linha in excecao.StackTrace.Split('\n'))
			{
				string linhaLimpa = linha.Trim();
				if (linhaLimpa.StartsWith("at "))
					linhaLimpa = linhaLimpa[3..];
				else
					continue;

				if (linhaLimpa.StartsWith(namespaceProjeto))
					mensagem.AppendLine($"↑ {linhaLimpa}");
			}
		}
		
		this.Tipo = TipoLog.Excecao;
		this.Mensagem = mensagem.ToString()[..Math.Min(mensagem.Length, 2048)];
	}

	public MLog(string informacao) : this()
	{
		this.Tipo = TipoLog.Informacao;
		this.Mensagem = informacao[..Math.Min(informacao.Length, 2048)];
	}

	public override string ToString()
	{
		return $"[{this.Data:HH:mm:ss}] [LOG {(this.Codigo != 0 ? $"#{this.Codigo}" : "LOCAL")}] [{this.Tipo.ToString().ToUpper()}]\n{this.Mensagem}";
	}
}
