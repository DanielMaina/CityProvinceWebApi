using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CityProvinceWebApi.Data;
using CityProvinceWebApi.Models;

namespace CityProvinceWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        private readonly CityProvinceContext _context;

        public CitiesController(CityProvinceContext context)
        {
            _context = context;
        }

        // GET: api/Cities
        [HttpGet]
        public async Task<ActionResult<IEnumerable<City>>> GetCities()
        {
            return await _context.Cities.Include(c => c.Province).ToListAsync();
        }

        // GET: api/Cities/5
        [HttpGet("{id}")]
        public async Task<ActionResult<City>> GetCity(int id)
        {
            var city = await _context.Cities.Include(c => c.Province)
                .FirstOrDefaultAsync(c => c.ID == id);

            if (city == null)
            {
                return NotFound();
            }

            return city;
        }

        // GET: api/CitiesByProvince
        [HttpGet("ByProvince/{id}")]
        public async Task<ActionResult<IEnumerable<City>>> GetCitiesByProvince(int id)
        {
            return await _context.Cities.Include(c => c.Province)
                .Where(e => e.ProvinceID == id).ToListAsync();
        }

        // PUT: api/Cities/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCity(int id, City city)
        {
            if (id != city.ID)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Entry(city).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CityExists(id))
                {
                    return BadRequest(new { message = "Concurrency Error: City has been Removed." });
                }
                else
                {
                    return BadRequest(new { message = "Concurrency Error: City has been updated by another user.  Cancel and try editing the record again." });
                }
            }
            catch (DbUpdateException dex)
            {
                if (dex.GetBaseException().Message.Contains("UNIQUE"))
                {
                    return BadRequest(new { message = "Unable to save: Duplicate City/Province combination." });
                }
                else
                {
                    return BadRequest(new { message = "Unable to save changes to the database. Try again, and if the problem persists see your system administrator." });
                }
            }
            return NoContent();
        }

        // POST: api/Cities
        [HttpPost]
        public async Task<ActionResult<City>> PostCity(City city)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _context.Cities.Add(city);
            try
            {
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetCity", new { id = city.ID }, city);
            }
            catch (DbUpdateException dex)
            {
                if (dex.GetBaseException().Message.Contains("UNIQUE"))
                {
                    return BadRequest(new { message = "Unable to save: Duplicate City/Province combination." });
                }
                else
                {
                    return BadRequest(new { message = "Unable to save changes to the database. Try again, and if the problem persists see your system administrator." });
                }
            }
        }

        // DELETE: api/Cities/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<City>> DeleteCity(int id)
        {
            //Prevent deleting any of the original 7 cities seeded
            if (id <= 7)
            {
                return BadRequest(new { message = "Delete Error: You cannot delete the original data." });
            }
            var city = await _context.Cities.FindAsync(id);
            if (city == null)
            {
                return BadRequest(new { message = "Delete Error: City has already been Removed." });
            }
            try
            {
                _context.Cities.Remove(city);
                await _context.SaveChangesAsync();
                return city;
            }
            catch (DbUpdateException)
            {
                return BadRequest(new { message = "Delete Error: Unable to delete City." });
            }
        }

        private bool CityExists(int id)
        {
            return _context.Cities.Any(e => e.ID == id);
        }
    }
}
