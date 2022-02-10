using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using StarmarxWeb.ModelsView;
using StarmarxWeb.Repository;
using System.Configuration;
using StarmarxWeb.Models;

namespace StarmarxWeb.Controllers
{
    public class AccountController : Controller
    {
        ServiceRepository serviceObj = new ServiceRepository();
        string Baseurl = ConfigurationManager.AppSettings["ServiceUrl"].ToString();
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        //public async Task<ActionResult> Option()
        //{
        //    string Baseurl = "http://api.reconnectvacations.com/api/Main/";

        //    IEnumerable<TeacherViewModel> TeacherList = null;
        //    using (var client = new HttpClient())
        //    {
        //        //client.BaseAddress = new Uri("http://localhost:64189/api/student");
        //        TeacherViewModel _objmodel = new TeacherViewModel();
        //        _objmodel.TeacherID = 0;
        //        _objmodel.IsActive = -1;
        //        client.BaseAddress = new Uri(Baseurl);
        //        var response = await client.PostAsJsonAsync("teacher_list", _objmodel);
        //        //var postTask = client.PostAsJsonAsync<StudentViewModel>("student", student);
        //        //postTask.Wait();

        //        // var result = response.Result;
        //        if (response.IsSuccessStatusCode)
        //        {
        //            TeacherList = await response.Content.ReadAsAsync<IList<TeacherViewModel>>();
        //        }
        //    }

        //    return View();
        //}

        public ActionResult Option()
        {
            return View();
        }

        public ActionResult Register()
        {
            StudentViewModel _objmodel = new StudentViewModel();
            FillClassCombo();FillGroupCombo();FillDivCombo();FillInstituteCombo();FillRoleCombo();
            return View(_objmodel);
        }

        [HttpPost]
        public async Task<ActionResult> Register(StudentViewModel model)
        {
            model.WalletID = model.MobileNo;
            using (var client = new HttpClient())
            {
                HttpResponseMessage response = serviceObj.PostResponse("Main/student_add", model);
                if (response.IsSuccessStatusCode)
                {
                    var teacherid = await response.Content.ReadAsAsync<StudentViewModel>();
                    return RedirectToAction("EmailVerify", new { customerid = teacherid.StudentID, type = 2 });
                }
            }
            FillClassCombo(); FillGroupCombo(); FillDivCombo(); FillInstituteCombo(); FillRoleCombo();
            return View();
        }

        public ActionResult TeacherRegister()
        {
            TeacherViewModel _objmodel = new TeacherViewModel();
            FillClassCombo(); FillGroupCombo(); FillDivCombo(); FillInstituteCombo(); FillRoleCombo();
            return View(_objmodel);
        }

        [HttpPost]
        public async Task<ActionResult> TeacherRegister(TeacherViewModel model)
        {
            model.WalletID = model.MobileNo;
            using (var client = new HttpClient())
            {
                //string Baseurl = "http://api.reconnectvacations.com/api/Main/";
                string Baseurl= ConfigurationManager.AppSettings["ServiceUrl"].ToString();
                client.BaseAddress = new Uri(Baseurl);
                var response = await client.PostAsJsonAsync("Main/teacher_add", model);
                //var postTask = client.PostAsJsonAsync<StudentViewModel>("student", student);
                //postTask.Wait();

                // var result = response.Result;
                if (response.IsSuccessStatusCode)
                {
                    var teacherid = await response.Content.ReadAsAsync<TeacherViewModel>();
                    return RedirectToAction("EmailVerify", new { customerid = teacherid.TeacherID, type=1});
                }

                //ServiceRepository serviceObj = new ServiceRepository();
                //HttpResponseMessage response = serviceObj.PostResponse("Main/teacher_add", model);
                //response.EnsureSuccessStatusCode();
                //return RedirectToAction("EmailVerify", "Account", new { id = 1, userid = 1 });
            }
            FillClassCombo(); FillGroupCombo(); FillDivCombo(); FillInstituteCombo(); FillRoleCombo();
            return View();
        }

        public ActionResult ParentRegister()
        {
            ParentViewModel _objmodel = new ParentViewModel();
            FillClassCombo(); FillGroupCombo(); FillDivCombo(); FillInstituteCombo(); FillRoleCombo();
            return View(_objmodel);
        }

        [HttpPost]
        public async Task<ActionResult> ParentRegister(ParentViewModel model)
        {
            model.WalletID = model.MobileNo;
            using (var client = new HttpClient())
            {
                HttpResponseMessage response = serviceObj.PostResponse("Main/parent_add", model);
                if (response.IsSuccessStatusCode)
                {
                    var teacherid = await response.Content.ReadAsAsync<ParentViewModel>();
                    return RedirectToAction("EmailVerify", new { customerid = teacherid.ParentID, type = 3 });
                }
                else
                {
                    var error = await response.Content.ReadAsAsync<ResponseModel>();
                    ViewBag.ErrorMesssage = error.Message;
                }
            }
            FillClassCombo(); FillGroupCombo(); FillDivCombo(); FillInstituteCombo(); FillRoleCombo();
            return View();
        }

        public async Task<ActionResult> EmailVerify(int customerid=0,int Type=0)
        {
            OtpViewModel otpViewModel = new OtpViewModel();
            UserModel userModel = new UserModel();
            userModel.UserID = 0;
            userModel.CustomerID = customerid;
            userModel.CustomerType = Type;
            userModel.Active = -1;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                var response = await client.PostAsJsonAsync("Main/UserMaster_ListAll", userModel);
                if (response.IsSuccessStatusCode)
                {
                    var userdetails = await response.Content.ReadAsAsync<IList<UserModel>>();
                    otpViewModel.UserID = userdetails[0].UserID;
                    otpViewModel.Receiver = userdetails[0].EmailID;
                    otpViewModel.Type = 1;
                    using (var client1 = new HttpClient())
                    {
                        client1.BaseAddress = new Uri(Baseurl);
                        var otpresponse = await client1.PostAsJsonAsync("Main/OTPTrans_Add", otpViewModel);
                        if (otpresponse.IsSuccessStatusCode)
                        {
                            
                            var otpdetails = await otpresponse.Content.ReadAsAsync<OtpViewModel>();
                            ViewBag.OTP = otpdetails.OTP;
                            EmailModel _objEmail = new EmailModel();
                            _objEmail.To = userdetails[0].EmailID;
                            _objEmail.Subject = "Starmarx Email Verify";
                            _objEmail.Body = "<p>Dear " + userdetails[0].Name + ",</p> <p> Please find below OTP " + otpdetails.OTP + "</p>";
                          // await sendMail(_objEmail);
                        }
                    }

                }
            }
            return View(otpViewModel);
        }
        [HttpPost]
        public ActionResult EmailVerify(OtpViewModel otpViewModel)
        {
            var otpresponse = serviceObj.PostResponse("Main/OTPCheck", otpViewModel);
            if (otpresponse.IsSuccessStatusCode)
            {
                return RedirectToAction("MobileVerify", new { userid=otpViewModel.UserID });
            }
           return View();
        }

        public async Task<ActionResult> MobileVerify(int userid = 0)
        {
            OtpViewModel otpViewModel = new OtpViewModel();
            UserModel userModel = new UserModel();
            userModel.UserID = userid;
            userModel.CustomerID = 0;
            userModel.CustomerType = 0;
            userModel.Active = -1;
            var response = serviceObj.PostResponse("Main/UserMaster_ListAll", userModel);
            if (response.IsSuccessStatusCode)
            {
                var userdetails = await response.Content.ReadAsAsync<IList<UserModel>>();
                otpViewModel.UserID = userdetails[0].UserID;
                otpViewModel.Receiver = userdetails[0].MobileNo;
                otpViewModel.Type = 2;
                var otpresponse = serviceObj.PostResponse("Main/OTPTrans_Add", otpViewModel);
                if (otpresponse.IsSuccessStatusCode)
                {
                    var otpdetails = await otpresponse.Content.ReadAsAsync<OtpViewModel>();
                    ViewBag.OTP = otpdetails.OTP;
                    EmailModel _objEmail = new EmailModel();
                    _objEmail.To = userdetails[0].EmailID;
                    _objEmail.Subject = "Starmarx Mobile Verify";
                    _objEmail.Body = "<p>Dear " + userdetails[0].Name + ",</p> <p> Please find below OTP " + otpdetails.OTP + "</p>";
                    //await sendMail(_objEmail);
                }

            }

            return View(otpViewModel);
        }


        [HttpPost]
        public ActionResult MobileVerify(OtpViewModel otpViewModel)
        {
            var otpresponse = serviceObj.PostResponse("Main/OTPCheck", otpViewModel);
            if (otpresponse.IsSuccessStatusCode)
            {
                return RedirectToAction("Thankyou", new { userid = otpViewModel.UserID });
            }
            return View();
        }

        public async Task<ActionResult> Thankyou(int userid = 0)
        {
            UserModel userModeldetail = new UserModel();
            UserModel userModel = new UserModel();
            userModel.UserID = userid;
            userModel.CustomerID = 0;
            userModel.CustomerType = 0;
            userModel.Active = -1;
            var response = serviceObj.PostResponse("Main/UserMaster_ListAll", userModel);
            if (response.IsSuccessStatusCode)
            {
                var userList = await response.Content.ReadAsAsync<IList<UserModel>>();
                userModeldetail = userList.LastOrDefault();
                UserModel userUpdModel = new UserModel();
                userUpdModel.UserID = userid;
                userUpdModel.EmailVerify = 1;
                userUpdModel.MobileVerify = 1;
                userUpdModel.Active = 1;
                var updresponse = serviceObj.PostResponse("Main/UserMaster_update", userUpdModel);
                if (updresponse.IsSuccessStatusCode)
                {
                    var otpdetails = await updresponse.Content.ReadAsAsync<OtpViewModel>();
                    EmailModel _objEmail = new EmailModel();
                    _objEmail.To = userModeldetail.EmailID;
                    _objEmail.Subject = "Starmarx Login Details";
                    _objEmail.Body = "<p>Dear " + userModeldetail.Name + ",</p> <p> User Name : " + userModeldetail.LoginName + "</p> <p> Password : "+userModeldetail.Password+"</p>";
                    //await sendMail(_objEmail);
                }

            }

            return View(userModeldetail);
        }

        public async Task<bool> sendMail(EmailModel emailModel)
        {
            emailModel.CC = "shaikhazhar48@gmail.com";
            using (var client1 = new HttpClient())
            {
                client1.BaseAddress = new Uri(Baseurl);
                var emailresponse = await client1.PostAsJsonAsync("Main/MailSend", emailModel);
                //var emailresponse = serviceObj.PostResponse("Main/MailSend", emailModel);
                if (emailresponse.IsSuccessStatusCode)
                {
                    return true;
                }
            }
            return false;
        }

        public void FillClassCombo()
        {
            //MasterModel _objModel = new MasterModel();
            //_objModel.IsActive = 1;
            //var DDLDesignation = _clsMaster.DesignationMaster_ListAll(_objModel).ToList();
            //if (DDLClass != null && DDLClass.Count > 0)
            //    ViewBag.DDLClassID = DDLClass;
            //else
            //    ViewBag.DDLClassID = new List<SelectListItem> { };
            ViewBag.DDLClassID = new List<SelectListItem> { new SelectListItem() { Text = "1", Value = "1" },
                                                                  new SelectListItem() { Text = "2", Value = "2" },
                                                                  new SelectListItem() { Text = "3", Value = "3" },
                                                                  new SelectListItem() { Text = "4", Value = "4" }};
        }

        public void FillDivCombo()
        {
            ViewBag.DDLDivID = new List<SelectListItem> { new SelectListItem() { Text = "A", Value = "1" },
                                                                  new SelectListItem() { Text = "B", Value = "2" },
                                                                  new SelectListItem() { Text = "C", Value = "3" },
                                                                  new SelectListItem() { Text = "D", Value = "4" }};
        }

        public void FillInstituteCombo()
        {
            ViewBag.DDLInstitudeID = new List<SelectListItem> { new SelectListItem() { Text = "Abc School", Value = "1" },
                                                                  new SelectListItem() { Text = "Xyz School", Value = "2" },
                                                                  new SelectListItem() { Text = "qwer School", Value = "3" },
                                                                  new SelectListItem() { Text = "POI School", Value = "4" }};
        }

        public void FillRoleCombo()
        {
            ViewBag.DDLRoleID = new List<SelectListItem> { new SelectListItem() { Text = "Monitor", Value = "1" },
                                                                  new SelectListItem() { Text = "Group Leader", Value = "2" },
                                                                  new SelectListItem() { Text = "Teacher", Value = "3" },
                                                                  new SelectListItem() { Text = "Head Master", Value = "4" },
                                                                  new SelectListItem() { Text = "Student", Value = "5" }};
        }

        public void FillGroupCombo()
        {
            ViewBag.DDLGroupID = new List<SelectListItem> { new SelectListItem() { Text = "Read", Value = "1" },
                                                                  new SelectListItem() { Text = "Blue", Value = "2" },
                                                                  new SelectListItem() { Text = "Green", Value = "3" },
                                                                  new SelectListItem() { Text = "Yellow", Value = "4" }};
        }
    }
}