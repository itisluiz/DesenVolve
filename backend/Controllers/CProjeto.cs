using Desenvolve.Contexts;
using Desenvolve.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;

namespace Desenvolve.Controllers;

[Route ("api/projeto")]

public class CProjeto : Controller
{
    [Authorize]
    [HttpGet]
    public IActionResult ObterProjeto([FromQuery] int codigoProjeto)
    {
        using CTXDesenvolve ctx = new CTXDesenvolve();

        MProjeto? projeto = ctx.Projetos.FirstOrDefault(projeto => projeto.Codigo == codigoProjeto);
        if (projeto == null)
            throw new ArgumentException("Código de projeto não encontrado")
    }
}