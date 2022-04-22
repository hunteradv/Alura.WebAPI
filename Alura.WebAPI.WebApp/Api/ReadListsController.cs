using Alura.ListaLeitura.Modelos;
using Alura.ListaLeitura.Persistencia;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using ReadList = Alura.ListaLeitura.Modelos.ListaLeitura;

namespace Alura.WebAPI.WebApp.Api
{
    [ApiController]
    [Route("[controller]")]
    public class ReadListsController : ControllerBase
    {
        private readonly IRepository<Livro> _repo;

        public ReadListsController(IRepository<Livro> repository)
        {
            _repo = repository;
        }

        private ReadList CreateList(TipoListaLeitura type)
        {
            return new ReadList
            {
                Tipo = type.ParaString(),
                Livros = _repo.All
                    .Where(l => l.Lista == type)
                    .Select(l => l.ToApi())
                    .ToList()
            };
        }

        [HttpGet]
        public IActionResult AllLists()
        {            
            ReadList toRead = CreateList(TipoListaLeitura.ParaLer);
            ReadList reading = CreateList(TipoListaLeitura.Lendo);
            ReadList alreadyRead = CreateList(TipoListaLeitura.Lidos);
            var collection = new List<ReadList> { toRead, reading, alreadyRead };
            return Ok(collection);
        }

        [HttpGet("{type}")]
        public IActionResult Recovery(TipoListaLeitura type)
        {
            var list = CreateList(type);
            return Ok(list);
        }
    }
}
