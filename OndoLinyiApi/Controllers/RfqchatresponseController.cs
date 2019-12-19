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
    public class RfqchatresponseController : ControllerBase
    {
        ondolinyiContext _context;
        public RfqchatresponseController(ondolinyiContext _context)
        {
            this._context = _context;
        }

        [HttpGet]
        public async Task<ActionResult<Rfqchatresponse[]>> Get()
        {
            return await _context.Rfqchatresponse.ToArrayAsync();
        }

        [Route("[action]/{chatid}")]
        [HttpGet]
        public async Task<ActionResult<Rfqchatresponse[]>> GetResponses(int chatid)
        {
            return await _context.Rfqchatresponse.Where(rf=>rf.Chatid==chatid).ToArrayAsync();
        }

        [HttpGet("{chatresid}")]
        public async Task<ActionResult<Rfqchatresponse>> Get(int chatresid)
        {
            var target = await _context.Rfqchatresponse.SingleOrDefaultAsync(obj => obj.Chatresid == chatresid);
            if (target == null)
            {
                return NotFound();
            }
            return target;
        }

        [HttpPost]
        public async Task<ActionResult<Rfqchatresponse>> Post([FromBody] Rfqchatresponse obj)
        {
            if (_context.Rfqchatlog.SingleOrDefault(rf => rf.Chatid == obj.Chatid) == null)
            {
                return BadRequest("Invalid chatid");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid model state");
            }
            else
            {
                _context.Rfqchatresponse.Add(obj);
                await _context.SaveChangesAsync();
                return Created("api/Rfqchatresponse", obj);
            }
        }

        [HttpPut("{chatresid}")]
        public async Task<ActionResult> Put(int chatresid, [FromBody] Rfqchatresponse obj)
        {
            var target = await _context.Rfqchatresponse.SingleOrDefaultAsync(nobj => nobj.Chatresid == chatresid);
            if (target != null && ModelState.IsValid)
            {
                _context.Entry(target).CurrentValues.SetValues(obj);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest();
        }

        [HttpDelete("{chatresid}")]
        public async Task<ActionResult> Delete(int chatresid)
        {
            var target = await _context.Rfqchatresponse.SingleOrDefaultAsync(obj => obj.Chatresid == chatresid);
            if (target != null)
            {
                _context.Rfqchatresponse.Remove(target);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return NotFound();
        }
    }
}