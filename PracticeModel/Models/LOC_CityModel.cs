namespace PracticeModel.Models
{
    public class LOC_CityModel
    {
        public int? CityId { get; set; } 
        public string CityName { get; set; }
        public string CityCode { get; set; }  
        public int StateID { get; set; }    
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }
        public int CountryID { get; set; }

    }
}
