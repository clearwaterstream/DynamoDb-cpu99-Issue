using clearwaterstream.IoC;
using DynamoDBCPU99.Application.Features;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace DynamoDBCPU99.Controllers
{
    public class HomeController : ControllerBase
    {
        public IActionResult Index()
        {
            return Content("We the 99%");
        }

        public async Task<IActionResult> Seed()
        {
            var dataStore = ServiceRegistrar.Current.GetInstance<ZipCodeDataStore>();

            var sw = Stopwatch.StartNew();

            await dataStore.Seed();

            sw.Stop();

            return Content($"seeded. operation took {sw.ElapsedMilliseconds} ms");
        }

        public async Task<IActionResult> Search()
        {
            var dataStore = ServiceRegistrar.Current.GetInstance<IZipCodeLookup>();

            var sw = Stopwatch.StartNew();

            var items = await dataStore.Search();

            sw.Stop();

            return Content($"search done. operation took {sw.ElapsedMilliseconds} ms. {items.Count} items returned.");
        }
    }
}
