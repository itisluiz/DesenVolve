namespace Desenvolve.Filters;

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

public class FLTExcecao : IExceptionFilter
{
	private static JsonResult ResultadoErroJson(string erro, string detalhes, int codigoStatus)
	{
		return new JsonResult(new
		{
			Erro = erro,
			Detalhes = detalhes
		})
		{
			StatusCode = codigoStatus
		};
	}

	public void OnException(ExceptionContext contexto)
	{
		// TODO: LOG ERROS
		Exception excecao = contexto.Exception;

		switch (excecao)
		{
			case ArgumentException:
			case ValidationException:
			{
				contexto.Result = ResultadoErroJson("valorinvalido", excecao.Message, 400);
				contexto.ExceptionHandled = true;
				break;
			}
			case UnauthorizedAccessException:
			{
				contexto.Result = ResultadoErroJson("acessonegado", excecao.Message, 401);
				contexto.ExceptionHandled = true;
				break;
			}
			case DbUpdateException:
			{
				DbUpdateException excecaoBanco = (DbUpdateException)excecao;

				if (excecaoBanco.InnerException is SqlException excecaoSql)
				{
					switch (excecaoSql.Number)
					{
						case 2627: // Erro UNIQUE constriant (PK ou UNIQUE)
						{
							contexto.Result = ResultadoErroJson("valorduplicado", "Um ou mais valores já existem na base de dados e não podem ser duplicados", 400);
							contexto.ExceptionHandled = true;
							break;
						}
					}
				}

				break;
			}
		}

		if (!contexto.ExceptionHandled)
		{
			contexto.Result = ResultadoErroJson("errointerno", "Ocorreu um erro interno no servidor", 500);
			contexto.ExceptionHandled = true;
		}
	}
}
