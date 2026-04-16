using LKM_PAA.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace LKM_PAA.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DokterController : ControllerBase
    {
        private readonly SqlDBHelper _db;
        private readonly IConfiguration _config;

        public DokterController(IConfiguration config)
        {
            _config = config;
            _db = new SqlDBHelper(_config.GetConnectionString("DefaultConnection"));
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            using var conn = _db.GetConnection();
            conn.Open();

            var cmd = _db.GetCommand(conn, "SELECT * FROM dokter");
            var reader = cmd.ExecuteReader();

            var list = new List<object>();

            while (reader.Read())
            {
                list.Add(new
                {
                    id = reader["id_dokter"],
                    nama = reader["nama"],
                    spesialisasi = reader["spesialisasi"],
                    no_hp = reader["no_hp"]
                });
            }

            return Ok(new ApiResponse<object>
            {
                Status = "success",
                Data = list,
                Meta = new { total = list.Count }
            });
        }

        // ================= GET BY ID =================
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            using var conn = _db.GetConnection();
            conn.Open();

            var cmd = _db.GetCommand(conn, "SELECT * FROM dokter WHERE id_dokter = @id");
            cmd.Parameters.AddWithValue("@id", id);

            var reader = cmd.ExecuteReader();

            if (!reader.Read())
            {
                return NotFound(new ApiError
                {
                    Message = "Dokter tidak ditemukan"
                });
            }

            var data = new
            {
                id = reader["id_dokter"],
                nama = reader["nama"],
                spesialisasi = reader["spesialisasi"],
                no_hp = reader["no_hp"]
            };

            return Ok(new ApiResponse<object>
            {
                Status = "success",
                Data = data
            });
        }

        [HttpPost]
        public IActionResult Create([FromQuery] string nama, [FromQuery] string spesialisasi, [FromQuery] string no_hp)
        {
            if (string.IsNullOrEmpty(nama))
            {
                return BadRequest(new ApiError
                {
                    Message = "Nama dokter wajib diisi"
                });
            }

            using var conn = _db.GetConnection();
            conn.Open();

            var query = @"INSERT INTO dokter 
                (nama, spesialisasi, no_hp, created_at, updated_at)
                VALUES (@nama, @sp, @hp, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP)";

            var cmd = _db.GetCommand(conn, query);
            cmd.Parameters.AddWithValue("@nama", nama);
            cmd.Parameters.AddWithValue("@sp", spesialisasi ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@hp", no_hp ?? (object)DBNull.Value);

            cmd.ExecuteNonQuery();

            return StatusCode(201, new ApiResponse<object>
            {
                Status = "success",
                Data = new { message = "Dokter berhasil ditambahkan" }
            });
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromQuery] string nama, [FromQuery] string spesialisasi, [FromQuery] string no_hp)
        {
            using var conn = _db.GetConnection();
            conn.Open();

            // cek data
            var check = _db.GetCommand(conn, "SELECT id_dokter FROM dokter WHERE id_dokter = @id");
            check.Parameters.AddWithValue("@id", id);

            if (check.ExecuteScalar() == null)
            {
                return NotFound(new ApiError
                {
                    Message = "Dokter tidak ditemukan"
                });
            }

            var query = @"UPDATE dokter SET
                nama = @nama,
                spesialisasi = @sp,
                no_hp = @hp,
                updated_at = CURRENT_TIMESTAMP
                WHERE id_dokter = @id";

            var cmd = _db.GetCommand(conn, query);
            cmd.Parameters.AddWithValue("@nama", nama);
            cmd.Parameters.AddWithValue("@sp", spesialisasi ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@hp", no_hp ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();

            return Ok(new ApiResponse<object>
            {
                Status = "success",
                Data = new { message = "Dokter berhasil diupdate" }
            });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            using var conn = _db.GetConnection();
            conn.Open();

            var check = _db.GetCommand(conn, "SELECT id_dokter FROM dokter WHERE id_dokter = @id");
            check.Parameters.AddWithValue("@id", id);

            if (check.ExecuteScalar() == null)
            {
                return NotFound(new ApiError
                {
                    Message = "Dokter tidak ditemukan"
                });
            }

            var cmd = _db.GetCommand(conn, "DELETE FROM dokter WHERE id_dokter = @id");
            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();

            return Ok(new ApiResponse<object>
            {
                Status = "success",
                Data = new { message = "Dokter berhasil dihapus" }
            });
        }
    }
}