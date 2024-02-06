namespace Desenvolve.Controllers;

using Desenvolve.Contexts;
using Desenvolve.Models;
using Desenvolve.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/equipe")]
public class CEquipe : Controller
{
	[Authorize]
	[HttpGet]
	public IActionResult ObterEquipe([FromQuery] int codigoEquipe)
	{
		FormHelper.Requeridos(codigoEquipe);
		using CTXDesenvolve ctx = new CTXDesenvolve();

		MEquipe? equipe = ctx.Equipes
			.Include(equipe => equipe.UsuarioEquipes)
			.FirstOrDefault(equipe => equipe.Codigo == codigoEquipe);

		if (equipe == null)
			throw new ArgumentException("Código de equipe não encontrado");

		Login login = new Login(User);

		if (!equipe.Membros.Any(login.RepresentaUsuario))
			throw new UnauthorizedAccessException("Usuário não é membro da equipe e não tem permissão para acessar esta equipe");

		return Ok(equipe);
	}

	[Authorize]
	[HttpPost]
	public IActionResult CadastrarEquipe([FromForm] string nome)
	{
		FormHelper.Requeridos(nome);
		using CTXDesenvolve ctx = new CTXDesenvolve();

		Login login = new Login(User);
		MUsuario criador = login.ObterUsuario(ctx);

		MEquipe novaEquipe = ctx.Equipes.Add(new MEquipe(nome)).Entity;
		novaEquipe.AdicionarMembro(criador, MUsuarioEquipe.TipoCargo.Lider);

		ctx.SaveChanges();
		return Ok(novaEquipe);
	}
	
	[Authorize]
	[HttpPatch]
	public IActionResult AtualizarEquipe([FromForm] int codigoEquipe, [FromForm] string? nome)
	{
		FormHelper.Requeridos(codigoEquipe);
		using CTXDesenvolve ctx = new CTXDesenvolve();

		MEquipe? equipe = ctx.Equipes
			.Include(equipe => equipe.UsuarioEquipes)
			.FirstOrDefault(equipe => equipe.Codigo == codigoEquipe);

		if (equipe == null)
			throw new ArgumentException("Código de equipe não encontrado");

		Login login = new Login(User);
		MUsuario usuario = login.ObterUsuario(ctx);

		if (!equipe.Administradores.Contains(usuario))
			throw new UnauthorizedAccessException("Usuário sem permissão para atualizar esta equipe");

		if (nome != null)
			equipe.Nome = nome;

		ctx.SaveChanges();
		return Ok();
	}

	[Authorize]
	[HttpDelete]
	public IActionResult DeletarEquipe([FromForm] int codigoEquipe)
	{
		FormHelper.Requeridos(codigoEquipe);
		using CTXDesenvolve ctx = new CTXDesenvolve();

		MEquipe? equipe = ctx.Equipes
			.Include(equipe => equipe.UsuarioEquipes)
			.FirstOrDefault(equipe => equipe.Codigo == codigoEquipe);

		if (equipe == null)
			throw new ArgumentException("Código de equipe não encontrado");

		Login login = new Login(User);
		MUsuario usuario = login.ObterUsuario(ctx);

		if (!equipe.Administradores.Contains(usuario))
			throw new UnauthorizedAccessException("Usuário sem permissão para remover esta equipe");

		// remove projetos associados a equipe que deseja remover --> remove as tarefas do projeto também
		ctx.Projetos.RemoveRange(equipe.Projetos);

		ctx.Equipes.Remove(equipe);
		ctx.SaveChanges();

		return Ok();
	}

#region Membros
	[Authorize]
	[HttpPost("membro")]
	public IActionResult AdicionarMembroEquipe([FromForm] int codigoEquipe, [FromForm] int codigoUsuario, [FromForm] MUsuarioEquipe.TipoCargo cargo)
	{
		FormHelper.Requeridos(codigoEquipe, codigoUsuario, cargo);
		using CTXDesenvolve ctx = new CTXDesenvolve();

		MEquipe? equipe = ctx.Equipes.Include(equipe => equipe.UsuarioEquipes).FirstOrDefault(equipe => equipe.Codigo == codigoEquipe);

		if (equipe == null)
			throw new ArgumentException("Código de equipe não encontrado");

		MUsuario? usuario = ctx.Usuarios.Find(codigoUsuario);

		if (usuario == null)
			throw new ArgumentException("Código de usuário não encontrado");

		Login login = new Login(User);
		MUsuario usuarioLogado = login.ObterUsuario(ctx);

		if (!equipe.Administradores.Contains(usuarioLogado))
			throw new UnauthorizedAccessException("Usuário sem permissão para adicionar membros a esta equipe");

		equipe.AdicionarMembro(usuario, cargo);
		ctx.SaveChanges();
		return Ok();
	}

	[Authorize]
	[HttpDelete("membro")]
	public IActionResult RemoverMembroEquipe([FromForm] int codigoEquipe, [FromForm] int codigoUsuario)
	{
		FormHelper.Requeridos(codigoEquipe, codigoUsuario);
		using CTXDesenvolve ctx = new CTXDesenvolve();

		MEquipe? equipe = ctx.Equipes.Include(equipe => equipe.UsuarioEquipes).FirstOrDefault(equipe => equipe.Codigo == codigoEquipe);

		if (equipe == null)
			throw new ArgumentException("Código de equipe não encontrado");

		MUsuario? usuario = ctx.Usuarios.Find(codigoUsuario);

		if (usuario == null)
			throw new ArgumentException("Código de usuário não encontrado");

		Login login = new Login(User);
		MUsuario usuarioLogado = login.ObterUsuario(ctx);

		// Permite que o usuário remova a si mesmo
		if (!equipe.Administradores.Contains(usuarioLogado) && usuarioLogado != usuario)
			throw new UnauthorizedAccessException("Usuário sem permissão para remover membros desta equipe");

		equipe.RemoverMembro(usuario);
		ctx.SaveChanges();
		return Ok();
	}
		
	[Authorize]
	[HttpPatch("membro")]
	public IActionResult AtualizarMembroEquipe([FromForm] int codigoEquipe, [FromForm] int codigoUsuario, [FromForm] MUsuarioEquipe.TipoCargo cargo)
	{
		FormHelper.Requeridos(codigoEquipe, codigoUsuario, cargo);
		using CTXDesenvolve ctx = new CTXDesenvolve();

		MEquipe? equipe = ctx.Equipes.Include(equipe => equipe.UsuarioEquipes).FirstOrDefault(equipe => equipe.Codigo == codigoEquipe);

		if (equipe == null)
			throw new ArgumentException("Código de equipe não encontrado");

		MUsuario? usuario = ctx.Usuarios.Find(codigoUsuario);

		if (usuario == null)
			throw new ArgumentException("Código de usuário não encontrado");

		Login login = new Login(User);
		MUsuario usuarioLogado = login.ObterUsuario(ctx);

		if (equipe.Lider != usuarioLogado)
			throw new UnauthorizedAccessException("Somente o líder pode alterar cargos de membros desta equipe");

		equipe.AlterarCargo(usuario, cargo);
		ctx.SaveChanges();
		return Ok();
	}
	
#endregion
}
