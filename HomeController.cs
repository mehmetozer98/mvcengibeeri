using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MVCEngiBeering.Classes;
using MVCEngiBeering.Data;
using MVCEngiBeering.ViewModels;

namespace MVCEngiBeering.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private MvcEngibeeringContext _mvcEngibeeringContext;

        public HomeController(ILogger<HomeController> logger, MvcEngibeeringContext mvcEngibeeringContext)
        {
            _logger = logger;
            _mvcEngibeeringContext = mvcEngibeeringContext;
        }

        public IActionResult Index()
        {
            ViewBag.oee = Math.Round(CalculateOEE(), 2);

            return View(UpdateViewModel() );
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }

        //Making BBMachineViewModel objects from the database
        private List<BBMachineViewModel> UpdateViewModel()
        {
            List<BBMachineViewModel> machines = new List<BBMachineViewModel>();

            foreach (var machine in _mvcEngibeeringContext.machines.Include(m => m.currentState)
                .Include(m => m.currentProduct).ToList())
            {
                BBMachineViewModel temp = new BBMachineViewModel
                {
                    currentamount = machine.currentamount,
                    currentproduct = new ProductTypeViewModel
                    {
                        id = machine.currentProduct.id,
                        name = machine.currentProduct.name
                    },
                    currentspeed = machine.currentspeed,
                    currentstate = new MachineStateViewModel
                    {
                        id = machine.currentState.id,
                        name = machine.currentState.name
                    },
                    id = machine.id,
                    uniqueid = machine.uniqueid
                };


                machines.Add(temp);
            }

            return machines;
        }
        
        //Starting the calculation of the oee
        private double CalculateOEE()
        {
            List<OEECalculator> OEEList = new List<OEECalculator>();
            List<int> BatchIdList = new List<int>();
            
            //Getting all information of each batch nessesary for calculating the oee
            foreach(var item in _mvcEngibeeringContext.batches)
            {
                BatchViewModel temp = new BatchViewModel
                {
                    id = item.id,
                    machine = item.machine,
                    producttype = item.producttype,
                    setamount = item.setamount,
                    setspeed = item.setspeed,
                    actualAmount = item.actualamount,
                    defectiveAmount = item.defectiveamount
                };
            BatchIdList.Add(temp.id);
            OEECalculator oee = new OEECalculator(temp.actualAmount, temp.defectiveAmount, temp.producttype);
                OEEList.Add(oee);
            }
            
            //Geting numbers of work days
            int numberOfDays = GetNumberOfDays(BatchIdList);
            
            //calculating
            return GetCalculatedOee(numberOfDays,OEEList)*100;
        }

        private double GetCalculatedOee(int numberOfDays, List<OEECalculator> oeeList)
        {
            List<double> OEEResult = new List<double>();
            foreach (var oee in oeeList)
            {
                oee.AddTime(16*numberOfDays,2*numberOfDays);
                OEEResult.Add(oee.GetOEE());
            }
            return OEEResult.Sum(); 
        }

        private int GetNumberOfDays(List<int> batchIdList)
        {
            List<String> ListOfTimes = new List<String>();
            foreach(int batchId in batchIdList)
            {
                String date = _mvcEngibeeringContext.datareadings.Where(dataReading => dataReading.batchid == batchId).First().time.ToString("MMMM dd, yyyy");
                if(!ListOfTimes.Contains(date))
                {
                    ListOfTimes.Add(date);
                }
            }

            return ListOfTimes.Count;
        }
    }
}