using DAN_XLII_Andreja_Kolesar.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAN_XLII_Andreja_Kolesar.View
{
    class MainWindowViewModel : ViewModelBase
    {
        MainWindow main;

        #region Property
        //BackgroundWorker object, setting properties of this object to true
        BackgroundWorker backgroundWorker = new BackgroundWorker()
        {
            WorkerReportsProgress = true,
            WorkerSupportsCancellation = true
        };

        private List<vwEmployee> _employeesList;
        public List<vwEmployee> employeesList
        {
            get
            {
                return _employeesList;
            }
            set
            {
                _employeesList = value;
                OnPropertyChanged("employeesList");
            }
        }

        private vwEmployee _viewEmployee;
        public vwEmployee viewEmployee
        {
            get
            {
                return _viewEmployee;
            }
            set
            {
                _viewEmployee = value;
                OnPropertyChanged("viewEmployee");
            }
        }
        private bool _isDeletedEmployee;
        public bool isDeletedEmployee
        {
            get
            {
                return _isDeletedEmployee;
            }
            set
            {
                _isDeletedEmployee = value;
            }
        }
        #endregion

        #region Constructor
        public MainWindowViewModel(MainWindow mainOpen)
        {
            main = mainOpen;
            Service.Service.AddLocationsToDb();
            employeesList = Service.Service.GetAllEmployees();
            // adding method to DoWork event
            backgroundWorker.DoWork += DoWork;
            //adding method to ProgressChanged event
            backgroundWorker.ProgressChanged += ProgressChanged;
            //adding method to RunWorkerCompleted event
            backgroundWorker.RunWorkerCompleted += RunWorkerCompleted;
        }
        #endregion

        #region BackgroundWorkers's events handlers
        public void DoWork(object sender, DoWorkEventArgs e)
        {

            //e.Result();
        }

        public void ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //setting value of user interface elements

        }

        public void RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //if printing cancelled
            if (e.Cancelled)
            {
                // Message = "Printing cancelled.";
            }
            //if some error occurred during document printing
            else if (e.Error != null)
            {
                //Message = e.Error.Message.ToString();
            }
            //if printing successfully finished
            else
            {
                //Message = "Printing completed.";
            }
        }
        #endregion
    }
}
