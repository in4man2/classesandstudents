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
using FluentValidation.Results;

namespace ClassesAndStudents.Controllers
{
    public class StudentEntitiesController : ApiController
    {
        private EFModel1 db = new EFModel1();

        // GET: api/StudentEntities
        [HttpGet]
        [Route("api/StudentEntities/{classId}")]
        public IQueryable<StudentEntityViewModel> GetStudentEntities(Guid classId)
        {
            return db.StudentEntities.Where(m=>m.ClassEntities.Any(n=>n.ID==classId)).ToList().Select(n=>new StudentEntityViewModel() { StudentFullName = n.StudentName+" "+n.StudentSurname, ID = n.ID, Gpa = n.GPA, Dob = n.DOB, Age = Convert.ToInt32((DateTime.Today - n.DOB).TotalDays/365) } ).AsQueryable();
        }

        // GET: api/StudentEntities/5
        [HttpGet]
        [ResponseType(typeof(StudentEntity))]
        [Route("api/StudentEntities/{id}/Get")]
        public async Task<IHttpActionResult> GetStudentEntity(Guid id)
        {
            StudentEntity studentEntity = await db.StudentEntities.FindAsync(id);
            if (studentEntity == null)
            {
                return NotFound();
            }

            return Ok(studentEntity);
        }

        [HttpPut]
        // PUT: api/StudentEntities/5
        [ResponseType(typeof(void))]
        [Route("api/StudentEntities/{id}")]
        public async Task<IHttpActionResult> PutStudentEntity(Guid id, StudentEntity studentEntity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            studentEntity.ID = id;

            db.Entry(studentEntity).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentEntityExists(id))
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

        // POST: api/StudentEntities
        [HttpPost]
        [ResponseType(typeof(StudentEntity))]
        [Route("api/StudentEntities/Create/{classId}")]
        public async Task<IHttpActionResult> PostStudentEntity(Guid classId, StudentEntity studentEntity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            StudentValidator validator = new StudentValidator(classId);
            ValidationResult results = validator.Validate(studentEntity);

            bool validationSucceeded = results.IsValid;
            if (!validationSucceeded)
            {
                return BadRequest("Surname must be unique.");
            }


            db.StudentEntities.Add(studentEntity);

            try
            {
                await db.SaveChangesAsync();
                var classEntity = db.ClassEntities.Find(classId);
                classEntity.StudentEntities.Add(studentEntity);
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (StudentEntityExists(studentEntity.ID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Ok(studentEntity.ID); //CreatedAtRoute("DefaultApi", new { id = studentEntity.ID }, studentEntity);
        }

        // DELETE: api/StudentEntities/5
        [HttpDelete]
        [ResponseType(typeof(StudentEntity))]
        [Route("api/StudentEntities/{id}/{studentId}")]
        public async Task<IHttpActionResult> DeleteStudentEntity(Guid id, Guid studentId)
        {
            StudentEntity studentEntity = await db.StudentEntities.FindAsync(studentId);
            var stInClass = db.ClassEntities.FirstOrDefault(n => n.ID==id);
            db.Entry(stInClass).Collection("StudentEntities").Load();

            stInClass.StudentEntities.Remove(studentEntity);            

            if (studentEntity == null)
            {
                return NotFound();
            }

            //db.StudentEntities.Remove(studentEntity);

            await db.SaveChangesAsync();

            return Ok(studentEntity);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool StudentEntityExists(Guid id)
        {
            return db.StudentEntities.Count(e => e.ID == id) > 0;
        }
    }
}