using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSMS.Controllers
{
    public class Template2Controller : Controller
    {
        public IActionResult index()
        {
            return View();
        }
        public IActionResult elements()
        {
            return View();
        }
        public IActionResult about()
        {
            return View();
        }
        public IActionResult blog()
        {
            return View();
        }
        public IActionResult cases()
        {
            return View();
        }
        public IActionResult single_blog()
        {
            return View();
        }
    }
}
