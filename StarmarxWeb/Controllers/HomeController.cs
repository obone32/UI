using StarmarxWeb.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using StarmarxWeb.ModelsView;

namespace StarmarxWeb.Controllers
{
    public class HomeController : Controller
    {
        ServiceRepository serviceObj = new ServiceRepository();
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> TeacherList()
        {
            TeacherViewModel model = new TeacherViewModel();
            model.TeacherID = 0;
            model.IsActive = -1;
           HttpResponseMessage response = serviceObj.PostResponse("Main/teacher_list", model);
            if (response.IsSuccessStatusCode)
            {
                var Data = await response.Content.ReadAsAsync<IList<TeacherViewModel>>();
                return View(Data);
            }
           return View();
        }

        public async Task<ActionResult> StudentList()
        {
            StudentViewModel model = new StudentViewModel();
            model.StudentID = 0;
            model.IsActive = -1;
            HttpResponseMessage response = serviceObj.PostResponse("Main/student_list", model);
            if (response.IsSuccessStatusCode)
            {
                var Data = await response.Content.ReadAsAsync<IList<StudentViewModel>>();
                return View(Data);
            }
            return View();
        }
    }
}