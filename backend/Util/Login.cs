namespace Desenvolve.Util;

using System.Security.Claims;
using Desenvolve.Contexts;
using Desenvolve.Models;

public class Login
{
	public int Codigo {get;}

	public string Email {get;}

	public string Nome {get;}

	public string Sobrenome {get;}

	public static bool Logado(ClaimsPrincipal user)
	{
		return user.Identity != null && user.Identity.IsAuthenticated && user.FindFirst("Codigo") != null;
	}

	public Login(ClaimsPrincipal user)
	{
		if (int.TryParse(user.FindFirstValue("Codigo"), out int codigo))
		{
			this.Codigo = codigo;

			// Em geral, se tem o código era para ter os outros atributos também
			this.Email = user.FindFirstValue(ClaimTypes.Email) ?? throw new UnauthorizedAccessException("Usuário sem e-mail");
			this.Nome = user.FindFirstValue(ClaimTypes.Name) ?? throw new UnauthorizedAccessException("Usuário sem nome");
			this.Sobrenome = user.FindFirstValue(ClaimTypes.Surname) ?? throw new UnauthorizedAccessException("Usuário sem sobrenome");
		}
		else
			throw new UnauthorizedAccessException("Usuário não logado");
	}

	public MUsuario ObterUsuario(CTXDesenvolve ctx)
	{
		return ctx.Usuarios.Find(this.Codigo) ?? throw new UnauthorizedAccessException("Usuário está logado mas não está cadastrado");
	}

	public bool RepresentaUsuario(MUsuario? usuario)
	{
		if (usuario == null)
			return false;

		return this.Codigo == usuario.Codigo;
	}
}
