using Desenvolve.Contexts;
using Desenvolve.Models;
using Desenvolve.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Desenvolve.Controllers;

[Route("api/tarefa")]
public class CTarefa : Controller
{
	[Authorize]
	[HttpGet]
	public IActionResult ObterTarefa([FromQuery] int codigoTarefa)
	{
		FormHelper.Requeridos(codigoTarefa);
		using CTXDesenvolve ctx = new CTXDesenvolve();

		MTarefa? tarefa = ctx.Tarefas
			.Include(tarefa => tarefa.Projeto)
			.ThenInclude(projeto => projeto.Equipe)
			.ThenInclude(equipe => equipe.UsuarioEquipes)
			.FirstOrDefault(tarefa => tarefa.Codigo == codigoTarefa);
			
		if (tarefa == null)
			throw new ArgumentException("Código de tarefa não encontrado");

		Login login = new Login(User);

		if (!tarefa.Projeto.Equipe.Membros.Any(login.RepresentaUsuario))
			throw new UnauthorizedAccessException("Usuário não é membro da equipe e não tem permissão para acessar esta tarefa");

		return Ok(tarefa);
	}

	[Authorize]
	[HttpPost]
	public IActionResult CadastrarTarefa([FromForm] int codigoProjeto, [FromForm] string nome, [FromForm] string descricao,
		[FromForm] MTarefa.NivelComplexidade complexidade, [FromForm] int codigoResponsavel, [FromForm] DateTime? prazo)
	{
		FormHelper.Requeridos(codigoProjeto, nome, descricao, complexidade, codigoResponsavel);
		using CTXDesenvolve ctx = new CTXDesenvolve();

		Login login = new Login(User);

		MProjeto? projeto = ctx.Projetos
			.Include(projeto => projeto.Equipe)
			.ThenInclude(equipe => equipe.UsuarioEquipes)
			.FirstOrDefault(projeto => projeto.Codigo == codigoProjeto);

		if (projeto == null)
			throw new ArgumentException("Código de projeto não encontrado");
		
		if (!projeto.Equipe.Administradores.Any(login.RepresentaUsuario))
			throw new UnauthorizedAccessException("Usuário não tem permissão para cadastrar uma tarefa neste projeto");

		MUsuario? responsavel = ctx.Usuarios.FirstOrDefault(usuario => usuario.Codigo == codigoResponsavel);
		if (responsavel == null)
			throw new ArgumentException("Código de responsável não encontrado");

		MTarefa novaTarefa = new MTarefa(nome, descricao, complexidade, projeto, responsavel, prazo);

		ctx.Tarefas.Add(novaTarefa);
		ctx.SaveChanges();

		return Ok(novaTarefa);
	}

	[Authorize]
	[HttpPatch]
	public IActionResult AtualizarTarefa([FromForm] int codigoTarefa, [FromForm] string? nome, [FromForm] string? descricao,
		[FromForm] MTarefa.NivelComplexidade? complexidade, [FromForm] int? codigoResponsavel, [FromForm] DateTime? prazo, 
		[FromForm] DateTime? inicio, [FromForm] DateTime? finalizado)
	{
		FormHelper.Requeridos(codigoTarefa);
		using CTXDesenvolve ctx = new CTXDesenvolve();

		MTarefa? tarefa = ctx.Tarefas
			.Include(tarefa => tarefa.Projeto)
			.ThenInclude(projeto => projeto.Equipe)
			.ThenInclude(equipe => equipe.UsuarioEquipes)
			.FirstOrDefault(tarefa => tarefa.Codigo == codigoTarefa);

		if (tarefa == null)
			throw new ArgumentException("Código de tarefa não encontrado");

		Login login = new Login(User);

		if (!tarefa.Projeto.Equipe.Administradores.Any(login.RepresentaUsuario) && !login.RepresentaUsuario(tarefa.Responsavel))
			throw new UnauthorizedAccessException("Usuário sem permissão para editar esta tarefa");

		if (nome != null)
			tarefa.Nome = nome;

		if (descricao != null)
			tarefa.Descricao = descricao;

		if (complexidade != null)
			tarefa.Complexidade = (MTarefa.NivelComplexidade)complexidade; // Dá erro sem o cast, não sei por que

		if (codigoResponsavel != null)
		{
			MUsuario? responsavel = ctx.Usuarios.FirstOrDefault(usuario => usuario.Codigo == codigoResponsavel);
			if (responsavel == null)
				throw new ArgumentException("Código de responsável não encontrado");
			
			tarefa.Responsavel = responsavel;
		}

		if (prazo != null)
			tarefa.Prazo = prazo;
			
		if (inicio != null)
			tarefa.Inicio = inicio;

		if (finalizado != null)
			tarefa.Finalizado = finalizado;
			
		ctx.SaveChanges();
		return Ok();
	}

	[Authorize]
	[HttpDelete]
	public IActionResult DeletarTarefa([FromForm] int codigoTarefa)
	{
		FormHelper.Requeridos(codigoTarefa);
		using CTXDesenvolve ctx = new CTXDesenvolve();

		MTarefa? tarefa = ctx.Tarefas
			.Include(tarefa => tarefa.Projeto)
			.ThenInclude(projeto => projeto.Equipe)
			.ThenInclude(equipe => equipe.UsuarioEquipes)
			.FirstOrDefault(tarefa => tarefa.Codigo == codigoTarefa);

		if (tarefa == null)
			throw new ArgumentException("Código de Tarefa não encontrado");

		Login login = new Login(User);

		if (!tarefa.Projeto.Equipe.Administradores.Any(login.RepresentaUsuario) && !login.RepresentaUsuario(tarefa.Responsavel))
			throw new UnauthorizedAccessException("Usuário sem permissão para remover esta tarefa");

		ctx.Tarefas.Remove(tarefa);
		ctx.SaveChanges();

		return Ok();
	}
}