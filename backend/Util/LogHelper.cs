using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Desenvolve.Contexts;
using Desenvolve.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Desenvolve.Util;

public static class LogHelper
{
	private static void PrintLog(MLog log)
	{
		switch (log.Tipo)
		{
			case MLog.TipoLog.Excecao:
			{
				Console.ForegroundColor = ConsoleColor.DarkRed;
				break;
			}
			case MLog.TipoLog.Informacao:
			{
				Console.ForegroundColor = ConsoleColor.Cyan;
				break;
			}
			default:
			{
				Console.ForegroundColor = ConsoleColor.Gray;
				break;
			}
		}
		
		Console.WriteLine(log);
		Console.ResetColor();
	}

	public static async void LogExcecao(Exception excecao, CTXDesenvolve? ctx = null)
	{
		ctx = ctx ?? new CTXDesenvolve();
		MLog log = ctx.Add(new MLog(excecao)).Entity;

		try { await ctx.SaveChangesAsync(); }
		catch (Exception) { log.Codigo = 0; }
		
		PrintLog(log);
	}

	public static async void LogInformacao(string mensagem, CTXDesenvolve? ctx = null)
	{
		ctx = ctx ?? new CTXDesenvolve();
		MLog log = ctx.Add(new MLog(mensagem)).Entity;

		try { await ctx.SaveChangesAsync(); }
		catch (Exception) { log.Codigo = 0; }
		
		PrintLog(log);
	}
}
