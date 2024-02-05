namespace Desenvolve.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Microsoft.AspNetCore.Identity;

public class MLogExcecao
{
	[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int Codigo {get; set;}

	[MaxLength(64)]
	public string Excecao {get; set;}

	[MaxLength(128)]
	public string Mensagem {get; set;}
	
	[MaxLength(1024)]
	public string? StackTrace {get; set;}

	public DateTime Data {get; set;}

	private MLogExcecao()
	{
		Excecao = "";
		Mensagem = "";
	}

	public MLogExcecao(Exception excecao)
	{
		this.Excecao = excecao.GetType().Name;
		this.Mensagem = excecao.Message;

		if (excecao.StackTrace != null)
		{
			string stackTraceTruncado = excecao.StackTrace[..1024];
			this.StackTrace = stackTraceTruncado[..stackTraceTruncado.LastIndexOf('\n')].Trim();
		}

		this.Data = DateTime.Now;
	}

}
