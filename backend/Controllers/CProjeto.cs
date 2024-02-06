namespace Desenvolve.Controllers;

using Desenvolve.Contexts;
using Desenvolve.Models;
using Desenvolve.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/projeto")]
public class CProjeto : Controller
{
	[Authorize]
	[HttpGet]
	public IActionResult ObterProjeto([FromQuery] int codigoProjeto)
	{
		FormHelper.Requeridos(codigoProjeto);
		using CTXDesenvolve ctx = new CTXDesenvolve();

		MProjeto? projeto = ctx.Projetos
			.Include(projeto => projeto.Equipe)
			.ThenInclude(equipe => equipe.UsuarioEquipes)
			.FirstOrDefault(projeto => projeto.Codigo == codigoProjeto);

		if (projeto == null)
			throw new ArgumentException("Código de projeto não encontrado");
	
		Login login = new Login(User);

		if (!projeto.Equipe.Membros.Any(login.RepresentaUsuario))
			throw new UnauthorizedAccessException("Usuário não é membro da equipe e não tem permissão para acessar este projeto");

		return Ok(projeto);
	}

	[Authorize]
	[HttpPost]
	public IActionResult CadastrarProjeto([FromForm] int codigoEquipe, [FromForm] string nome)
	{
		FormHelper.Requeridos(codigoEquipe, nome);
		using CTXDesenvolve ctx = new CTXDesenvolve();

		Login login = new Login(User);
		MUsuario usuario = login.ObterUsuario(ctx);

		MEquipe? equipe = ctx.Equipes.Include(equipe => equipe.UsuarioEquipes).FirstOrDefault(equipe => equipe.Codigo == codigoEquipe);

		if (equipe == null)
			throw new ArgumentException("Código de equipe não encontrado");

		if (!equipe.Administradores.Contains(usuario))
			throw new UnauthorizedAccessException("Usuário sem permissão para cadastrar um novo projeto");

		MProjeto novoProjeto = new MProjeto(nome, equipe);
		equipe.Projetos.Add(novoProjeto);
		
		ctx.SaveChanges();
		return Ok(novoProjeto);
	}

	[Authorize]
	[HttpPatch]
	public IActionResult AtualizarProjeto([FromForm] int codigoProjeto, [FromForm] string? nome)
	{
		FormHelper.Requeridos(codigoProjeto);
		using CTXDesenvolve ctx = new CTXDesenvolve();

		MProjeto? projeto = ctx.Projetos
			.Include(projeto => projeto.Equipe)
			.ThenInclude(equipe => equipe.UsuarioEquipes)
			.FirstOrDefault(projeto => projeto.Codigo == codigoProjeto);
		
		if (projeto == null)
			throw new ArgumentException("Código de projeto não encontrado");

		Login login = new Login(User);
		MUsuario usuario = login.ObterUsuario(ctx);

		if (!projeto.Equipe.Administradores.Contains(usuario))
			throw new UnauthorizedAccessException("Usuário sem permissão para atualizar este projeto");

		if (nome != null)
			projeto.Nome = nome;

		ctx.SaveChanges();
		return Ok();
	}

	[Authorize]
	[HttpDelete]
	public IActionResult DeletarProjeto([FromForm] int codigoProjeto)
	{
		FormHelper.Requeridos(codigoProjeto);
		using CTXDesenvolve ctx = new CTXDesenvolve();

		MProjeto? projeto = ctx.Projetos
			.Include(projeto => projeto.Equipe)
			.ThenInclude(equipe => equipe.UsuarioEquipes)
			.FirstOrDefault(projeto => projeto.Codigo == codigoProjeto);

		if (projeto == null)
			throw new ArgumentException("Código de projeto não encontrado");

		Login login = new Login(User);
		MUsuario usuario = login.ObterUsuario(ctx);

		if (!projeto.Equipe.Administradores.Contains(usuario))
			throw new UnauthorizedAccessException("Usuário sem permissão para remover este projeto");

		// remove tarefas associados ao projeto que deseja remover
		ctx.Tarefas.RemoveRange(projeto.Tarefas);

		ctx.Projetos.Remove(projeto);
		ctx.SaveChanges();

		return Ok();
	}
}
