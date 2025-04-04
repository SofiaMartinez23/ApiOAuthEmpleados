using ApiOAuthEmpleados.Helpers;
using ApiOAuthEmpleados.Models;
using ApiOAuthEmpleados.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiOAuthEmpleados.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpleadosController : ControllerBase
    {
        private RepositoryHospital repo;
        private HelperEmpleadoToken helper;
        public EmpleadosController(RepositoryHospital repo, HelperEmpleadoToken helper)
        {
            this.repo = repo;
            this.helper = helper;
        }

        [HttpGet]
        public async Task<ActionResult<List<Empleado>>> GetEmpleados()
        {
            return await this.repo.GetEmpleadosAsync();
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Empleado>> FindEmpleados(int id)
        {
            return await this.repo.FindEmpleadoAsync(id);
        }


        [HttpGet]
        [Authorize]
        [Route("[action]")]
        public async Task<ActionResult<EmpleadoModel>> Perfil()
        {
            EmpleadoModel model = this.helper.GetEmpleado();
            return model;
            /*Claim claim = HttpContext.User.FindFirst(z => z.Type == "UserData");
              string json = claim.Value;
              Empleado empleado = JsonConvert.DeserializeObject<Empleado>(json);
              return await this.repo.FindEmpleadoAsync(empleado.IdEmpleado);*/
        }

        [HttpGet]
        [Authorize(Roles = "PRESIDENTE")]
        [Route("[action]")]
        public async Task<List<Empleado>> Compis()
        {
            string json = HttpContext.User.FindFirst(x => x.Type == "UserData").Value;
            Empleado empleado = JsonConvert.DeserializeObject<Empleado>(json);
            return await this.repo.GetCompisEmpleadoAsync(empleado.IdDepartamento);

        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult> MultiplesValores([FromQuery] List<int> datos)
        {
            return Ok();
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<List<string>>> Oficios()
        {
            return await this.repo.GetOfiosAsync();
        }

        //?oficio=ANALISTA&oficio=DIRECTOR
        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<List<Empleado>>> EmpleadosOficios([FromQuery] List<string> oficio)
        {
            return await this.repo.GetEmpeladosByOfiosAsync(oficio);
        }

        [HttpPut]
        [Route("[action]/{incremento}")]
        public async Task<ActionResult> IncrementoSalario(int incremento, [FromQuery] List<string> oficio)
        {
            await this.repo.IncrementarSalariosAsync(incremento, oficio);
            return Ok();
        }

    }
}
