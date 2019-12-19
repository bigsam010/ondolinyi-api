using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OndoLinyiApi.Models;
using LightQuery.EntityFrameworkCore;
using LightQuery.Client;
using LightQuery.Shared;
namespace OndoLinyiApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaxController : ControllerBase
    {
        ondolinyiContext _context;
        public TaxController(ondolinyiContext _context)
        {
            this._context = _context;
        }

        [HttpGet]
        public async Task<ActionResult> Get(int pageNo = 1, int pageSize = 10)
        {
            // Determine the number of records to skip
            int skip = (pageNo - 1) * pageSize;

            // Get total number of records
            int total = _context.Tax.Count();

            // Select the records based on paging parameters
            var taxes = await _context.Tax
                .OrderBy(c => c.Name)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();

            // Return the list of records
            return Ok(new PagedResult<Tax>(taxes, pageNo, pageSize, total));

        }

        [HttpGet]
        [Route("[action]/{type}")]
        public async Task<ActionResult> GetByType(string type, int pageNo = 1, int pageSize = 10)
        {
            if (type.ToLower() != "percentage" && type.ToLower() != "flat")
            {
                return BadRequest("Invalid tax type");
            }
            // Determine the number of records to skip
            int skip = (pageNo - 1) * pageSize;

            int total = _context.Tax.Where(t => t.Type == type).Count();
            // Select the records based on paging parameters
            var taxes = await _context.Tax
                .Where(t => t.Type == type)
                .OrderBy(c => c.Name)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();

            // Return the list of records
            return Ok(new PagedResult<Tax>(taxes, pageNo, pageSize, total));

        }

        [HttpGet]
        [Route("[action]/{searchtext}")]
        public async Task<ActionResult> Search(string searchtext, int pageNo = 1, int pageSize = 10)
        {

            // Determine the number of records to skip
            int skip = (pageNo - 1) * pageSize;

            int total = _context.Tax.Where(t => t.Name.Contains(searchtext) || t.Invoicelabel.Contains(searchtext)).Count();
            // Select the records based on paging parameters
            var taxes = await _context.Tax
                .Where(t => t.Name.Contains(searchtext) || t.Invoicelabel.Contains(searchtext))
                .OrderBy(c => c.Name)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();

            // Return the list of records
            return Ok(new PagedResult<Tax>(taxes, pageNo, pageSize, total));

        }
        [HttpGet("{taxid}")]
        public async Task<ActionResult<Tax>> Get(int taxid)
        {
            var target = await _context.Tax.SingleOrDefaultAsync(obj => obj.Taxid == taxid);
            if (target == null)
            {
                return NotFound();
            }
            return target;
        }

        [HttpPost]
        public async Task<ActionResult<Tax>> Post([FromBody] Tax obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (obj.Type.ToLower() != "flat" && obj.Type.ToLower() != "percentage")
            {
                return BadRequest("Invalid tax type");
            }
            else
            {
                _context.Tax.Add(obj);
                await _context.SaveChangesAsync();
                return Created("api/Tax", obj);
            }
        }

        [HttpPut("{taxid}")]
        public async Task<ActionResult> Put(int taxid, [FromBody] Tax obj)
        {
            var target = await _context.Tax.SingleOrDefaultAsync(nobj => nobj.Taxid == taxid);
            if (target != null && ModelState.IsValid)
            {
                _context.Entry(target).CurrentValues.SetValues(obj);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest();
        }

        [HttpDelete("{taxid}")]
        public async Task<ActionResult> Delete(int taxid)
        {
            var target = await _context.Tax.SingleOrDefaultAsync(obj => obj.Taxid == taxid);
            var ptax = await _context.Product.SingleOrDefaultAsync(p => p.Taxtype==taxid);
            if (ptax != null)
            {
                return BadRequest("Cannot delete tax with product.");
            }
            if (target != null)
            {
                _context.Tax.Remove(target);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return NotFound();
        }
        [Route("[action]/{taxid}")]
        [HttpGet]
        public async Task<ActionResult<bool>> HasProduct(int taxid)
        {
            var target = await _context.Tax.SingleOrDefaultAsync(obj => obj.Taxid == taxid);
            if (target == null)
            {
                return BadRequest("Invalid taxid.");
            }
            return await _context.Product.SingleOrDefaultAsync(p => p.Taxtype==taxid) != null;
        }
        [Route("[action]/{taxid}")]
        [HttpGet]
        public async Task<ActionResult> TaxProducts(int taxid, int pageNo = 1, int pageSize = 10)
        {
            // return await _context.Product.Where(p => p.Taxtype.Contains(taxid.ToString())).ToArrayAsync();

            // Determine the number of records to skip
            int skip = (pageNo - 1) * pageSize;
            // Select the records based on paging parameters
            var products = await _context.Product
                .Where(p => p.Taxtype==taxid)
                         .OrderBy(p => p.Name)
                         .Skip(skip)
                         .Take(pageSize)
                         .ToListAsync();
            // Get total number of records
            int total = products.Count();
            // Return the list of records
            return Ok(new PagedResult<Product>(products, pageNo, pageSize, total));
        }

        [Route("[action]/{taxid}")]
        [HttpGet]
        public ActionResult TaxProductCount(int taxid)
        {

            return Ok(_context.Product.Where(p => p.Taxtype==taxid).Count());
        }
    }
}