using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

//Para adicionar a árvore de objetos adicionamos uma nova blibioteca JSON
// dotnet add package Microsoft.AspNetCore

namespace backend.Controllers
{
    //Definimos nossa rota do controller e dizemos que é um controller de API
    [Route("api/[controller]")]
    [ApiController]
    public class EventoController : ControllerBase
    {
        GufosContext _contexto = new GufosContext();
        //GET: api/Evento
        /// <summary>
        /// Pegamos todos os eventos 
        /// </summary>
        /// <returns>Lista de eventos</returns>
        [HttpGet]
        public async Task<ActionResult<List<Evento>>> Get()
        {
            var eventos = await _contexto.Evento.Include("Categoria").Include("Localizacao").ToListAsync();

            if (eventos == null)
            {
                return NotFound();
            }
            return eventos;
        }
        //GET: api/Evento2
        [HttpGet("{id}")]
        public async Task<ActionResult<Evento>> Get(int id)
        {
            // FindAsync = procura algo especifico no banco 
            var evento = await _contexto.Evento.Include("Categoria").Include("Localizacao").FirstOrDefaultAsync(e => e.EventoId == id);

            if (evento == null)
            {
                return NotFound();
            }
            return evento;
        }

        //POST api/Evento
        [HttpPost]
        public async Task<ActionResult<Evento>> Post(Evento evento)
        {
            try
            {
                // Tratamos contra ataques de SQL Injection
                await _contexto.AddAsync(evento);
                //Salvamos efetivamente o nosso objeto no banco de dadOS
                await _contexto.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return evento;
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, Evento evento)
        {
            //Se o Id do objeto não existir
            //ele retorna 404
            if (id != evento.EventoId)
            {
                return BadRequest();
            }
            //Comparamos os atributos que foram modificados atraves EF
            _contexto.Entry(evento).State = EntityState.Modified;
            try
            {
                await _contexto.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                //verificamos se o objeto insirido realmente existe no banco
                var evento_valido = await _contexto.Evento.FindAsync(id);

                if (evento_valido == null)
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
        //DELETE api/evento/id
        [HttpDelete("{id}")]
        public async Task<ActionResult<Evento>> Delete(int id)
        {
            var evento = await _contexto.Evento.FindAsync(id);
            if (evento == null)
            {
                return NotFound();
            }
            _contexto.Evento.Remove(evento);
            await _contexto.SaveChangesAsync();

            return evento;
        }

    }
}