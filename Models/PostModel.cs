namespace operation_OLX.Models
{
    public class PostModel
    {
        public Post Post { get; set; }
        [Required(ErrorMessage ="Atlesast One Image Is Required")]
        public IFormFile image1 { get; set; }
        
        public IFormFile image2 { get; set; }
        public IFormFile image3 { get; set; }
    }
}
