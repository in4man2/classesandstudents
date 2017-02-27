using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using ClassesAndStudents.Models;

namespace ClassesAndStudents.Controllers
{
  //  [RoutePrefix("api/ClassEntities")]
    public class ClassEntitiesController : ApiController
    {
        private EFModel1 db = new EFModel1();

        // GET: api/ClassEntities
        [Route("api/classentities")]
        [HttpGet]
        public IQueryable<ClassEntity> GetClassEntities()
        {
            return db.ClassEntities;
        }

        // GET: api/ClassEntities/5
        [HttpGet]
        [Route("api/classentities/{id}")]
        [ResponseType(typeof(ClassEntity))]
        public async Task<IHttpActionResult> GetClassEntity(Guid id)
        {
            ClassEntity classEntity = await db.ClassEntities.FindAsync(id);
            if (classEntity == null)
            {
                return NotFound();
            }

            return Ok(classEntity);
        }

        // PUT: api/ClassEntities/5
        [HttpPut]
        [Route("api/classentities/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutClassEntity(Guid id, ClassEntity classEntity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            classEntity.ID = id;
            
            db.Entry(classEntity).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClassEntityExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/ClassEntities
        [HttpPost]
        [Route("api/classentities")]
        [ResponseType(typeof(ClassEntity))]
        public async Task<IHttpActionResult> PostClassEntity(ClassEntity classEntity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ClassEntities.Add(classEntity);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ClassEntityExists(classEntity.ID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Ok(classEntity.ID); //CreatedAtRoute("DefaultApi", new { id = classEntity.ID }, classEntity);
        }

        // DELETE: api/ClassEntities/5
        [HttpDelete]
        [Route("api/classentities/{id}")]
        [ResponseType(typeof(ClassEntity))]
        public async Task<IHttpActionResult> DeleteClassEntity(Guid id)
        {
            ClassEntity classEntity = await db.ClassEntities.FindAsync(id);
            if (classEntity == null)
            {
                return NotFound();
            }

            db.ClassEntities.Remove(classEntity);
            await db.SaveChangesAsync();

            return Ok(classEntity);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ClassEntityExists(Guid id)
        {
            return db.ClassEntities.Count(e => e.ID == id) > 0;
        }
    }
}