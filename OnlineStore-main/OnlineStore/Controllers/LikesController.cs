using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Authorization;
using OnlineStore.Entities;
using OnlineStore.Helpers;
using OnlineStore.Models.Likes;

namespace OnlineStore.Controllers
{
    [Authorize(Role.User)]
    [Route("[controller]")]
    [ApiController]
    public class LikesController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public LikesController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<LikeResponse>> Get()
        {
            var currentUser = (User)HttpContext.Items["User"];
            var UserId = currentUser.Id;
            var user = GetUser(UserId);
            if (user == null)
            {
                return NotFound(new { message = "Khong tim thay User nay." });
            }
            var like = _context.Likes.Where(x => x.UserId == user.Id).ToArray();
            var alllike = new List<LikeResponse>();
            foreach (var item in like)
            {
                var product = GetProduct(item.ProductId);
                item.product = product;
                var response = _mapper.Map<LikeResponse>(item);
                response.Id = item.Id;
                response.Image = item.product.Image;
                response.Name = item.product.Name;
                response.Description = item.product.Description;
                response.Content = item.product.Content;
                response.Price = item.product.Price;
                alllike.Add(response);
            }


            return Ok(alllike);
        }


        [HttpPost]
        public ActionResult<LikeResponse> Post(LikeRequest model)
        {

            if (model.ProductId == 0)
            {
                return NotFound(new { message = "Nhap du thong tin." });

            }
            var currentUser = (User)HttpContext.Items["User"];
            var userId = currentUser.Id;
            var user = GetUser(userId);
            if (user == null)
            {
                return NotFound(new { message = "Khong tim thay User nay." });
            }
            var product = GetProduct(model.ProductId);
            if (product == null)
            {
                return NotFound(new { message = "Khong tim thay Product nay." });
            }
            if (_context.Likes.Any(x=>x.ProductId == model.ProductId))
            {
                return NotFound(new { message = "Sản phẩm đã tồn tại trong danh sách yêu thích." });
            }

            var like = _mapper.Map<Like>(model);
            like.ProductId = model.ProductId;
            like.UserId = userId;
            like.product = product;
            like.user = user;
            _context.Likes.Add(like);
            _context.SaveChanges();
            return Ok(new { message = "Them san pham yeu thich thanh cong." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var like = await _context.Likes.FindAsync(id);
            if (like == null)
            {
                return NotFound(new { message = "Id Not Found." });
            }

            _context.Likes.Remove(like);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Delete successfully." });
        }

        private User GetUser(int id)
        {
            var user = _context.Users.Find(id);
            return user;
        }
        private Product GetProduct(int id)
        {
            var product = _context.Products.Find(id);
            return product;
        }
        private Like GetLike(int id)
        {
            var like = _context.Likes.Find(id);
            return like;
        }
    }
}
