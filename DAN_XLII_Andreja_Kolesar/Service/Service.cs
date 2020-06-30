using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAN_XLII_Andreja_Kolesar.Service
{
    /// <summary>
    /// Communication with database and file
    /// </summary>
    class Service
    {
        public static EmployeeSystemEntities db = new EmployeeSystemEntities();

        public void AddLocationsToDb()
        {
            string fileName = @"..\..\Lokacije.txt";
            using (StreamReader sr = File.OpenText(fileName))
            {
                tblLocation loc = new tblLocation();
                string s;
                while ((s = sr.ReadLine()) != null)
                {
                    loc.street = s.Split(',')[0];
                    loc.city = s.Split(',')[1];
                    loc.country = s.Split(',')[2];
                    //check existance of this location in db
                    bool isIn = (from l in db.tblLocations where l.street == loc.street && l.city == loc.city && l.country == loc.country select l).Any();
                    //if this location doesn't exist, add
                    if (!isIn)
                    {
                        db.tblLocations.Add(loc);
                        db.SaveChanges();
                    }
                    
                }
            }
        }
        #region Get lists of locations and employees
        //get all locations from the list
        public List<tblLocation> GetAllLocations()
        {
            try
            {
                using (EmployeeSystemEntities context = new EmployeeSystemEntities())
                {
                    List<tblLocation> list = new List<tblLocation>();
                    list = (from x in context.tblLocations select x).ToList();
                    return list;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception" + ex.Message.ToString());
                return null;
            }
        }

        //get all employees
        public List<vwEmployee> GetAllEmployees()
        {
            try
            {
                using (EmployeeSystemEntities context = new EmployeeSystemEntities())
                {
                    List<vwEmployee> list = new List<vwEmployee>();
                    list = (from x in context.vwEmployees select x).ToList();
                    return list;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception" + ex.Message.ToString());
                return null;
            }
        }
        #endregion

        #region DELETE Employee
        public void DeleteEmployee(int employeeID)
        {
            try
            {
                using (EmployeeSystemEntities context = new EmployeeSystemEntities())
                {
                    tblEmployee employeeToDelete = (from u in context.tblEmployees where u.employeeId == employeeID select u).First();
                    context.tblEmployees.Remove(employeeToDelete);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception" + ex.Message.ToString());
            }
        }

        #endregion

    }
}
