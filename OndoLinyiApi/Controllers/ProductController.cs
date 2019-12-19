using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OndoLinyiApi.Models;
using System.Collections;
using System.IO;
namespace OndoLinyiApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        ondolinyiContext _context;
        public ProductController(ondolinyiContext _context)
        {
            this._context = _context;
        }

        [HttpGet]
        public async Task<ActionResult> Get(int pageNo = 1, int pageSize = 10)
        {
            int skip = (pageNo - 1) * pageSize;
            int total = _context.Product.Count();
            var records = await _context.Product.
                OrderBy(p => p.Name).
                Skip(skip).
                Take(pageSize).
                ToListAsync();
            return Ok(new PagedResult<Product>(records, pageNo, pageSize, total));
        }
        [Route("[action]/{status}")]
        [HttpGet]
        public async Task<ActionResult> GetByStatus(string status, int pageNo = 1, int pageSize = 10)
        {

            int skip = (pageNo - 1) * pageSize;
            int total = _context.Product.Where(p => p.Status == status).Count();
            var records = await _context.Product.Where(p => p.Status == status).
                OrderBy(p => p.Name).
                Skip(skip).
                Take(pageSize).
                ToListAsync();
            return Ok(new PagedResult<Product>(records, pageNo, pageSize, total));
        }

        [HttpGet("{productid}")]
        public async Task<ActionResult<Product>> Get(int productid)
        {
            var target = await _context.Product.SingleOrDefaultAsync(obj => obj.Productid == productid);
            if (target == null)
            {
                return NotFound();
            }
            return target;
        }
        [Route("[action]/{productid}")]
        [HttpGet]
        public async Task<ActionResult> GetPrice(int productid)
        {
            var target = await _context.Product.SingleOrDefaultAsync(obj => obj.Productid == productid);
            if (target == null)
            {
                return BadRequest("Invalid productid");
            }
            if (target.Pricemodel.ToLower() == "range")
            {
                var pm = await _context.Pricerangemodel.SingleOrDefaultAsync(p => p.Productid == productid);
                if (pm == null)
                {
                    return Ok("No price record found for this product");
                }
                return Ok(pm);
            }
            var mpm = await _context.Multiplepricerangemodel.Where(m => m.Productid == productid).ToListAsync();
            if (mpm.Count == 0)
            {
                return Ok("No price record found for this product");
            }
            return Ok(mpm);
        }
        [Route("[action]/{searchtext}")]
        [HttpGet]
        public async Task<ActionResult> Search(string searchtext, int pageNo = 1, int pageSize = 10)
        {
            var man = await _context.Manufacturer.Where(m => m.Companyname.Contains(searchtext)).Select(m => m.Manufacturerid).ToListAsync();
            int skip = (pageNo - 1) * pageSize;
            int total = _context.Product.Where(p => p.Name.Contains(searchtext) || man.Contains(p.Manufacturer)).Count();
            var results = await _context.Product.Where(p => p.Name.Contains(searchtext) || man.Contains(p.Manufacturer)).
                OrderBy(p => p.Name).
                Skip(skip).
                Take(pageSize).
                ToListAsync();

            return Ok(new PagedResult<Product>(results, pageNo, pageSize, total));
        }

        [Route("[action]/{productid}")]
        [HttpGet]
        public async Task<ActionResult<Product[]>> GetRelatedProductByCat(int productid)
        {

            var target = await _context.Product.SingleOrDefaultAsync(p => p.Productid == productid);
            if (target == null)
            {
                return BadRequest("Invalid productid");
            }
            List<Product> rcourses = new List<Product>();
            foreach (Product c in _context.Product)
            {
                if (c == target)
                {
                    continue;
                }
                foreach (string t in target.Category.Split(";"))
                {
                    if (c.Category.Contains(t))
                    {
                        rcourses.Add(c);
                        break;
                    }
                }
            }

            return rcourses.ToArray();
        }
        [Route("[action]/{productid}/{qty}/{addedby}")]
        [HttpPost]
        public async Task<ActionResult> Restock(int productid, int qty, string addedby)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == addedby);
            if (user == null)
            {
                return BadRequest("Unknown user");
            }
            if (user.Status.ToLower() != "active")
            {
                return BadRequest("User account is not active");
            }
            var target = await _context.Product.SingleOrDefaultAsync(obj => obj.Productid == productid);

            if (target == null)
            {
                return BadRequest("Invalid product");
            }

            var sh = new Stockhistory()
            {
                Addedby = addedby,
                Dateadded = DateTime.Now,
                Productid = productid,
                Qtyadded = qty,
                Qtyavailable = target.Qtyavailable

            };
            target.Qtyavailable += qty;
            _context.Stockhistory.Add(sh);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [Route("[action]/{productid}")]
        [HttpGet]
        public async Task<ActionResult<Product[]>> GetRelatedProductByTag(int productid)
        {
            var target = await _context.Product.SingleOrDefaultAsync(p => p.Productid == productid);
            if (target == null)
            {
                return BadRequest("Invalid productid");
            }
            List<Product> rcourses = new List<Product>();
            foreach (Product c in _context.Product)
            {
                if (c == target)
                {
                    continue;
                }
                foreach (string t in target.Tag.Split(";"))
                {
                    if (c.Tag.Contains(t))
                    {
                        rcourses.Add(c);
                        break;
                    }
                }
            }

            return rcourses.ToArray();
        }
        [Route("[action]/{tag}")]
        [HttpGet]
        public async Task<ActionResult> GetProductByTag(string tag, int pageNo = 1, int pageSize = 10)
        {
            int skip = (pageNo - 1) * pageSize;
            int total = _context.Product.Where(p => p.Tag.Contains(tag)).Count();
            var records = await _context.Product.Where(p => p.Tag.Contains(tag)).
                OrderBy(p => p.Name).
                Skip(skip).
                Take(pageSize).
                ToListAsync();
            return Ok(new PagedResult<Product>(records, pageNo, pageSize, total));
        }

        [Route("[action]/{cat}")]
        [HttpGet]
        public async Task<ActionResult> GetProductByCat(int cat, int pageNo = 1, int pageSize = 10)
        {
            int skip = (pageNo - 1) * pageSize;
            int total = _context.Product.Where(p => p.Category.Contains(cat.ToString())).Count();
            var records = await _context.Product.Where(p => p.Category.Contains(cat.ToString())).
                OrderBy(p => p.Name).
                Skip(skip).
                Take(pageSize).
                ToListAsync();
            return Ok(new PagedResult<Product>(records, pageNo, pageSize, total));
        }

        [Route("[action]/{productid}")]
        [HttpGet]
        public async Task<ActionResult> GetProductReview(int productid, int pageNo = 1, int pageSize = 10)
        {
            var target = await _context.Product.SingleOrDefaultAsync(p => p.Productid == productid);

            if (target == null)
            {
                return BadRequest("Invalid product");
            }

            int skip = (pageNo - 1) * pageSize;
            var orderids = await _context.Orderrequest.Where(o => o.Productid == productid).Select(o => o.Orderid).ToListAsync();
            int total = _context.Survey.Where(s => orderids.Contains(s.Orderid)).Count();
            var records = await _context.Survey.Where(s => orderids.Contains(s.Orderid)).
            OrderByDescending(s => s.Surveydate).
                Skip(skip).
                Take(pageSize).
                ToListAsync();
            return Ok(new PagedResult<Survey>(records, pageNo, pageSize, total));
        }

        [Route("[action]/{productid}")]
        [HttpGet]
        public async Task<ActionResult> GetProductReviewCount(int productid)
        {
            var target = await _context.Product.SingleOrDefaultAsync(p => p.Productid == productid);

            if (target == null)
            {
                return BadRequest("Invalid product");
            }

            var orderids = await _context.Orderrequest.Where(o => o.Productid == productid).Select(o => o.Orderid).ToListAsync();
            return Ok(_context.Survey.Where(s => orderids.Contains(s.Orderid)).Count());

        }
        [Route("[action]/{productid}")]
        [HttpGet]
        public async Task<ActionResult> GetProductReviewHiddenCount(int productid)
        {
            var target = await _context.Product.SingleOrDefaultAsync(p => p.Productid == productid);

            if (target == null)
            {
                return BadRequest("Invalid product");
            }

            var orderids = await _context.Orderrequest.Where(o => o.Productid == productid).Select(o => o.Orderid).ToListAsync();
            return Ok(_context.Survey.Where(s => orderids.Contains(s.Orderid) && s.Status == "hidden").Count());

        }
        [Route("[action]/{productid}")]
        [HttpGet]
        public async Task<ActionResult> GetReviewBreakdown(int productid)
        {
            var target = await _context.Product.SingleOrDefaultAsync(p => p.Productid == productid);
            Hashtable breakdown = new Hashtable();


            if (target == null)
            {
                return BadRequest("Invalid product");
            }

            var orderids = await _context.Orderrequest.Where(o => o.Productid == productid).Select(o => o.Orderid).ToListAsync();
            for (int i = 1; i <= 5; i++)
            {
                breakdown[i] = _context.Survey.Where(s => orderids.Contains(s.Orderid) && s.Rating == i).Count();
            }
            return Ok(breakdown);
        }

        [Route("[action]/{productid}")]
        [HttpPost, DisableRequestSizeLimit]
        public async Task<ActionResult> UploadFeaturedImage(int productid)
        {
            var target = await _context.Product.SingleOrDefaultAsync(obj => obj.Productid == productid);
            if (target == null || Request.Form.Files.Count == 0)
            {
                return BadRequest("Invalid product or no file in the request");
            }
            try
            {
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("Res", "Featured");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (file.Length > 0)
                {
                    var fullPath = Path.Combine(pathToSave, productid + ".png");
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

        [Route("[action]/{productid}")]
        [HttpPost, DisableRequestSizeLimit]
        public async Task<ActionResult> UploadCertificate(int productid)
        {
            var target = await _context.Product.SingleOrDefaultAsync(obj => obj.Productid == productid);
            if (target == null || Request.Form.Files.Count == 0)
            {
                return BadRequest("Invalid product or no file in the request");
            }
            try
            {
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("Res", "Certificate");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (file.Length > 0)
                {
                    var fullPath = Path.Combine(pathToSave, productid + ".png");
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
        private string GenImgToken()
        {
            string id = "";
            do
            {
                var gen = new Random();
                for (int i = 1; i <= 30; i++)
                {
                    id += gen.Next(0, 9);
                }
            }
            while (_context.Productimages.SingleOrDefault(pi => pi.Imgname == id) != null);
            return id;
        }
        [Route("[action]")]
        [HttpPost]
        public async Task<ActionResult> AddReview([FromBody]Survey obj)
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
        [Route("[action]/{productid}/{type}")]
        [HttpPost, DisableRequestSizeLimit]
        public async Task<ActionResult> AddFile(int productid, string type)
        {


            if (type.ToLower() != "image" && type.ToLower() != "video")
            {
                return BadRequest("Invalid file type");
            }
            var target = await _context.Product.SingleOrDefaultAsync(obj => obj.Productid == productid);
            if (target == null || Request.Form.Files.Count == 0)
            {
                return BadRequest("Invalid product or no file in the request");
            }
            var limitchk = _context.Productimages.Where(pi => pi.Productid == productid).Count();
            if (limitchk == 10)
            {
                return BadRequest("File limit reached");
            }
            try
            {
                var proImg = new Productimages()
                {
                    Productid = productid,
                    Imgname = GenImgToken(),
                    Type = type
                };
                _context.Productimages.Add(proImg);
                string ext = ".png";
                if (type.ToLower() == "video")
                {
                    ext = ".mp4";
                }
                await _context.SaveChangesAsync();
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("Res", "Others");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (file.Length > 0)
                {
                    var fullPath = Path.Combine(pathToSave, proImg.Imgname + ext);
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
        [Route("[action]/{productid}")]
        [HttpGet]
        public async Task<ActionResult> GetFiles(int productid)
        {
            return Ok(await _context.Productimages.Where(pi => pi.Productid == productid).ToListAsync());
        }

        [Route("[action]/{productid}")]
        [HttpGet]
        public async Task<ActionResult<string>> GetFeaturedImage(int productid)
        {
            var target = await _context.Product.SingleOrDefaultAsync(obj => obj.Productid == productid);
            if (target == null)
            {
                return BadRequest("Invalid product");
            }
            var folderName = Path.Combine("Res", "Featured");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            var fullPath = Path.Combine(pathToSave, productid + ".png");
            if (!System.IO.File.Exists(fullPath))
            {
                return NotFound();
            }
            return Path.Combine(folderName, productid + ".png"); ;
        }

        [Route("[action]/{fileid}")]
        [HttpGet]
        public async Task<ActionResult> GetFile(int fileid)
        {
            var target = await _context.Productimages.SingleOrDefaultAsync(obj => obj.Id == fileid);
            if (target == null)
            {
                return BadRequest("Invalid fileid");
            }
            var folderName = Path.Combine("Res", "Others");
            string ext = ".png";
            if (target.Type.ToLower() == "video")
            {
                ext = ".mp4";
            }
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            var fullPath = Path.Combine(pathToSave, target.Imgname + ext);
            if (!System.IO.File.Exists(fullPath))
            {
                return NotFound();
            }
            return Ok(Path.Combine(folderName, target.Imgname + ext));
        }

        [Route("[action]/{productid}")]
        [HttpGet]
        public async Task<ActionResult<byte[]>> GetFeaturedImageRaw(int productid)
        {
            var target = await _context.Product.SingleOrDefaultAsync(obj => obj.Productid == productid);
            if (target == null)
            {
                return BadRequest("Invalid product");
            }
            var folderName = Path.Combine("Res", "Featured");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            var fullPath = Path.Combine(pathToSave, productid + ".png");
            if (!System.IO.File.Exists(fullPath))
            {
                return NotFound();
            }
            return await System.IO.File.ReadAllBytesAsync(Path.Combine(folderName, productid + ".png").ToString());
        }

        [Route("[action]/{fileid}")]
        [HttpDelete]
        public async Task<ActionResult> RemoveFile(int fileid)
        {
            var target = await _context.Productimages.SingleOrDefaultAsync(pi => pi.Id == fileid);
            if (target == null)
            {
                return BadRequest("Invalid fileid");
            }
            string ext = ".png";
            if (target.Type.ToLower() == "video")
            {
                ext = ".mp4";
            }
            var folderName = Path.Combine("Res", "Others");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            var fullPath = Path.Combine(pathToSave, target.Imgname + ext);
            if (!System.IO.File.Exists(fullPath))
            {
                return NotFound();
            }
            System.IO.File.Delete(Path.Combine(folderName, target.Imgname + ext).ToString());
            _context.Productimages.Remove(target);
            await _context.SaveChangesAsync();
            return Ok();
        }
        class CustomerProduct
        {
            public string Customer { get; set; }
            public int Orders { get; set; }
            public decimal Total { get; set; }
            public DateTime Joindate { get; set; }
        }
        [Route("[action]/{productid}")]
        [HttpGet]
        public async Task<ActionResult> GetCustomers(int productid, DateTime start, DateTime end, int pageNo = 1, int pageSize = 10)
        {
            if (start == new DateTime(1, 1, 1, 0, 0, 0))
            {
                start = DateTime.Now.AddDays(-30);
            }
            if (end == new DateTime(1, 1, 1, 0, 0, 0))
            {
                end = DateTime.Now;
            }
            List<CustomerProduct> results = new List<CustomerProduct>();
            var customers = _context.Rfq.Where(r => r.Productid == productid).Select(r => r.Customer);

            foreach (string cus in customers)
            {
                var cusOrder = _context.Rfq.Where(rf => rf.Customer == cus).Select(rf => rf.Rfqid);
                var payLogs = _context.Paymentlog.Where(pl => cusOrder.Contains(pl.Rfqid) && pl.Status == "accepted" && (pl.Paymentdate >= start && pl.Paymentdate <= end));
                var cp = new CustomerProduct()
                {
                    Customer = cus,
                    Orders = payLogs.Count(),
                    Total = payLogs.Sum(pl => pl.Totalamount),

                };
                var tc = await _context.Customer.SingleOrDefaultAsync(c => c.Email == cus);
                cp.Joindate = tc.Joindate;
                results.Add(cp);
            }
            int skip = (pageNo - 1) * pageSize;
            var records = results.OrderByDescending(pc => pc.Orders).Skip(skip).Take(pageSize).ToList();
            return Ok(new PagedResult<CustomerProduct>(records, pageNo, pageSize, results.Count));
        }

        [Route("[action]/{productid}")]
        [HttpGet]
        public ActionResult GetCustomerCount(int productid, DateTime start, DateTime end)
        {
            if (start == new DateTime(1, 1, 1, 0, 0, 0))
            {
                start = DateTime.Now.AddDays(-30);
            }
            if (end == new DateTime(1, 1, 1, 0, 0, 0))
            {
                end = DateTime.Now;
            }
            var customers = _context.Rfq.Where(r => r.Productid == productid).Select(r => r.Customer);
            return Ok(customers.Count());
        }

        class ProductSales
        {
            public string Orderid { set; get; }
            public decimal Unitprice { set; get; }
            public int Quantity { set; get; }
            public decimal Total { set; get; }
            public DateTime Date { set; get; }
        }
        [Route("[action]/{productid}")]
        [HttpGet]
        public ActionResult GetSales(int productid, DateTime start, DateTime end, int pageNo = 1, int pageSize = 10)
        {
            if (start == new DateTime(1, 1, 1, 0, 0, 0))
            {
                start = DateTime.Now.AddDays(-30);
            }
            if (end == new DateTime(1, 1, 1, 0, 0, 0))
            {
                end = DateTime.Now;
            }

            var productSales = _context.Orderrequest.Where(ps => ps.Productid == productid && ps.Status == "success" && (ps.Deliverydate >= start && ps.Deliverydate <= end)).Select(ps => ps.Rfqid);
            var orders = _context.Reqinvoice.Where(po => productSales.Contains(po.Rfqid));
            List<ProductSales> results = new List<ProductSales>();
            foreach (Reqinvoice ri in orders)
            {
                var order = _context.Orderrequest.SingleOrDefault(o => o.Rfqid == ri.Rfqid);
                var ps = new ProductSales()
                {
                    Orderid = order.Orderid,
                    Unitprice = ri.Productprice,
                    Quantity = ri.Quantity,
                    Total = ri.Totalamount,
                    Date = order.Deliverydate
                };
            }
            int skip = (pageNo - 1) * pageSize;
            var records = results.OrderByDescending(ps => ps.Date).Skip(skip).Take(pageSize).ToList();
            return Ok(new PagedResult<ProductSales>(records, pageNo, pageSize, results.Count));

        }



        [Route("[action]/{fileid}")]
        [HttpGet]
        public async Task<ActionResult<byte[]>> GetFileRaw(int fileid)
        {
            var target = await _context.Productimages.SingleOrDefaultAsync(obj => obj.Id == fileid);
            if (target == null)
            {
                return BadRequest("Invalid fileid");
            }
            string ext = ".png";
            if (target.Type.ToLower() == "video")
            {
                ext = ".mp4";
            }
            var folderName = Path.Combine("Res", "Others");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            var fullPath = Path.Combine(pathToSave, target.Imgname + ext);
            if (!System.IO.File.Exists(fullPath))
            {
                return NotFound();
            }
            return await System.IO.File.ReadAllBytesAsync(Path.Combine(folderName, target.Imgname + ext).ToString());
        }

        [Route("[action]/{productid}")]
        [HttpGet]
        public async Task<ActionResult<byte[]>> GetCertificateRaw(int productid)
        {
            var target = await _context.Product.SingleOrDefaultAsync(obj => obj.Productid == productid);
            if (target == null)
            {
                return BadRequest("Invalid product");
            }
            var folderName = Path.Combine("Res", "Certificate");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            var fullPath = Path.Combine(pathToSave, productid + ".png");
            if (!System.IO.File.Exists(fullPath))
            {
                return NotFound();
            }
            return await System.IO.File.ReadAllBytesAsync(Path.Combine(folderName, productid + ".png").ToString());
        }

        [Route("[action]/{productid}")]
        [HttpGet]
        public async Task<ActionResult<string>> GetCertificate(int productid)
        {
            var target = await _context.Product.SingleOrDefaultAsync(obj => obj.Productid == productid);
            if (target == null)
            {
                return BadRequest("Invalid product");
            }
            var folderName = Path.Combine("Res", "Certificate");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            var fullPath = Path.Combine(pathToSave, productid + ".png");
            if (!System.IO.File.Exists(fullPath))
            {
                return NotFound();
            }
            return Path.Combine(folderName, productid + ".png"); ;
        }

        [Route("[action]/{productid}/{status}")]
        [HttpGet]
        public async Task<ActionResult> GetProductReviewByStatus(int productid, string status, int pageNo = 1, int pageSize = 10)
        {
            if (status.ToLower() != "visible" && status.ToLower() != "hidden")
            {
                return BadRequest("Invalid status");
            }

            var target = await _context.Product.SingleOrDefaultAsync(p => p.Productid == productid);

            if (target == null)
            {
                return BadRequest("Invalid product");
            }

            int skip = (pageNo - 1) * pageSize;
            var orderids = await _context.Orderrequest.Where(o => o.Productid == productid).Select(o => o.Orderid).ToListAsync();
            int total = _context.Survey.Where(s => orderids.Contains(s.Orderid) && s.Status == status).Count();
            var records = await _context.Survey.Where(s => orderids.Contains(s.Orderid) && s.Status == status).
            OrderByDescending(s => s.Surveydate).
                Skip(skip).
                Take(pageSize).
                ToListAsync();
            return Ok(new PagedResult<Survey>(records, pageNo, pageSize, total));
        }

        [Route("[action]/{productid}")]
        [HttpGet]
        public async Task<ActionResult> GetAverageReview(int productid)
        {
            var target = await _context.Product.SingleOrDefaultAsync(p => p.Productid == productid);

            if (target == null)
            {
                return BadRequest("Invalid product");
            }
            var orderids = await _context.Orderrequest.Where(o => o.Productid == productid).Select(o => o.Orderid).ToListAsync();
            var surveys = _context.Survey.Where(s => orderids.Contains(s.Orderid));
            if (surveys.Count() == 0)
            {
                return Ok(0);
            }
            return Ok(surveys.AverageAsync(s => s.Rating));
        }

        [Route("[action]/{productid}")]
        [HttpPost]
        public async Task<ActionResult> AddRangePrice(int productid, [FromBody] Pricerangemodel model)
        {
            var pro = await _context.Product.SingleOrDefaultAsync(p => p.Productid == productid);
            if (pro == null)
            {
                return BadRequest("Invalid productid");
            }
            if (pro.Pricemodel.ToLower() != "range")
            {
                return BadRequest("Price model mismatch");
            }
            var pm = await _context.Pricerangemodel.SingleOrDefaultAsync(p => p.Productid == productid);
            if (pm != null)
            {
                return BadRequest("There already exist a price value for this product");
            }
            _context.Pricerangemodel.Add(model);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [Route("[action]/{productid}")]
        [HttpPost]
        public async Task<ActionResult> AddMultipleRangePrice(int productid, [FromBody] Multiplepricerangemodel model)
        {
            var pro = await _context.Product.SingleOrDefaultAsync(p => p.Productid == productid);
            if (pro == null)
            {
                return BadRequest("Invalid productid");
            }
            if (pro.Pricemodel.ToLower() == "range")
            {
                return BadRequest("Price model mismatch");
            }
            if (model.Upperbound < model.Lowerbound)
            {
                return BadRequest("Upperbound must be greater than lowerbound");
            }
            var prices = await _context.Multiplepricerangemodel.Where(p => p.Productid == productid).ToListAsync();
            foreach (var p in prices)
            {
                if (model.Upperbound >= p.Lowerbound && model.Upperbound <= p.Upperbound)
                {
                    return BadRequest("Range already defined");
                }
            }
            _context.Multiplepricerangemodel.Add(model);
            await _context.SaveChangesAsync();
            return Ok();
        }


        [Route("[action]/{productid}")]
        [HttpPost]
        public async Task<ActionResult> ChangeRangePrice(int productid, [FromBody] Pricerangemodel model)
        {
            var pro = await _context.Product.SingleOrDefaultAsync(p => p.Productid == productid);
            if (pro == null)
            {
                return BadRequest("Invalid productid");
            }
            var pm = await _context.Pricerangemodel.SingleOrDefaultAsync(p => p.Productid == productid);
            if (pm == null)
            {
                return BadRequest("No price record found for this product");
            }
            _context.Entry(pm).CurrentValues.SetValues(model);
            await _context.SaveChangesAsync();
            return Ok();
        }
        [Route("[action]/{productid}/{lowerbound}/{upperbound}")]
        [HttpDelete]
        public async Task<ActionResult> RemovePriceRange(int productid, int lowerbound, int upperbound)
        {
            var pro = await _context.Product.SingleOrDefaultAsync(p => p.Productid == productid);
            if (pro == null)
            {
                return BadRequest("Invalid productid");
            }
            var pm = await _context.Multiplepricerangemodel.SingleOrDefaultAsync(p => p.Productid == productid && p.Lowerbound == lowerbound && p.Upperbound == upperbound);
            if (pm == null)
            {
                return BadRequest("Matching entry not found");
            }
            _context.Remove(pm);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult<Product>> Post([FromBody] Product obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                List<string> vStatus = new List<string>
                {
                    "draft",
                    "live",
                    "hidden",
                    "archived"
                };
                List<string> pModel = new List<string> {
                    "multiple range",
                    "range"
                };
                if (!vStatus.Contains(obj.Status.ToLower()))
                {
                    return BadRequest("Invalid product status");
                }
                if (!pModel.Contains(obj.Pricemodel))
                {
                    return BadRequest("Invalid model price");
                }
                else
                {
                    _context.Product.Add(obj);
                    await _context.SaveChangesAsync();
                    return Created("api/Product", obj);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPut("{productid}")]
        public async Task<ActionResult> Put(int productid, [FromBody] Product obj)
        {
            var target = await _context.Product.SingleOrDefaultAsync(nobj => nobj.Productid == productid);
            if (target != null && ModelState.IsValid)
            {
                _context.Entry(target).CurrentValues.SetValues(obj);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest();
        }

        [HttpDelete("{productid}")]
        public async Task<ActionResult> Delete(int productid)
        {
            var target = await _context.Product.SingleOrDefaultAsync(obj => obj.Productid == productid);
            if (target != null)
            {
                target.Status = "archived";
                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest("Invalid productid");
        }
    }
}