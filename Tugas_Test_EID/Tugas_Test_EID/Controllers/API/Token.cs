// using Microsoft.AspNetCore.Mvc;

// namespace Tugas_Test_EID
// {
//     [ApiController]
//     [Route("/api/[controller]")]
//     public class Token : Controller
//     {
//         private readonly AppCtx _db;
//         public Token (AppCtx db)
//         {
//             _db = db;
//         }

//         [HttpPost("get_token")]
//         public async Task<IActionResult> GetToken([FromBody] POST_Token post)
//         {
//             if(post == null)
//             {
//                 return BadRequest(new {
//                     status = "error",
//                     msg = "Invalid Secret Key!"
//                 });
//             }


//         }
//     }
// }