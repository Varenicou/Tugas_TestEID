using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Tugas_Test_EID
{
    [ApiController]
    [Route("/api/[controller]")]
    public class API_Login : Controller
    {
        private readonly AppCtx _db;
        public API_Login(AppCtx db)
        {
            _db = db;
        }
        [HttpPost("handshake")]
        public async Task<IActionResult> Index([FromBody] POST_Login post)
        {
            // Cek apakah ada POST?
            if(post == null)
            {
                return BadRequest(new {
                    status = "error",
                    msg = "Mohon Masukkan Username dan Password dengan benar!"
                });
            }

            // Cek ke DB apakah username itu ada?
            var user = await _db.db_user.FirstOrDefaultAsync(x => x.username == post.username && x.password == post.password);
            if(user != null)
            {
                // Membuat Token
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, post.username),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("TugasTestEID2024_ElvinWilis202412"));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: "localhost",
                    audience: "localhost",
                    claims: claims,
                    expires: DateTime.Now.AddHours(1),
                    signingCredentials: creds
                );

                return Ok(new {
                    status = "success",
                    msg = "Welcome, " + user.username,
                    token = new JwtSecurityTokenHandler().WriteToken(token)
                });
            }
            else
            {
                return BadRequest(new {
                    status = "error",
                    msg = "Username atau Password Salah. Silakan coba kembali."
                });
            }
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] POST_Login post)
        {
            // Cek apakah ada POST?
            if(post == null)
            {
                return BadRequest( new {
                    status = "error",
                    msg = "Mohon Masukkan Username dan Password dengan benar!"
                });
            }

            // Cek ke DB apakah username itu ada?
            var reg = await _db.db_user.FirstOrDefaultAsync(x => x.username == post.username);
            if(reg != null)
            {
                return Ok(new {
                    status = "exist",
                    msg = "Username ini sudah pernah dibuat!"
                });
            }
            else
            {
                var ins_user = new DB_User_Login
                {
                    id = Guid.NewGuid().ToString(),
                    username = post.username,
                    password = post.password
                };
                _db.db_user.Add(ins_user);

                try
                {
                    await _db.SaveChangesAsync();
                    return Ok(new {
                        status = "success",
                        msg = "Akun anda sudah terdaftar. Silakan Login menggunakan Username dan Password yang anda daftarkan!"
                    });
                }
                catch(DbUpdateException ex)
                {
                    Console.WriteLine("Insert DB_User Failed: " + ex.Message + " | Excpetion: " + ex.InnerException?.Message);
                    return Ok(new {
                        status = "error",
                        msg = "Ada kesalahan Database: " + ex.Message + " | Exception: " + ex.InnerException?.Message
                    });
                }
                catch(Exception ex)
                {
                    Console.WriteLine("An Error Occured: " + ex.Message);
                    return Ok(new {
                        status = "error",
                        msg = "Ada kesalahan Database: " + ex.Message + " | Exception: " + ex.InnerException?.Message
                    });
                }
            }
        }
    }
}