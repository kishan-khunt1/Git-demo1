using Microsoft.AspNetCore.Mvc;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using PracticeModel.Models;

namespace PracticeModel.Controllers
{
    #region Configuration Data
    public class LOC_StateController : Controller
{
        private IConfiguration Configuration;
        public LOC_StateController(IConfiguration _configuration)
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
            cmd.CommandText = "PR_LOC_State_SelectAll";
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            return View("LOC_StateList", dt);
        }
        #endregion

    #region Delete any Record
        public IActionResult Delete(int StateID)
        {
            DataTable dt = new DataTable();
            string str = this.Configuration.GetConnectionString("myConnectionString");
            SqlConnection conn = new SqlConnection(str);
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_LOC_State_DeleteByPK";
            cmd.Parameters.AddWithValue("@StateID", StateID);
            cmd.ExecuteNonQuery();
            conn.Close();
            return RedirectToAction("Index");
        }
        #endregion

    #region Add Records
        public IActionResult Add(int? StateID)
        {
            //    #region DropDown
            //DataTable dt1 = new DataTable();
            //string str1 = this.Configuration.GetConnectionString("myConnectionString");
            //SqlConnection conn1 = new SqlConnection(str1);
            //conn1.Open();
            //SqlCommand cmd1  = conn1.CreateCommand();
            //cmd1.CommandType = CommandType.StoredProcedure;
            //cmd1.CommandText = "PR_LOC_Country_SelectionForDropDown";
            //SqlDataReader sdr1 = cmd1.ExecuteReader();
            //dt1.Load(sdr1);
            //conn1.Close();

            //List<LOC_CountryDropDownModel> list = new List<LOC_CountryDropDownModel>();
            //foreach ( DataRow dr in dt1.Rows )
            //{
            //    LOC_CountryDropDownModel list1 = new LOC_CountryDropDownModel();
            //    list1.CountryID = Convert.ToInt32(dr["CountryID"]);
            //    list1.CountryName = dr["CountryName"].ToString();
            //    list.Add(list1);
            //}
            //ViewBag.CountryList =list;
            //#endregion

                #region SelectByPK
            if (StateID != null)
            {
                string str = this.Configuration.GetConnectionString("myConnectionString");
                SqlConnection conn = new SqlConnection(str);
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PR_LOC_State_SelectByPK";
                cmd.Parameters.Add("@StateID", SqlDbType.Int).Value = StateID;
                DataTable dt = new DataTable();
                SqlDataReader sdr = cmd.ExecuteReader();
                dt.Load(sdr);
                LOC_StateModel modelLOC_State = new LOC_StateModel();

                foreach ( DataRow dr in dt.Rows )
                {
                    modelLOC_State.StateID= Convert.ToInt32(dr["StateID"]);
                    modelLOC_State.StateName =dr["StateName"].ToString();
                    modelLOC_State.StateCode =dr["StateCode"].ToString();
                    modelLOC_State.CountryID= Convert.ToInt32(dr["CountryID"]);
                    modelLOC_State.CreationDate = Convert.ToDateTime(dr["CreationDate"]);
                    modelLOC_State.ModificationDate = Convert.ToDateTime(dr["ModificationDate"]);
                }
                return View("LOC_StateAddEdit", modelLOC_State);
            }
            return View("LOC_StateAddEdit");
            #endregion
        }
        #endregion

        #region Save region
        [HttpPost]
        public IActionResult Save(LOC_StateModel modelLOC_State)
        {
            string str = this.Configuration.GetConnectionString("myConnectionString");
            SqlConnection conn = new SqlConnection(str);
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;


            if (modelLOC_State.StateID == null)
            {
                cmd.CommandText = "PR_LOC_State_Insert";
                cmd.Parameters.Add("@CreationDate", SqlDbType.Date).Value = modelLOC_State.CreationDate;
            }
            else
            {
                cmd.CommandText = "PR_LOC_State_UpdateByPK";
                cmd.Parameters.Add("@StateID", SqlDbType.Int).Value = modelLOC_State.StateID;
            }
            cmd.Parameters.Add("@StateName", SqlDbType.NVarChar).Value = modelLOC_State.StateName;
            cmd.Parameters.Add("@StateCode", SqlDbType.NVarChar).Value = modelLOC_State.StateCode;
            cmd.Parameters.Add("@CountryID", SqlDbType.Int).Value = modelLOC_State.CountryID;
            cmd.Parameters.Add("@ModificationDate", SqlDbType.Date).Value = modelLOC_State.ModificationDate;

            if (Convert.ToBoolean(cmd.ExecuteNonQuery()))
            {
                if (modelLOC_State.StateID == null)
                {
                    TempData["SuccessMSG"] = "Record Inserted Successfully ! ";
                }
                else
                {
                    TempData["SuccessMSG"] = "Record Updated Successfully ! ";

                }
            }
            conn.Close();
            return View("LOC_StateAddEdit");
        }
        #endregion

    }
}
