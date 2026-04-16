namespace LKM_PAA.Models
{
    public class Dokter
    {
        public int Id_ { get; set; }
        public string Nama { get; set; }
        public string Spesialisasi { get; set; }
        public string No_Hp { get; set; }

        public DateTime Created_At { get; set; }
        public DateTime Updated_At { get; set; }
    }
}