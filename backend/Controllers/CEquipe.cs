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
		using CTXDesenvolve ctx = new CTXDesenvolve();

		MEquipe? equipe = ctx.Equipes.Include(equipe => equipe.UsuarioEquipes).FirstOrDefault(equipe => equipe.Codigo == codigoEquipe);

		if (equipe == null)
			throw new ArgumentException("Código de equipe não encontrado");

		Login login = new Login(User);
		if (!equipe.Membros.Any(login.RepresentaUsuario))
			throw new UnauthorizedAccessException("Usuário não é um membro desta equipe");

		return Ok(equipe);
	}

	[Authorize]
	[HttpPut]
	public IActionResult CadastrarEquipe([FromForm] string nome)
	{
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
		using CTXDesenvolve ctx = new CTXDesenvolve();

		MEquipe? equipe = ctx.Equipes.Include(equipe => equipe.UsuarioEquipes).FirstOrDefault(equipe => equipe.Codigo == codigoEquipe);

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

	// Aguardar CProjeto e CEquipe para concluir esse método
	[Authorize]
	[HttpDelete]
	public IActionResult DeletarEquipe([FromForm] int codigoEquipe)
	{
		using CTXDesenvolve ctx = new CTXDesenvolve();
		throw new NotImplementedException();
	}

#region Membros
	[Authorize]
	[HttpPut("membro")]
	public IActionResult AdicionarMembroEquipe([FromForm] int codigoEquipe, [FromForm] int codigoUsuario, [FromForm] int codigoCargo)
	{
		using CTXDesenvolve ctx = new CTXDesenvolve();

		MEquipe? equipe = ctx.Equipes.Include(equipe => equipe.UsuarioEquipes).FirstOrDefault(equipe => equipe.Codigo == codigoEquipe);

		if (equipe == null)
			throw new ArgumentException("Código de equipe não encontrado");

		MUsuario? usuario = ctx.Usuarios.FirstOrDefault(usuario => usuario.Codigo == codigoUsuario);

		if (usuario == null)
			throw new ArgumentException("Código de usuário não encontrado");

		Login login = new Login(User);
		MUsuario usuarioLogado = login.ObterUsuario(ctx);

		if (!equipe.Administradores.Contains(usuarioLogado))
			throw new UnauthorizedAccessException("Usuário sem permissão para adicionar membros a esta equipe");

		equipe.AdicionarMembro(usuario, (MUsuarioEquipe.TipoCargo)codigoCargo);
		ctx.SaveChanges();
		return Ok();
	}

	[Authorize]
	[HttpDelete("membro")]
	public IActionResult RemoverMembroEquipe([FromForm] int codigoEquipe, [FromForm] int codigoUsuario)
	{
		using CTXDesenvolve ctx = new CTXDesenvolve();

		MEquipe? equipe = ctx.Equipes.Include(equipe => equipe.UsuarioEquipes).FirstOrDefault(equipe => equipe.Codigo == codigoEquipe);

		if (equipe == null)
			throw new ArgumentException("Código de equipe não encontrado");

		MUsuario? usuario = ctx.Usuarios.FirstOrDefault(usuario => usuario.Codigo == codigoUsuario);

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
	public IActionResult AtualizarMembroEquipe([FromForm] int codigoEquipe, [FromForm] int codigoUsuario, [FromForm] int codigoCargo)
	{
		using CTXDesenvolve ctx = new CTXDesenvolve();

		MEquipe? equipe = ctx.Equipes.Include(equipe => equipe.UsuarioEquipes).FirstOrDefault(equipe => equipe.Codigo == codigoEquipe);

		if (equipe == null)
			throw new ArgumentException("Código de equipe não encontrado");

		MUsuario? usuario = ctx.Usuarios.FirstOrDefault(usuario => usuario.Codigo == codigoUsuario);

		if (usuario == null)
			throw new ArgumentException("Código de usuário não encontrado");

		Login login = new Login(User);
		MUsuario usuarioLogado = login.ObterUsuario(ctx);

		if (equipe.Lider != usuarioLogado)
			throw new UnauthorizedAccessException("Somente o líder pode alterar cargos de membros desta equipe");

		equipe.AlterarCargo(usuario, (MUsuarioEquipe.TipoCargo)codigoCargo);
		ctx.SaveChanges();
		return Ok();
	}
	
#endregion
}
