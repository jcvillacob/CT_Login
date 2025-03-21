using CT_Login.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace CT_Login.Helpers
{
    public static class JwtHelper
    {
        public static string GenerateToken(User user, string secret, int expireHours)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secret);

            // Aquí se crean las claims con la información del usuario.
            var claims = new List<Claim>
            {
                new Claim("UsuarioID", user.UsuarioID.ToString()),
                new Claim("Username", user.Username),
                new Claim("Nombre", user.Nombre),
                new Claim("Cedula", user.Cedula)
            };

            // Se pueden agregar roles y subroles (por ejemplo, como una lista separada por comas)
            if (user.Roles != null)
                claims.Add(new Claim("roles", string.Join(",", user.Roles.ConvertAll(r => r.NombreRol))));
            if (user.Subroles != null)
                claims.Add(new Claim("subroles", string.Join(",", user.Subroles.ConvertAll(s => s.NombreSubrol))));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(expireHours),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        // Método para refrescar token a partir de claims extraídas del token expirado
        public static string GenerateTokenFromClaims(IEnumerable<Claim> claims, string secret, int expireHours)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(expireHours),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        // Método para obtener ClaimsPrincipal de un token expirado
        public static ClaimsPrincipal GetPrincipalFromExpiredToken(string token, string secret)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret)),
                ValidateLifetime = false // Ignorar expiración
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
                if (securityToken is not JwtSecurityToken jwtSecurityToken)
                    return null;
                return principal;
            }
            catch
            {
                return null;
            }
        }
    }
}
