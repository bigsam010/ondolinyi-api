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
    public class StockhistoryController : ControllerBase
    {
        ondolinyiContext _context;
        public StockhistoryController(ondolinyiContext _context)
        {
            this._context = _context;
        }

        [HttpGet]
        public async Task<ActionResult> Get(int pageNo = 1, int pageSize = 10)
        {
            int skip = (pageNo - 1) * pageSize;
            int total = _context.Stockhistory.Count();
            var records = await _context.Stockhistory.
                OrderByDescending(sh => sh.Dateadded).
                Skip(skip).
                Take(pageSize).
                ToListAsync();
            return Ok(new PagedResult<Stockhistory>(records, pageNo, pageSize, total));
        }

        [HttpGet("{logid}")]
        public async Task<ActionResult<Stockhistory>> Get(int logid)
        {
            var target = await _context.Stockhistory.SingleOrDefaultAsync(obj => obj.Logid == logid);
            if (target == null)
            {
                return NotFound();
            }
            return target;
        }

        [HttpPost]
        public async Task<ActionResult<Stockhistory>> Post([FromBody] Stockhistory obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            else
            {
                _context.Stockhistory.Add(obj);
                await _context.SaveChangesAsync();
                return Created("api/Stockhistory", obj);
            }
        }

        [HttpPut("{logid}")]
        public async Task<ActionResult> Put(int logid, [FromBody] Stockhistory obj)
        {
            var target = await _context.Stockhistory.SingleOrDefaultAsync(nobj => nobj.Logid == logid);
            if (target != null && ModelState.IsValid)
            {
                _context.Entry(target).CurrentValues.SetValues(obj);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest();
        }

        [HttpDelete("{logid}")]
        public async Task<ActionResult> Delete(int logid)
        {
            var target = await _context.Stockhistory.SingleOrDefaultAsync(obj => obj.Logid == logid);
            if (target != null)
            {
                _context.Stockhistory.Remove(target);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return NotFound();
        }
    }
}