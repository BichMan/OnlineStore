using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Authorization;
using OnlineStore.Entities;
using OnlineStore.Helpers;
using OnlineStore.Models.Products;

namespace OnlineStore.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public ProductsController(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        [HttpGet]
        public IEnumerable<Product> Gets()
        {
            return _context.Products
                .Include(x => x.productAttributes).Include(x => x.productCategory)
                .AsEnumerable();
        }
        [Authorize(Role.Admin)]
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> Get(int id)
        {
            var product = await _context.Products.FindAsync(id);
            var productCategory = GetCategory(product.CategoryId);
            var productAttribute = GetAttribute(product.AttributesId);
            if (product == null)
            {
                return NotFound(new { message = "Không tìm thấy sản phẩm này." });
            }
            product.productAttributes = productAttribute;
            product.productCategory = productCategory;
            return Ok(product);
        }

        [Authorize(Role.Admin)]
        [HttpPut("{id}")]
        public ActionResult<Product> Put(int id, ProductRequest model)
        {
            var product = GetProduct(id);
            if (product == null)
            {
                return NotFound(new { message = "Không tìm thấy sản phẩm này." });

            }
            var name = product.Name;
            var image = product.Image;
            var price = product.Price;
            var description = product.Description;
            var content = product.Content;
            var categoryId = product.CategoryId;
            var attributeId = product.AttributesId;

            if (model.Name == null)
            {
                model.Name = name;
            }
            if (model.Image == null)
            {
                model.Image = image;
            }
            if (model.Price == 0)
            {
                model.Price = price;
            }
            if (model.Description == null)
            {
                model.Description = description;
            }
            if (model.Content == null)
            {
                model.Content = content;
            }
            if (model.CategoryId == 0)
            {
                model.CategoryId = categoryId;
            }
            if (model.AttributesId == 0)
            {
                model.AttributesId = attributeId;
            }
            var productCategory = GetCategory(model.CategoryId);
            var productAttribute = GetAttribute(model.AttributesId);
            if (productCategory == null)
            {
                return NotFound(new { message = "Không tìm thấy danh mục sản phẩm này." });
            }
            if (productAttribute == null)
            {
                return NotFound(new { message = "Không tìm thấy màu sắc này." });
            }

            _mapper.Map(model, product);
            product.productAttributes = productAttribute;
            product.productCategory = productCategory;
            _context.Products.Update(product);
            _context.SaveChanges();
            return Ok(product);
        }

        [Authorize(Role.Admin)]
        [HttpPost]
        public async Task<ActionResult<Product>> Post(ProductRequest model)
        {
            if ((model.Name == null) || (model.Image == null) || (model.Description == null) || (model.Content == null))
            {
                return NotFound(new { message = "Vui lòng điền đầy đủ thông tin." });
            }
            if (model.Price == 0)
            {
                return NotFound(new { message = "Vui lòng nhập số tiền." });
            }
            var productCategory = GetCategory(model.CategoryId);
            if (productCategory == null)
            {
                return NotFound(new { message = "Không tìm thấy danh mục sản phẩm này." });
            }

            var productAttribute = GetAttribute(model.AttributesId);
            if (productAttribute == null)
            {
                return NotFound(new { message = "Không tìm thấy màu sắc này." });
            }

            var product = _mapper.Map<Product>(model);
            product.productAttributes = productAttribute;
            product.productCategory = productCategory;
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return Ok(product);
        }


        [Authorize(Role.Admin)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound(new { message = "Không tìm thấy Id này." });
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Xóa thông tin thành công." });

        }

        private Product GetProduct(int id)
        {
            var product = _context.Products.Find(id);
            return product;
        }

        private ProductCategory GetCategory(int id)
        {
            var productCategory = _context.ProductCategories.Find(id);
            return productCategory;
        }
        private ProductAttribute GetAttribute(int id)
        {
            var productAttribute = _context.ProductAttributes.Find(id);
            return productAttribute;
        }
    }
}
