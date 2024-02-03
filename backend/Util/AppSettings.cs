namespace Desenvolve.Util;

public static class AppSettings
{
	public static IConfigurationRoot Raiz { get; private set; }

	static AppSettings()
	{
		Raiz = new ConfigurationBuilder()
			.AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
			.Build();
	}
	
	public static T Valor<T>(string chave)
	{
		return Raiz.GetValue<T>(chave) ?? throw new ArgumentNullException($"A chave {chave} não foi encontrada");
	}
}
