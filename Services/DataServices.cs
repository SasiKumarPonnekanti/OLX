using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using System.Net.Http.Headers;

namespace operation_OLX.Services
{
    public class DataServices
    {
        IWebHostEnvironment hostEnvironment;
        private readonly SellingPlatformContext ctx;
        string UserId= AccountController.CurrentUserName;
        public DataServices(SellingPlatformContext ctx, IWebHostEnvironment hostEnvironment)
        {
            this.ctx = ctx;
            this.hostEnvironment = hostEnvironment;
        }

        public async Task<PersonalInfo> UpdateDetailsAsync(PersonalInfo Details)
        {
            var Id = AccountController.CurrentUserName;
            var MyInfo = await ctx.PersonalInfos.Where(p => p.UserId ==Id).FirstOrDefaultAsync();
            if (MyInfo == null)
            {
                Details.UserId = Id;
                var res2 = await ctx.PersonalInfos.AddAsync(Details);
                await ctx.SaveChangesAsync();
                return res2.Entity;
            }
            else
            {
                MyInfo.AboutMe = Details.AboutMe;
                MyInfo.Email = Details.Email;
                MyInfo.LandMark = Details.LandMark;
                MyInfo.City = Details.City;
                MyInfo.State = Details.State;
                MyInfo.Name=Details.Name;
                MyInfo.Phone = Details.Phone;
                await ctx.SaveChangesAsync();
                return MyInfo;
            }
        }

        public async Task<PersonalInfo> GetDetailsAsync()
        {

            var Id = AccountController.CurrentUserName;
            var MyDetails = await ctx.PersonalInfos.Where(p => p.UserId == Id).FirstOrDefaultAsync();
            if(MyDetails == null)
            {
                PersonalInfo Person = new PersonalInfo();
                Person.UserId = Id;
                return Person;
            }
            else
            {
                return MyDetails;
            }

        }

        public async Task<List<Post>> GetPostsAsync()
        {
            var Posts = await ctx.Posts.ToListAsync();
            return Posts;
        }

        public async Task<bool> AddPostAsync(PostModel Product)
        {
            var Id = AccountController.CurrentUserName;
            if (Product.image1 != null)
            {
                var image1FileName = ContentDispositionHeaderValue.Parse(Product.image1.ContentDisposition).FileName.Trim('"');
                var FinalPath1 = Path.Combine(hostEnvironment.WebRootPath, "images", image1FileName);
                using (var fs = new FileStream(FinalPath1, FileMode.Create))
                {
                    await Product.image1.CopyToAsync(fs);
                }
                Product.Post.ImagePath1 = image1FileName;
            }
            if (Product.image2 != null)
            {
                var image2FileName = ContentDispositionHeaderValue.Parse(Product.image2.ContentDisposition).FileName.Trim('"');

                var FinalPath2 = Path.Combine(hostEnvironment.WebRootPath, "images", image2FileName);
                using (var fs = new FileStream(FinalPath2, FileMode.Create))
                {
                    await Product.image2.CopyToAsync(fs);
                }
                Product.Post.ImagePath2 = image2FileName;
            }
            if (Product.image3 != null)
            {
                var image3FileName = ContentDispositionHeaderValue.Parse(Product.image2.ContentDisposition).FileName.Trim('"');
               
                var FinalPath3 = Path.Combine(hostEnvironment.WebRootPath, "images", image3FileName);
                using (var fs = new FileStream(FinalPath3, FileMode.Create))
                {
                    await Product.image3.CopyToAsync(fs);
                }
                Product.Post.ImagePath3 = image3FileName;
            }
            Product.Post.UserId=Id;
            Product.Post.Status = "Active";
            await ctx.Posts.AddAsync(Product.Post);
            await ctx.SaveChangesAsync();
            return true;
        }

        public async Task DeactivatePostAsync(int id)
        {
            var Post = await ctx.Posts.FindAsync(id);
            if (Post != null)
            {
                Post.Status = "Deactivated";
                await ctx.SaveChangesAsync();
            }
        }

        public async Task ActivatePostAsync(int id)
        {
           var Post =  await ctx.Posts.FindAsync(id);
            if (Post != null)
            {
                Post.Status = "Active";
                await ctx.SaveChangesAsync();
            }
        }
        public async Task AddfavouriteAsync(int id)
        {
            var UserId = AccountController.CurrentUserName;
            var favourite = new Favourite() { UserId=UserId,PostId=id};
            if (!IsfavoritedAsync(id).Result)
            {
                await ctx.Favourites.AddAsync(favourite);
                await ctx.SaveChangesAsync();
            }
        }

        public async Task<List<Post>> LoadfavouriteAsync()
        {
            var favourites = await ctx.Favourites.Where(f => f.UserId == UserId).ToListAsync();
            var Posts = new List<Post>();
            foreach(var f in favourites)
            {
               Posts.Add(await ctx.Posts.FindAsync(f.PostId));
            }
            return Posts;
        }

        public async Task RemovefavoriteAsync(int id)
        {
            var favourite = await ctx.Favourites.Where(f => f.UserId == UserId && f.PostId == id).FirstOrDefaultAsync();
            if (favourite != null)
            {
                ctx.Favourites.Remove(favourite);
               await ctx.SaveChangesAsync();
            }
        }

        public async Task<bool> IsfavoritedAsync(int Id)
        {
            var favourite = await ctx.Favourites.Where(f => f.UserId == UserId && f.PostId==Id).FirstOrDefaultAsync();
            if(favourite!= null)
            {
                return true;
            }
            return false;
        }
        public async Task<List<Post>> GetFilteredPostsAsync(string Name,string Location,string Category)
        {
            List<Post> posts = new List<Post>();
            posts = await ctx.Posts.ToListAsync();
            if(Name!=null)
            {
               posts =  posts.Where(p=>p.Title.ToLower().Contains(Name.ToLower())).ToList();
            }
            if(Location!=null)
            {
                posts = posts.Where(p => p.State.ToLower().Contains(Location.ToLower()) ||p.City.ToLower().Contains(Location.ToLower())).ToList();
            }
            if(Category!= null)
            {
                posts = posts.Where(p => p.Category.ToLower().Contains(Category.ToLower())).ToList();
            }
            return posts;
        }

        public async Task<List<Category>> GetCatsAsync()
        {
           return  await ctx.Categories.ToListAsync();
        }
    }
}
