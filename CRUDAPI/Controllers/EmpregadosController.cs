using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CRUDAPI.models;
using System.Text.RegularExpressions;

namespace CRUDAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpregadosController : ControllerBase
    {
        private readonly RfscamContext _context;

        public EmpregadosController(RfscamContext context)
        {
            _context = context;
        }

        // GET: api/Empregados
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Empregado>>> GetEmpregados()
        {
            return await _context.Empregados.ToListAsync();
        }

        // GET: api/Empregados/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Empregado>> GetEmpregado(int id)
        {
            var empregado = await _context.Empregados.FindAsync(id);

            if (empregado == null)
            {
                return NotFound();
            }

            return empregado;
        }

        // PUT: api/Empregados/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmpregado(int id, Empregado empregado)
        {
            if (id != empregado.EmprId)
            {
                return BadRequest();
            }

            // As condições sobre os campos indicam que:
            //  - Somente podem conter letras e números, e espaço em branco.
            //  - Não estar vazio.
            //  - Não pode conter formato do e-mail inválido

            bool bNomeEmpr = ContemCaractEspeciais(empregado.Nome) && ContemEspacos(empregado.Nome);
            bool bCargoEmpr = ContemCaractEspeciais(empregado.Cargo) && ContemEspacos(empregado.Cargo);
            bool bEmailEmpr = EmailValido(empregado.Email);

            if (!bNomeEmpr && !bCargoEmpr && !bEmailEmpr)
            {
                _context.Entry(empregado).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }

                catch (DbUpdateConcurrencyException)
                {
                    if (!EmpregadoExists(id))
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

        // POST: api/Empregados
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Empregado>> PostEmpregado(Empregado empregado)
        {
            // As condições sobre os campos indicam que:
            //  - Somente podem conter letras e números, e espaço em branco.
            //  - Não estar vazio.
            //  - Não pode conter formato do e-mail inválido

            bool bNomeEmpr = ContemCaractEspeciais(empregado.Nome) && ContemEspacos(empregado.Nome);
            bool bCargoEmpr = ContemCaractEspeciais(empregado.Cargo) && ContemEspacos(empregado.Cargo);
            bool bEmailEmpr = EmailValido(empregado.Email);

            if (!bNomeEmpr && !bCargoEmpr && !bEmailEmpr)
            {
                _context.Empregados.Add(empregado);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetEmpregado", new { id = empregado.EmprId }, empregado);
            }

            return NoContent();
        }

        // DELETE: api/Empregados/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmpregado(int id)
        {
            var empregado = await _context.Empregados.FindAsync(id);

            if (empregado == null)
            {
                return NotFound();
            }

            _context.Empregados.Remove(empregado);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmpregadoExists(int id)
        {
            return _context.Empregados.Any(e => e.EmprId == id);
        }

        // Métodos estáticos para validação de informações.

        public static bool ContemEspacos(string nome)
        {
            return (nome.Trim().Equals(null) || nome.Trim().Equals(""));
        }

        public static bool ContemCaractEspeciais(string nome)
        {
            string patterns = "[^a-zA-z0-9\\s]{1,}";
            Regex rgex = new Regex(patterns);

            // Find match between given string & regular expression
            MatchCollection matchedCharacters = rgex.Matches(nome);

            return (matchedCharacters.Count == 0);
        }

        public static bool EmailValido(string email)
        {
            if ((email == null) || (email.Length < 4))
                return false;

            var partes = email.Split('@');

            if (partes.Length < 2)
                return false;

            var pre = partes[0];

            if (pre.Length == 0)
                return false;

            var validadorPre = new Regex("^[a-zA-Z0-9_.-/+]+$");

            if (!validadorPre.IsMatch(pre))
                return false;

            // gmail.com, outlook.com, terra.com.br, etc.
            var partesDoDominio = partes[1].Split('.');

            if (partesDoDominio.Length < 2)
                return false;

            var validadorDominio = new Regex("^[a-zA-Z0-9-]+$");

            for (int indice = 0; indice < partesDoDominio.Length; indice++)
            {
                var parteDoDominio = partesDoDominio[indice];

                // Evitando @gmail...com
                if (parteDoDominio.Length == 0) return false;

                if (!validadorDominio.IsMatch(parteDoDominio))
                    return false;
            }

            return true;
        }
    }
}