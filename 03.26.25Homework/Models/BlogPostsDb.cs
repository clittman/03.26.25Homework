using Microsoft.Data.SqlClient;

namespace _03._26._25Homework.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime DateTime { get; set; }
    }

    public class Comment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public DateTime DateTime { get; set; }
        public int PostId { get; set; }
    }

    public class BlogPostsDb
    {
        private readonly string _connectionString;

        public BlogPostsDb(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Post> GetPosts(int page)
        {
            List<Post> posts = new();

            using SqlConnection connection = new(_connectionString);
            using SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Posts "+
                "ORDER BY DATETIME desc "+
                "OFFSET @skip ROWS " +
                "FETCH NEXT 3 ROWS ONLY";
            cmd.Parameters.AddWithValue("@skip", page * 3 - 3);
            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                posts.Add(new Post
                {
                    Id = (int)reader["Id"],
                    Title = (string)reader["Title"],
                    Text = (string)reader["Text"],
                    DateTime = (DateTime)reader["DateTime"],                   
                });
            }

            return posts;
        }

        public Post GetPost(int id)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Posts WHERE Id = @id";
            cmd.Parameters.AddWithValue("@id", id);
            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            reader.Read();
            return new Post
            {
                Id = (int)reader["Id"],
                Title = (string)reader["Title"],
                Text = (string)reader["Text"],
                DateTime = (DateTime)reader["DateTime"]
            };
        }

        public List<Comment> GetComments(int id)
        {
            List<Comment> comments = new();

            using SqlConnection connection = new(_connectionString);
            using SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Comments WHERE PostId = @id";
            cmd.Parameters.AddWithValue("@id", id);
            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                comments.Add(new Comment
                {
                    Id = (int)reader["Id"],
                    Name = (string)reader["Name"],
                    Text = (string)reader["Text"],
                    DateTime = (DateTime)reader["DateTime"],
                });
            }

            return comments;
        }

        public int AddPost(Post p)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO Posts (Title, Text, DateTime) " +
                "VALUES (@title, @text, @dateTime) " +
                "SELECT SCOPE_Identity()";
            cmd.Parameters.AddWithValue("@title", p.Title);
            cmd.Parameters.AddWithValue("@text", p.Text);
            cmd.Parameters.AddWithValue("@dateTime", p.DateTime);
            connection.Open();
            int newId = (int)(decimal)cmd.ExecuteScalar();
            cmd.ExecuteNonQuery();
            return newId;
        }

        public void AddComment(Comment c)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO Comments (Name, Text, DateTime, PostId) " +
                "VALUES (@name, @text, @dateTime, @postid)";
            cmd.Parameters.AddWithValue("@name", c.Name);
            cmd.Parameters.AddWithValue("@text", c.Text);
            cmd.Parameters.AddWithValue("@dateTime", c.DateTime);
            cmd.Parameters.AddWithValue("@postId", c.PostId);
            connection.Open();
            cmd.ExecuteNonQuery();
        }

        public int GetCount()
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT COUNT(*) AS Count FROM Posts";
            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            reader.Read();
            int count = (int)reader["Count"];
            return count;
        }

    }
}
