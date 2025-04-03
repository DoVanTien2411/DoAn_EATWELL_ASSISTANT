namespace EatWellAssistant.Models
{
    public class LoadPostModel
    {
        public string SearchKey { get; set; }
        public int OrderBy { get; set; }
        public IEnumerable<int> CurrentIds { get; set; } = new int[0]; 
    }

    public class CommentModel
    {
        public int PostId { get; set; }
        public string Content { get; set; }
    }

    public class CreatePost
    {
        public string Content { get; set; }
        public IFormFile Image { get; set; }
    }
}
