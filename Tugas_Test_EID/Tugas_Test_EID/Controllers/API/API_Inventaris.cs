using System.Diagnostics;
using System.Reflection;
using System.Security.AccessControl;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Tugas_Test_EID
{
    [ApiController]
    [Route("/api/inventaris/")]
    public class API_Inventaris : Controller
    {
        private readonly AppCtx _db;
        private readonly IWebHostEnvironment _env;
        public API_Inventaris(AppCtx db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        [HttpGet("get_inventaris")]
        [Authorize]
        public async Task<IActionResult> GetInventaris()
        {
            // Ambil data dari DB
            try
            {
                var inventaris = await _db.db_inventory.Where(x => x.status == true).ToListAsync();
                if(inventaris.Count > 0)
                {
                    return Ok(new JSON_Get_Inventaris
                    {
                        status = "success",
                        msg = "Data Retrieval Successfully!",
                        data = inventaris
                    });
                }
                else
                {
                    return Ok(new JSON_Get_Inventaris
                    {
                        status = "success",
                        msg = "Data Not available!",
                        data = []
                    });
                }
            }
            catch(Exception ex)
            {
                return BadRequest(new {
                    status = "error",
                    msg = "Terdapat kesalahan pada saat mengambil data: " + ex.Message
                });
            }
        }

        [HttpPost("upd_inventaris")]
        [Authorize]
        public async Task<IActionResult> UpdInventaris([FromForm] POST_Upd_Inventaris post, [FromForm] IFormFile item_img)
        {
            // Cek kalo ga ada Input maka return False
            if(post == null)
            {
                return BadRequest(new {
                    status = "error",
                    msg = "Periksa kembali Input yang anda berikan. Lalu coba kembali."
                });
            }
            else if(item_img == null || item_img.Length == 0)
            {
                return BadRequest(new {
                    status = "error",
                    msg = "Mohon unggah file gambar!"
                });
            }
            try
            {
                // Step 1: Proses Upload
                string? filename = null, filepath;
                bool upload_file = false;
                // Cek apakah ada file atau tidak
                if(item_img != null && item_img.Length > 0)
                {
                    var uploadPath = Path.Combine(_env.WebRootPath, "uploads");
                    if(!Directory.Exists(uploadPath))
                    {
                        // Jika belum ada maka buatkan folder baru
                        Directory.CreateDirectory(uploadPath);
                    }

                    // Menentukan Filename
                    filename = DateTime.Now.ToString("yyyyMMddhhmmss") + Path.GetExtension(item_img.FileName);
                    filepath = Path.Combine(uploadPath, filename);

                    // Simpan Gambar
                    using (var stream = new FileStream(uploadPath, FileMode.Create))
                    {
                        await item_img.CopyToAsync(stream);
                        upload_file = true;
                    }
                }

                // Step 2: Proses Insert ke Database
                var inventaris = await _db.db_inventory.FirstOrDefaultAsync(x => x.item_id == post.item_id && x.status == true);
                if(inventaris != null)
                {
                    inventaris.item_name = post.item_name;
                    inventaris.item_desc = post.item_desc;
                    if(upload_file == true)
                    {
                        inventaris.item_img = "/uploads/"+filename;
                    }
                    inventaris.item_stock = post.item_stock;
                    _db.db_inventory.Update(inventaris);
                }
                else
                {
                    var ins_inv = new DB_Item_Inventory
                    {
                        id = Guid.NewGuid().ToString(),
                        item_id = "INV-" + DateTime.Now.ToString("yyyymmHHss"),
                        item_name = post.item_name,
                        item_desc = post.item_desc,
                        item_img = "/uploads/"+filename,
                        item_stock = post.item_stock,
                        status = true
                    };
                    _db.db_inventory.Add(ins_inv);
                }

                try
                {
                    await _db.SaveChangesAsync();
                    return Ok(new {
                        status = "success",
                        msg = "Pengkinian data telah berhasil!"
                    });
                }
                catch(Exception ex)
                {
                    return Ok(new {
                        status = "error",
                        msg = "Terdapat kesalahan pada saat melakukan Pengkinian data: " + ex.Message
                    });
                }
            }
            catch(Exception ex)
            {
                return Ok(new {
                    status = "error",
                    msg = "Terdapat kesalahan pada saat melakukan Pengkinian data: " + ex.Message
                });
            }
        }

        [HttpPost("get_inventaris")]
        [Authorize]
        public async Task<IActionResult> GetSpcInventaris([FromBody] POST_Spc_Inventaris post)
        {
            if(post.item_id == null)
            {
                return BadRequest(new {
                    status = "error",
                    msg = "Periksa kembali data yang diinput!"
                });
            }

            var inventaris = await _db.db_inventory.FirstOrDefaultAsync(x => x.item_id == post.item_id);
            if(inventaris == null)
            {
                return NotFound(new {
                    status = "error",
                    msg = "Data Tidak Ditemukan!"
                });
            }
            else
            {
                return Ok(new {
                    status = "success",
                    msg = "Data ditemukan!",
                    data = inventaris
                });
            }
        }

        [HttpPost("add_inventaris")]
        [Authorize]
        public async Task<IActionResult> AddInventaris([FromForm] POST_Upd_Inventaris post, [FromForm] IFormFile item_img)
        {
            // Cek kalo ga ada Input maka return False
            if(post == null)
            {
                return BadRequest(new {
                    status = "error",
                    msg = "Periksa kembali Input yang anda berikan. Lalu coba kembali."
                });
            }
            else if(item_img == null || item_img.Length == 0)
            {
                return BadRequest(new {
                    status = "error",
                    msg = "Mohon unggah file gambar!"
                });
            }

            try
            {
                // Step 1: Proses Upload
                string? filename = null, filepath = null;
                bool upload_file = false;
                // Cek apakah ada file atau tidak
                if(item_img != null && item_img.Length > 0)
                {
                    string uploadPath = Path.Combine(_env.WebRootPath, "uploads");
                    //string uploadPath = "/mnt/e/Interview/TugasASPNET_PID/Tugas_TestEID/Tugas_Test_EID/Tugas_Test_EID/wwwroot/uploads";

                    if (!Directory.Exists(uploadPath))
                    {
                        // Jika belum ada maka buatkan folder baru
                        Directory.CreateDirectory(uploadPath);
                        //DirectoryInfo dirinfo = new DirectoryInfo(uploadPath);
                        //DirectorySecurity dirsec = dirinfo.GetAccessControl();

                        //dirsec.AddAccessRule(new FileSystemAccessRule(
                        //    "Everyone",
                        //    FileSystemRights.FullControl,
                        //    AccessControlType.Allow
                        //));
                        //dirinfo.SetAccessControl(dirsec);
                    }

                    // Menentukan Filename
                    filename = DateTime.Now.ToString("yyyyMMddhhmmss") + Path.GetExtension(item_img.FileName);
                    filepath = Path.Combine(uploadPath, filename);
                    Debug.WriteLine("Env : " + Environment.UserName);
                    Debug.WriteLine("Upload Path : " + uploadPath);

                    // Simpan Gambar
                        using (var stream = new FileStream(uploadPath, FileMode.Create))
                        {
                            await item_img.CopyToAsync(stream);
                            upload_file = true;
                        }
                }

                // Step 2: Proses Insert ke Database
                var ins_inv = new DB_Item_Inventory
                {
                    id = Guid.NewGuid().ToString(),
                    item_id = "INV-" + DateTime.Now.ToString("yyyymmHHss"),
                    item_name = post.item_name,
                    item_desc = post.item_desc,
                    item_img = filepath,
                    item_stock = post.item_stock,
                    status = true
                };
                _db.db_inventory.Add(ins_inv);

                try
                {
                    await _db.SaveChangesAsync();
                    return Ok(new {
                        status = "success",
                        msg = "Pengkinian data telah berhasil!"
                    });
                }
                catch(Exception ex)
                {
                    return Ok(new {
                        status = "error",
                        msg = "Terdapat kesalahan pada saat melakukan Pengkinian data: " + ex.Message
                    });
                }
            }
            catch(Exception ex)
            {
                return Ok(new {
                    status = "error",
                    msg = "Terdapat kesalahan pada saat melakukan Pengkinian data: " + ex.Message
                });
            }
        }

        [HttpDelete("del_inventaris")]
        [Authorize]
        public async Task<IActionResult> DelInventaris([FromBody] POST_Spc_Inventaris data)
        {
            if(data == null)
            {
                return BadRequest(new {
                    status = "error",
                    msg = "Tidak ada data yang akan dihapus!"
                });
            }

            var inventaris = await _db.db_inventory.FirstOrDefaultAsync(x => x.item_id == data.item_id);
            if(inventaris != null)
            {
                _db.db_inventory.Remove(inventaris);
                try
                {
                    await _db.SaveChangesAsync();
                    return Ok(new {
                        status = "success",
                        msg = "Data telah berhasil dihapus!"
                    });
                }
                catch(Exception ex)
                {
                    return Ok(new {
                        status = "error",
                        msg = "Terdapat kesalahan pada saat melakukan penghapusan data! " + ex.Message
                    });
                }
            }
            else
            {
                return Ok(new {
                    status = "error",
                    msg = "Data yang ingin dihapus tidak ditemukan!"
                });
            }
        }
    }
}