using Microsoft.AspNetCore.Mvc;

namespace Tugas_Test_EID
{
    public class HomePage : Controller
    {
        private readonly AppCtx _db;
        public HomePage(AppCtx db)
        {
            _db = db;
        }
        
        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}