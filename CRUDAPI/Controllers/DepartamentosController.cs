using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CRUDAPI.models;
using NuGet.ContentModel;
using System.Text.RegularExpressions;

namespace CRUDAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartamentosController : ControllerBase
    {
        private readonly RfscamContext _context;

        public DepartamentosController(RfscamContext context)
        {
            _context = context;
        }

        // GET: api/Departamentos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Departamento>>> GetDepartamentos()
        {
            return await _context.Departamentos.ToListAsync();
        }

        // GET: api/Departamentos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Departamento>> GetDepartamento(int id)
        {
            var departamento = await _context.Departamentos.FindAsync(id);

            if (departamento == null)
            {
                return NotFound();
            }

            return departamento;
        }

        // PUT: api/Departamentos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDepartamento(int id, Departamento departamento)
        {
            if (id != departamento.DeptoId)
            {
                return BadRequest();
            }

            // As condições indicam que não podem alterar :
            //  - Os que não contenham letras e números, e espaço em branco.
            //  - O que não pode estar vazio.

            if (!ContemCaractEspeciais(departamento.Nome) && !ContemEspacos(departamento.Nome))
            {
                _context.Entry(departamento).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }

                catch (DbUpdateConcurrencyException)
                {
                    if (!DepartamentoExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return NoContent();
        }

        // POST: api/Departamentos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Departamento>> PostDepartamento(Departamento departamento)
        {
            // As condições sobre o campo 'Nome do Departamento' indicam que:
            //  - Somente podem conter letras e números, e espaço em branco.
            //  - Não estar vazio.
            if (!ContemCaractEspeciais(departamento.Nome) && !ContemEspacos(departamento.Nome))
            {
                _context.Departamentos.Add(departamento);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetDepartamento", new { id = departamento.DeptoId }, departamento);
            }

            return NoContent();
        }

        // DELETE: api/Departamentos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartamento(int id)
        {
            var departamento = await _context.Departamentos.FindAsync(id);

            if (departamento == null)
            {
                return NotFound();
            }

            _context.Departamentos.Remove(departamento);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DepartamentoExists(int id)
        {
            return _context.Departamentos.Any(e => e.DeptoId == id);
        }

        // Métodos estáticos para validação de informações.

        public static bool ContemCaractEspeciais(string nome)
        {
            string patterns = "[^a-zA-z0-9\\s]{1,}";
            Regex rgex = new Regex(patterns);

            // Find match between given string & regular expression
            MatchCollection matchedCharacters = rgex.Matches(nome);

            return (matchedCharacters.Count != 0);
        }

        public static bool ContemEspacos(string nome)
        {
            return (nome.Trim().Equals(null) || nome.Trim().Equals(""));
        }
    }
}
