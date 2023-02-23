using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using PracticeModel.Models;

#region All Methods
namespace PracticeModel.Controllers
{
    #region Configuration Date
    public class LOC_CityController : Controller
    {
        private IConfiguration Configuration;
        public LOC_CityController(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }
        #endregion

        #region SelectAll Records
        public IActionResult Index()
        {
            DataTable dt = new DataTable();
            string str = this.Configuration.GetConnectionString("myConnectionString");
            SqlConnection conn = new SqlConnection(str);
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_LOC_City_SelectAll";
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            return View("LOC_CityList", dt);
        }
        #endregion

        #region Delete any Record
        public IActionResult Delete(int CityID)
        {
            DataTable dt = new DataTable();
            string str = this.Configuration.GetConnectionString("myConnectionString");
            SqlConnection conn = new SqlConnection(str);
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_LOC_City_DeleteByPK";
            cmd.Parameters.AddWithValue("@CityID", CityID);
            cmd.ExecuteNonQuery();
            conn.Close();
            return RedirectToAction("Index");
        }
        #endregion

        #region Add Records
        public IActionResult Add(int? CityID)
        {

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

            List<LOC_StateDropDownModel> list3 = new List<LOC_StateDropDownModel>();
            ViewBag.StateList = list3;


            if (CityID != null)
            {
                string str = this.Configuration.GetConnectionString("myConnectionString");
                SqlConnection conn = new SqlConnection(str);
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PR_LOC_City_SelectByPK";
                cmd.Parameters.Add("@CityID", SqlDbType.Int).Value = CityID;
                DataTable dt = new DataTable();
                SqlDataReader sdr = cmd.ExecuteReader();
                dt.Load(sdr);

                LOC_CityModel modelLOC_City = new LOC_CityModel();

                foreach (DataRow dr in dt.Rows)
                {
                    modelLOC_City.CityId = Convert.ToInt32(dr["CityID"]);
                    modelLOC_City.CityName = dr["CityName"].ToString();
                    modelLOC_City.CityCode = dr["CityCode"].ToString();
                    modelLOC_City.StateID = Convert.ToInt32(dr["StateID"]);
                    modelLOC_City.CreationDate = Convert.ToDateTime(dr["CreationDate"]);
                    modelLOC_City.ModificationDate = Convert.ToDateTime(dr["ModificationDate"]);
                    modelLOC_City.CountryID = Convert.ToInt32(dr["CountryID"]);
                }
                return View("LOC_CityAddEdit", modelLOC_City);
            }
            return View("LOC_CityAddEdit");
        }
        #endregion

        #region Save region
        [HttpPost]
        public IActionResult Save(LOC_CityModel modelLOC_City)
        {
            string str = this.Configuration.GetConnectionString("myConnectionString");
            SqlConnection conn = new SqlConnection(str);
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            if (modelLOC_City.CityId == null)
            {
                cmd.CommandText = "PR_LOC_City_Insert";
                //cmd.Parameters.Add("@CreationDate", SqlDbType.Date).Value = modelLOC_City.CreationDate;
                cmd.Parameters.Add("@CreationDate", SqlDbType.Date).Value = DBNull.Value;
            }
            else
            {
                cmd.CommandText = "PR_LOC_City_UpdateByPK";
                cmd.Parameters.Add("@CityID", SqlDbType.Int).Value = modelLOC_City.CityId;
            }
            cmd.Parameters.Add("@CityName", SqlDbType.NVarChar).Value = modelLOC_City.CityName;
            cmd.Parameters.Add("@CityCode", SqlDbType.NVarChar).Value = modelLOC_City.CityCode;
            cmd.Parameters.Add("@StateID", SqlDbType.NVarChar).Value = modelLOC_City.StateID;
            //cmd.Parameters.Add("@ModificationDate", SqlDbType.Date).Value = modelLOC_City.ModificationDate;
            cmd.Parameters.Add("@ModificationDate", SqlDbType.Date).Value = DBNull.Value;
            cmd.Parameters.Add("@CountryID", SqlDbType.Int).Value = modelLOC_City.CountryID;


            if (Convert.ToBoolean(cmd.ExecuteNonQuery()))
            {
                if (modelLOC_City.CityId == null)
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
    }
}
#endregion
