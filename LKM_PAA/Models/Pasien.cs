namespace LKM_PAA.Models
{
    public class Pasien
    {
        public int Id { get; set; }
        public string Nama { get; set; }
        public string Jenis_Kelamin { get; set; }
        public DateTime Tanggal_Lahir { get; set; }
        public string Alamat { get; set; }
        public string No_Hp { get; set; }

        public DateTime Created_At { get; set; }
        public DateTime Updated_At { get; set; }
        public DateTime? Deleted_At { get; set; }
    }
}