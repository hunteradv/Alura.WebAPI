using Alura.ListaLeitura.Modelos;
using Alura.ListaLeitura.Persistencia;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Lista = Alura.ListaLeitura.Modelos.ListaLeitura;

namespace Alura.ListaLeitura.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ListasLeituraController : ControllerBase
    {
        readonly IRepository<Livro> _repo;

        public ListasLeituraController(IRepository<Livro> repo)
        {
            _repo = repo;
        }

        private Lista CriaLista(TipoListaLeitura tipo)
        {
            return new Lista
            {
                Tipo = tipo.ParaString(),
                Livros = _repo.All
                .Where(x => x.Lista == tipo)
                .Select(x => x.ToApi())
                .ToList()
            };
        }
        [HttpGet]
        public IActionResult TodasListas()
        {
            Lista paraLer = CriaLista(TipoListaLeitura.ParaLer);
            Lista lendo = CriaLista(TipoListaLeitura.ParaLer);
            Lista lidos = CriaLista(TipoListaLeitura.ParaLer);

            var colecao = new List<Lista> { paraLer, lendo, lidos };
            return Ok(colecao);
        }

        [HttpGet("{tipo}")]
        public IActionResult Recuperar(TipoListaLeitura tipo)
        {
            var lista = CriaLista(tipo);
            return Ok(lista);
        }
    }
}
