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
    public class SurveyController : ControllerBase
    {
        ondolinyiContext _context;
        public SurveyController(ondolinyiContext _context)
        {
            this._context = _context;
        }

        [HttpGet]
        public async Task<ActionResult<Survey[]>> Get()
        {
            return await _context.Survey.ToArrayAsync();
        }
        [Route("[action]/{surveyid}")]
        [HttpPost]
        public async Task<ActionResult> ToogleVisible(int surveyid)
        {
            var target = await _context.Survey.SingleOrDefaultAsync(s => s.Surveyid == surveyid);
            if (target == null)
            {
                return BadRequest("Invalid surveyid");
            }
            if (target.Status.ToLower() == "visible")
            {
                target.Status = "Hidden";
            }
            else
            {
                target.Status = "Visible";
            }
            await _context.SaveChangesAsync();
            return Ok();
        }
        [HttpGet("{surveyid}")]
        public async Task<ActionResult<Survey>> Get(int surveyid)
        {
            var target = await _context.Survey.SingleOrDefaultAsync(obj => obj.Surveyid == surveyid);
            if (target == null)
            {
                return NotFound();
            }
            return target;
        }

        [HttpPost]
        public async Task<ActionResult<Survey>> Post([FromBody] Survey obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            else
            {
                _context.Survey.Add(obj);
                await _context.SaveChangesAsync();
                return Created("api/Survey", obj);
            }
        }

        [HttpPut("{surveyid}")]
        public async Task<ActionResult> Put(int surveyid, [FromBody] Survey obj)
        {
            var target = await _context.Survey.SingleOrDefaultAsync(nobj => nobj.Surveyid == surveyid);
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
        //    var target = await _context.Survey.SingleOrDefaultAsync(obj => obj.Surveyid == surveyid);
        //    if (target != null)
        //    {
        //        _context.Survey.Remove(target);
        //        await _context.SaveChangesAsync();
        //        return Ok();
        //    }
        //    return NotFound();
        //}
    }
}