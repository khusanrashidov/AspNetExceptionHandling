using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using School.API.Data;
using School.API.Data.Models;
using School.API.Exceptions;
using School.API.Exceptions.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace School.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public StudentsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("get-all-students")]
        [CustomExceptionFilter] // use it as an attribute
        public IActionResult GetAllStudents()
        {
            //throw new StudentNameException("This is an unhandled exception test for a custom exception filter.");
            //throw new Exception("This is an unhandled exception test for a global exception handler.");

            try
            {
                var allStudents = _context.Students.ToList();
                return Ok(allStudents);
                throw new Exception("Could not get data from the database.");

            }
            catch (Exception ex)
            {
                return BadRequest("Custom meaningful message.");
                return BadRequest(ex.Message);
            }
            finally
            {
                string final = "This finally code will execute both after try or catch statements.";
            }
        }

        [HttpGet("get-student-by-id/{id}")]
        public IActionResult GetStudentById(int id)
        {
            // System.ArgumentException: 'Please, provide an id that is greater than 0.'
            if (id <= 0) throw new ArgumentException("Please, provide an id that is greater than 0.");

            // System.ArgumentOutOfRangeException: 'Index was out of range. Must be non-negative and less than the size of the collection. (Parameter 'index')'
            //var allStudents = _context.Students.ToList();
            //var ninthStudent = allStudents[8];

            // System.IndexOutOfRangeException: 'Index was outside the bounds of the array.'
            //var allStudent = _context.Students.ToArray();
            //var eighthStudent = allStudent[9];

            var studentInfo = _context.Students.FirstOrDefault(n => n.Id == id);
            var studentFullName = studentInfo.FullName;
            return Ok($"Student name = {studentFullName}");
        }

        [HttpPost("add-new-student")]
        public IActionResult AddNewStudent([FromBody] Student payload)
        {
            // Ctrl-K+S to wrap or surround the selected code with some pattern.
            try
            {
                if (Regex.IsMatch(payload.FullName, @"^\d"))
                    throw new StudentNameException("Name starts with number.", payload.FullName);

                if (StudentIs20OrYounger(payload.DateOfBirth))
                    throw new StudentAgeException($"The age of {payload.FullName} needs to be older than 20.", payload.DateOfBirth.Year);

                //throw new Exception("This is just a text exception throw for the last exception handler to catch.");

                _context.Students.Add(payload);
                _context.SaveChanges();

                return Created("", null);
            }
            catch (StudentNameException snex)
            {
                return BadRequest($"{snex.Message} The passed student's name: {snex.StudentName}");
            }
            catch (StudentAgeException saex)
            {
                return BadRequest($"{saex.Message} The passed student's birth year: {saex.Age}");
            }
            catch (Exception)
            {
                return BadRequest("Could not add a student to the database.");
            }
        }

        private bool StudentIs20OrYounger(DateTime dateOfBirth)
        {
            var today = DateTime.Today;
            var age = DateTime.Now.Year - dateOfBirth.Year;

            if (dateOfBirth.Date > today.AddYears(-age)) age--;

            if (age <= 20)
                return true;

            return false;
        }
    }
}
// Visual Studio ran into an unexpected problem, and may affect some project functions. Open log file.