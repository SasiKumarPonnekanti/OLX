namespace operation_OLX.Models
{
    public class PostListModel
    {
        //ViewModel That is Used To Display Posts in Index Page along with taking input of Name Loaction and category feilds to filter the Posts As Required
        public string? Name { get; set; }    
        public string? Location { get; set; }
        public string? Date { get; set; }

        public string? Category { get; set; }

        public int Price { get; set; }

        public List<Post>? Posts { get; set;}
    }
}
