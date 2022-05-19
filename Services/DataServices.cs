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
        string UserId = SecurityServices.UserName ??"";
        public DataServices(SellingPlatformContext ctx, IWebHostEnvironment hostEnvironment)
        {
            this.ctx = ctx;
            this.hostEnvironment = hostEnvironment;
        }

        public async Task<PersonalInfo> UpdateUserDetailsAsync(PersonalInfo UpdatedDetails)
        {

            var UserDetails = await ctx.PersonalInfos.Where(p => p.UserId == UserId).FirstOrDefaultAsync();
            if (UserDetails == null)
            {
                UpdatedDetails.UserId = UserId;
                var res2 = await ctx.PersonalInfos.AddAsync(UpdatedDetails);
                await ctx.SaveChangesAsync();
                return res2.Entity;
            }
            else
            {
                UserDetails.AboutMe = UpdatedDetails.AboutMe;
                UserDetails.Email = UpdatedDetails.Email;
                UserDetails.LandMark = UpdatedDetails.LandMark;
                UserDetails.City = UpdatedDetails.City;
                UserDetails.State = UpdatedDetails.State;
                UserDetails.Name = UpdatedDetails.Name;
                UserDetails.Phone = UpdatedDetails.Phone;
                await ctx.SaveChangesAsync();
                return UserDetails;
            }
        }

        public async Task<PersonalInfo> GetUserDetailsAsync()
        {
            var UserDetails = await ctx.PersonalInfos.Where(p => p.UserId == UserId).FirstOrDefaultAsync();
            if (UserDetails == null)
            {
                PersonalInfo Person = new PersonalInfo();
                Person.UserId = UserId;
                return Person;
            }
            else
            {
                return UserDetails;
            }

        }

        public async Task<List<Post>> GetPostsAsync()
        {
            var Posts = await ctx.Posts.ToListAsync();
            return Posts;
        }

        public async Task<bool> AddPostAsync(PostModel Product)
        {

            if (Product.image1 != null&&Product.Post!=null)
            {
                var image1FileName = (ContentDispositionHeaderValue.Parse(Product.image1.ContentDisposition??"").FileName??"").Trim('"');
                var FinalPath1 = Path.Combine(hostEnvironment.WebRootPath, "images", image1FileName);
                using (var fs = new FileStream(FinalPath1, FileMode.Create))
                {
                    await Product.image1.CopyToAsync(fs);
                }
                Product.Post.ImagePath1 = image1FileName ;
            }
            if (Product.image2 != null && Product.Post != null)
            {
                var image2FileName =(ContentDispositionHeaderValue.Parse(Product.image2.ContentDisposition??"").FileName ??"").Trim('"');

                var FinalPath2 = Path.Combine(hostEnvironment.WebRootPath, "images", image2FileName);
                using (var fs = new FileStream(FinalPath2, FileMode.Create))
                {
                    await Product.image2.CopyToAsync(fs);
                }
                Product.Post.ImagePath2 = image2FileName;
            }
            if (Product.image3 != null && Product.Post != null)
            {
                var image3FileName = (ContentDispositionHeaderValue.Parse(Product.image3.ContentDisposition??"").FileName??"").Trim('"');

                var FinalPath3 = Path.Combine(hostEnvironment.WebRootPath, "images", image3FileName);
                using (var fs = new FileStream(FinalPath3, FileMode.Create))
                {
                    await Product.image3.CopyToAsync(fs);
                }
                Product.Post.ImagePath3 = image3FileName;
            }
            if (Product.Post != null)
            {
                Product.Post.UserId = UserId;
                Product.Post.Status = "Pending";
                Product.Post.Dateposted = DateTime.Now;
                await ctx.Posts.AddAsync(Product.Post);
            }
            //await ctx.Posts.AddAsync(Product.Post);
            await ctx.SaveChangesAsync();
            return true;
        }
        public async Task UpdatePostStatusAsync(int id, string Status)
        {
            var Post = await ctx.Posts.FindAsync(id);
            if (Post != null)
            {
                Post.Status = Status;
                await ctx.SaveChangesAsync();
            }
        }
        public async Task AddfavouriteAsync(int id)
        {

            var favourite = new Favourite() { UserId = UserId, PostId = id };
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
            foreach (var f in favourites)
            {
                var Post = await ctx.Posts.FindAsync(f.PostId);
                if (Post != null)
                {
                    Posts.Add(Post);
                }
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
            var favourite = await ctx.Favourites.Where(f => f.UserId == UserId && f.PostId == Id).FirstOrDefaultAsync();
            if (favourite != null)
            {
                return true;
            }
            return false;
        }
        public async Task<List<Post>> GetPostsAsync(string Name, string Location, string Category)
        {
            List<Post> posts = new List<Post>();
            posts = await ctx.Posts.ToListAsync();
            if (Name != null)
            {
                posts = posts.Where(p => p.Title.ToLower().Contains(Name.ToLower())).ToList();
            }
            if (Location != null)
            {
                posts = posts.Where(p => p.State.ToLower().Contains(Location.ToLower()) || p.City.ToLower().Contains(Location.ToLower())).ToList();
            }
            if (Category != null)
            {
                posts = posts.Where(p => p.Category.ToLower().Contains(Category.ToLower())).ToList();
            }
            return posts;
        }

        public async Task<List<Category>> GetCatsAsync()
        {
            return await ctx.Categories.ToListAsync();
        }

        public async Task<List<Chat>> LoadChats(string Id)
        {
            var Messages = await ctx.Chats.Where(c => (c.SenderId == UserId&&c.ReceiverId==Id) ||( c.ReceiverId == UserId&&c.SenderId==Id)).ToListAsync();
            return Messages;
        }

        public async Task<List<string>> LoadChatHeadsAsync()
        {
            var Messages = await ctx.Chats.Where(c => c.SenderId == UserId || c.ReceiverId == UserId).ToListAsync();
            var chatHeads = new List<string>();
            foreach (var message in Messages)
            {
                if (message.SenderId == UserId)
                {
                    if (!chatHeads.Contains(message.ReceiverId))
                    {
                        chatHeads.Add(message.ReceiverId);
                    }
                }
                else
                {
                    if (!chatHeads.Contains(message.SenderId))
                    {
                        chatHeads.Add(message.SenderId);
                    }
                }
            }
            return chatHeads;
        }
        public async Task SendMessageAsync(string Id,String Message)
        {
            var newMessage = new Chat();
            newMessage.SenderId = UserId;
            newMessage.ReceiverId = Id;
            newMessage.Message = Message;
            newMessage.DateTime = DateTime.Now.ToString();
            await ctx.Chats.AddAsync(newMessage);
            await ctx.SaveChangesAsync();
        }
       
    }
}
