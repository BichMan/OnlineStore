﻿using System;
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
        [AllowAnonymous]
        [HttpGet]
        public IEnumerable<Product> Gets()
        {
            return _context.Products
                .Include(x => x.productAttributes).Include(x => x.productBrand)
                .AsEnumerable();
        }
        [Authorize(Role.Admin)]
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> Get(int id)
        {
            var product = await _context.Products.FindAsync(id);
            var productBrand = GetBrand(product.BrandId);
            var productAttribute = GetAttribute(product.AttributesId);
            if (product == null)
            {
                return NotFound(new { message = "Không tìm thấy sản phẩm này." });
            }
            product.productAttributes = productAttribute;
            product.productBrand = productBrand;
            return Ok(product);
        }
        
        [Authorize(Role.Admin)]
        [HttpGet("ProductsByBrand/{brandid}")]
        public ActionResult<IEnumerable<Product>> GetByBrand(int brandid)
        {
            var productBrand = GetBrand(brandid);

           
            if (productBrand == null)
            {
                return NotFound(new { message = "Không tìm thấy id này." });
            }
            var products = _context.Products.Where(x=>x.productBrand.Id == brandid)
                .Include(x => x.productAttributes).Include(x => x.productBrand).AsEnumerable();
           
            return Ok(products);
        }
        
        [Authorize(Role.Admin)]
        [HttpGet("ProductsByAttribute/{attributeId}")]
        public ActionResult<IEnumerable<Product>> GetByAttribute(int attributeId)
        {

            var productAttribute = GetAttribute(attributeId);

            if (productAttribute == null)
            {
                return NotFound(new { message = "Không tìm thấy id này." });
            }
            var products = _context.Products.Where(x=>x.productAttributes.Id == attributeId)
                .Include(x => x.productAttributes).Include(x => x.productBrand).AsEnumerable();
           
            return Ok(products);
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
            var BrandId = product.BrandId;
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
            if (model.BrandId == 0)
            {
                model.BrandId = BrandId;
            }
            if (model.AttributesId == 0)
            {
                model.AttributesId = attributeId;
            }
            var productBrand = GetBrand(model.BrandId);
            var productAttribute = GetAttribute(model.AttributesId);
            if (productBrand == null)
            {
                return NotFound(new { message = "Không tìm thấy danh mục sản phẩm này." });
            }
            if (productAttribute == null)
            {
                return NotFound(new { message = "Không tìm thấy màu sắc này." });
            }

            _mapper.Map(model, product);
            product.productAttributes = productAttribute;
            product.productBrand = productBrand;
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
            var productBrand = GetBrand(model.BrandId);
            if (productBrand == null)
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
            product.productBrand = productBrand;
            product.BrandId = productBrand.Id;
            product.AttributesId = productAttribute.Id;
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

        private ProductBrand GetBrand(int id)
        {
            var productBrand = _context.ProductBrands.Find(id);
            return productBrand;
        }
        private ProductAttribute GetAttribute(int id)
        {
            var productAttribute = _context.ProductAttributes.Find(id);
            return productAttribute;
        }
    }
}
