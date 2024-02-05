namespace Desenvolve.Util;

public static class SettingsHelper
{
	public static IConfigurationRoot Raiz { get; private set; }

	static SettingsHelper()
	{
		Raiz = new ConfigurationBuilder()
			.AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
			.Build();
	}
	
	// Retorna valor ou lança exceção
	public static T Valor<T>(string chave)
	{
		return Raiz.GetValue<T>(chave) ?? throw new ArgumentNullException($"A chave {chave} não foi encontrada");
	}
}
