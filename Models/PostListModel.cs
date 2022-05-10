namespace operation_OLX.Models
{
    public class PostListModel
    {
        public string Name { get; set; }    
        public string Location { get; set; }
        public string Date { get; set; }

        public string Category { get; set; }

        public int Price { get; set; }

        public List<Post> Posts { get; set;}
    }
}
