namespace LKM_PAA.Models
{
    public class Kunjungan
    {
        public int Id { get; set; }
        public int Pasien_Id { get; set; }
        public DateTime Tanggal_Kunjungan { get; set; }
        public string Keluhan { get; set; }

        public DateTime Created_At { get; set; }
        public DateTime Updated_At { get; set; }
    }
}