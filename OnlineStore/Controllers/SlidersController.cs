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
using OnlineStore.Models.Sliders;

namespace OnlineStore.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class SlidersController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public SlidersController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Slider>>> Gets()
        {
            return await _context.Sliders.ToListAsync();
        }
        [Authorize(Role.Admin)]

        [HttpGet("{id}")]
        public async Task<ActionResult<Slider>> Get(int id)
        {
            var slider = await _context.Sliders.FindAsync(id);

            if (slider == null)
            {
                return NotFound(new { message = "Khong tim thay slider nay." });
            }

            return Ok(slider);
        }

        [Authorize(Role.Admin)]
        [HttpPut("{id}")]
        public IActionResult Put(int id, SliderRequest model)
        {
            var slider = GetSlider(id);
            if (slider == null)
            {
                return NotFound(new { message = "Khong tim thay slider nay." });
            }
            var name = slider.Name;
            var image = slider.Image;
            if (model.Image == null)
            {
                model.Image = image;
            }
            if (model.Name == null)
            {
                model.Name = name;
            }
            _mapper.Map(model, slider);
            _context.Sliders.Update(slider);
            _context.SaveChanges();
            return Ok(slider);
        }

        [Authorize(Role.Admin)]
        [HttpPost]
        public async Task<ActionResult<Slider>> Post(SliderRequest model)
        {
            if (model.Image == null || model.Name == null)
            {
                return NotFound(new { message = "Hay nhap day du thong tin." });
            }
            var slider = _mapper.Map<Slider>(model);
            _context.Sliders.Add(slider);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Them slider thanh cong." });
        }

        [Authorize(Role.Admin)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var slider = await _context.Sliders.FindAsync(id);
            if (slider == null)
            {
                return NotFound(new { message = "Khong tin thay id nay." });
            }

            _context.Sliders.Remove(slider);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Xoa thong tin thanh cong." });
        }
        private Slider GetSlider(int id)
        {
            var slider = _context.Sliders.Find(id);
            return slider;
        }
    }
}
