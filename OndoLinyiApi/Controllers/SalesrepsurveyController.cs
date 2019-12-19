using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OndoLinyiApi.Models;
namespace OndoLinyiApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesrepsurveyController : ControllerBase
    {
        ondolinyiContext _context;
        public SalesrepsurveyController(ondolinyiContext _context)
        {
            this._context = _context;
        }

        [HttpGet]
        public async Task<ActionResult> Get(int pageNo = 1, int pageSize = 10)
        {
            int skip = (pageNo - 1) * pageSize;
            int total = _context.Salesrepsurvey.Count();
            var records = await _context.Salesrepsurvey.
                OrderByDescending(s => s.Surveydate).
                Skip(skip).
                Take(pageSize).
                ToListAsync();
            return Ok(new PagedResult<Salesrepsurvey>(records, pageNo, pageSize, total));
        }
       
       
        [HttpGet("{surveyid}")]
        public async Task<ActionResult<Salesrepsurvey>> Get(int surveyid)
        {
            var target = await _context.Salesrepsurvey.SingleOrDefaultAsync(obj => obj.Surveyid == surveyid);
            if (target == null)
            {
                return NotFound();
            }
            return target;
        }
             
       
        [HttpPut("{surveyid}")]
        public async Task<ActionResult> Put(int surveyid, [FromBody] Salesrepsurvey obj)
        {
            var target = await _context.Salesrepsurvey.SingleOrDefaultAsync(nobj => nobj.Surveyid == surveyid);
            if (target != null && ModelState.IsValid)
            {
                _context.Entry(target).CurrentValues.SetValues(obj);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest();
        }

        //[HttpDelete("{surveyid}")]
        //public async Task<ActionResult> Delete(int surveyid)
        //{
        //    var target = await _context.Salesrepsurvey.SingleOrDefaultAsync(obj => obj.Surveyid == surveyid);
        //    if (target != null)
        //    {
        //        _context.Salesrepsurvey.Remove(target);
        //        await _context.SaveChangesAsync();
        //        return Ok();
        //    }
        //    return NotFound();
        //}
    }
}