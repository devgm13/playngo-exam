using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using PlayNGOExam.Models;
using Newtonsoft.Json;
using System.IO;

namespace PlayNGOExam.Controllers
{
    public class EmployeeController : Controller
    {
        // GET: Employee
        public ActionResult Index()
        {
            var employees = Employee.GetEmployees();
            return View(employees);
        }

        public ActionResult Create()
        {
            ViewBag.Submitted = false;
            var created = false;
            if (HttpContext.Request.RequestType == "POST")
            {
                ViewBag.Submitted = true;
                var id = Request.Form["id"];
                var firstname = Request.Form["firstname"];
                var lastname = Request.Form["lastname"];
                var phonenumber = Request.Form["phonenumber"];

                Employee employee = new Employee()
                {
                    ID = Convert.ToInt16(id),
                    Firstname = firstname,
                    Lastname = lastname,
                    Phonenumber = phonenumber
                };
                
                var EmployeeFile = Employee.EmployeeFile;
                var EmployeeData = System.IO.File.ReadAllText(EmployeeFile);
                List<Employee> EmployeeList = new List<Employee>();
                EmployeeList = JsonConvert.DeserializeObject<List<Employee>>(EmployeeData);

                if (EmployeeList == null)
                {
                    EmployeeList = new List<Employee>();
                }
                EmployeeList.Add(employee);
                System.IO.File.WriteAllText(EmployeeFile, JsonConvert.SerializeObject(EmployeeList));
                created = true;
            }

            if (created)
            {
                ViewBag.Message = "Employee was created successfully.";
            }
            else
            {
                ViewBag.Message = "There was an error while creating the employee.";
            }
            return View();
        }

        public ActionResult Update(int id)
        {
            if (HttpContext.Request.RequestType == "POST")
            {
                var firstname = Request.Form["firstname"];
                var lastname = Request.Form["lastname"];
                var phonenumber = Request.Form["phonenumber"];

                var employee = Employee.GetEmployees();

                foreach (Employee emp in employee)
                {
                    if (emp.ID == id)
                    {
                        emp.Firstname = firstname;
                        emp.Lastname = lastname;
                        emp.Phonenumber = phonenumber;
                        break;
                    }
                }
                System.IO.File.WriteAllText(Employee.EmployeeFile, JsonConvert.SerializeObject(employee));
                Response.Redirect("~/Employee/Index?Message=Employee_Updated");
            }
            
            var temp = new Employee();
            var employees = Employee.GetEmployees();
            foreach (Employee empl in employees)
            {
                if (empl.ID == id)
                {
                    temp = empl;
                    break;
                }
                
            }
            if (temp == null)
            {
                ViewBag.Message = "No employee was found.";
            }
            return View(temp);
        }

        public ActionResult Delete(int id)
        {
            var Employees = Employee.GetEmployees();
            var deleted = false;
            foreach (Employee employees in Employees)
            {
                if (employees.ID == id)
                {
                    var index = Employees.IndexOf(employees);
                    Employees.RemoveAt(index);
                    
                    System.IO.File.WriteAllText(Employee.EmployeeFile, JsonConvert.SerializeObject(Employees));
                    deleted = true;
                    break;
                }
            }
            if (deleted)
            {
                ViewBag.Message = "Employee was deleted successfully.";
            }
            else
            {
                ViewBag.Message = "There was an error while deleting the employee.";
            }
            return View();
        }
    }
}