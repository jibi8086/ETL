public JsonResult LoginData(string UserName, string UserPassword)
        {
            int employeeId;
            string employeeName, employeeEmail;
            bool userType = false;
            try
            {
                ETLLoginService.ETLCenterLoginService loginService = new ETLLoginService.ETLCenterLoginService();
                loginService.Url = Constants.LoginService;
                string logDetails = loginService.Login(UserName, UserPassword);
                DataTable dt = JsonConvert.DeserializeObject<DataTable>(logDetails);
                employeeId = Convert.ToInt32(dt.Rows[0]["ID"]);
                if (employeeId > 0)
                {
                    employeeName = Convert.ToString(dt.Rows[0]["EmpName"]);
                    employeeEmail = Convert.ToString(dt.Rows[0]["EmpEmail"]);
                    Session["EmployeeID"] = employeeId;
                    userType = Convert.ToBoolean(dt.Rows[0]["EmpISAdmin"]);
                    Session["IsAdmin"] = Convert.ToInt32(userType);
                    Session["EmployeeName"] = employeeName;
                    Session["EmployeeEmail"] = employeeEmail;
                }
                return Json(new { id = employeeId, type = userType }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                CommonFunctions commonFun = new CommonFunctions();
                commonFun.ExceptionLog(ControllerContext.HttpContext, ex.Message, ex.TargetSite.Name,
                    Convert.ToString(ControllerContext.RouteData.Values["action"]),
                    Convert.ToString(ControllerContext.RouteData.Values["controller"]));
                commonFun = null;
                return Json(new { id = -1 }, JsonRequestBehavior.AllowGet);
            }
            finally
            {
                Dispose();
            }
        }
