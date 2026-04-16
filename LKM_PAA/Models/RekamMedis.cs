namespace LKM_PAA.Models
{
    public class RekamMedis
    {
        public int Id { get; set; }
        public int Kunjungan_Id { get; set; }
        public int Dokter_Id { get; set; }

        public string Diagnosa { get; set; }
        public string Tindakan { get; set; }
        public decimal Biaya { get; set; }

        public DateTime Created_At { get; set; }
        public DateTime Updated_At { get; set; }
    }
}