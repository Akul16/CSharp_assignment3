using Assignment3.Models;
using MySql.Data.MySqlClient;
using Mysqlx.Datatypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Assignment3.Controllers
{
    public class TeacherDataController : ApiController
    {
        // The database context class which allows us to access our MySQL Database.
        private SchoolDbContext SchoolDb = new SchoolDbContext();


        //  //This Controller Will access the teachers table of our SchoolDb database.
        /// <summary>
        /// Returns a list of teachers in the system and gives the specific searched data
        /// </summary>
        /// <param name="SearchKey">The user input search</param>
        /// <example>GET api/TeachersData/ListTeachers</example>
        /// <returns>
        /// A list of teachers
        /// </returns>

        [HttpGet]
        [Route("api/TeacherData/ListTeachers/{SearchKey?}")]
        public IEnumerable<Teacher> ListTeachers(string SearchKey = null)

        {
            //Create an instance of a connection
            MySqlConnection Conn = SchoolDb.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            cmd.CommandText = "SELECT * FROM teachers WHERE LCASE(teacherfname) LIKE (@search) OR LCASE(teacherlname) LIKE (@search)  OR Date_Format(hiredate,'%d-%b-%Y') LIKE (@search) OR salary LIKE (@search) OR LCASE(employeenumber) LIKE (@search)";

            cmd.Parameters.AddWithValue("@search", String.Concat("%", SearchKey, "%"));
            cmd.Prepare();


            //Gather Result Set of Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();


            //Create an empty list of Teachers
            List<Teacher> Teachers = new List<Teacher> { };

            while (ResultSet.Read())
            {
                //Access Column information by the DB column name as an index
                int TeacherId = (int)(ResultSet["teacherid"]);
                string TeacherFName = ResultSet["teacherfname"].ToString();
                string TeacherLName = ResultSet["teacherlname"].ToString();
                string EmployeeNumber = ResultSet["employeenumber"].ToString();
                DateTime HireDate = DateTime.Parse(ResultSet["hiredate"].ToString());
                decimal Salary = Decimal.Parse(ResultSet["salary"].ToString());

                Teacher NewTeacher = new Teacher();
                NewTeacher.TeacherId = TeacherId;
                NewTeacher.TeacherFName = TeacherFName;
                NewTeacher.TeacherLName = TeacherLName;
                NewTeacher.EmployeeNumber = EmployeeNumber;
                NewTeacher.HireDate = HireDate;
                NewTeacher.Salary = Salary;

                //Add the Teacher Name to the List
                Teachers.Add(NewTeacher);
            }
            //Close the connection between the MySQL Database and the WebServer
            Conn.Close();


            return Teachers;
        }

  
        /// <summary>
        /// Find  teachers details from the Database
        /// </summary>
        /// <param name="id">The primary key of teachers</param>
        /// <returns></returns>

        [HttpGet]
        public Teacher FindTeacher(int id)
        {
            Teacher NewTeacher = new Teacher();

            //Create an instance of a connection
            MySqlConnection Conn = SchoolDb.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();


            //SQL QUERY
            cmd.CommandText = "Select * from teachers where teacherid = "+ id;

            //Gather Result Set of Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            //Create an empty list of Teachers
            List<Teacher> Teachers = new List<Teacher> { };

            while (ResultSet.Read())
            {
                int TeacherId = (int)(ResultSet["teacherid"]);
                string TeacherFName = ResultSet["teacherfname"].ToString();
                string TeacherLName = ResultSet["teacherlname"].ToString();
                string EmployeeNumber = ResultSet["employeenumber"].ToString();
                DateTime HireDate = DateTime.Parse(ResultSet["hiredate"].ToString());
                decimal Salary = Decimal.Parse(ResultSet["salary"].ToString());


                NewTeacher.TeacherId = TeacherId;
                NewTeacher.TeacherFName = TeacherFName;
                NewTeacher.TeacherLName = TeacherLName;
                NewTeacher.EmployeeNumber = EmployeeNumber;
                NewTeacher.HireDate = HireDate;
                NewTeacher.Salary = Salary;

            }

            return NewTeacher;
        }
    }
}
