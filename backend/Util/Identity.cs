using System.Security.Claims;
using Desenvolve.Contexts;
using Desenvolve.Models;

namespace Desenvolve.Util;

public static class Identity
{
	public static MUsuario? ObterUsuarioLogado(CTXDesenvolve ctx, ClaimsPrincipal user)
	{
		if (int.TryParse(user.FindFirstValue("Codigo"), out int codigoLogado))
			return ctx.Usuarios.FirstOrDefault(usuario => usuario.Codigo == codigoLogado);
		else
			return null;
	}

}