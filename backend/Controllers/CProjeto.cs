using Desenvolve.Contexts;
using Desenvolve.Models;
using Desenvolve.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Desenvolve.Controllers;

[Route("api/projeto")]

public class CProjeto : Controller
{
    [Authorize]
    [HttpGet]
    public IActionResult ObterProjeto([FromQuery] int codigoProjeto)
    {
        using CTXDesenvolve ctx = new CTXDesenvolve();

        MProjeto? projeto = ctx.Projetos.FirstOrDefault(projeto => projeto.Codigo == codigoProjeto);
        if (projeto == null)
            throw new ArgumentException("Código de projeto não encontrado");

        MUsuario? usuario = Identity.ObterUsuarioLogado(ctx, User);
        if (usuario == null)
            throw new ArgumentException("Código de usuário logado inválido");

        MEquipe? equipe = ctx.Equipes.FirstOrDefault(equipe => equipe.Projetos.Contains(projeto));
        if (equipe == null || !equipe.Membros.Contains(usuario))
        {
            throw new UnauthorizedAccessException("Usuário não é membro da equipe e não pode acessar esse projeto");
        }

        return Ok(projeto);
    }

    [Authorize]
    [HttpPost("cadastro")]
    public IActionResult CadastrarProjeto([FromForm] int codigoEquipe, [FromForm] string nome)
    {
        using CTXDesenvolve ctx = new CTXDesenvolve();

        MUsuario? usuario = Identity.ObterUsuarioLogado(ctx, User);
        if (usuario == null)
            throw new ArgumentException("Código de usuário logado inválido");

        MEquipe? equipe = ctx.Equipes.Include(equipe => equipe.UsuarioEquipes).FirstOrDefault(equipe => equipe.Codigo == codigoEquipe);
        if (equipe == null || !equipe.Membros.Contains(usuario))
            throw new UnauthorizedAccessException("Usuário não é membro da equipe e não pode cadastrar um novo projeto");
        else if (!(equipe.Administradores.Contains(usuario) || equipe.Lider == usuario))
            throw new UnauthorizedAccessException("Usuário não é lider ou administrador e não pode cadastrar um novo projeto");
        
        MProjeto novoProjeto = new MProjeto {Nome = nome};
        equipe.Projetos.Add(novoProjeto);
        
        ctx.SaveChanges();
        return Ok(novoProjeto);
    }
}