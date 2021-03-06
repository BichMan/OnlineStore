using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Authorization;
using OnlineStore.Entities;
using OnlineStore.Helpers;
using OnlineStore.Models;
using OnlineStore.Models.Carts;
using OnlineStore.Models.Products;

namespace OnlineStore.Controllers
{
    [Authorize(Role.User)]
    [Route("[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public CartsController(DataContext context, IMapper mapper)
        {
            _context = context;

            _mapper = mapper;
        }

        //[Authorize(Role.Admin)]
        //[HttpGet]
        //public ActionResult<IEnumerable<CartResponse>> Gets()
        //{
        //    var allcart = new List<CartResponse>();
        //    var carts = _context.Carts.ToArray();
        //    foreach (var item in carts)
        //    {
        //        var product = GetProduct(item.productId);
        //        item.product = product;
        //        var response = _mapper.Map<CartResponse>(item);
        //        response.Id = item.Id;
        //        response.UserId = item.userId;
        //        response.ProductId = item.productId;
        //        response.Quantity = item.Quantity;
        //        response.Price = item.product.Price;
        //        response.Image = item.product.Image;
        //        response.ProductName = item.product.Name;
        //        response.Content = item.product.Content;

        //        allcart.Add(response);
        //    }
        //    for (int i = 0; i < allcart.Count; i++)
        //    {
        //        for (int j = i + 1; j < allcart.Count; j++)
        //        {
        //            if (allcart[i].ProductName == allcart[j].ProductName)
        //            {
        //                allcart[i].Quantity = allcart[i].Quantity + allcart[j].Quantity;
        //                allcart.RemoveAt(j);

        //            }
        //        }
        //        allcart[i].Total = allcart[i].Price * allcart[i].Quantity;
        //    }
        //    return Ok(allcart);
        //}

        [HttpGet("{userid}")]
        public ActionResult<IEnumerable<CartResponse>> Get(int userid)
        {
            var currentUser = (User)HttpContext.Items["User"];
            if (userid != currentUser.Id)
                return Unauthorized(new { message = "Unauthorized. Enter your Id." });
            var user = GetUser(userid);
            var allcart = new List<CartResponse>();
            var cart = _context.Carts.Where(p => p.userId == userid).ToArray();
            if (user == null)
            {
                return NotFound(new { message = "Khong tim thay user nay." });
            }

            foreach (var item in cart)
            {
                var product = GetProduct(item.productId);
                var response = _mapper.Map<CartResponse>(item);
                response.Id = item.Id;
                response.UserId = item.userId;
                response.ProductId = item.productId;
                response.Quantity = item.Quantity;
                response.Price = item.product.Price;
                response.Image = item.product.Image;
                response.ProductName = item.product.Name;
                response.Content = item.product.Content;

                allcart.Add(response);
            }
            for (int i = 0; i < allcart.Count; i++)
            {
                for (int j = i + 1; j < allcart.Count; j++)
                {
                    if (allcart[i].ProductName == allcart[j].ProductName)
                    {
                        allcart[i].Quantity = allcart[i].Quantity + allcart[j].Quantity;
                        allcart.RemoveAt(j);

                    }
                }
                allcart[i].Total = allcart[i].Price * allcart[i].Quantity;
            }
            return Ok(allcart);

        }
        [HttpPut("{id}")]
        public ActionResult<CartResponse> Put(int id, CartRequest model)
        {
            var cart = GetCart(id);
            if (cart == null)
            {
                return NotFound(new { message = "Khong tim thay id nay." });
            }
            var productId = cart.productId;
            var userId = cart.userId;
            var quantity = cart.Quantity;

            if (model.Quantity <= 0)
            {
                model.Quantity = quantity;
            }
            if (model.productId == 0)
            {
                model.productId = productId;
            }
            if (model.userId == 0)
            {
                model.userId = userId;
            }
            var product = GetProduct(model.productId);
            var user = GetUser(model.userId);
            if (product == null)
            {
                return NotFound(new { message = "Khong tim thay san pham." });
            }
            if (user == null)
            {
                return NotFound(new { message = "Khong tim thay nguoi dung." });
            }

            var productCategory = GetCategory(product.BrandId);
            var productAttribute = GetAttribute(product.AttributesId);
            product.productAttributes = productAttribute;
            product.productBrand = productCategory;
            _mapper.Map(model, cart);
            cart.Quantity = model.Quantity;
            cart.product = product;
            cart.user = user;
            _context.Carts.Update(cart);
            _context.SaveChanges();
            var response = _mapper.Map<CartResponse>(cart);
            response.Id = cart.Id;
            response.UserId = cart.userId;
            response.ProductId = cart.productId;
            response.Quantity = cart.Quantity;
            response.Price = cart.product.Price;
            response.Total = cart.product.Price * cart.Quantity;
            response.Image = cart.product.Image;
            response.ProductName = cart.product.Name;
            response.Content = cart.product.Content;
            return Ok(response);
        }


        [HttpPost]
        public async Task<ActionResult<CartResponse>> Post(CartRequest model)
        {
            var currentUser = (User)HttpContext.Items["User"];
            if (model.userId != currentUser.Id)
                return Unauthorized(new { message = "Unauthorized. Enter your Id." });
            var product = GetProduct(model.productId);
            var user = GetUser(model.userId);
            if (model.Quantity <= 0)
            {
                model.Quantity = 1;
            }
            if (product == null)
            {
                return NotFound(new { message = "Khong tim thay san pham." });
            }
            if (user == null)
            {
                return NotFound(new { message = "Khong tim thay nguoi dung." });
            }
            var productCategory = GetCategory(product.BrandId);
            var productAttribute = GetAttribute(product.AttributesId);
            product.productAttributes = productAttribute;
            product.productBrand = productCategory;
            var cart = _mapper.Map<Cart>(model);
            cart.product = product;
            cart.user = user;
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();

            var response = _mapper.Map<CartResponse>(cart);
            response.Id = cart.Id;
            response.UserId = cart.userId;
            response.ProductId = cart.productId;
            response.Quantity = cart.Quantity;
            response.Price = cart.product.Price;
            response.Total = cart.product.Price * cart.Quantity;
            response.Image = cart.product.Image;
            response.ProductName = cart.product.Name;
            response.Content = cart.product.Content;
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var cart = await _context.Carts.FindAsync(id);
            if (cart == null)
            {
                return NotFound(new { message = "Không tìm thấy Id này." });
            }

            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Xóa thông tin thành công." });
        }
        private User GetUser(int userId)
        {
            var user = _context.Users.Find(userId);
            return user;
        }

        private Product GetProduct(int productId)
        {
            var product = _context.Products.Find(productId);
            return product;
        }
        private ProductBrand GetCategory(int id)
        {
            var productCategory = _context.ProductBrands.Find(id);
            return productCategory;
        }
        private ProductAttribute GetAttribute(int id)
        {
            var productAttribute = _context.ProductAttributes.Find(id);
            return productAttribute;
        }
        private Cart GetCart(int id)
        {
            var cart = _context.Carts.Find(id);
            return cart;
        }

    }
}
