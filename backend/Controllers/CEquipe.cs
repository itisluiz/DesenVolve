

using System.Security.Claims;
using Desenvolve.Contexts;
using Desenvolve.Models;
using Desenvolve.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Desenvolve.Controllers;

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
            throw new ArgumentException("Código de equipe não encontrado.");

        MUsuario? usuario = Identity.ObterUsuarioLogado(ctx, User);
        if (usuario == null)
            throw new ArgumentException("Código de usuário logado inválido");

        if (!equipe.Membros.Contains(usuario))
            throw new ArgumentException("Usuário não é um membro desta equipe");

        return Ok(equipe);
    }

    [Authorize]
    [HttpPost("cadastro")]
    public IActionResult CadastrarEquipe([FromForm] string nome)
    {
        using CTXDesenvolve ctx = new CTXDesenvolve();

        MUsuario? lider = Identity.ObterUsuarioLogado(ctx, User);
        if (lider == null)
            throw new ArgumentException("Código de usuário líder não encontrado.");

        MEquipe novaEquipe = ctx.Equipes.Add(new MEquipe(nome)).Entity;

        novaEquipe.AdicionarMembro(lider, MUsuarioEquipe.TipoCargo.Lider);

        ctx.SaveChanges();
        return Ok(novaEquipe);
    }

    [Authorize]
    [HttpPatch("atualizacao")]
    public IActionResult AtualizarEquipe([FromForm] int codigoEquipe, [FromForm] string? nome)
    {
        using CTXDesenvolve ctx = new CTXDesenvolve();

        MEquipe? equipe = ctx.Equipes.Include(equipe => equipe.UsuarioEquipes).FirstOrDefault(equipe => equipe.Codigo == codigoEquipe);
        if (equipe == null)
            throw new ArgumentException("Código de equipe não encontrado.");

        MUsuario? usuario = Identity.ObterUsuarioLogado(ctx, User);
        if (usuario == null) {
            throw new ArgumentException("Código de usuário não encontrado");
        }
        if (equipe.Lider.Codigo == usuario.Codigo || equipe.Administradores.Any(admin => admin.Codigo == usuario.Codigo))
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
    [HttpDelete("exclusao")]
    public IActionResult DeletarEquipe([FromForm] int codigoEquipe)
    {
        using CTXDesenvolve ctx = new CTXDesenvolve();

        return Ok();
    }
}