namespace Desenvolve.Contexts;

using Desenvolve.Util;
using Microsoft.EntityFrameworkCore;

public class CTXDesenvolve : DbContext
{
	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		IConfigurationSection dbConnection = AppSettings.Section("DBConnection");
		string server = dbConnection.GetValue<string>("Server") ?? @".\";
		string database = dbConnection.GetValue<string>("Database") ?? throw new ArgumentNullException("Database name is null");
		
		optionsBuilder.UseSqlServer($"Server={server};Database={database};Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;");
	}
}
