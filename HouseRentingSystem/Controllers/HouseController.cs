using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HouseRentingSystem.Controllers
{
    [Authorize]   
    public class HouseController : Controller
    {
        public  async Task<IActionResult> All()
        {
            return View();
        }
    }
}
