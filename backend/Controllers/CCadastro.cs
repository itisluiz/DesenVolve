using System.Security.Claims;
using Desenvolve.Util;
using Microsoft.AspNetCore.Mvc;

namespace Desenvolve.Controllers;

// Não implementado, é apenas um exemplo e teste para verificar se o JWT está funcionando.
[Route("api/cadastro")]
public class CCadastro : Controller
{
	[HttpGet]
	public IActionResult TestAuth()
	{
		var username = User.Identity?.Name;
		return Ok(new { Username = username });
	}

	[HttpPost]
	public IActionResult SetAuth(string nome)
	{
		string token = TokenHelper.GerarJWT([new Claim(ClaimTypes.Name, nome)], DateTime.Now.AddDays(1));
		Response.Cookies.Append("token", token);

		return Ok();
	}

	[HttpDelete]
	public IActionResult RemoveAuth()
	{
		Response.Cookies.Delete("token");
		return Ok();
	}

}
