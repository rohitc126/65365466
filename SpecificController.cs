using BusinessLayer;
using BusinessLayer.DAL;
using BusinessLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eSGBIZ.Controllers
{
    public class SpecificController : BaseController
    {
        //
        // GET: /Specific/
         [Authorize]
        public ActionResult SpecificGravity()
        {
            Specific_Gravity_Abs _mixDesign = new Specific_Gravity_Abs();
            ViewBag.Header = "Specific Gravity & Water Absorption";

            var empExceptionList = new List<string> { "CAL0229", "CAL0230" };
            List<EMPLOYEE_DETAILS> _empList = new DAL_Common().GetEmployee_List("", "", "", "", "", emp.Employee_Code, "").Where(x => x.activeFlag == "Y" && !empExceptionList.Contains(x.Employee_Code)).ToList<EMPLOYEE_DETAILS>();
            _mixDesign.EMPLOYEE_LIST = new SelectList(_empList.OrderBy(x => x.EmployeeName), "Employee_Code", "EmployeeName");

            return View(_mixDesign); 
        }


        [HttpPost]
        [SubmitButtonSelector(Name = "Save")]
        [ActionName("SpecificGravity")]
        public ActionResult INSERT_SPECIFIC_GRAVITY_SAVE(Specific_Gravity_Abs _mixDesign)
        {
            var errors = ModelState.Where(x => x.Value.Errors.Count > 0).Select(x => new { x.Key, x.Value.Errors }).ToArray();

            if (ModelState.IsValid)
            {
                try
                {
                    ResultMessage objMst;
                    string result = new DAL_MATERIAL_REQUISITION_ENTRY().INSERT_SPECIFIC_GRAVITY(emp.Employee_Code, _mixDesign, out objMst);

                    if (result == "")
                    {
                        Success(string.Format("<b>SPECIFIC GRAVITY inserted successfully. Test No : </b> <b style='color:red'> " + objMst.CODE + "</b>"), true);
                       
                    }
                    else
                    {
                        Danger(string.Format("<b>Error:</b>" + result), true);
                    }
                }
                catch (Exception ex)
                {
                    Danger(string.Format("<b>Error:</b>" + ex.Message), true);
                }
            }
            else
            {
                Danger(string.Format("<b>Error:102 :</b>" + string.Join("; ", ModelState.Values.SelectMany(z => z.Errors).Select(z => z.ErrorMessage))), true);
            }
            var empExceptionList = new List<string> { "CAL0229", "CAL0230" };
            List<EMPLOYEE_DETAILS> _empList = new DAL_Common().GetEmployee_List("", "", "", "", "", emp.Employee_Code, "").Where(x => x.activeFlag == "Y" && !empExceptionList.Contains(x.Employee_Code)).ToList<EMPLOYEE_DETAILS>();
            _mixDesign.EMPLOYEE_LIST = new SelectList(_empList.OrderBy(x => x.EmployeeName), "Employee_Code", "EmployeeName");

        
            return View(_mixDesign);
        }
    }
}
