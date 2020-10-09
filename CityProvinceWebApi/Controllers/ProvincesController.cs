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
    public class ProvincesController : ControllerBase
    {
        private readonly CityProvinceContext _context;

        public ProvincesController(CityProvinceContext context)
        {
            _context = context;
        }

        // GET: api/Provinces
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Province>>> GetProvinces()
        {
            return await _context.Provinces.ToListAsync();
        }

        // GET: api/Provinces/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Province>> GetProvince(int id)
        {
            var province = await _context.Provinces.FindAsync(id);

            if (province == null)
            {
                return NotFound();
            }

            return province;
        }

        // PUT: api/Provinces/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProvince(int id, Province province)
        {
            if (id != province.ID)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Entry(province).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProvinceExists(id))
                {
                    return BadRequest(new { message = "Concurrency Error: Province has been Removed." });
                }
                else
                {
                    return BadRequest(new { message = "Concurrency Error: Province has been updated by another user.  Cancel and try editing the record again." });
                }
            }

            return NoContent();
        }

        // POST: api/Provinces
        [HttpPost]
        public async Task<ActionResult<Province>> PostProvince(Province province)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Provinces.Add(province);
            try
            {
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetProvince", new { id = province.ID }, province);
            }
            catch (DbUpdateException dex)
            {
                if (dex.GetBaseException().Message.Contains("UNIQUE"))
                {
                    return BadRequest(new { message = "Unable to save: Duplicate Province." });
                }
                else
                {
                    return BadRequest(new { message = "Unable to save changes to the database. Try again, and if the problem persists see your system administrator." });
                }
            }

        }

        // DELETE: api/Provinces/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Province>> DeleteProvince(int id)
        {
            //Prevent deleting any of the original 13 provinces seeded
            if (id <= 13)
            {
                return BadRequest(new { message = "Delete Error: You cannot delete the original data." });
            }
            var province = await _context.Provinces.FindAsync(id);

            if (province == null)
            {
                return BadRequest(new { message = "Delete Error: Province has already been deleted." });
            }
            try
            {
                _context.Provinces.Remove(province);
                await _context.SaveChangesAsync();
                return province;
            }
            catch (DbUpdateException dex)
            {
                if (dex.GetBaseException().Message.Contains("FOREIGN"))
                {
                    return BadRequest(new { message = "Unable to Delete: You cannot delete a Province that has Cities." });
                }
                else
                {
                    return BadRequest(new { message = "Unable to save changes to the database. Try again, and if the problem persists see your system administrator." });
                }
            }
        }

        private bool ProvinceExists(int id)
        {
            return _context.Provinces.Any(e => e.ID == id);
        }
    }
}
