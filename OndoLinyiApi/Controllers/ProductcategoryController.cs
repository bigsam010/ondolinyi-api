using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OndoLinyiApi.Models;
using System.IO;
namespace OndoLinyiApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductcategoryController : ControllerBase
    {
        ondolinyiContext _context;
        public ProductcategoryController(ondolinyiContext _context)
        {
            this._context = _context;
        }

        [HttpGet]
        public async Task<ActionResult> Get(int pageNo = 1, int pageSize = 10)
        {
            int skip = (pageNo - 1) * pageSize;
            int total = _context.Productcategory.Count();
            var cats = await _context.Productcategory.
                OrderBy(c => c.Name).
                Skip(skip).
                Take(pageSize).
                ToListAsync();
            return Ok(new PagedResult<Productcategory>(cats, pageNo, pageSize, total));
        }
        [Route("[action]/{catid}")]
        [HttpGet]
        public async Task<ActionResult> CategoryProducts(int catid, int pageNo = 1, int pageSize = 10)
        {
            int skip = (pageNo - 1) * pageSize;
            int total = _context.Product.Where(p => p.Category.Contains(catid.ToString())).Count();
            var products = await _context.Product.Where(p => p.Category.Contains(catid.ToString())).
                OrderBy(c => c.Name).
                Skip(skip).
                Take(pageSize).
                ToListAsync();
            return Ok(new PagedResult<Product>(products, pageNo, pageSize, total));
        }

        [Route("[action]/{searchtext}")]
        [HttpGet]
        public async Task<ActionResult> Search(string searchtext, int pageNo = 1, int pageSize = 10)
        {
            int skip = (pageNo - 1) * pageSize;
            int total = _context.Productcategory.Where(p => p.Name.Contains(searchtext)).Count();
            var cats = await _context.Productcategory.Where(p => p.Name.Contains(searchtext)).
                OrderBy(c => c.Name).
                Skip(skip).
                Take(pageSize).
                ToListAsync();
            return Ok(new PagedResult<Productcategory>(cats, pageNo, pageSize, total));
        }

        [HttpGet("{catid}")]
        public async Task<ActionResult<Productcategory>> Get(int catid)
        {
            var target = await _context.Productcategory.SingleOrDefaultAsync(obj => obj.Catid == catid);
            if (target == null)
            {
                return NotFound();
            }
            return target;
        }
        [Route("[action]/{catid}")]
        [HttpGet]
        public async Task<ActionResult> CategoryProductCount(int catid)
        {
            var target = await _context.Productcategory.SingleOrDefaultAsync(obj => obj.Catid == catid);
            if (target == null)
            {
                return NotFound();
            }
            return Ok(_context.Product.Where(p => p.Category.Contains(catid.ToString())).Count());
        }

        [Route("[action]/{catid}")]
        [HttpPost, DisableRequestSizeLimit]
        public async Task<ActionResult> UploadThumbnail(int catid)
        {
            var target = await _context.Productcategory.SingleOrDefaultAsync(obj => obj.Catid == catid);
            if (target == null || Request.Form.Files.Count == 0)
            {
                return BadRequest("Invalid category or no file in the request");
            }
            try
            {
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("Res", "Category");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (file.Length > 0)
                {
                    var fullPath = Path.Combine(pathToSave, catid + ".png");
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [Route("[action]/{catid}")]
        [HttpGet]
        public async Task<ActionResult<string>> GetThumbnail(int catid)
        {
            var target = await _context.Productcategory.SingleOrDefaultAsync(obj => obj.Catid == catid);
            if (target == null)
            {
                return BadRequest("Invalid category");
            }
            var folderName = Path.Combine("Res", "Category");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            var fullPath = Path.Combine(pathToSave, catid + ".png");
            if (!System.IO.File.Exists(fullPath))
            {
                return NotFound();
            }
            return Path.Combine(folderName, catid + ".png"); ;
        }

        [Route("[action]/{catid}")]
        [HttpGet]
        public async Task<ActionResult<byte[]>> GetThumbnailRaw(int catid)
        {
            var target = await _context.Productcategory.SingleOrDefaultAsync(obj => obj.Catid == catid);
            if (target == null)
            {
                return BadRequest("Invalid category");
            }
            var folderName = Path.Combine("Res", "Category");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            var fullPath = Path.Combine(pathToSave, catid + ".png");
            if (!System.IO.File.Exists(fullPath))
            {
                return NotFound();
            }
            return await System.IO.File.ReadAllBytesAsync(Path.Combine(folderName, catid + ".png").ToString());
        }
        [Route("[action]/{catid}")]
        [HttpGet]
        public async Task<ActionResult> HasProduct(int catid)
        {
            var target = await _context.Productcategory.SingleOrDefaultAsync(obj => obj.Catid == catid);
            if (target == null)
            {
                return NotFound();
            }
            return Ok(_context.Product.Where(p => p.Category.Contains(catid.ToString())).Count() > 0);
        }

        [HttpPost]
        public async Task<ActionResult<Productcategory>> Post([FromBody] Productcategory obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            else
            {
                _context.Productcategory.Add(obj);
                await _context.SaveChangesAsync();
                return Created("api/Productcategory", obj);
            }
        }

        [HttpPut("{catid}")]
        public async Task<ActionResult> Put(int catid, [FromBody] Productcategory obj)
        {
            var target = await _context.Productcategory.SingleOrDefaultAsync(nobj => nobj.Catid == catid);
            if (target != null && ModelState.IsValid)
            {
                _context.Entry(target).CurrentValues.SetValues(obj);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest();
        }

        [HttpDelete("{catid}")]
        public async Task<ActionResult> Delete(int catid)
        {
            var target = await _context.Productcategory.SingleOrDefaultAsync(obj => obj.Catid == catid);
            if (target != null)
            {
                if (_context.Product.Where(p => p.Category.Contains(catid.ToString())).Count() > 0)
                {
                    return BadRequest("Can't delete category-contains products");
                }
                _context.Productcategory.Remove(target);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return NotFound();
        }
    }
}