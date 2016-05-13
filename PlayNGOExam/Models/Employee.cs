using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Newtonsoft.Json;
using System.IO;

namespace PlayNGOExam.Models
{
    public class Employee
    {
        public static string EmployeeFile = HttpContext.Current.Server.MapPath("~/App_Data/Employees.json");

        public int ID { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Phonenumber { get; set; }

        public static List<Employee> GetEmployees()
        {
            List<Employee> employees = new List<Employee>();
            if (File.Exists(EmployeeFile))
            {
                // File exists..
                string content = File.ReadAllText(EmployeeFile);
                employees = JsonConvert.DeserializeObject<List<Employee>>(content);
                return employees;
            }
            else
            {
                // Create the file 
                File.Create(EmployeeFile).Close();
                File.WriteAllText(EmployeeFile, "[]");
                GetEmployees();
            }

            return employees;
        }
    }
}