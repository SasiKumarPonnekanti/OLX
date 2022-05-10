using Microsoft.EntityFrameworkCore;
using System.Text;

namespace operation_OLX.Services
{
    public class SecurityServices
    {
        private readonly SellingPlatformContext ctx;
        public SecurityServices(SellingPlatformContext ctx)
        {
            this.ctx = ctx;
        }

        public async Task<bool> RegisterUser(RegisterUser newUser)
        {
            Account account = new Account();
            account.UserName = newUser.Email;
            // account.Password = CalculateSHA256(newUser.Password) ;
            account.Password = EncryptAsync(newUser.Password).Result;
          var entity=  await ctx.AddAsync(account);
            await ctx.SaveChangesAsync();
            if (entity != null)
                return true;
            else
                return false;   
            
        }

        public async Task<bool> ValidateUserAsync(Account account)
        {
           var res =   ctx.Accounts.Where(e=>e.UserName==account.UserName).FirstOrDefault();
            if (res != null)
            {
                if (DecryptAsync(res.Password).Result == account.Password)
                {
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

        public async Task<string> EncryptAsync(string message)
        {
            var textToEncrypt = message;
            string toReturn = string.Empty;
            string publicKey = "12345678";
            string secretKey = "87654321";
            byte[] secretkeyByte = { };
            secretkeyByte = System.Text.Encoding.UTF8.GetBytes(secretKey);
            byte[] publickeybyte = { };
            publickeybyte = System.Text.Encoding.UTF8.GetBytes(publicKey);
            MemoryStream ms = null;
            CryptoStream cs = null;
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


        public async Task<string> DecryptAsync(string text)
        {
            var textToDecrypt = text;
            string toReturn = "";
            string publickey = "12345678";
            string secretkey = "87654321";
            byte[] privatekeyByte = { };
            privatekeyByte = System.Text.Encoding.UTF8.GetBytes(secretkey);
            byte[] publickeybyte = { };
            publickeybyte = System.Text.Encoding.UTF8.GetBytes(publickey);
            MemoryStream ms = null;
            CryptoStream cs = null;
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

        public async Task<bool> ValidateUserName(string UserName)
        {
            var res = ctx.Accounts.Where(e => e.UserName == UserName).FirstOrDefault();
            if(res == null)
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

