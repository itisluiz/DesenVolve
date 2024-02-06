namespace Desenvolve.Util;

public static class FormHelper
{
	public static void Requeridos(params object?[] parametros)
	{
		foreach (object? parametro in parametros)
		{
			if (parametro is string parametroString && string.IsNullOrWhiteSpace(parametroString)
				|| parametro is int parametroInt && parametroInt == 0
				|| parametro == null)
				throw new ArgumentException("Um ou mais campos requeridos não foram fornecidos");
		}
	}
}
