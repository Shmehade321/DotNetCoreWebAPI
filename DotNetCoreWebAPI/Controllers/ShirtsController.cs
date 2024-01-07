using DotNetCoreWebAPI.Attributes;
using DotNetCoreWebAPI.Data;
using DotNetCoreWebAPI.Filters;
using DotNetCoreWebAPI.Filters.AuthFilters;
using DotNetCoreWebAPI.Filters.ExceptionFilters;
using DotNetCoreWebAPI.Models;
using DotNetCoreWebAPI.Models.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DotNetCoreWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [JwtTokenAuthFilter]
    public class ShirtsController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public ShirtsController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        [RequiredClaim("read", "true")]
        public IActionResult GetShirts()
        {
            return Ok(_db.Shirts.ToList());
        }

        [HttpGet("{id}")]
        [TypeFilter(typeof(ValidateShirtIdFilterAttribute))]
        [RequiredClaim("read", "true")]
        public IActionResult GetShirtById(int id)
        {            
            return Ok(HttpContext.Items["shirt"]);
        }

        [HttpPost]
        [TypeFilter(typeof(ValidateCreateShirtFilterAttribute))]
        [RequiredClaim("write", "true")]
        public IActionResult CreateShirt([FromBody] Shirt shirt)
        {
            _db.Shirts.Add(shirt);
            _db.SaveChanges();

            return CreatedAtAction(nameof(GetShirtById), new { id = shirt.Id }, shirt);
        }

        [HttpPut("{id}")]
        [TypeFilter(typeof(ValidateShirtIdFilterAttribute))]
        [ValidateUpdateShirtFilter]
        [TypeFilter(typeof(HandleUpdateExceptionsFilterAttribute))]
        [RequiredClaim("write", "true")]
        public IActionResult UpdateShirt(int id, Shirt shirt)
        {
            var shirtToUpdate = HttpContext.Items["shirt"] as Shirt;
            shirtToUpdate.Brand = shirt.Brand;
            shirtToUpdate.Price = shirt.Price;
            shirtToUpdate.Size = shirt.Size;
            shirtToUpdate.Color = shirt.Color;
            shirtToUpdate.Gender = shirt.Gender;

            _db.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [TypeFilter(typeof(ValidateShirtIdFilterAttribute))]
        [RequiredClaim("delete", "true")]
        public IActionResult DeleteShirt(int id)
        {
            var shirtToDelete = HttpContext.Items["shirt"] as Shirt;

            _db.Shirts.Remove(shirtToDelete);
            _db.SaveChanges();

            return Ok(shirtToDelete);
        }
    }
}
