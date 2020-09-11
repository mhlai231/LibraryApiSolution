using LibraryApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.Controllers
{
    public class StatusController : ControllerBase
    {
        ISystemTime Clock;
        IConfiguration Config;


        public StatusController(ISystemTime clock, IConfiguration config)
        {
            Clock = clock; 
            Config = config;
        }


        [HttpPost("employees")]
        public ActionResult Hire([FromBody] EmployeeCreateRequest employeeToHire)
        {
            // 1. Validate it. (we'll do this tomorrow)
            //   - if not valid, send a 400 back telling them they are a bonehead.
            // 2. Add it to the database, whatever.
            var employeeResponse = new EmployeeResponse
            {
                Id = new Random().Next(10, 10000),
                Name = employeeToHire.Name,
                Department = employeeToHire.Department,
                HireDate = DateTime.Now,
                StartingSalary = employeeToHire.StartingSalary
            };
            // 3. Return a 201 Created status code.
            // 4. Inlclude in the response a link to the brand new baby resource (Location: http://localhost:1337/employees/58)
            // 5. (usually) just send them a copy of what they would get if they went to that location.
            return CreatedAtRoute("employees#getanemployee",
                new { employeeId = employeeResponse.Id },
                employeeResponse);
        }

        [HttpGet("whoami")]
        public ActionResult WhoAmi([FromHeader(Name = "User-Agent")] string userAgent)
        {
            return Ok($"I see you are running {userAgent}");
        }


        // GET /employees
        // GET /employees?department=DEV

        [HttpGet("employees")]
        public ActionResult GetAllEmployees([FromQuery] string department = "All")
        {
            return Ok($"all the employees (filtered on {department})");
        }

        // GET /employees/938938
        [HttpGet("employees/{employeeId:int}", Name = "employees#getanemployee")]
        public ActionResult GetAnEmployee(int employeeId)
        {
            // go to the database and get the thing...
            var response = new EmployeeResponse
            {
                Id = employeeId,
                Name = "Bob Smith",
                Department = "DEV",
                HireDate = DateTime.Now.AddDays(-399),
                StartingSalary = 250000
            };
            return Ok(response);
        }


        // GET /status -> StatusController#GetStatus
        [HttpGet("/status")]
        public ActionResult GetStatus()
        {
            var status = new StatusResponse
            {
                Message = "Looks good on my end. Party On!",
                CheckedBy = "Joe Schmidt",
                //checkedBy = Config.GetValue<string>("onCall");
                WhenChecked = Clock.GetCurrent()
            };

            return Ok(status);
        }
    }

    public class StatusResponse
    {
        public string Message { get; set; }
        public string CheckedBy { get; set; }
        public DateTime WhenChecked { get; set; }
    }

    public class EmployeeCreateRequest
    {
        public string Name { get; set; }
        public string Department { get; set; }
        public decimal StartingSalary { get; set; }
    }

    public class EmployeeResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Department { get; set; }
        public decimal StartingSalary { get; set; }
        public DateTime HireDate { get; set; }
    }
}