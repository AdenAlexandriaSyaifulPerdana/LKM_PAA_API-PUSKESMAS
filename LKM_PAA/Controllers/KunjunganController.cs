using LKM_PAA.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace LKM_PAA.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class KunjunganController : ControllerBase
    {
        private readonly SqlDBHelper _db;
        private readonly IConfiguration _config;

        public KunjunganController(IConfiguration config)
        {
            _config = config;
            _db = new SqlDBHelper(_config.GetConnectionString("DefaultConnection"));
        }

        // ================= CREATE =================
        [HttpPost]
        public IActionResult Create([FromQuery] int pasien_id, [FromQuery] string keluhan)
        {
            using var conn = _db.GetConnection();
            conn.Open();

            // cek pasien
            var checkCmd = _db.GetCommand(conn,
                "SELECT id_pasien FROM pasien WHERE id_pasien = @id AND deleted_at IS NULL");
            checkCmd.Parameters.AddWithValue("@id", pasien_id);

            if (checkCmd.ExecuteScalar() == null)
            {
                return NotFound(new ApiError
                {
                    Message = "Pasien tidak ditemukan"
                });
            }

            var query = @"INSERT INTO kunjungan
                (id_pasien, tanggal_kunjungan, keluhan, created_at, updated_at)
                VALUES (@pid, CURRENT_DATE, @keluhan, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP)";

            var cmd = _db.GetCommand(conn, query);
            cmd.Parameters.AddWithValue("@pid", pasien_id);
            cmd.Parameters.AddWithValue("@keluhan", keluhan ?? (object)DBNull.Value);

            cmd.ExecuteNonQuery();

            return StatusCode(201, new ApiResponse<object>
            {
                Status = "success",
                Data = new { message = "Kunjungan berhasil ditambahkan" }
            });
        }

        // ================= GET ALL =================
        [HttpGet]
        public IActionResult GetAll()
        {
            using var conn = _db.GetConnection();
            conn.Open();

            var query = @"
                SELECT 
                    k.id_kunjungan,
                    p.nama,
                    k.tanggal_kunjungan,
                    k.keluhan
                FROM kunjungan k
                JOIN pasien p ON k.id_pasien = p.id_pasien";

            var cmd = _db.GetCommand(conn, query);
            var reader = cmd.ExecuteReader();

            var list = new List<object>();

            while (reader.Read())
            {
                list.Add(new
                {
                    id = reader["id_kunjungan"],
                    nama_pasien = reader["nama"],
                    tanggal = reader["tanggal_kunjungan"],
                    keluhan = reader["keluhan"]
                });
            }

            return Ok(new ApiResponse<object>
            {
                Status = "success",
                Data = list,
                Meta = new { total = list.Count }
            });
        }

        // ================= GET BY PASIEN =================
        [HttpGet("pasien/{id}")]
        public IActionResult GetByPasien(int id)
        {
            using var conn = _db.GetConnection();
            conn.Open();

            var query = @"
                SELECT 
                    id_kunjungan,
                    tanggal_kunjungan,
                    keluhan
                FROM kunjungan
                WHERE id_pasien = @id";

            var cmd = _db.GetCommand(conn, query);
            cmd.Parameters.AddWithValue("@id", id);

            var reader = cmd.ExecuteReader();
            var list = new List<object>();

            while (reader.Read())
            {
                list.Add(new
                {
                    id = reader["id_kunjungan"],
                    tanggal = reader["tanggal_kunjungan"],
                    keluhan = reader["keluhan"]
                });
            }

            return Ok(new ApiResponse<object>
            {
                Status = "success",
                Data = list,
                Meta = new { total = list.Count }
            });
        }
    }
}