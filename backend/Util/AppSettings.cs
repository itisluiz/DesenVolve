namespace Desenvolve.Util;

public static class AppSettings
{
	public static IConfigurationRoot Configuration { get; private set; }
	public static bool Development
	{ 
		get { return Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development"; }
	}

	static AppSettings()
	{
		Configuration = new ConfigurationBuilder()
			.AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
			.Build();
	}
	
	public static IConfigurationSection Section(string sectionName)
	{
		return Configuration.GetSection(sectionName);
	}
}
