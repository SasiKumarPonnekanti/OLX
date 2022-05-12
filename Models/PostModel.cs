namespace operation_OLX.Models
{
    public class PostModel
    {
        public Post Post { get; set; }
       [Required(ErrorMessage ="Atlesast One Image Is Required")]
       [ValidateFile(ErrorMessage ="image should be <1Mb and Type .png or .jpg")]
        public IFormFile image1 { get; set; }
        
        public IFormFile image2 { get; set; }
        public IFormFile image3 { get; set; }
    }
}
