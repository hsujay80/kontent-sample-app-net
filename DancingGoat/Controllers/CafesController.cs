﻿using DancingGoat.Models;
using KenticoCloud.Delivery;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DancingGoat.Controllers
{
    [RoutePrefix("cafes")]
    public class CafesController : AsyncController
    {
        private readonly DeliveryClient client = new DeliveryClient(ConfigurationManager.AppSettings["ProjectId"]);

        [Route]
        public async Task<ActionResult> Index()
        {
            var filters = new List<IFilter> {
                new EqualsFilter("system.type", "cafe"),
                new Order("system.name")
            };

            var cafes = (await client.GetItemsAsync(filters)).Items;

            var viewModel = new CafesViewModel
            {
                CompanyCafes = cafes.Where(c => c.GetString("country") == "USA").ToList(),
                PartnerCafes = cafes.Where(c => c.GetString("country") != "USA").ToList()
            };

            return View(viewModel);
        }
    }
}