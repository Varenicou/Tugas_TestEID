using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tugas_Test_EID
{
    [Table("item_inventory")]
    public class DB_Item_Inventory
    {
        [Key]
        public required string id { get; set; }
        public required string item_id { get; set; }
        public required string item_name { get; set; }
        public string? item_desc { get; set; }
        public string? item_img { get; set; }
        public int item_stock { get; set; }
        public int item_stock_current { get; set; }
        [Timestamp]
        public byte[] ts_update {get; set; }
        public bool status {get;set;}
    }
}