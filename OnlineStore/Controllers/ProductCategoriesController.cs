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
    public class ProductCategoriesController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;


        public ProductCategoriesController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductCategory>>> Gets()
        {
            return await _context.ProductCategories.ToListAsync();
        }
        [Authorize(Role.Admin)]
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductCategory>> Get(int id)
        {
            var productCategory = await _context.ProductCategories.FindAsync(id);

            if (productCategory == null)
            {
                return NotFound(new { message = "Không tìm thấy danh mục này." });
            }

            return Ok(productCategory);
        }

        [Authorize(Role.Admin)]
        [HttpPut("{id}")]
        public ActionResult<ProductCategory> Put(int id, ProductCategoryRequest model)
        {
            var productCategory = GetCategory(id);
            if (productCategory == null)
            {
                return NotFound(new { message = "Không tìm thấy danh mục này." });
            }
            var name = productCategory.Name;
            var description = productCategory.Description;
            if (model.Name == null)
            {
                model.Name = name;
            }
            if (model.Description == null)
            {
                model.Description = description;
            }
            _mapper.Map(model, productCategory);

            _context.ProductCategories.Update(productCategory);
            _context.SaveChanges();
            return Ok(productCategory);
        }

        [Authorize(Role.Admin)]
        [HttpPost]
        public async Task<ActionResult<ProductCategory>> Post(ProductCategoryRequest model)
        {
            if ((model.Name == null) || (model.Description == null))
            {
                return NotFound(new { message = "Vui lòng điền đầy đủ thông tin." });
            }
            var productCategory = _mapper.Map<ProductCategory>(model);
            _context.ProductCategories.Add(productCategory);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Thêm danh mục sản phẩm thành công." });
        }

        [Authorize(Role.Admin)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var productCategory = await _context.ProductCategories.FindAsync(id);
                if (productCategory == null)
                {
                    return NotFound(new { message = "Không tìm thấy Id này." });
                }

                _context.ProductCategories.Remove(productCategory);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Xóa thông tin thành công." });
            }
            catch (Exception)
            {
                return NotFound(new { message = "Lỗi liên kết khóa ngoại không thể xóa được." });
            }

        }

        private ProductCategory GetCategory(int id)
        {
            var productCategory = _context.ProductCategories.Find(id);
            return productCategory;
        }
    }
}
