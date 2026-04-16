using LKM_PAA.DTOs;
using LKM_PAA.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace LKM_PAA.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PasienController : ControllerBase
    {
        private readonly SqlDBHelper _db;
        private readonly IConfiguration _config;

        public PasienController(IConfiguration config)
        {
            _config = config;
            _db = new SqlDBHelper(_config.GetConnectionString("DefaultConnection"));
        }

        // ================= GET ALL =================
        [HttpGet]
        public IActionResult GetAll()
        {
            using var conn = _db.GetConnection();
            conn.Open();

            var cmd = _db.GetCommand(conn,
                "SELECT * FROM pasien WHERE deleted_at IS NULL");

            var reader = cmd.ExecuteReader();
            var list = new List<object>();

            while (reader.Read())
            {
                list.Add(new
                {
                    id = reader["id_pasien"],
                    nama = reader["nama"],
                    jenis_kelamin = reader["jenis_kelamin"],
                    tanggal_lahir = reader["tanggal_lahir"],
                    alamat = reader["alamat"],
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

            var cmd = _db.GetCommand(conn,
                "SELECT * FROM pasien WHERE id_pasien = @id AND deleted_at IS NULL");
            cmd.Parameters.AddWithValue("@id", id);

            var reader = cmd.ExecuteReader();

            if (!reader.Read())
            {
                return NotFound(new ApiError
                {
                    Message = "Pasien tidak ditemukan"
                });
            }

            var data = new
            {
                id = reader["id_pasien"],
                nama = reader["nama"],
                jenis_kelamin = reader["jenis_kelamin"],
                tanggal_lahir = reader["tanggal_lahir"],
                alamat = reader["alamat"],
                no_hp = reader["no_hp"]
            };

            return Ok(new ApiResponse<object>
            {
                Status = "success",
                Data = data
            });
        }

        // ================= CREATE =================
        [HttpPost]
        public IActionResult Create([FromBody] PasienDTO dto)
        {
            if (dto == null || string.IsNullOrEmpty(dto.Nama))
            {
                return BadRequest(new ApiError
                {
                    Message = "Data tidak valid"
                });
            }

            using var conn = _db.GetConnection();
            conn.Open();

            var query = @"INSERT INTO pasien 
                (nama, jenis_kelamin, tanggal_lahir, alamat, no_hp, created_at, updated_at)
                VALUES (@nama, @jk, @tgl, @alamat, @hp, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP)";

            var cmd = _db.GetCommand(conn, query);
            cmd.Parameters.AddWithValue("@nama", dto.Nama);
            cmd.Parameters.AddWithValue("@jk", dto.Jenis_Kelamin);
            cmd.Parameters.AddWithValue("@tgl", dto.Tanggal_Lahir);
            cmd.Parameters.AddWithValue("@alamat", dto.Alamat ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@hp", dto.No_Hp ?? (object)DBNull.Value);

            cmd.ExecuteNonQuery();

            return StatusCode(201, new ApiResponse<object>
            {
                Status = "success",
                Data = new { message = "Pasien berhasil ditambahkan" }
            });
        }

        // ================= UPDATE =================
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] PasienDTO dto)
        {
            using var conn = _db.GetConnection();
            conn.Open();

            var checkCmd = _db.GetCommand(conn,
                "SELECT id_pasien FROM pasien WHERE id_pasien = @id AND deleted_at IS NULL");
            checkCmd.Parameters.AddWithValue("@id", id);

            if (checkCmd.ExecuteScalar() == null)
            {
                return NotFound(new ApiError
                {
                    Message = "Pasien tidak ditemukan"
                });
            }

            var query = @"UPDATE pasien SET 
                nama = @nama,
                jenis_kelamin = @jk,
                tanggal_lahir = @tgl,
                alamat = @alamat,
                no_hp = @hp,
                updated_at = CURRENT_TIMESTAMP
                WHERE id_pasien = @id";

            var cmd = _db.GetCommand(conn, query);
            cmd.Parameters.AddWithValue("@nama", dto.Nama);
            cmd.Parameters.AddWithValue("@jk", dto.Jenis_Kelamin);
            cmd.Parameters.AddWithValue("@tgl", dto.Tanggal_Lahir);
            cmd.Parameters.AddWithValue("@alamat", dto.Alamat ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@hp", dto.No_Hp ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();

            return Ok(new ApiResponse<object>
            {
                Status = "success",
                Data = new { message = "Pasien berhasil diupdate" }
            });
        }

        // ================= DELETE (SOFT DELETE) =================
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            using var conn = _db.GetConnection();
            conn.Open();

            var checkCmd = _db.GetCommand(conn,
                "SELECT id_pasien FROM pasien WHERE id_pasien = @id AND deleted_at IS NULL");
            checkCmd.Parameters.AddWithValue("@id", id);

            if (checkCmd.ExecuteScalar() == null)
            {
                return NotFound(new ApiError
                {
                    Message = "Pasien tidak ditemukan"
                });
            }

            var cmd = _db.GetCommand(conn,
                "UPDATE pasien SET deleted_at = CURRENT_TIMESTAMP WHERE id_pasien = @id");

            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();

            return Ok(new ApiResponse<object>
            {
                Status = "success",
                Data = new { message = "Pasien berhasil dihapus (soft delete)" }
            });
        }
    }
}