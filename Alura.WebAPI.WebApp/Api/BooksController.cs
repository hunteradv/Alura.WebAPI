using Alura.ListaLeitura.Modelos;
using Alura.ListaLeitura.Persistencia;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Alura.WebAPI.WebApp.Api
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IRepository<Livro> _repo;

        public BooksController(IRepository<Livro> repository)
        {
            _repo = repository;
        }

        [HttpGet]
        public IActionResult BooksList()
        {
            var list = _repo.All.Select(l => l.ToApi()).ToList();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public IActionResult Recovery(int id)
        {
            var model = _repo.Find(id);
            if (model is null)
            {
                return NotFound();
            }
            return Ok(model.ToApi());
        }

        [HttpGet("{id}/capa")]
        public IActionResult CoverImage(int id)
        {
            byte[] img = _repo.All
                .Where(l => l.Id == id)
                .Select(l => l.ImagemCapa)
                .FirstOrDefault();
            if (img != null)
            {
                return File(img, "image/png");
            }
            return File("~/images/capas/capa-vazia.png", "image/png");
        }

        [HttpPost]
        public IActionResult Include([FromBody] LivroUpload model)
        {
            if (ModelState.IsValid)
            {
                var book = model.ToLivro();
                _repo.Incluir(book);
                var uri = Url.Action("Recovery", new {id = book.Id});
                return Created(uri, book);
            }
            return BadRequest(); //201
        }

        [HttpPut]
        public IActionResult Alter([FromBody] LivroUpload model)
        {
            if (ModelState.IsValid)
            {
                var livro = model.ToLivro();
                if (model.Capa == null)
                {
                    livro.ImagemCapa = _repo.All
                        .Where(l => l.Id == livro.Id)
                        .Select(l => l.ImagemCapa)
                        .FirstOrDefault();
                }
                _repo.Alterar(livro);
                return Ok(); //200
            }
            return BadRequest();
        }

        [HttpDelete("{id}")]
        public IActionResult Remove(int id)
        {
            var model = _repo.Find(id);
            if (model == null)
            {
                return NotFound();
            }
            _repo.Excluir(model);
            return NoContent(); //204
        }
    }
}