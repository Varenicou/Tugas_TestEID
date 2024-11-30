namespace Tugas_Test_EID
{
    public class POST_Upd_Inventaris
    {
        public string? item_id {get;set;}
        public required string item_name {get;set;}
        public string? item_desc { get; set; }
        public string? item_img { get; set; }
        public int item_stock { get; set; }
        public int item_stock_current { get; set; }
    }
}