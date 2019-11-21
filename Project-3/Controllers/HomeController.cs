using Project_3.Helpers;
using Project_3.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Project_3.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public ActionResult Dashboard()
        {
            return View();
        }
      
        public ActionResult Contact()
        {
            EmailModel model = new EmailModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Contact(EmailModel email)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await EmailHelper.ComposeEmailAsync(email);
                    return RedirectToAction("Login", "Account");
                }

                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    await Task.FromResult(0);
                }

            }
            return View(email);


        }
    }

}