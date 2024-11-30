using Microsoft.AspNetCore.Mvc;

namespace Tugas_Test_EID
{
    public class Login : Controller
    {
        private readonly AppCtx _db;
        public Login(AppCtx db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}