using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var obj = _context.CelestialObjects.Find(id);

            if (obj == null) return NotFound();

            var satellites = _context.CelestialObjects.Where(o => o.OrbitedObjectId == id);

            obj.Satellites = satellites.ToList();

            return Ok(obj);
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var objects = _context.CelestialObjects.Where(o => o.Name == name);

            if (objects.Count() == 0) return NotFound();

            foreach (var obj in objects)
            {
                var satellites = _context.CelestialObjects.Where(o => o.OrbitedObjectId == obj.Id);
                obj.Satellites = satellites.ToList();
            }

            return Ok(objects);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var objects = _context.CelestialObjects;

            foreach (var obj in objects)
            {
                var satellites = _context.CelestialObjects.Where(o => o.OrbitedObjectId == obj.Id);
                obj.Satellites = satellites.ToList();
            }

            return Ok(objects);
        }
    }
}
