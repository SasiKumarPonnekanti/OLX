

using static System.Net.Mime.MediaTypeNames;

namespace operation_OLX.CustomValidators
{
    public class ValidateFileAttribute : RequiredAttribute
    {
        public override bool IsValid(object value)
        {
            var file = value as IFormFile;
            if (file == null)
            {
                return false;
            }

            if (file.Length > 1 * 1024 * 1024)
            {
                return false;
            }
            FileInfo fileInfo = new FileInfo(file.FileName);
            if (fileInfo.Extension != ".jpg" && fileInfo.Extension != ".png")
            {
                return false;
            }
            return true;
        }
    }
}
