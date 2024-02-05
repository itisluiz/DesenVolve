using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Desenvolve.Util;

public static class TokenHelper
{
	public static SecurityTokenDescriptor GerarDescriptorJWT(Claim[] claims, DateTime? expiracao)
	{
		SymmetricSecurityKey signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SettingsHelper.Valor<string>("JWT:Secret")));
		return new SecurityTokenDescriptor
		{
			Subject = new ClaimsIdentity(claims),
			Expires = expiracao ?? DateTime.Now.AddDays(1),
			Issuer = SettingsHelper.Valor<string>("JWT:Issuer"),
			SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256Signature),
		};
	}

	public static string GerarJWT(Claim[] claims, DateTime? expiracao)
	{
		JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
		SecurityTokenDescriptor tokenDescriptor = GerarDescriptorJWT(claims, expiracao);
		SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

		return tokenHandler.WriteToken(token);
	}
}
