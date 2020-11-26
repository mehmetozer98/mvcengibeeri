using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCEngiBeering.Classes;
using MVCEngiBeering.Data;
using MVCEngiBeering.Models;
using MVCEngiBeering.ViewModels;
using Newtonsoft.Json;

namespace MVCEngiBeering.Controllers
{
    public class MachineController : Controller
    {
        private MvcEngibeeringContext _mvcEngibeeringContext;

        public MachineController(MvcEngibeeringContext mvcEngibeeringContext)
        {
            _mvcEngibeeringContext = mvcEngibeeringContext;
        }

        public IActionResult Get(int id)
        {
            BBMachine machine = _mvcEngibeeringContext.machines.Include(m => m.currentState).Include(m => m.currentProduct).Single(m => m.id == id);
            
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

            ViewBag.currentAmount = GetCurrentAmount();
         //   ViewBag.currentSpeed = GetCurrentSpeed();
           // ViewBag.currentProduct = GetCurrentProduct();
            //ViewBag.currentState = GetCurrentState();
         

            return View(temp);
        }

        [HttpPost]
        public IActionResult SendCntrlCmd(int cntrlCmdValue, int machSpeedValue, int productID, int amount, int id)
        {
            int batchId = 0;
            if (cntrlCmdValue == 2)
            {
                Console.WriteLine(_mvcEngibeeringContext.batches.Max(b => b.id));
                batchId = _mvcEngibeeringContext.batches.Max(b => b.id) + 1;
            }
            
            CntrlCmd cntrlCmd = new CntrlCmd(cntrlCmdValue, machSpeedValue, productID, amount, batchId);
            string output = JsonConvert.SerializeObject(cntrlCmd);
            Sender sender = new Sender("10.126.132.216");
            sender.send(output);
            return Redirect("get/" + id);
        }

        [HttpGet("/machine/json/index")]
        public IActionResult GetCurrentAmount()
        {
            var item = _mvcEngibeeringContext.datareadings.Where(dataReading => dataReading.type == 1).OrderBy(dataReading => dataReading.id).ToList();
            float test1 = item.Last().value;
            return Content(test1.ToString());
        }


        /*     public IActionResult GetCurrentProduct()
           {
               var item2 = _mvcEngibeeringContext.datareadings.Where(dataReading => dataReading.type == 2).OrderBy(dataReading => dataReading.id).ToList();
               float test2 = item2.Last().value;
               return Content(test2.ToString());
           }
          public IActionResult GetCurrentState()
           {
               var item3 = _mvcEngibeeringContext.datareadings.Where(dataReading => dataReading.type == 3).OrderBy(dataReading => dataReading.id).ToList();
               float test3 = item3.Last().value;
               return Content(test.ToString());
           }

       
        public IActionResult GetCurrentSpeed()
        {
            var item4 = _mvcEngibeeringContext.datareadings.Where(dataReading => dataReading.type == 4).OrderBy(dataReading => dataReading.id).ToList();
            float test4 = item4.Last().value;
            return Content(test43.ToString());
        }
         
                  */ 
         
    }
}