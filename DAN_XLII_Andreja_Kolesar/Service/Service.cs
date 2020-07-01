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
        public static EmployeeEntities db = new EmployeeEntities();

        public static void AddLocationsToDb()
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
        #region Get lists of tables
        //get all locations from the list
        public static List<tblLocation> GetAllLocations()
        {
            try
            {
                using (EmployeeEntities context = new EmployeeEntities())
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
        public static List<tblGender> GetAllGenders()
        {
            try
            {
                using (EmployeeEntities context = new EmployeeEntities())
                {
                    List<tblGender> list = new List<tblGender>();
                    list = (from x in context.tblGenders select x).ToList();
                    return list;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception" + ex.Message.ToString());
                return null;
            }
        }

        public static List<vwEmployee> GetAllEmployees()
        {
            try
            {
                using (EmployeeEntities context = new EmployeeEntities())
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

        public static List<tblEmployee> GetAllManagers()
        {
            try
            {
                using (EmployeeEntities context = new EmployeeEntities())
                {
                    List<tblEmployee> list = new List<tblEmployee>();
                    list = (from x in context.tblEmployees select x).ToList();
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
        public static void DeleteEmployee(int employeeID)
        {
            try
            {
                using (EmployeeEntities context = new EmployeeEntities())
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

        #region ADD AND UPDATE
        public static vwEmployee AddNewEmployee(vwEmployee employee)
        {
            try
            {
                using (EmployeeEntities context = new EmployeeEntities())
                {
                    //update
                    if (employee.employeeId != 0)
                    {
                        //tblEmployee employeeToEdit = (from c in context.tblEmployees where c.employeeId == employee.employeeId select c).First();
                        //employeeToEdit.fullname = employee.fullname;
                        //employeeToEdit.dateOfBirth = employee.dateOfBirth;
                        //employeeToEdit.genderId = employee.genderId;
                        //employeeToEdit.IdentityCardNumber = employee.IdentityCardNumber;
                        //employeeToEdit.jmbg = employee.jmbg;
                        //employeeToEdit.phone = employee.phone;
                        //employeeToEdit.locationId = employee.locationId;
                        //employeeToEdit.managerId = employee.managerId;
                        //employeeToEdit.sectorId = employee.sectorId;
                        //context.SaveChanges();
                        return employee;
                    }
                    //add new
                    else
                    {
                        tblEmployee newEmployee = new tblEmployee();
                        newEmployee.fullname = employee.fullname;
                        newEmployee.dateOfBirth = employee.dateOfBirth;
                        newEmployee.genderId = employee.getTblGender.genderId;

                        newEmployee.IdentityCardNumber = employee.IdentityCardNumber;
                        newEmployee.jmbg = employee.jmbg;
                        newEmployee.phone = employee.phone;
                        newEmployee.locationId = employee.getTblLocation.locationId;
                        if (employee.getTblManager != null)
                        {
                            newEmployee.managerId = employee.getTblManager.employeeId;
                        }
                        else
                        {
                            newEmployee.managerId = null;
                        }
                        

                        //check existance of sector name. Add new one if sectorName do not exist
                        tblSector s = AddNewSector(employee.sectorName);
                        newEmployee.sectorId = s.sectorId;
                        context.tblEmployees.Add(newEmployee);
                        context.SaveChanges();
                        employee.employeeId = newEmployee.employeeId;
                        return employee;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception" + ex.Message.ToString());
                return null;
            }
        }
        //return sector if it exist, or add new one if it do not exist
        public static tblSector AddNewSector(string sector)
        {
            try
            {
                using (EmployeeEntities context = new EmployeeEntities())
                {
                    tblSector result = (from x in context.tblSectors where x.sectorName == sector select x).FirstOrDefault();

                    if (result == null)
                    {
                        tblSector newSector = new tblSector();
                        newSector.sectorName = sector;
                        context.tblSectors.Add(newSector);
                        context.SaveChanges();
                        return newSector;
                        
                    }
                    else
                    {
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception " + ex.Message.ToString());
                return null;
                
            }
        }

        #endregion


        public static bool IsEmployeeJmbg(string jmbg)
        {
            try
            {
                using (EmployeeEntities context = new EmployeeEntities())
                {
                    string result = (from x in context.tblEmployees where x.jmbg == jmbg select x.jmbg).FirstOrDefault();

                    if (result != null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception " + ex.Message.ToString());
                return false;
            }
        }
    }
}
