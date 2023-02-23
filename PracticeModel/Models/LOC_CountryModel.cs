namespace PracticeModel.Models
{
    public class LOC_CountryModel
    {
        public int? CountryID { get; set; }
        public string CountryName { get; set; }

        public string CountryCode { get; set; }

        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }

    }

    public class LOC_CountryDropDownModel
    {
        public int? CountryID { get; set; }

        public string CountryName { get; set; }
    }
}
