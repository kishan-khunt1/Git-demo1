using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using PracticeModel.Models;

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
                    modelCON_Contact.StateId = Convert.ToInt32(dr["StateId"]);
                    modelCON_Contact.ContactCreationDate = Convert.ToDateTime(dr["ContactCreationDate"]);
                    modelCON_Contact.ContactModificationDate = Convert.ToDateTime(dr["ContactModificationDate"]);
                    modelCON_Contact.CountryID = Convert.ToInt32(dr["CountryID"]);
                    modelCON_Contact.ContactCategoryID = Convert.ToInt32(dr["ContactCategoryID"]);

                }
                return View("CON_ContactAddEdit", modelCON_Contact);
            }
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
                cmd.Parameters.Add("@ContactCreationDate", SqlDbType.Date).Value = modelCON_Contact.ContactCreationDate;
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
            cmd.Parameters.Add("@StateID", SqlDbType.Int).Value = modelCON_Contact.StateId;
            cmd.Parameters.Add("@ContactModificationDate", SqlDbType.Date).Value =modelCON_Contact.ContactModificationDate;
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
            return View("CON_ContactAddEdit");
        }
        #endregion
    }
}
