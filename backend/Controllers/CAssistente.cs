namespace Desenvolve.Controllers;

using System.Text.Json;
using Desenvolve.Contexts;
using Desenvolve.Models;
using Desenvolve.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenAI.ObjectModels.ResponseModels;

[Route("api/assistente")]
public class CAssistente : Controller
{
	private static readonly Dictionary<string, AssistenteIA> assistentes = new Dictionary<string, AssistenteIA>();

	[Authorize]
	[HttpPost]
	public async Task<IActionResult> Mensagem([FromForm] string idAssistente, [FromForm] string? mensagem)
	{
		AssistenteIA? assistente = assistentes.GetValueOrDefault(idAssistente);

		if (assistente == null)
			throw new ArgumentException("ID de assistente inválido");

		string? resposta = await assistente.NovaMensagem(mensagem);

		if (resposta == null)
			throw new ArgumentException("Houve um erro na chamada de API do assistente");

		return Ok(new {Resultado = resposta});
	}

	[Authorize]
	[HttpPost("nova")]
	public IActionResult NovaConversa([FromForm] int codigoProjeto)
	{
		FormHelper.Requeridos(codigoProjeto);
		using CTXDesenvolve ctx = new CTXDesenvolve();

		MProjeto? projeto = ctx.Projetos
			.Include(projeto => projeto.Tarefas)
			.Include(projeto => projeto.Equipe)
			.ThenInclude(equipe => equipe.UsuarioEquipes)
			.FirstOrDefault(projeto => projeto.Codigo == codigoProjeto);

		if (projeto == null)
			throw new ArgumentException("Código de projeto não encontrado");
	
		Login login = new Login(User);
		MUsuario usuario = login.ObterUsuario(ctx);

		if (!projeto.Equipe.Membros.Any(login.RepresentaUsuario))
			throw new UnauthorizedAccessException("Usuário não é membro da equipe e não pode iniciar o assistente neste projeto");

		AssistenteIA assistente = new AssistenteIA();
		assistente.AdicionarInstrucao($"Você vai conversar com {usuario.Nome} sobre o projeto {projeto.Nome}.");
		assistente.AdicionarInstrucao($"Considere que a data e hora atuais são {DateTime.Now}.");
		assistente.AdicionarInstrucao($"O projeto {projeto.Nome} consiste das seguintas tarefas, que aqui estão listadas em formato JSON: {JsonSerializer.Serialize(projeto.Tarefas)}.");
		assistente.AdicionarInstrucao($"Se uma data for nula então quer dizer que ainda não ocorreu: Por exemplo, se a data de início for nula então a tarefa ainda não foi iniciada e assim por diante.");
		assistente.AdicionarInstrucao($"Comece brevemente falando quem é você e perguntando como pode ajudar. Não entre em detalhes sobre as tarefas do projeto imediatamente, apenas tenha-as em mente pois elas podem entrar em pauta.");

		string novaChave;
		do
		{
			novaChave = Random.Shared.Next().ToString();
		} while (assistentes.ContainsKey(novaChave));
		
		assistentes.Add(Random.Shared.Next().ToString(), assistente);
		return Ok(new {
			IdAssistente = assistentes.Last().Key
		});
	}

}
