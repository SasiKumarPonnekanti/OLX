using Microsoft.AspNetCore.Mvc;

namespace operation_OLX.Controllers
{
    [Admin]
    public class AdminController : Controller
    {
        private readonly DataServices _DataServices;
        public AdminController(DataServices _DataServices)
        {
            this._DataServices = _DataServices;
        }
        public IActionResult Index(string Status)
        {
            var Posts= _DataServices.GetPostsAsync().Result;
            if(Status!=null)
            {
                Posts= Posts.Where(p=>p.Status==Status).ToList();
            }
            return View(Posts);
        }

        public IActionResult FilterPosts(string Status)
        {
            return RedirectToAction("Index",new {Status=Status});
        }

        public PartialViewResult BlockedPosts()
        {
            var Posts = _DataServices.GetPostsAsync().Result;
            return PartialView(Posts);
        }
        public async Task<IActionResult> ApprovePost(int Id)
        {
            await _DataServices.UpdatePostStatusAsync(Id, "Active");
            return RedirectToAction("Index");
        }

        public async Task<RedirectToActionResult> BlockPost(int id)
        {
            await _DataServices.UpdatePostStatusAsync(id, "Blocked");
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> UnblockPost(int id)
        {
            await _DataServices.UpdatePostStatusAsync(id, "Active");
            return RedirectToAction("Index");
        }
    }
}
