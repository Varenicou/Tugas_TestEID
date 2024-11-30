namespace Tugas_Test_EID
{
    public class JSON_Get_Inventaris
    {
        public required string status {get;set;}
        public required string msg {get;set; }
        public List<DB_Item_Inventory> data {get;set;}
    }
}