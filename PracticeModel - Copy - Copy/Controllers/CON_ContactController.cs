using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using PracticeModel.Models;

//pr_LOC_City_SelectionForDropDownByStateID


namespace PracticeModel.Controllers
{
    #region Configuration Data
    public class CON_ContactController : Controller
    {
        private IConfiguration Configuration;
        public CON_ContactController(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }
        #endregion

        #region Select All data
        public IActionResult Index()
        {
            DataTable dt = new DataTable();
            string str = this.Configuration.GetConnectionString("myConnectionString");
            SqlConnection conn = new SqlConnection(str);
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_CON_Contact_SelectAll";
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            return View("CON_ContactList", dt);
        }
        #endregion

        #region Delete any Record
        public IActionResult Delete(int ContactID)
        {
            DataTable dt = new DataTable();
            string str = this.Configuration.GetConnectionString("myConnectionString");
            SqlConnection conn = new SqlConnection(str);
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_CON_Contact_DeleteByPK";
            cmd.Parameters.AddWithValue("@ContactID", ContactID);
            cmd.ExecuteNonQuery();
            conn.Close();
            return RedirectToAction("Index");
        }
        #endregion

        #region Add Records
        public IActionResult Add(int? ContactID)
        {
            #region DropDown
            #region DropDown Country
            string str1 = this.Configuration.GetConnectionString("myConnectionString");
            SqlConnection conn1 = new SqlConnection(str1);
            conn1.Open();
            SqlCommand cmd1 = conn1.CreateCommand();
            cmd1.CommandType = CommandType.StoredProcedure;
            cmd1.CommandText = "PR_LOC_Country_SelectionForDropDown";
            DataTable dt1 = new DataTable();
            SqlDataReader sdr1 = cmd1.ExecuteReader();
            dt1.Load(sdr1);
            conn1.Close();

            List<LOC_CountryDropDownModel> list = new List<LOC_CountryDropDownModel>();
            foreach (DataRow dr in dt1.Rows)
            {
                LOC_CountryDropDownModel cntrlst = new LOC_CountryDropDownModel();
                cntrlst.CountryID = Convert.ToInt32(dr["CountryID"]);
                cntrlst.CountryName = dr["CountryName"].ToString();
                list.Add(cntrlst);
            }
            ViewBag.CountryList = list;


            #endregion

            
            List<LOC_StateDropDownModel> list2 = new List<LOC_StateDropDownModel>();
            ViewBag.StateList = list2;

            List<LOC_CityDropDownModel> list4 = new List<LOC_CityDropDownModel>();
            ViewBag.CityList= list4;

            #region DropDown Category
            string str5 = this.Configuration.GetConnectionString("myConnectionString");
            SqlConnection conn4 = new SqlConnection(str5);
            conn4.Open();
            SqlCommand cmd4 = conn4.CreateCommand();
            cmd4.CommandType = CommandType.StoredProcedure;
            cmd4.CommandText = "PR_MST_ContactCategory_SelectionForDropDown";
            DataTable dt5 = new DataTable();
            SqlDataReader sdr4 = cmd4.ExecuteReader();
            dt5.Load(sdr4);
            conn4.Close();

            List<MST_ContactCategoryDropDownModel> list5 = new List<MST_ContactCategoryDropDownModel>();
            foreach (DataRow dr5 in dt5.Rows)
            {
                MST_ContactCategoryDropDownModel ccddlst = new MST_ContactCategoryDropDownModel();
                ccddlst.ContactCategoryID = Convert.ToInt32(dr5["ContactCategoryID"]);
                ccddlst.ContactCategoryName = dr5["ContactCategoryName"].ToString();
                list5.Add(ccddlst);
            }
            ViewBag.ContactCategoryList = list5;


            #endregion
            

            #region Add
            if (ContactID != null)
            {
                string str = this.Configuration.GetConnectionString("myConnectionString");
                SqlConnection conn = new SqlConnection(str);
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PR_CON_Contact_SelectByPK";
                cmd.Parameters.Add("@ContactID", SqlDbType.Int).Value = ContactID;
                DataTable dt = new DataTable();
                SqlDataReader sdr = cmd.ExecuteReader();
                dt.Load(sdr);
                CON_ContactModel modelCON_Contact = new CON_ContactModel();
                foreach (DataRow dr in dt.Rows)
                {
                    modelCON_Contact.ContactID = Convert.ToInt32(dr["ContactID"]);
                    modelCON_Contact.ContactName = dr["ContactName"].ToString();
                    modelCON_Contact.ContactMobileNo = dr["ContactMobileNo"].ToString();
                    modelCON_Contact.ContactAddress = dr["ContactAddress"].ToString();
                    modelCON_Contact.ContactEmail = dr["ContactEmail"].ToString();
                    modelCON_Contact.CityID = Convert.ToInt32(dr["CityID"]);
                    modelCON_Contact.StateID = Convert.ToInt32(dr["StateId"]);
                    modelCON_Contact.ContactCreationDate = Convert.ToDateTime(dr["ContactCreationDate"]);
                    modelCON_Contact.ContactModificationDate = Convert.ToDateTime(dr["ContactModificationDate"]);
                    modelCON_Contact.CountryID = Convert.ToInt32(dr["CountryID"]);
                    modelCON_Contact.ContactCategoryID = Convert.ToInt32(dr["ContactCategoryID"]);

                }
                return View("CON_ContactAddEdit", modelCON_Contact);
            }
            #endregion
            return View("CON_ContactAddEdit");
        }
        #endregion

        #region Save region
        [HttpPost]
        public IActionResult Save(CON_ContactModel modelCON_Contact)
        {
            string str = this.Configuration.GetConnectionString("myConnectionString");
            SqlConnection conn = new SqlConnection(str);
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            if (modelCON_Contact.ContactID == null)
            {
                cmd.CommandText = "PR_CON_Contact_Insert";
                //cmd.Parameters.Add("@ContactCreationDate", SqlDbType.Date).Value = modelCON_Contact.ContactCreationDate;
                cmd.Parameters.Add("@ContactCreationDate", SqlDbType.Date).Value = DBNull.Value;
            }
            else
            {
                cmd.CommandText = "PR_CON_Contact_UpdateByPK";
                cmd.Parameters.Add("@ContactID", SqlDbType.Int).Value = modelCON_Contact.ContactID;
            }
            cmd.Parameters.Add("@ContactName", SqlDbType.NVarChar).Value = modelCON_Contact.ContactName;
            cmd.Parameters.Add("@ContactMobileNo", SqlDbType.NVarChar).Value = modelCON_Contact.ContactMobileNo;
            cmd.Parameters.Add("@ContactAddress", SqlDbType.NVarChar).Value = modelCON_Contact.ContactAddress;
            cmd.Parameters.Add("@ContactEmail", SqlDbType.NVarChar).Value = modelCON_Contact.ContactEmail;
            cmd.Parameters.Add("@CountryID", SqlDbType.Int).Value = modelCON_Contact.CountryID;
            cmd.Parameters.Add("@StateID", SqlDbType.Int).Value = modelCON_Contact.StateID;
            //cmd.Parameters.Add("@ContactModificationDate", SqlDbType.Date).Value =modelCON_Contact.ContactModificationDate;
            cmd.Parameters.Add("@ContactModificationDate", SqlDbType.Date).Value = DBNull.Value;
            cmd.Parameters.Add("@CityID", SqlDbType.Int).Value = modelCON_Contact.CityID;
            cmd.Parameters.Add("@ContactCategoryID", SqlDbType.Int).Value = modelCON_Contact.ContactCategoryID;


            if (Convert.ToBoolean(cmd.ExecuteNonQuery()))
            {
                if (modelCON_Contact.ContactID == null)
                {
                    TempData["SuccessMSG"] = "Record Inserted Successfully ! ";
                }
                else
                {
                    TempData["SuccessMSG"] = "Record Updated Successfully ! ";

                }
            }
            conn.Close();
            return RedirectToAction("Index");
        }
        #endregion



        [HttpPost]
        public IActionResult DropdownByCountry(int CountryID)
        {
            #region DropDown State
            string str2 = this.Configuration.GetConnectionString("myConnectionString");
            SqlConnection conn2 = new SqlConnection(str2);
            conn2.Open();
            SqlCommand cmd2 = conn2.CreateCommand();
            cmd2.CommandType = CommandType.StoredProcedure;
            cmd2.CommandText = "PR_LOC_State_SelectionForDropDownByCountryID";
            cmd2.Parameters.AddWithValue("@CountryID", CountryID);
            DataTable dt3 = new DataTable();
            SqlDataReader sdr2 = cmd2.ExecuteReader();
            dt3.Load(sdr2);
            conn2.Close();
            List<LOC_StateDropDownModel> list2 = new List<LOC_StateDropDownModel>();
            foreach (DataRow dr3 in dt3.Rows)
            {
                LOC_StateDropDownModel sdmlst = new LOC_StateDropDownModel();
                sdmlst.StateID = Convert.ToInt32(dr3["StateID"]);
                sdmlst.StateName = dr3["StateName"].ToString();
                list2.Add(sdmlst);
            }
            var vModel = list2;
            return Json(vModel);
            #endregion
        }

        public IActionResult DropdownByState(int StateID)
        {
            string str3 = this.Configuration.GetConnectionString("myConnectionString");
            SqlConnection conn3 = new SqlConnection(str3);
            conn3.Open();
            SqlCommand cmd3 = conn3.CreateCommand();
            cmd3.CommandType = CommandType.StoredProcedure;
            cmd3.CommandText = "PR_LOC_City_SelectionForDropDownByStateID";
            cmd3.Parameters.AddWithValue("StateID", StateID);
            DataTable dt4 = new DataTable();
            SqlDataReader sdr3 = cmd3.ExecuteReader();
            dt4.Load(sdr3);
            conn3.Close();

            List<LOC_CityDropDownModel> list4 = new List<LOC_CityDropDownModel>();
            foreach (DataRow dr4 in dt4.Rows)
            {
                LOC_CityDropDownModel ctylst = new LOC_CityDropDownModel();
                ctylst.CityID = Convert.ToInt32(dr4["CityID"]);
                ctylst.CityName = dr4["CityName"].ToString();
                list4.Add(ctylst);
            }
            var vmodel = list4;
            return Json(vmodel);
            
        }
    }
}
#endregion

        

