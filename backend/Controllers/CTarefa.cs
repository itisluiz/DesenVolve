using Desenvolve.Contexts;
using Desenvolve.Models;
using Desenvolve.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Desenvolve.Models.MTarefa;

namespace Desenvolve.Controllers;

[Route("api/tarefa")]
public class CTarefa : Controller
{
    [Authorize]
    [HttpGet]
    public IActionResult ObterTarefa([FromQuery] int codigoTarefa)
    {
        using CTXDesenvolve ctx = new CTXDesenvolve();

        MTarefa? tarefa = ctx.Tarefas.FirstOrDefault(tarefa => tarefa.Codigo == codigoTarefa);
        if (tarefa == null)
            throw new ArgumentException("Código de tarefa não encotnrado");

        MUsuario? usuario = Identity.ObterUsuarioLogado(ctx, User); //mudar forma de pegar usuário aqui
        if (usuario == null || (!tarefa.Projeto.Equipe.Membros.Any(membro => membro.Codigo == usuario.Codigo)))
            throw new UnauthorizedAccessException("Usuário sem permissão para acessar esta tarefa");

        return Ok(tarefa);
    }

    //continuar
    [Authorize]
    [HttpPost]
    public IActionResult CadastrarTarefa([FromForm] int codigoProjeto, [FromForm] string nome, [FromForm] string descricao,
        [FromForm] NivelComplexidade complexidade, [FromForm] int codigoResponsavel, [FromForm] DateTime? prazo)
    {
        return Ok();
    }

}