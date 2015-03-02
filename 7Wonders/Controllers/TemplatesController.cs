using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _7Wonders.Controllers
{
    public class TemplatesController : Controller
    {
        protected override void HandleUnknownAction(string actionName)
        {
            View(actionName).ExecuteResult(ControllerContext);
        }
        // GET: Game
        
    }

    
}