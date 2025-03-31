using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ApiOAuthEmpleados.Helpers;
using ApiOAuthEmpleados.Models;
using ApiOAuthEmpleados.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiOAuthEmpleados.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private RepositoryHospital repo;
        private HelperActionServicesOAuth helper;

        public AuthController(RepositoryHospital repo, HelperActionServicesOAuth helper)
        {
            this.repo = repo;
            this.helper = helper;
        }


        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> Login(LoginModel model)
        {
            Empleado empleado = await this.repo.LogInEmpleadoAsync(model.UserName, int.Parse(model.Password));
            if (empleado == null)
            {
                return Unauthorized();
            }
            else
            {
                SigningCredentials credentials = new SigningCredentials
                    (this.helper.GetKeyToken(), SecurityAlgorithms.HmacSha256);

                string  jsonEmpleado = JsonConvert.SerializeObject(empleado);
                Claim[] informacion = new[]
                {
                    new Claim("UserData", jsonEmpleado),
                    new Claim(ClaimTypes.Role, empleado.Oficio)
                };

                JwtSecurityToken token = new JwtSecurityToken(
                    claims: informacion,
                    issuer: this.helper.Issuer,
                    audience: this.helper.Audience,
                    signingCredentials: credentials,
                    expires: DateTime.UtcNow.AddMinutes(20),
                    notBefore: DateTime.UtcNow
                );

                return Ok(new
                {
                    reponse = new JwtSecurityTokenHandler().WriteToken(token)
                });
            }
        } 
    }
}
