using System.Text;
using Desenvolve.Contexts;
using Desenvolve.Util;
using Desenvolve.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<CTXDesenvolve>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMvc(options =>
{
	options.Filters.Add<FLTExcecao>();
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
	options.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuer = true,
		ValidateAudience = false,
		ValidateLifetime = true,
		ValidateIssuerSigningKey = true,
		ValidIssuer = SettingsHelper.Valor<string>("JWT:Issuer"),
		IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SettingsHelper.Valor<string>("JWT:Secret")))
	};

	options.Events = new JwtBearerEvents
	{
		OnMessageReceived = (context) =>
		{
			context.Token = context.Request.Cookies["token"];
			return Task.CompletedTask;
		}
	};
});

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();

	app.UseCors(builder => builder
		.WithOrigins(SettingsHelper.Valor<string>("CORS:AllowedHosts").Split(','))
		.AllowAnyMethod()
		.AllowAnyHeader()
		.AllowCredentials()
	);
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapControllers();
app.Run();
