using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tugas_Test_EID
{
    [Table("item_trx")]
    public class DB_Item_Trx
    {
        [Key]
        public required string id { get; set; }
        public required string trx_id { get; set; }
        public required string item_id { get; set; }
        public int qty_out {get; set; }
        public int qty_in {get; set; }
        public DateTime ts_update {get; set;}
    }
}