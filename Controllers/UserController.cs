using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace operation_OLX.Controllers
{
   [User]
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
            ViewBag.Categories = new SelectList(await _DataServices.GetCatsAsync(), "CatName", "CatName");
            var FavoritedPosts = _DataServices.LoadfavouriteAsync().Result;
            ViewBag.favs = FavoritedPosts;
            postModel.Posts=  _DataServices.GetPostsAsync(Name,Location,Category).Result.Where(P => P.UserId != SecurityServices.UserName && P.Status == "Active").ToList();
            return View(postModel);
        }


        
        public IActionResult FilterPosts(string Name, string Location, string Category)
        {
            return RedirectToAction("Index",new {Name=Name,Location=Location,Category=Category});
        }



        public async Task<IActionResult> ViewProfile()
        {
            var Profile =await  _DataServices.GetUserDetailsAsync();
            return View(Profile);
        }



        [HttpPost]
        public async Task<IActionResult> ViewProfile(PersonalInfo UpdatedDetails)
        {
           await  _DataServices.UpdateUserDetailsAsync(UpdatedDetails);
            return RedirectToAction("ViewProfile");
        }
        public async Task<IActionResult> ViewPosts()
        {
            var Posts =   _DataServices.GetPostsAsync().Result.Where(P=>P.UserId!=SecurityServices.UserName&&P.Status=="Active").ToList();
            var FPosts = await _DataServices.LoadfavouriteAsync();
            ViewBag.favs= FPosts;
            return View(Posts);
        }

        public async Task<IActionResult> AddPost()
        {
           ViewBag.Categories = new SelectList(await _DataServices.GetCatsAsync(), "CatName", "CatName");
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

            var Posts = await _DataServices.GetPostsAsync();
            var Post = Posts.Where(p => p.Id== id).FirstOrDefault();
            return View(Post);
        }

        public async Task<IActionResult> ViewMyPosts()
        {
            var Posts = await _DataServices.GetPostsAsync();
            var MyPosts =Posts.Where(P => (P.UserId == SecurityServices.UserName)&&(P.Status=="Active")).ToList();
            return View(MyPosts);
        }
        public async Task<IActionResult> Repost()
        {
            var DeactivatedPosts = await _DataServices.GetPostsAsync().Where(P => P.UserId == SecurityServices.UserName && P.Status != "Active").ToListAsync();
            DeactivatedPosts.Where(P => P.UserId == SecurityServices.UserName && P.Status != "Active").ToList();
            return View(DeactivatedPosts);

        }
        public async Task<IActionResult> Activate(int id)
        {
            await _DataServices.UpdatePostStatusAsync(id,"Active");
            return RedirectToAction("Repost");
        }

        public async Task<IActionResult> Deactivate(int id)
        {
            await _DataServices.UpdatePostStatusAsync(id,"Deactivated");
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

        public IActionResult ChatHome(string StartHead)
        {
            var ChatModel = new ChatModel();
            ViewBag.ChatHead = StartHead;
            ChatModel.ChatHeads= _DataServices.LoadChatHeadsAsync().Result;
            return View(ChatModel);
        }
        public IActionResult LoadChats(string Id)
        {
            ViewBag.ChatHead = Id;
            var messages = _DataServices.LoadChats(Id).Result;
            return PartialView("MessageBox",messages);
        }
         
        public async Task<IActionResult> SendMessage(string Id,String Message)
        {
            await _DataServices.SendMessageAsync(Id, Message);
            return Json(true);
        }

    }
}
