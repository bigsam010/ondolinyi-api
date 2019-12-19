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
    public class ManufacturerController : ControllerBase
    {
        ondolinyiContext _context;
        public ManufacturerController(ondolinyiContext _context)
        {
            this._context = _context;
        }

        [HttpGet]
        public async Task<ActionResult> Get(int pageNo = 1, int pageSize = 10)
        {
            int skip = (pageNo - 1) * pageSize;
            int total = _context.Manufacturer.Count();
            var records = await _context.Manufacturer
                .OrderBy(m => m.Companyname)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();
            return Ok(new PagedResult<Manufacturer>(records, pageNo, pageSize, total));
        }
        [Route("[action]")]
        [HttpGet]
        public async Task<ActionResult> GetActive(int pageNo = 1, int pageSize = 10)
        {
            int skip = (pageNo - 1) * pageSize;
            int total = _context.Manufacturer.Where(m => m.Status == "active").Count();
            var records = await _context.Manufacturer.Where(m => m.Status == "active").
                OrderBy(m => m.Companyname).
                Skip(skip).
                Take(pageSize).
                ToListAsync();
            return Ok(new PagedResult<Manufacturer>(records, pageNo, pageSize, total));
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<ActionResult> GetBlocked(int pageNo = 1, int pageSize = 10)
        {
            int skip = (pageNo - 1) * pageSize;
            int total = _context.Manufacturer.Where(m => m.Status == "blocked").Count();
            var records = await _context.Manufacturer.Where(m => m.Status == "blocked").
                OrderBy(m => m.Companyname).
                Skip(skip).
                Take(pageSize).
                ToListAsync();
            return Ok(new PagedResult<Manufacturer>(records, pageNo, pageSize, total));
        }

        [Route("[action]/{searchtext}")]
        [HttpGet]
        public async Task<ActionResult> Search(string searchtext, int pageNo = 1, int pageSize = 10)
        {
            int skip = (pageNo - 1) * pageSize;
            int total = _context.Manufacturer.Where(m => m.Companyname.Contains(searchtext) || m.Companyphone.Contains(searchtext)).Count();
            var records = await _context.Manufacturer.Where(m => m.Companyname.Contains(searchtext) || m.Companyphone.Contains(searchtext)).
                OrderBy(m => m.Companyname).
                Skip(skip).
                Take(pageSize).
                ToListAsync();
            return Ok(new PagedResult<Manufacturer>(records, pageNo, pageSize, total));
        }
        [Route("[action]/{manid}")]
        [HttpGet]
        public async Task<ActionResult<bool>> Isblocked(int manid)
        {
            return await _context.Manufacturer.SingleOrDefaultAsync(m => m.Manufacturerid == manid && m.Status == "blocked") != null;
        }
        [Route("[action]/{manid}")]
        [HttpPost]
        public async Task<ActionResult> Unblock(int manid)
        {
            var target = await _context.Manufacturer.SingleOrDefaultAsync(m => m.Manufacturerid == manid);
            if (target == null)
            {
                return BadRequest("Invalid manufacturerid");
            }
            target.Status = "Active";
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("{manufacturerid}")]
        public async Task<ActionResult<Manufacturer>> Get(int manufacturerid)
        {
            var target = await _context.Manufacturer.SingleOrDefaultAsync(obj => obj.Manufacturerid == manufacturerid);
            if (target == null)
            {
                return NotFound();
            }
            return target;
        }

        [Route("[action]/{manufacturerid}")]
        [HttpGet]
        public async Task<ActionResult> ManufacturerProducts(int manufacturerid, int pageNo = 1, int pageSize = 10)
        {
            int skip = (pageNo - 1) * pageSize;
            int total = _context.Product.Where(p => p.Manufacturer == manufacturerid).Count();
            var records = await _context.Product.Where(p => p.Manufacturer == manufacturerid).
                OrderBy(p => p.Name).
                Skip(skip).
                Take(pageSize).
                ToListAsync();
            return Ok(new PagedResult<Product>(records, pageNo, pageSize, total));
        }
        [Route("[action]/{manufacturerid}")]
        [HttpGet]
        public ActionResult ManufacturerProductCount(int manufacturerid, int pageNo = 1, int pageSize = 10)
        {

            return Ok(_context.Product.Where(p => p.Manufacturer == manufacturerid).Count());
        }
        [HttpPost]
        public async Task<ActionResult<Manufacturer>> Post([FromBody] Manufacturer obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            else
            {
                _context.Manufacturer.Add(obj);
                await _context.SaveChangesAsync();
                return Created("api/Manufacturer", obj);
            }
        }

        [HttpPut("{manufacturerid}")]
        public async Task<ActionResult> Put(int manufacturerid, [FromBody] Manufacturer obj)
        {
            var target = await _context.Manufacturer.SingleOrDefaultAsync(nobj => nobj.Manufacturerid == manufacturerid);
            if (target != null && ModelState.IsValid)
            {
                _context.Entry(target).CurrentValues.SetValues(obj);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest();
        }

        [HttpDelete("{manufacturerid}")]
        public async Task<ActionResult> Delete(int manufacturerid)
        {
            var target = await _context.Manufacturer.SingleOrDefaultAsync(obj => obj.Manufacturerid == manufacturerid);
            if (target != null)
            {
                target.Status = "Blocked";
                await _context.SaveChangesAsync();
                return Ok();
            }
            return NotFound();
        }
    }
}