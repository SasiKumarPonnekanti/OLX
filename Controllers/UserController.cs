using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace operation_OLX.Controllers
{
   [Autherized]
    public class UserController : Controller
    {
        private readonly DataServices _DataServices;
        public UserController(DataServices _DataServices)
        {
            this._DataServices=_DataServices;
        }



        public async Task<IActionResult> Index(string Name,string Location,string Category)
        {
            var postModel = new PostListModel();
            ViewBag.Categories = new SelectList(_DataServices.GetCatsAsync().Result, "CatName", "CatName");
            var FavoritedPosts = _DataServices.LoadfavouriteAsync().Result;
            ViewBag.favs = FavoritedPosts;
            postModel.Posts=  _DataServices.GetPostsAsync(Name,Location,Category).Result.Where(P => P.UserId != AccountController.CurrentUserName && P.Status == "Active").ToList();
            return View(postModel);
        }


        
        public IActionResult FilterPosts(string Name, string Location, string Category)
        {
            return RedirectToAction("Index",new {Name=Name,Location=Location,Category=Category});
        }



        public async Task<IActionResult> ViewProfile()
        {
            var Profile = _DataServices.GetDetailsAsync().Result;
            return View(Profile);
        }



        [HttpPost]
        public async Task<IActionResult> ViewProfile(PersonalInfo Profile)
        {
           await  _DataServices.UpdateDetailsAsync(Profile);
            return RedirectToAction("ViewProfile");
        }
        public async Task<IActionResult> ViewPosts()
        {
            var Posts =   _DataServices.GetPostsAsync().Result.Where(P=>P.UserId!=AccountController.CurrentUserName&&P.Status=="Active").ToList();
            var FPosts = _DataServices.LoadfavouriteAsync().Result;
            ViewBag.favs= FPosts;
            return View(Posts);
        }

        public async Task<IActionResult> AddPost()
        {
           ViewBag.Categories = new SelectList(_DataServices.GetCatsAsync().Result, "CatName", "CatName");
            return View(new PostModel());
        }
        [HttpPost]
        public async Task<IActionResult> AddPost(PostModel Product)
        {
            ViewBag.Categories = new SelectList(_DataServices.GetCatsAsync().Result, "CatName", "CatName");
            ModelState.Remove("Post.Status"); ModelState.Remove("Post.UserId"); ModelState.Remove("image2"); ModelState.Remove("image3");
            if (ModelState.IsValid)
            {
                await _DataServices.AddPostAsync(Product);
                return RedirectToAction("ViewMyPosts");
            }else
            {
                return View(Product);
            }
        }

        public async Task<IActionResult> ViewPostDetails(int id)
        {
          
            var Post = _DataServices.GetPostsAsync().Result.Where(p => p.Id== id).FirstOrDefault();
            return View(Post);
        }

        public async Task<IActionResult> ViewMyPosts()
        {
            var Posts = _DataServices.GetPostsAsync().Result.Where(P => P.UserId == AccountController.CurrentUserName&&P.Status=="Active").ToList();
            return View(Posts);
        }
        public async Task<IActionResult> Repost()
        {
            var Posts = _DataServices.GetPostsAsync().Result.Where(P => P.UserId == AccountController.CurrentUserName && P.Status != "Active").ToList();
            return View(Posts);

        }
        public async Task<IActionResult> Activate(int id)
        {
            await _DataServices.ActivatePostAsync(id);
            return RedirectToAction("Repost");
        }

        public async Task<IActionResult> Deactivate(int id)
        {
            await _DataServices.DeactivatePostAsync(id);
            return RedirectToAction("ViewMyPosts");
        }
        
        public async Task<IActionResult> RemoveFavorite(int id)
        {
            await _DataServices.RemovefavoriteAsync(id);
            return RedirectToAction("ViewFavorites");
        }

        [HttpPost]
        public async Task AddFavorite(int id)
        {
            await _DataServices.AddfavouriteAsync(id);
        }

        public async Task<IActionResult> ViewFavorites()
        {
            var Posts = await _DataServices.LoadfavouriteAsync();
            return View(Posts);
        }
         
        [HttpPost]
        public async Task Isfavourited(int id)
        {
           Json((await _DataServices.IsfavoritedAsync(id)));
        }

        

    }
}
