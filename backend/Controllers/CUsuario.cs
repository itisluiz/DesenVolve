namespace Desenvolve.Controllers;

using Desenvolve.Contexts;
using Desenvolve.Models;
using Desenvolve.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[Route("api/usuario")]
public class CUsuario : Controller
{
	[HttpGet]
	public IActionResult ObterUsuario([FromQuery] int? codigoUsuario)
	{
		using CTXDesenvolve ctx = new CTXDesenvolve();

		if (codigoUsuario == null)
		{
			if (!Login.Logado(User))
				throw new ArgumentException("Código de usuário não fornecido e não está logado");
			
			Login login = new Login(User);
			return Ok(login.ObterUsuario(ctx));
		}

		MUsuario? usuario = ctx.Usuarios.FirstOrDefault(usuario => usuario.Codigo == codigoUsuario);		
		if (usuario == null)
			throw new ArgumentException("Código de usuário não encontrado");

		return Ok(usuario);
	}

	[HttpPut]
	public IActionResult CadastrarUsuario([FromForm] string nome, [FromForm] string sobrenome, 
		[FromForm] string email, [FromForm] string senha)
	{
		using CTXDesenvolve ctx = new CTXDesenvolve();
		ctx.Usuarios.Add(new MUsuario(nome, sobrenome, email, senha));
		ctx.SaveChanges();

		return LoginUsuario(email, senha);
	}

	[Authorize]
	[HttpPatch]
	public IActionResult AtualizarUsuario([FromForm] string? nome, 
		[FromForm] string? sobrenome, [FromForm] string? senha)
	{
		using CTXDesenvolve ctx = new CTXDesenvolve();

		Login login = new Login(User);
		MUsuario usuario = login.ObterUsuario(ctx);

		if (nome != null)
			usuario.Nome = nome;

		if (sobrenome != null)
			usuario.Sobrenome = sobrenome;

		if (senha != null)
			usuario.Senha = senha;
		
		ctx.SaveChanges();
		return Ok();
	}

	[HttpPost("/api/login")]
	public IActionResult LoginUsuario([FromForm] string email, [FromForm] string senha)
	{
		using CTXDesenvolve ctx = new CTXDesenvolve();

		MUsuario? usuario = ctx.Usuarios.FirstOrDefault(usuario => usuario.Email == email);

		if (usuario == null || !usuario.TestarSenha(senha))
			throw new UnauthorizedAccessException("Email ou senha incorretos");

		Claim[] claims = 
		[
			new Claim("Codigo", usuario.Codigo.ToString()),
			new Claim(ClaimTypes.Email, usuario.Email),
			new Claim(ClaimTypes.Name, usuario.Nome),
			new Claim(ClaimTypes.Surname, usuario.Sobrenome)
		];

		string token = TokenHelper.GerarJWT(claims, DateTime.Now.AddDays(7));
		Response.Cookies.Append("token", token);
		return Ok();
	}

	[HttpPost("/api/logout")]
	public IActionResult LogoutUsuario()
	{
		Response.Cookies.Delete("token");
		return Ok();
	}
}
