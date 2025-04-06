namespace _03._26._25Homework.Models
{
    public class GetPostViewModel
    {
        public Post Post { get; set; }
        public List<Comment> Comments { get; set; }
        public string CommenterName { get; set; }
    }
}
