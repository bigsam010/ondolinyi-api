using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OndoLinyiApi.Models;
namespace OndoLinyiApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductimagesController : ControllerBase
    {
        ondolinyiContext _context;
        public ProductimagesController(ondolinyiContext _context)
        {
            this._context = _context;
        }

        [HttpGet]
        public async Task<ActionResult<Productimages[]>> Get()
        {
            return await _context.Productimages.ToArrayAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Productimages>> Get(int id)
        {
            var target = await _context.Productimages.SingleOrDefaultAsync(obj => obj.Id == id);
            if (target == null)
            {
                return NotFound();
            }
            return target;
        }

        [HttpPost]
        public async Task<ActionResult<Productimages>> Post([FromBody] Productimages obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            else
            {
                _context.Productimages.Add(obj);
                await _context.SaveChangesAsync();
                return Created("api/Productimages", obj);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] Productimages obj)
        {
            var target = await _context.Productimages.SingleOrDefaultAsync(nobj => nobj.Id == id);
            if (target != null && ModelState.IsValid)
            {
                _context.Entry(target).CurrentValues.SetValues(obj);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var target = await _context.Productimages.SingleOrDefaultAsync(obj => obj.Id == id);
            if (target != null)
            {
                _context.Productimages.Remove(target);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return NotFound();
        }
    }
}