using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    //Definimos nossa rota do controller e dizemos que é um controller de API
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        GufosContext _contexto = new GufosContext();
        //GET: api/Categoria
        [HttpGet]
        public async Task<ActionResult<List<Categoria>>> Get()
        {
            var categorias = await _contexto.Categoria.ToListAsync();

            if (categorias == null)
            {
                return NotFound();
            }
            return categorias;
        }
        //GET: api/Categoria2
        [HttpGet("{id}")]
        public async Task<ActionResult<Categoria>> Get(int id)
        {
            // FindAsync = procura algo especifico no banco 
            var categoria = await _contexto.Categoria.FindAsync(id);

            if (categoria == null)
            {
                return NotFound();
            }
            return categoria;
        }

        //POST api/Categoria
        [HttpPost]
        public async Task<ActionResult<Categoria>> Post(Categoria categoria)
        {
            try
            {
                // Tratamos contra ataques de SQL Injection
                await _contexto.AddAsync(categoria);
                //Salvamos efetivamente o nosso objeto no banco de dadOS
                await _contexto.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return categoria;
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, Categoria categoria)
        {
            //Se o Id do objeto não existir
            //ele retorna 404
            if (id != categoria.CategoriaId)
            {
                return BadRequest();
            }
            //Comparamos os atributos que foram modificados atraves EF
            _contexto.Entry(categoria).State = EntityState.Modified;
            try
            {

                await _contexto.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                //verificamos se o objeto insirido realmente existe no banco
                var categoria_valido = await _contexto.Categoria.FindAsync(id);

                if (categoria_valido == null)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            //Nocontent = retorna 204. sem nada
            return NoContent();
        }
        //DELETE api/categoria/id
        [HttpDelete("{id}")]
        public async Task<ActionResult<Categoria>> Delete(int id)
        {
            var categoria = await _contexto.Categoria.FindAsync(id);
            if (categoria == null)
            {
                return NotFound();
            }
            _contexto.Categoria.Remove(categoria);
            await _contexto.SaveChangesAsync();

            return categoria;
        }

    }
}