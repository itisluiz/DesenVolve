using Desenvolve.Contexts;
using Desenvolve.Models;
using Desenvolve.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Desenvolve.Controllers;

[Route("api/tarefa")]
public class CTarefa : Controller
{
	[Authorize]
	[HttpGet]
	public IActionResult ObterTarefa([FromQuery] int codigoTarefa)
	{
		FormHelper.Requeridos(codigoTarefa);
		using CTXDesenvolve ctx = new CTXDesenvolve();

		MTarefa? tarefa = ctx.Tarefas
			.Include(tarefa => tarefa.Projeto)
			.ThenInclude(projeto => projeto.Equipe)
			.ThenInclude(equipe => equipe.UsuarioEquipes)
			.FirstOrDefault(tarefa => tarefa.Codigo == codigoTarefa);
			
		if (tarefa == null)
			throw new ArgumentException("Código de tarefa não encontrado");

		Login login = new Login(User);

		if (!tarefa.Projeto.Equipe.Membros.Any(login.RepresentaUsuario))
			throw new UnauthorizedAccessException("Usuário sem permissão para acessar esta tarefa");

		return Ok(tarefa);
	}

	[Authorize]
	[HttpPost]
	public IActionResult CadastrarTarefa([FromForm] int codigoProjeto, [FromForm] string nome, [FromForm] string descricao,
		[FromForm] MTarefa.NivelComplexidade complexidade, [FromForm] int codigoResponsavel, [FromForm] DateTime? prazo)
	{
		FormHelper.Requeridos(codigoProjeto, nome, descricao, complexidade, codigoResponsavel);

		throw new NotImplementedException();
	}
}