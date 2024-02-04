using Desenvolve.Contexts;
using Desenvolve.Models;
using Microsoft.EntityFrameworkCore;

namespace Desenvolve.Util;

public static class Logger
{
	public static void PrintLogExcecao(MLogExcecao logExcecao)
	{
		Console.ForegroundColor = ConsoleColor.Red;
		Console.Write($"[{DateTime.Now:HH:mm:ss}] Exceção capturada ");

		Console.ForegroundColor = ConsoleColor.DarkGray;
		Console.Write(logExcecao.Excecao);

		Console.ForegroundColor = ConsoleColor.Red;
		Console.Write(": \"");

		Console.ForegroundColor = ConsoleColor.DarkGray;
		Console.Write(logExcecao.Mensagem);

		Console.ForegroundColor = ConsoleColor.Red;
		Console.WriteLine("\"");

		if (logExcecao.StackTrace != null)
		{
			Console.ForegroundColor = ConsoleColor.DarkYellow;
			Console.WriteLine("Stacktrace:");

			Console.ForegroundColor = ConsoleColor.DarkGray;
			Console.WriteLine(logExcecao.StackTrace);
		}

		Console.ResetColor();
	}

	private static void LogarFalhaLog()
	{
		Console.ForegroundColor = ConsoleColor.Red;
		Console.WriteLine("Erro ao salvar log na base de dados");

		Console.ResetColor();
	}

	public static int LogarExcecao(Exception excecao)
	{
		using CTXDesenvolve ctx = new CTXDesenvolve();

		if (excecao is DbUpdateException excecaoBanco)
			excecao = excecaoBanco.InnerException ?? excecaoBanco;

		MLogExcecao logExcecao = ctx.LogsExcecao.Add(new MLogExcecao(excecao)).Entity;
		PrintLogExcecao(logExcecao);

		try { ctx.SaveChanges(); }
		catch (Exception) { LogarFalhaLog(); }

		return logExcecao.Codigo;
	}
}