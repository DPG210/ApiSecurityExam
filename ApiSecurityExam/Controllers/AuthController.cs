using ApiOAuthEmpleados.Helpers;
using ApiSecurityExam.Helpers;
using ApiSecurityExam.Models;
using ApiSecurityExam.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ApiSecurityExam.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private RepositoryLibros repo;
        private HelperActionOAuthService helper;
        public AuthController(RepositoryLibros repo, HelperActionOAuthService helper)
        {
            this.repo = repo;
            this.helper = helper;
        }
        [HttpPost]
        public async Task<ActionResult> Login(LoginModel model)
        {
            Usuarios usuario = await this.repo.LoginUsuario(model.Nombre, model.Password);
            if (usuario == null)
            {
                return Unauthorized();
            }
            else
            {
                SigningCredentials credentials =
                    new SigningCredentials(this.helper.GetKeyToken(), SecurityAlgorithms.HmacSha256);

                UserModel userModel = new UserModel
                {
                    IdUsuario = usuario.IdUsuario,
                    Nombre = usuario.Nombre,
                    Apellido = usuario.Apellido,
                    Email = usuario.Email,
                    Foto = usuario.Foto
                };

                string jsonEmpleado =
                    JsonConvert.SerializeObject(userModel);
                string jsonCypher =
                    HelperCryptography.CifrarString(jsonEmpleado);

                Claim[] informacion = new[]
                {
                    new Claim("UserData", jsonCypher),
                };

                JwtSecurityToken token =
                    new JwtSecurityToken(
                        claims: informacion,
                        issuer: this.helper.Issuer,
                        audience: this.helper.Audience,
                        signingCredentials: credentials,
                        expires: DateTime.UtcNow.AddMinutes(30),
                        notBefore: DateTime.UtcNow
                        );

                return Ok(new
                {
                    response =
                    new JwtSecurityTokenHandler()
                    .WriteToken(token)
                });
            }
        }
    }
}
