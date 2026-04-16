using LKM_PAA.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace LKM_PAA.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RekamMedisController : ControllerBase
    {
        private readonly SqlDBHelper _db;
        private readonly IConfiguration _config;

        public RekamMedisController(IConfiguration config)
        {
            _config = config;
            _db = new SqlDBHelper(_config.GetConnectionString("DefaultConnection"));
        }

        // ================= CREATE =================
        [HttpPost]
        public IActionResult Create([FromQuery] int kunjungan_id, [FromQuery] int dokter_id,
                                   [FromQuery] string diagnosa, [FromQuery] string tindakan,
                                   [FromQuery] decimal biaya)
        {
            using var conn = _db.GetConnection();
            conn.Open();

            // cek kunjungan
            var cekKunjungan = _db.GetCommand(conn,
                "SELECT id_kunjungan FROM kunjungan WHERE id_kunjungan = @id");
            cekKunjungan.Parameters.AddWithValue("@id", kunjungan_id);

            if (cekKunjungan.ExecuteScalar() == null)
            {
                return NotFound(new ApiError { Message = "Kunjungan tidak ditemukan" });
            }

            // cek dokter
            var cekDokter = _db.GetCommand(conn,
                "SELECT id_dokter FROM dokter WHERE id_dokter = @id");
            cekDokter.Parameters.AddWithValue("@id", dokter_id);

            if (cekDokter.ExecuteScalar() == null)
            {
                return NotFound(new ApiError { Message = "Dokter tidak ditemukan" });
            }

            var query = @"INSERT INTO rekam_medis
                (id_kunjungan, id_dokter, diagnosa, tindakan, biaya, created_at, updated_at)
                VALUES (@kid, @did, @diag, @tin, @biaya, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP)";

            var cmd = _db.GetCommand(conn, query);
            cmd.Parameters.AddWithValue("@kid", kunjungan_id);
            cmd.Parameters.AddWithValue("@did", dokter_id);
            cmd.Parameters.AddWithValue("@diag", diagnosa ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@tin", tindakan ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@biaya", biaya);

            cmd.ExecuteNonQuery();

            return StatusCode(201, new ApiResponse<object>
            {
                Status = "success",
                Data = new { message = "Rekam medis berhasil ditambahkan" }
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
                    rm.id_rekam_medis,
                    p.nama AS nama_pasien,
                    d.nama AS nama_dokter,
                    k.tanggal_kunjungan,
                    rm.diagnosa,
                    rm.tindakan,
                    rm.biaya
                FROM rekam_medis rm
                JOIN kunjungan k ON rm.id_kunjungan = k.id_kunjungan
                JOIN pasien p ON k.id_pasien = p.id_pasien
                JOIN dokter d ON rm.id_dokter = d.id_dokter
            ";

            var cmd = _db.GetCommand(conn, query);
            var reader = cmd.ExecuteReader();

            var list = new List<object>();

            while (reader.Read())
            {
                list.Add(new
                {
                    id = reader["id_rekam_medis"],
                    pasien = reader["nama_pasien"],
                    dokter = reader["nama_dokter"],
                    tanggal = reader["tanggal_kunjungan"],
                    diagnosa = reader["diagnosa"],
                    tindakan = reader["tindakan"],
                    biaya = reader["biaya"]
                });
            }

            return Ok(new ApiResponse<object>
            {
                Status = "success",
                Data = list,
                Meta = new { total = list.Count }
            });
        }

        // ================= GET BY DOKTER =================
        [HttpGet("dokter/{id}")]
        public IActionResult GetByDokter(int id)
        {
            using var conn = _db.GetConnection();
            conn.Open();

            var query = @"
                SELECT 
                    rm.id_rekam_medis,
                    p.nama,
                    rm.diagnosa,
                    rm.tindakan,
                    rm.biaya
                FROM rekam_medis rm
                JOIN kunjungan k ON rm.id_kunjungan = k.id_kunjungan
                JOIN pasien p ON k.id_pasien = p.id_pasien
                WHERE rm.id_dokter = @id
            ";

            var cmd = _db.GetCommand(conn, query);
            cmd.Parameters.AddWithValue("@id", id);

            var reader = cmd.ExecuteReader();
            var list = new List<object>();

            while (reader.Read())
            {
                list.Add(new
                {
                    id = reader["id_rekam_medis"],
                    pasien = reader["nama"],
                    diagnosa = reader["diagnosa"],
                    tindakan = reader["tindakan"],
                    biaya = reader["biaya"]
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