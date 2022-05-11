

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

            try
            {
                //using (var img = Image.FromStream(file.InputStream))
                //{
                //    return img.RawFormat.Equals(ImageFormat.);
                //}
            }
            catch { }
            return false;
        }
    }
}
