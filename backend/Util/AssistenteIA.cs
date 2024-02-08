namespace Desenvolve.Util;

using System.Security.Claims;
using Desenvolve.Contexts;

using OpenAI;
using OpenAI.Managers;
using OpenAI.ObjectModels;
using OpenAI.ObjectModels.RequestModels;
using OpenAI.ObjectModels.ResponseModels;

public class AssistenteIA
{
	private static readonly OpenAIService servicoOpenAi;
	private readonly ChatCompletionCreateRequest requisicao;

	static AssistenteIA()
	{
		servicoOpenAi = new OpenAIService(new OpenAiOptions() { ApiKey = SettingsHelper.Valor<string>("OpenAI:Token")});
		servicoOpenAi.SetDefaultModelId(Models.Gpt_3_5_Turbo);
	}

	public void AdicionarInstrucao(string instrucao)
	{
		requisicao.Messages.Add(ChatMessage.FromSystem(instrucao));
	}

	public AssistenteIA(string? sistema = null)
	{
		requisicao = new ChatCompletionCreateRequest
		{
			Messages = new List<ChatMessage>
			{
				ChatMessage.FromSystem("Você é o assistente da plataforma DesenVolve, uma plataforma de gestão e acompanhamento de projeto e tarefas.")
			}
		};

		if (sistema != null)
			AdicionarInstrucao(sistema);
	}
	
	public async Task<string?> NovaMensagem(string? mensagemUsuario = null)
	{
		if (mensagemUsuario != null)
			requisicao.Messages.Add(ChatMessage.FromUser(mensagemUsuario));

		ChatCompletionCreateResponse completionResult = await servicoOpenAi.ChatCompletion.CreateCompletion(requisicao);

		if (completionResult.Successful)
		{
			string? resposta = completionResult.Choices.First().Message.Content;

			if (resposta != null)
			{
				requisicao.Messages.Add(ChatMessage.FromAssistant(resposta));
				return resposta;
			}
		}

		return null;
	}
	
}
