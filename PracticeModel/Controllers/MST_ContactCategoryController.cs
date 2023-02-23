using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using PracticeModel.Models;

#region All Methods
namespace PracticeModel.Controllers
{
        #region Configuration Data
    public class MST_ContactCategoryController : Controller
    {
        private IConfiguration Configuration;
        public MST_ContactCategoryController(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }
        #endregion

        #region Select all Records

        public IActionResult Index()
        {
            DataTable dt = new DataTable();
            string str = this.Configuration.GetConnectionString("myConnectionString");
            SqlConnection conn = new SqlConnection(str);
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_MST_ContactCategory_SelectAll";
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            return View("MST_ContactCategoryList", dt);
        }
        #endregion

        #region Delete any Records
        public IActionResult Delete(int ContactCategoryID)
        {
            DataTable dt = new DataTable();
            string str = this.Configuration.GetConnectionString("myConnectionString");
            SqlConnection conn = new SqlConnection(str);
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_MST_ContactCategory_DeleteByPK";
            cmd.Parameters.AddWithValue("@ContactCategoryID", ContactCategoryID);
            cmd.ExecuteNonQuery();
            conn.Close();
            return RedirectToAction("Index");
        }
        #endregion

        #region Add Records
        public IActionResult Add(int? ContactCategoryID)
        {
            if (ContactCategoryID != null)
            {
                string str = this.Configuration.GetConnectionString("myConnectionString");
                SqlConnection conn = new SqlConnection(str);
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PR_MST_ContactCategory_SelectByPK";
                cmd.Parameters.Add("@ContactCategoryID", SqlDbType.Int).Value = ContactCategoryID;
                DataTable dt = new DataTable();
                SqlDataReader sdr = cmd.ExecuteReader();
                dt.Load(sdr);
                MST_ContactCategoryModel modelMST_ContactCategory = new MST_ContactCategoryModel();
                foreach (DataRow dr in dt.Rows)
                {
                    modelMST_ContactCategory.ContactCategoryID = Convert.ToInt32(dr["ContactCategoryID"]);
                    modelMST_ContactCategory.ContactCategoryName = dr["ContactCategoryName"].ToString();
                    modelMST_ContactCategory.CreationDate = Convert.ToDateTime(dr["CreationDate"]);
                    modelMST_ContactCategory.ModificationDate = Convert.ToDateTime(dr["ModificationDate"]);
                }
                return View("MST_ContactCategoryAddEdit", modelMST_ContactCategory);           
            }
            return View("MST_ContactCategoryAddEdit");
        }
        #endregion

        #region Save region
        [HttpPost]
        public IActionResult Save(MST_ContactCategoryModel modelMST_ContactCategory)
        {
            string str = this.Configuration.GetConnectionString("myConnectionString");
            SqlConnection conn = new SqlConnection(str);
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            if (modelMST_ContactCategory.ContactCategoryID == null)
            {
                cmd.CommandText = "PR_MST_ContactCategory_Insert";
                cmd.Parameters.Add("@CreationDate", SqlDbType.Date).Value = modelMST_ContactCategory.CreationDate;
            }
            else
            {
                cmd.CommandText = "PR_MST_ContactCategory_UpdateByPK";
                cmd.Parameters.Add("@ContactCategoryID", SqlDbType.Int).Value = modelMST_ContactCategory.ContactCategoryID;
            }
            cmd.Parameters.Add("@ContactCategoryName", SqlDbType.NVarChar).Value = modelMST_ContactCategory.ContactCategoryName;
            cmd.Parameters.Add("@ModificationDate", SqlDbType.Date).Value = modelMST_ContactCategory.ModificationDate;

            if (Convert.ToBoolean(cmd.ExecuteNonQuery()))
            {
                if (modelMST_ContactCategory.ContactCategoryID == null)
                {
                    TempData["SuccessMSG"] = "Record Inserted Successfully ! ";
                }
                else
                {
                    TempData["SuccessMSG"] = "Record Updated Successfully ! ";

                }
            }
            conn.Close();
            return View("MST_ContactCategoryAddEdit");
        }
        #endregion
    }
}
#endregion
