namespace Desenvolve.Controllers;

using Desenvolve.Contexts;
using Desenvolve.Models;
using Desenvolve.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// TODO: Métodos de API para gerir membros

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
	[HttpPatch]
	public IActionResult AtualizarEquipe([FromForm] int codigoEquipe, [FromForm] string? nome)
	{
		using CTXDesenvolve ctx = new CTXDesenvolve();

		MEquipe? equipe = ctx.Equipes.Include(equipe => equipe.UsuarioEquipes).FirstOrDefault(equipe => equipe.Codigo == codigoEquipe);

		if (equipe == null)
			throw new ArgumentException("Código de equipe não encontrado");

		Login login = new Login(User);
		if (equipe.UsuarioEquipes.Any())
		{
			if (nome != null)
				equipe.Nome = nome;

			ctx.SaveChanges();
			return Ok();
		}
		else
		{
			throw new ArgumentException("Usuário sem permissão para atualizar esta equipe");
		}
	}

	[Authorize]
	[HttpPost("cadastro")]
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

	// Aguardar CProjeto e CEquipe para concluir esse método
	[Authorize]
	[HttpDelete("exclusao")]
	public IActionResult DeletarEquipe([FromForm] int codigoEquipe)
	{
		using CTXDesenvolve ctx = new CTXDesenvolve();



		return Ok();
	}
}
