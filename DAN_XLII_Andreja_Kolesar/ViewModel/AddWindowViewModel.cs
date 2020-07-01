﻿using DAN_XLII_Andreja_Kolesar.Command;
using DAN_XLII_Andreja_Kolesar.Service;
using DAN_XLII_Andreja_Kolesar.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DAN_XLII_Andreja_Kolesar.ViewModel
{
    class AddWindowViewModel : ViewModelBase
    {
        AddAndEditEmployeeWindow addEmployee;

        BackgroundWorker backgroundWorker = new BackgroundWorker();

        private vwEmployee _newEmployee;
        public vwEmployee newEmployee
        {
            get
            {
                return _newEmployee; ;
            }
            set
            {
                _newEmployee = value;
                OnPropertyChanged("newEmployee");
            }
        }


        //if this is true, update main window
        private bool _isUpdatedEmployee;
        public bool isUpdatedEmployee
        {
            get
            {
                return _isUpdatedEmployee;
            }
            set
            {
                _isUpdatedEmployee = value;
            }
        }

        #region lists
        private List<tblLocation> _locationList;
        public List<tblLocation> locationList
        {
            get
            {
                return _locationList;
            }
            set
            {
                _locationList = value;
                OnPropertyChanged("locationList");
            }
        }


        private List<tblGender> _genderList;
        public List<tblGender> genderList
        {
            get
            {
                return _genderList;
            }
            set
            {
                _genderList = value;
                OnPropertyChanged("genderList");
            }
        }

        private List<tblEmployee> _managerList;
        public List<tblEmployee> managerList
        {
            get
            {
                return _managerList;
            }
            set
            {
                _managerList = value;
                OnPropertyChanged("managerList");
            }
        }
        #endregion


        //create object of UserCard, tblUser and tblIdentityCard classes
        public AddWindowViewModel(AddAndEditEmployeeWindow addEmployeeOpen)
        {
            addEmployee = addEmployeeOpen;
            newEmployee = new vwEmployee();
            locationList = Service.Service.GetAllLocations();
            managerList = Service.Service.GetAllManagers();
            genderList = Service.Service.GetAllGenders();
            backgroundWorker.DoWork += DoWorkAdd;
        }

        #region BackgroundWorkers's DoWork event handler
        public void DoWorkAdd(object sender, DoWorkEventArgs e)
        {
            string content = "Employee " + newEmployee.fullname + " with employeeId " + newEmployee.employeeId + " has been added.";
            LogIntoFile.getInstance().PrintActionIntoFile(content);
        }
        #endregion


        #region Commands

        private ICommand _save;
        public ICommand save
        {
            get
            {
                if (_save == null)
                {
                    _save = new RelayCommand(param => SaveExecute(), param => CanSaveExecute());
                }
                
                return _save;
            }
        }

        private void SaveExecute()
        {
            try
            {
                Service.Service.AddNewEmployee(newEmployee);
                backgroundWorker.RunWorkerAsync();
                isUpdatedEmployee = true;
                addEmployee.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private bool CanSaveExecute()
        {
            //if (String.IsNullOrEmpty(newEmployee.fullname) || String.IsNullOrEmpty(newEmployee.sectorName) || String.IsNullOrEmpty(newEmployee.jmbg) || String.IsNullOrEmpty(newEmployee.IdentityCardNumber) || String.IsNullOrEmpty(newEmployee.phone) || newEmployee.getTblLocation == null)
            //{
            //    return false;
            //}
            //else
            //{
            //    return true;
            //}
            return true;
        }

        private ICommand _close;
        public ICommand close
        {
            get
            {
                if (_close == null)
                {
                    _close = new RelayCommand(param => CloseExecute(), param => CanCloseExecute());
                }
                return _close;
            }
        }

        private void CloseExecute()
        {
            try
            {
                addEmployee.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private bool CanCloseExecute()
        {
            return true;
        }
        #endregion
    }

 
}
