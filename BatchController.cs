using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MVCEngiBeering.Data;
using MVCEngiBeering.Models;
using MVCEngiBeering.ViewModels;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MVCEngiBeering.Controllers
{
    public class BatchController : Controller
    {
        private MvcEngibeeringContext _mvcEngibeeringContext;

        public BatchController(MvcEngibeeringContext mvcEngibeeringContext)
        {
            _mvcEngibeeringContext = mvcEngibeeringContext;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Get(int id)
        {
            return View(GetBatchFromDB(id));
        }

        private BatchViewModel GetBatchFromDB(int id)
        {
            Batch temp = _mvcEngibeeringContext.batches.Include(datareadings => datareadings.DataReadings).Single(batches => batches.id == id);

            BatchViewModel batch = new BatchViewModel
            {
                id = temp.id,
                machine = temp.machine,
                producttype = temp.producttype,
                setamount = temp.setamount,
                setspeed = temp.setspeed
            };
                ICollection<DataReadingViewModel> dr = new List<DataReadingViewModel>();
                foreach(var item in _mvcEngibeeringContext.datareadings.Where(m => m.batchid == id))
            {
                    DataReadingViewModel DataReadings = new DataReadingViewModel
                {
                    id = item.id,
                    batchid = item.batchid,
                    type = item.type,
                    time = item.time.ToString(),
                    value = item.value
                };
                dr.Add(DataReadings);

            }
            batch.DataReadings = dr;

            return batch;     
        }

        private List<DataReadingViewModel> GetDataReadingFromDB(int id)
        {
            List<DataReadingViewModel> dataReadingsList = new List<DataReadingViewModel>();
            foreach (DataReading dataReading in _mvcEngibeeringContext.datareadings.ToList())
            {
                if (dataReading.batchid == id)
                {
                    DataReadingViewModel temp = new DataReadingViewModel
                    {
                        id = dataReading.id,
                        batchid = dataReading.batchid,
                        type = dataReading.type,
                        time = dataReading.time.ToString(),
                        value = dataReading.value
                    };
                    dataReadingsList.Add(temp);
                }
            }
            return dataReadingsList;
        }
          

        [HttpPost]
        public IActionResult AddReading(string json)
        {
            DataReadingViewModel data = JsonConvert.DeserializeObject<DataReadingViewModel>(json);
            DataReading temp = new DataReading();
            temp.time = DateTime.ParseExact(data.time,"dd-MM-yyyy HH:mm:ss",CultureInfo.InvariantCulture);
            temp.type = data.type;
            temp.value = data.value;
            temp.batchid = _mvcEngibeeringContext.batches.Find(data.batchid).id;
            if (temp.type == 1 || temp.type == 2)
            {
                var batch = _mvcEngibeeringContext.batches.Find(data.batchid);
                if (temp.type == 1)
                {
                    batch.actualamount = (int)temp.value;
                }
                else
                {
                    batch.defectiveamount = (int)temp.value;
                }
                _mvcEngibeeringContext.Update(batch);
            }
            _mvcEngibeeringContext.Add(temp);
            _mvcEngibeeringContext.SaveChanges();
            return Ok();
        }

        [HttpPost]
        public IActionResult AddBatch(string json)
        {
            BatchViewModel data = JsonConvert.DeserializeObject<BatchViewModel>(json);
            Batch batch = new Batch();
            batch.id = data.id;
            batch.machine = _mvcEngibeeringContext.machines.Find(data.machine).id;
            batch.producttype = _mvcEngibeeringContext.producttypes.Find(data.producttype).id;
            batch.setamount = data.setamount;
            batch.setspeed = data.setspeed;
            _mvcEngibeeringContext.batches.Add(batch);
            batch.id = data.id;
            _mvcEngibeeringContext.SaveChanges();
            return Ok();
        }


        [HttpGet("/Batch/json/index")]
        public IActionResult JsonIndex(int productType)
        {
            List<Batch> batchlist = _mvcEngibeeringContext.batches.Where(batch => batch.producttype == productType)
                .OrderBy(batch => batch.defectiveamount / batch.actualamount).ToList();
            int outputnunmber = batchlist[batchlist.Count - 1].setspeed;
            return Content(outputnunmber.ToString());
        }

        [HttpPost]
        public IActionResult SearchBatchId(int batchid)
        {
            return Redirect("/Batch/Get/"+batchid);
        }
    }
}