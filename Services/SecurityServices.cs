using Microsoft.EntityFrameworkCore;
using System.Text;

namespace operation_OLX.Services
{
    public class SecurityServices
    {
        protected SellingPlatformContext ctx;
        public static string? UserName;
        public static string? UserRole;
        public static bool IsLoogedIn;
        public SecurityServices(SellingPlatformContext ctx)
        {
            this.ctx = ctx;
        }

        public async Task<bool> RegisterUser(RegisterUser newUserDetails)
        {
            Account account = new Account();
            account.UserName = newUserDetails.Email;
            account.Password = EncryptAsync(newUserDetails.Password??"");
            account.UserRole = "User";
          var UserCreated=  await ctx.AddAsync(account);
            await ctx.SaveChangesAsync();
            if (UserCreated != null)
                return true;
            else
                return false;   
            
        }

        public async Task<bool> ValidateUserCredentialsAsync(Account UserCredentials)
        {
            if(UserCredentials.UserRole==null)
            { UserCredentials.UserRole = "User"; }

            var UserWithCurrentCredentials = await ctx.Accounts.Where(e => e.UserName == UserCredentials.UserName).FirstOrDefaultAsync(); ;
           
            if (UserWithCurrentCredentials != null)
            {
                if (UserWithCurrentCredentials.Password != null&& UserWithCurrentCredentials.UserName!=null)
                {
                    if (DecryptAsync(UserWithCurrentCredentials.Password) == UserCredentials.Password && UserWithCurrentCredentials.UserRole == UserCredentials.UserRole)
                    {
                        UserName = UserWithCurrentCredentials.UserName;
                        IsLoogedIn = true;
                        UserRole = UserWithCurrentCredentials.UserRole;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public string EncryptAsync(string message)
        {
            var textToEncrypt = message;
            string toReturn = string.Empty;
            string publicKey = "12345678";
            string secretKey = "87654321";
            byte[] secretkeyByte = { };
            secretkeyByte = System.Text.Encoding.UTF8.GetBytes(secretKey);
            byte[] publickeybyte = { };
            publickeybyte = System.Text.Encoding.UTF8.GetBytes(publicKey);
            MemoryStream ms;/*= null;*/
            CryptoStream cs; /*= null;*/
            byte[] inputbyteArray = System.Text.Encoding.UTF8.GetBytes(textToEncrypt);
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                ms = new MemoryStream();
                cs = new CryptoStream(ms, des.CreateEncryptor(publickeybyte, secretkeyByte), CryptoStreamMode.Write);
                cs.Write(inputbyteArray, 0, inputbyteArray.Length);
                cs.FlushFinalBlock();
                toReturn = Convert.ToBase64String(ms.ToArray());
            }
            return toReturn;

        }


        public string DecryptAsync(string text)
        {
            var textToDecrypt = text;
            string toReturn = "";
            string publickey = "12345678";
            string secretkey = "87654321";
            byte[] privatekeyByte = { };
            privatekeyByte = System.Text.Encoding.UTF8.GetBytes(secretkey);
            byte[] publickeybyte = { };
            publickeybyte = System.Text.Encoding.UTF8.GetBytes(publickey);
            MemoryStream ms /*= null*/;
            CryptoStream cs;/*= null;*/
            byte[] inputbyteArray = new byte[textToDecrypt.Replace(" ", "+").Length];
            inputbyteArray = Convert.FromBase64String(textToDecrypt.Replace(" ", "+"));
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                ms = new MemoryStream();
                cs = new CryptoStream(ms, des.CreateDecryptor(publickeybyte, privatekeyByte), CryptoStreamMode.Write);
                cs.Write(inputbyteArray, 0, inputbyteArray.Length);
                cs.FlushFinalBlock();
                Encoding encoding = Encoding.UTF8;
                toReturn = encoding.GetString(ms.ToArray());
            }
            return toReturn;
        }

        public async Task<bool> CheckUserNameAvaliability(string UserName)
        {
            var AccountWithCurrentUserName = await ctx.Accounts.Where(e => e.UserName == UserName).FirstOrDefaultAsync(); ;
            
            if(AccountWithCurrentUserName == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }




    }
    
}

