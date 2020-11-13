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

        [HttpPost]
        public IActionResult Create([FromBody]CelestialObject obj)
        {
            _context.CelestialObjects.Add(obj);
            _context.SaveChanges();

            return CreatedAtRoute("GetById", new { id = obj.Id }, obj);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, CelestialObject obj)
        {
            var co = _context.CelestialObjects.Find(id);

            if (co == null) return NotFound();

            co.Name = obj.Name;
            co.OrbitalPeriod = obj.OrbitalPeriod;
            co.OrbitedObjectId = obj.OrbitedObjectId;

            _context.CelestialObjects.Update(co);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject(int id, string name)
        {
            var co = _context.CelestialObjects.Find(id);

            if (co == null) return NotFound();

            co.Name = name;

            _context.CelestialObjects.Update(co);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var cos = _context.CelestialObjects.Where(o => o.OrbitedObjectId == id || o.Id == id);

            if (cos.Count() == 0) return NotFound();

            _context.CelestialObjects.RemoveRange(cos);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
