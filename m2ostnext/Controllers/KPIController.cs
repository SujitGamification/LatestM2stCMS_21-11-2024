using m2ostnext;
using m2ostnext.Models;
using Microsoft.Office.Interop.Excel;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace m2ostnext.Controllers
{
    [UserFilter]
    public class KPIController : Controller
    {
        private db_m2ostEntities db = new db_m2ostEntities();

        public KPIController()
        {
        }

        public ActionResult AddKPI()
        {
            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];
            if (userSession != null)
            {
                string orgid1 = userSession.id_ORGANIZATION;
                string id_user = userSession.ID_USER;
                string Page1 = "Add KPI";
                UserLogDetails userLogDetails = new UserLogDetails();

                userLogDetails.AddUserDataLog(id_user, orgid1, Page1);
                // Now you have orgid and id_user available for further processing
            }

            return base.View();
        }

        public List<tbl_assessment> getAssesmentList(int CID, int OID)
        {
            DateTime now = DateTime.Now;
            string str = string.Concat("", " SELECT p.* FROM tbl_assessment p left join  tbl_assessment_sheet a LEFT JOIN tbl_assessment_categoty_mapping b ON a.id_assessment_sheet = b.id_assessment_sheet ");
            str = string.Concat(new object[] { str, " on p.id_assessment=a.id_assesment WHERE a.status = 'A' and a.id_organization=", OID, " AND b.id_category = ", CID, " " });
            return this.db.tbl_assessment.SqlQuery(str, new object[0]).ToList<tbl_assessment>();
        }

        public string getProgramKPI(string id)
        {
            string str = "";
            UserSession item = (UserSession)base.HttpContext.Session.Contents["UserSession"];
            int num = Convert.ToInt32(item.id_ORGANIZATION);
            Convert.ToInt32(item.ID_USER);
            int num1 = Convert.ToInt32(id);
            string str1 = string.Concat(new object[] { "SELECT a.* FROM tbl_kpi_master a, tbl_kpi_program_scoring b where a.id_organization=", num, " and a.id_kpi_master=b.id_kpi_master and b.kpi_type=3 and b.id_category=", num1, " " });
            List<tbl_kpi_master> list = this.db.tbl_kpi_master.SqlQuery(str1, new object[0]).ToList<tbl_kpi_master>();
            if (list.Count <= 0)
            {
                str = "0";
            }
            else
            {
                foreach (tbl_kpi_master tblKpiMaster in list)
                {
                    str = string.Concat(new object[] { str, " <option value=\"", tblKpiMaster.id_kpi_master, "\" >", tblKpiMaster.kpi_name, "</option> " });
                }
            }
            return str;
        }

        public ActionResult Index()
        {
            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];
            if (userSession != null)
            {
                string orgid1 = userSession.id_ORGANIZATION;
                string id_user = userSession.ID_USER;
                string Page1 = "List KPI";
                UserLogDetails userLogDetails = new UserLogDetails();

                userLogDetails.AddUserDataLog(id_user, orgid1, Page1);
                // Now you have orgid and id_user available for further processing
            }
            UserSession item = (UserSession)base.HttpContext.Session.Contents["UserSession"];
            int num = Convert.ToInt32(item.id_ORGANIZATION);
            Convert.ToInt32(item.ID_USER);
            //string str = string.Concat("select * from tbl_kpi_master where id_organization=", num) ?? "";
            //List<tbl_kpi_master> list = this.db.tbl_kpi_master.SqlQuery(str, new object[0]).ToList<tbl_kpi_master>();
            KPI_name_service list = new KPI_name_service();
            base.ViewData["master"] = list.GetKpiMasterDetails(num);
            //string str = string.Concat("select * from tbl_kpi_master_details where IsActive ='A' AND id_organization=", num) ?? "";

            //List<tbl_kpi_master_details> list = this.db.tbl_kpi_master_details.SqlQuery(str, new object[0]).ToList<tbl_kpi_master_details>();
            //base.ViewData["master"] = list;
            return base.View();
        }

        public ActionResult kpi_add_action()
        {
            UserSession item = (UserSession)base.HttpContext.Session.Contents["UserSession"];
            int num = Convert.ToInt32(item.id_ORGANIZATION);
            int num1 = Convert.ToInt32(item.ID_USER);
            tbl_kpi_master tblKpiMaster = new tbl_kpi_master()
            {
                id_creator = new int?(num1),
                id_organization = new int?(num),
                kpi_name = base.Request.Form["kpi-title"].ToString(),
                kpi_description = base.Request.Form["kpi-desc"].ToString()
            };
            DateTime now = DateTime.Now;
            tblKpiMaster.KPIID = string.Concat("MKPI", now.ToString("yyyyMMddHHmmss"));
            tblKpiMaster.kpi_type = new int?(Convert.ToInt32(base.Request.Form["kpi-type"].ToString()));
            tblKpiMaster.expiry_date = new DateTime?((new Utility()).StringToDatetime(base.Request.Form["kpi-expiry-date"].ToString()));
            tblKpiMaster.start_date = new DateTime?((new Utility()).StringToDatetime(base.Request.Form["kpi-start-date"].ToString()));
            tblKpiMaster.status = "A";
            tblKpiMaster.updated_date_time = new DateTime?(DateTime.Now);
            this.db.tbl_kpi_master.Add(tblKpiMaster);
            this.db.SaveChanges();
            if (tblKpiMaster.id_kpi_master <= 0)
            {
                return base.View();
            }
            int num2 = Convert.ToInt32(base.Request.Form["grid-value"].ToString());
            for (int i = 1; i <= num2; i++)
            {
                tbl_kpi_grid tblKpiGrid = new tbl_kpi_grid()
                {
                    kpi_value = new int?(Convert.ToInt32(base.Request.Form[string.Concat("kpi-value-", i)].ToString())),
                    kpi_text = base.Request.Form[string.Concat("kpi-text-", i)].ToString(),
                    start_range = new double?(Convert.ToDouble(base.Request.Form[string.Concat("kpi-start-", i)].ToString())),
                    end_range = new double?(Convert.ToDouble(base.Request.Form[string.Concat("kpi-end-", i)].ToString())),
                    status = "A",
                    updated_date_time = new DateTime?(DateTime.Now),
                    id_kpi_master = new int?(tblKpiMaster.id_kpi_master)
                };
                this.db.tbl_kpi_grid.Add(tblKpiGrid);
                this.db.SaveChanges();
            }
            return base.RedirectToAction("KPIGrid", "KPI", new { kpid = tblKpiMaster.KPIID, flag = 1 });
        }

        public ActionResult KPIDataUpload()
        {
            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];
            if (userSession != null)
            {
                string orgid1 = userSession.id_ORGANIZATION;
                string id_user = userSession.ID_USER;
                string Page1 = "Upload KPI Score";
                UserLogDetails userLogDetails = new UserLogDetails();

                userLogDetails.AddUserDataLog(id_user, orgid1, Page1);
                // Now you have orgid and id_user available for further processing
            }
            UserSession item = (UserSession)base.HttpContext.Session.Contents["UserSession"];
            int num = Convert.ToInt32(item.id_ORGANIZATION);
            Convert.ToInt32(item.ID_USER);
            string str = "select distinct a.* from tbl_category a , tbl_kpi_program_scoring b";
            str = string.Concat(str, string.Concat(" where a.ID_CATEGORY = b.id_category and b.kpi_type = 3 and a.CATEGORY_TYPE in (0,1) and a.ID_ORGANIZATION = ", num));
            List<tbl_category> list = this.db.tbl_category.SqlQuery(str, new object[0]).ToList<tbl_category>();
            base.ViewData["category"] = list;
            return base.View();
        }

        public FileResult KPIExcel(string ids)
        {
            string[] strArrays = ids.Split(new char[] { '|' });
            UserSession item = (UserSession)base.HttpContext.Session.Contents["UserSession"];
            Convert.ToInt32(item.id_ORGANIZATION);
            Convert.ToInt32(item.ID_USER);
            int num = Convert.ToInt32(strArrays[0]);
            int num1 = Convert.ToInt32(strArrays[1]);
            tbl_kpi_master tblKpiMaster = this.db.tbl_kpi_master.Find(new object[] { num });
            if (tblKpiMaster == null)
            {
                return null;
            }
            tbl_category tblCategory = (
                from t in this.db.tbl_category
                where t.ID_CATEGORY == num1
                select t).FirstOrDefault<tbl_category>();
            if (tblCategory == null)
            {
                return null;
            }
            Application variable = (Application)Activator.CreateInstance(Marshal.GetTypeFromCLSID(new Guid("00024500-0000-0000-C000-000000000046")));
            variable.DisplayAlerts = false;
            if (variable == null)
            {
                return null;
            }
            object value = Missing.Value;
            Workbook variable1 = variable.Workbooks.Add(value);
            Worksheet item1 = (Worksheet)((dynamic)variable1.Worksheets[1]);
            item1.Cells[1, 1] = "Dont delete row 2.Duplicate KPI ID,KPI Name,PROGRAM from row 3 for each user you want to add.Do not change the order of the column.";
            item1.Cells[1, 2] = "";
            item1.Cells[1, 3] = "";
            item1.Cells[1, 4] = "";
            item1.Cells[1, 5] = "";
            item1.Cells[1, 6] = "";
            item1.Cells[1, 7] = "";
            item1.Cells[2, 1] = "KPI ID (Requiered,Mandatory)";
            item1.Cells[2, 2] = "KPI Name (R,M)";
            item1.Cells[2, 3] = "PROGRAM (R,M)";
            item1.Cells[2, 4] = "USERID  (R,M)";
            item1.Cells[2, 5] = "EMPLOYEEID (Optional)";
            item1.Cells[2, 6] = "Score (R,M)";
            item1.Cells[2, 7] = "Date (DD-MM-YYYY)";
            item1.Cells[3, 1] = tblKpiMaster.KPIID.Trim();
            item1.Cells[3, 2] = tblKpiMaster.kpi_name.Trim();
            item1.Cells[3, 3] = tblCategory.CATEGORYNAME.Trim();
            item1.Cells[3, 4] = "";
            item1.Cells[3, 5] = "";
            item1.Cells[3, 6] = "";
            item1.Cells[3, 7] = "";
            item1.Cells["a1", "f1"].Merge(false);
            string str = base.HttpContext.Server.MapPath("~/Content/SKILLMUNI_DATA/EXCEL/");
            if (!Directory.Exists(str))
            {
                Directory.CreateDirectory(str);
            }
            variable1.SaveCopyAs(string.Concat(str, "KPIUploadExport.xls"));
            variable1.Close(true, value, value);
            variable.Quit();
            Marshal.ReleaseComObject(item1);
            Marshal.ReleaseComObject(variable1);
            Marshal.ReleaseComObject(variable);
            string str1 = string.Concat(str, "KPIUploadExport.xls");
            FileInfo fileInfo = new FileInfo(str1);
            return new FilePathResult(str1, MimeMapping.GetMimeMapping(str1));
        }

        public ActionResult KPIGrid(string kpid, int flag)
        {
            UserSession item = (UserSession)base.HttpContext.Session.Contents["UserSession"];
            int num = Convert.ToInt32(item.id_ORGANIZATION);
            Convert.ToInt32(item.ID_USER);
            string str = string.Concat(new object[] { "select * from tbl_kpi_master where id_organization=", num, " and KPIID ='", kpid, "'" });
            tbl_kpi_master tblKpiMaster = this.db.tbl_kpi_master.SqlQuery(str, new object[0]).FirstOrDefault<tbl_kpi_master>();
            if (tblKpiMaster != null)
            {
                string str1 = string.Concat("select * from tbl_kpi_grid where id_kpi_master=", tblKpiMaster.id_kpi_master) ?? "";
                List<tbl_kpi_grid> list = this.db.tbl_kpi_grid.SqlQuery(str1, new object[0]).ToList<tbl_kpi_grid>();
                base.ViewData["master"] = tblKpiMaster;
                base.ViewData["grids"] = list;
            }
            base.ViewData["master"] = tblKpiMaster;
            base.ViewData["flag"] = flag;
            return base.View();
        }

        public ActionResult KPIUploadAction()
        {
            List<KPIUpload> kPIUploads = new List<KPIUpload>();
            UserSession item = (UserSession)base.HttpContext.Session.Contents["UserSession"];
            int num = Convert.ToInt32(item.id_ORGANIZATION);
            int num1 = Convert.ToInt32(base.Request.Form["category-select"].ToString());
            Convert.ToInt32(base.Request.Form["select-kpi-master"].ToString());
            string str = "TRNKPI000000001";
            tbl_category tblCategory = (
                from t in this.db.tbl_category
                where t.ID_CATEGORY == num1 && t.STATUS == "A" && t.ID_ORGANIZATION == num
                select t).FirstOrDefault<tbl_category>();
            if (tblCategory != null)
            {
                int i = 0;
                if (base.Request != null)
                {
                    HttpPostedFileBase httpPostedFileBase = base.Request.Files["uploadBtn"];
                    if (httpPostedFileBase != null && (httpPostedFileBase.ContentLength >= 0 || !string.IsNullOrEmpty(httpPostedFileBase.FileName)))
                    {
                        string fileName = httpPostedFileBase.FileName;
                        string contentType = httpPostedFileBase.ContentType;
                        byte[] numArray = new byte[httpPostedFileBase.ContentLength];
                        httpPostedFileBase.InputStream.Read(numArray, 0, Convert.ToInt32(httpPostedFileBase.ContentLength));
                        using (ExcelPackage excelPackage = new ExcelPackage(httpPostedFileBase.InputStream))
                        {
                            ExcelWorksheet excelWorksheet = excelPackage.Workbook.Worksheets.FirstOrDefault<ExcelWorksheet>();
                            if (excelWorksheet != null)
                            {
                                int column = excelWorksheet.Dimension.End.Column;
                                int row = excelWorksheet.Dimension.End.Row;
                                for (i = 3; i <= row; i++)
                                {
                                    KPIUpload kPIUpload = new KPIUpload();
                                    if (excelWorksheet.Cells[i, 1].Value != null)
                                    {
                                        try
                                        {
                                            kPIUpload.USERID = excelWorksheet.Cells[i, 4].Value.ToString();
                                            string str1 = string.Concat(new object[] { "select * from tbl_user where upper(USERID) like upper('", kPIUpload.USERID, "') and id_organization=", num, " " });
                                            tbl_user tblUser = this.db.tbl_user.SqlQuery(str1, new object[0]).FirstOrDefault<tbl_user>();
                                            if (tblUser != null)
                                            {
                                                kPIUpload.KPIID = excelWorksheet.Cells[i, 1].Value.ToString();
                                                kPIUpload.KPIName = excelWorksheet.Cells[i, 2].Value.ToString();
                                                kPIUpload.CategoryName = excelWorksheet.Cells[i, 3].Value.ToString();
                                                kPIUpload.id_category = tblCategory.ID_CATEGORY;
                                                string str2 = excelWorksheet.Cells[i, 5].Value.ToString();
                                                if (str2 != null)
                                                {
                                                    kPIUpload.EMPLOYEEID = str2.ToString();
                                                }
                                                else
                                                {
                                                    kPIUpload.EMPLOYEEID = "";
                                                }
                                                kPIUpload.SCORE_TEXT = Convert.ToString(excelWorksheet.Cells[i, 6].Value);
                                                kPIUpload.SCORE = double.Parse(kPIUpload.SCORE_TEXT);
                                                kPIUpload.id_user = tblUser.ID_USER;
                                                string str3 = string.Concat("select * from tbl_kpi_master where upper(KPIID) like upper('", kPIUpload.KPIID, "')");
                                                tbl_kpi_master tblKpiMaster = this.db.tbl_kpi_master.SqlQuery(str3, new object[0]).FirstOrDefault<tbl_kpi_master>();
                                                kPIUpload.id_kpi_master = tblKpiMaster.id_kpi_master;
                                                DateTime datetime = (new Utility()).StringToDatetime(excelWorksheet.Cells[i, 7].Value.ToString());
                                                kPIUpload.LOGDATE = datetime;
                                                kPIUploads.Add(kPIUpload);
                                            }
                                        }
                                        catch (Exception exception)
                                        {
                                            throw exception;
                                        }
                                    }
                                }
                                foreach (KPIUpload kPIUpload1 in kPIUploads)
                                {
                                    sc_program_kpi_score scProgramKpiScore = (
                                        from t in this.db.sc_program_kpi_score
                                        where t.id_category == (int?)kPIUpload1.id_category && t.id_kpi_master == (int?)kPIUpload1.id_kpi_master && t.id_user == (int?)kPIUpload1.id_user
                                        select t).FirstOrDefault<sc_program_kpi_score>();
                                    if (scProgramKpiScore != null)
                                    {
                                        continue;
                                    }
                                    scProgramKpiScore = new sc_program_kpi_score()
                                    {
                                        id_category = new int?(tblCategory.ID_CATEGORY),
                                        id_organization = new int?(num),
                                        id_user = new int?(kPIUpload1.id_user),
                                        log_datetime = new DateTime?(kPIUpload1.LOGDATE),
                                        updated_date_time = new DateTime?(DateTime.Now),
                                        status = "A",
                                        score = new double?(kPIUpload1.SCORE),
                                        id_kpi_master = new int?(kPIUpload1.id_kpi_master),
                                        transactionid = str
                                    };
                                    this.db.sc_program_kpi_score.Add(scProgramKpiScore);
                                    this.db.SaveChanges();
                                }
                            }
                        }
                    }
                }
            }
            (new Thread(() => this.setKPIWeightage(str))).Start();
            return base.RedirectToAction("KPIDataUpload", "KPI");
        }

        public ActionResult program_scoring_action()
        {
            UserSession item = (UserSession)base.HttpContext.Session.Contents["UserSession"];
            int num = Convert.ToInt32(item.id_ORGANIZATION);
            Convert.ToInt32(item.ID_USER);
            tbl_kpi_program_scoring tblKpiProgramScoring = new tbl_kpi_program_scoring()
            {
                id_category = new int?(Convert.ToInt32(base.Request.Form["id_category"].ToString())),
                id_kpi_master = new int?(Convert.ToInt32(base.Request.Form["content-kpi"].ToString())),
                id_organization = new int?(num),
                ps_weightage = new double?(Convert.ToDouble(base.Request.Form["content-kpi-weightage"])),
                status = "A",
                id_assessment = new int?(0),
                updated_date_time = new DateTime?(DateTime.Now),
                kpi_type = new int?(1)
            };
            this.db.tbl_kpi_program_scoring.Add(tblKpiProgramScoring);
            this.db.SaveChanges();
            int num1 = Convert.ToInt32(base.Request.Form["assessment-count"].ToString());
            for (int i = 1; i <= num1; i++)
            {
                tbl_kpi_program_scoring tblKpiProgramScoring1 = new tbl_kpi_program_scoring()
                {
                    id_category = new int?(Convert.ToInt32(base.Request.Form["id_category"].ToString())),
                    id_kpi_master = new int?(Convert.ToInt32(base.Request.Form[string.Concat("assessment-kpi-", i)].ToString())),
                    id_organization = new int?(num),
                    id_assessment = new int?(Convert.ToInt32(base.Request.Form[string.Concat("assessment-name-", i)].ToString())),
                    ps_weightage = new double?(Convert.ToDouble(base.Request.Form[string.Concat("assessement-kpi-weightage-", i)])),
                    status = "A",
                    updated_date_time = new DateTime?(DateTime.Now),
                    kpi_type = new int?(2)
                };
                this.db.tbl_kpi_program_scoring.Add(tblKpiProgramScoring1);
                this.db.SaveChanges();
            }
            int num2 = Convert.ToInt32(base.Request.Form["new-kpi-row"].ToString());
            for (int j = 0; j <= num2; j++)
            {
                if (base.Request.Form[string.Concat("extra-kpi-", j)] != null)
                {
                    tbl_kpi_program_scoring tblKpiProgramScoring2 = new tbl_kpi_program_scoring()
                    {
                        id_category = new int?(Convert.ToInt32(base.Request.Form["id_category"].ToString())),
                        id_kpi_master = new int?(Convert.ToInt32(base.Request.Form[string.Concat("extra-kpi-", j)].ToString())),
                        id_organization = new int?(num),
                        id_assessment = new int?(0),
                        ps_weightage = new double?(Convert.ToDouble(base.Request.Form[string.Concat("extra-kpi-weightage-", j)])),
                        status = "A",
                        updated_date_time = new DateTime?(DateTime.Now),
                        kpi_type = new int?(3)
                    };
                    this.db.tbl_kpi_program_scoring.Add(tblKpiProgramScoring2);
                    this.db.SaveChanges();
                }
            }
            return base.RedirectToAction("ScoringDashboard", "KPI");
        }

        public ActionResult ProgramScoring(string pids)
        {
            UserSession item = (UserSession)base.HttpContext.Session.Contents["UserSession"];
            int num = Convert.ToInt32(item.id_ORGANIZATION);
            Convert.ToInt32(item.ID_USER);
            string str = string.Concat(new object[] { "select * from tbl_category where id_category=", pids, " and CATEGORY_TYPE in (0,1) and status='A' and id_organization=", num, " " });
            tbl_category tblCategory = this.db.tbl_category.SqlQuery(str, new object[0]).FirstOrDefault<tbl_category>();
            if (tblCategory == null)
            {
                return base.RedirectToAction("Index", "KPI");
            }
            List<tbl_assessment> assesmentList = this.getAssesmentList(tblCategory.ID_CATEGORY, num);
            string str1 = string.Concat("select * from tbl_kpi_master where kpi_type=1 and id_organization=", num, " ");
            List<tbl_kpi_master> list = this.db.tbl_kpi_master.SqlQuery(str1, new object[0]).ToList<tbl_kpi_master>();
            string str2 = string.Concat("select * from tbl_kpi_master where kpi_type=2 and id_organization=", num, " ");
            List<tbl_kpi_master> tblKpiMasters = this.db.tbl_kpi_master.SqlQuery(str2, new object[0]).ToList<tbl_kpi_master>();
            string str3 = string.Concat("select * from tbl_kpi_master where kpi_type=3 and id_organization=", num, " ");
            List<tbl_kpi_master> list1 = this.db.tbl_kpi_master.SqlQuery(str3, new object[0]).ToList<tbl_kpi_master>();
            List<ProgramScoringClass> programScoringClasses = new List<ProgramScoringClass>();
            string str4 = string.Concat("", "  SELECT a.id_kpi_program_scoring, a.id_kpi_master, b.kpi_name, a.id_category,    a.id_assessment,     ");
            str4 = string.Concat(str4, "   CASE WHEN a.id_assessment > 0 THEN d.assessment_title ELSE ''    END assessment_title, ");
            str4 = string.Concat(str4, " ps_weightage,    a.id_organization, a.kpi_type, ");
            str4 = string.Concat(str4, " CASE WHEN a.kpi_type = 3 THEN 'KPI' WHEN a.kpi_type = 1 THEN 'Content KPI' WHEN a.kpi_type = 2 THEN 'Assessment KPI' END kpi_type_lable ");
            str4 = string.Concat(str4, " FROM tbl_kpi_program_scoring a LEFT JOIN tbl_kpi_master b ON a.id_kpi_master = b.id_kpi_master        LEFT JOIN ");
            str4 = string.Concat(str4, " tbl_category c ON c.ID_CATEGORY = a.id_category LEFT JOIN tbl_assessment d ON d.id_assessment = a.id_assessment AND d.id_assessment != NULL  where a.id_category=", pids, " order by kpi_type");
            programScoringClasses = (new GamificationModel()).getProgramScoring(str4);
            base.ViewData["scoring"] = programScoringClasses;
            base.ViewData["contentkpi"] = list;
            base.ViewData["assessmentkpi"] = tblKpiMasters;
            base.ViewData["normalkpi"] = list1;
            base.ViewData["program"] = tblCategory;
            base.ViewData["assessments"] = assesmentList;
            return base.View();
        }

        public ActionResult ScoringDashboard()
        {
            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];
            if (userSession != null)
            {
                string orgid1 = userSession.id_ORGANIZATION;
                string id_user = userSession.ID_USER;
                string Page1 = "Category";
                UserLogDetails userLogDetails = new UserLogDetails();

                userLogDetails.AddUserDataLog(id_user, orgid1, Page1);
                // Now you have orgid and id_user available for further processing
            }
            UserSession item = (UserSession)base.HttpContext.Session.Contents["UserSession"];
            int num = Convert.ToInt32(item.id_ORGANIZATION);
            Convert.ToInt32(item.ID_USER);
            string str = string.Concat("select * from tbl_category where category_type in (0,1) and status='A' and id_category in (select distinct id_category from tbl_kpi_program_scoring where status='A') and id_organization=", num) ?? "";
            List<tbl_category> list = this.db.tbl_category.SqlQuery(str, new object[0]).ToList<tbl_category>();
            string str1 = string.Concat("select * from tbl_category where category_type in (0,1) and status='A' and id_category not in (select distinct id_category from tbl_kpi_program_scoring where status='A') and id_organization=", num) ?? "";
            List<tbl_category> tblCategories = this.db.tbl_category.SqlQuery(str1, new object[0]).ToList<tbl_category>();
            base.ViewData["program"] = list;
            base.ViewData["category"] = tblCategories;
            return base.View();
        }

        public void setKPIWeightage(string tid)
        {
            foreach (sc_program_kpi_score list in (
                from t in this.db.sc_program_kpi_score
                where t.transactionid == tid
                select t).ToList<sc_program_kpi_score>())
            {
                sc_program_kpi_weightage nullable = (
                    from t in this.db.sc_program_kpi_weightage
                    where t.id_category == list.id_category && t.id_kpi_master == list.id_kpi_master && t.id_user == list.id_user
                    select t).FirstOrDefault<sc_program_kpi_weightage>();
                if (nullable != null)
                {
                    double kPIWeightage = (new ProgramScoringModel()).getKPIWeightage(Convert.ToInt32(list.id_kpi_master), Convert.ToInt32(list.id_category), list.score);
                    nullable.score = list.score;
                    nullable.kpi_wieghtage = new double?(kPIWeightage);
                    nullable.log_datetime = list.log_datetime;
                    nullable.updated_date_time = new DateTime?(DateTime.Now);
                    nullable.status = "A";
                    this.db.SaveChanges();
                }
                else
                {
                    nullable = new sc_program_kpi_weightage();
                    double num = (new ProgramScoringModel()).getKPIWeightage(Convert.ToInt32(list.id_kpi_master), Convert.ToInt32(list.id_category), list.score);
                    nullable.id_category = list.id_category;
                    nullable.id_kpi_master = list.id_kpi_master;
                    nullable.id_organization = list.id_organization;
                    nullable.id_user = list.id_user;
                    nullable.score = list.score;
                    nullable.kpi_wieghtage = new double?(num);
                    nullable.log_datetime = list.log_datetime;
                    nullable.updated_date_time = new DateTime?(DateTime.Now);
                    nullable.status = "A";
                    this.db.sc_program_kpi_weightage.Add(nullable);
                    this.db.SaveChanges();
                }
            }
        }

        public ActionResult AddKPINEW()
        {     
            return base.View();
        }
        public ActionResult kpi_add_new()
        {
            UserSession item = (UserSession)base.HttpContext.Session.Contents["UserSession"];
            int num = Convert.ToInt32(item.id_ORGANIZATION);
            string kpi_name = base.Request.Form["kpi-name"].ToString();
             string kpi_type = base.Request.Form["kpi-type"].ToString();
             string kpi_Subtype = base.Request.Form["kpi-Subtype"].ToString();

            KPI_name_service kpiService = new KPI_name_service();
            kpiService.Add_new(kpi_name, kpi_type, kpi_Subtype, num);
           

            return base.RedirectToAction("Index", "KPI");
        }
    }
}