using System.Web.Mvc;
using OAuth2.Demo.Attributes;
using OAuth2.Mvc;

namespace OAuth2.Demo.Controllers
{
    [NoCache]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult Secure()
        {
            return View();
        }
    }
}
