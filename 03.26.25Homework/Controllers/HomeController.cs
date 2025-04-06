using _03._26._25Homework.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace _03._26._25Homework.Controllers
{
    public class HomeController : Controller
    {
        private string _connectionString = @"Data Source=.\sqlexpress;Initial Catalog=BlogPosts2;Integrated Security=true;TrustServerCertificate=yes;";

        public IActionResult Index(int page = 1)
        {
            BlogPostsDb db = new(_connectionString);

            int count = db.GetCount();

            GetPostsViewModel vm = new()
            {
                Posts = db.GetPosts(page),
                PageCount = count / 3,
                Page = page
            };
            
            if(count % 3 != 0)
            {
                vm.PageCount++;
            }

            foreach(Post p in vm.Posts)
            {
                if(p.Text.Length > 200)
                {
                    p.Text = p.Text.Substring(0, 200) + "...";
                }
            }

            return View(vm);
        }

        public IActionResult ViewBlog(int id)
        {
            BlogPostsDb db = new(_connectionString);

            GetPostViewModel vm = new()
            {
                Post = db.GetPost(id),
                Comments = db.GetComments(id)
            };

            string name = Request.Cookies["Name"];
            if(name != null)
            {
                vm.CommenterName = name;
            };

            return View(vm);
        }

        public IActionResult SubmitPost()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SubmitPost(Post p)
        {
            p.DateTime = DateTime.Now;
            BlogPostsDb db = new(_connectionString);
            int id = db.AddPost(p);
            return Redirect($"/Home/ViewBlog?id={id}");
        }

        [HttpPost]
        public IActionResult AddComment(Comment c)
        {            
            BlogPostsDb db = new(_connectionString);
            c.DateTime = DateTime.Now;
            db.AddComment(c);
            Response.Cookies.Append("Name", c.Name);
            return Redirect($"/Home/ViewBlog?id={c.PostId}");
        }
    }
}
