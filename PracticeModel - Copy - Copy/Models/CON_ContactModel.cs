namespace PracticeModel.Models
{
    public class CON_ContactModel
    {
        public int? ContactID { get; set; }
        public string ContactName { get; set; }
        public string ContactMobileNo { get; set; }
        public string ContactAddress { get; set; }
        public string ContactEmail { get; set; }
        public int CityID { get; set; }
        public string CityName { get; set; }
        public int StateID { get; set; }
        public string StateName { get; set; }
        public DateTime ContactCreationDate { get; set; }
        public DateTime ContactModificationDate { get; set; }
        public int CountryID { get; set; }
        public string CountryName { get; set; }
        public int ContactCategoryID { get; set; }
        public string ContactCategoryName { get; set; }
    }
}