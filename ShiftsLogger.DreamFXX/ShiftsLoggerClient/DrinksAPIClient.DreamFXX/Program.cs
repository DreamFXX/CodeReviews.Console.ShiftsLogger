            using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace DrinksAPIClient.DreamFXX
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var client = new JsonPlaceholderClient(new HttpClient());
            var posts = await client.GetPostsAsync();
            foreach (var post in posts)
            {
                Console.WriteLine($"{post.Id}: {post.Title}");
            }
        }
    }

    public class JsonPlaceholderClient
   {
        private readonly HttpClient _httpClient;

        public JsonPlaceholderClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Post>> GetPostsAsync()
        {
            var response = await _httpClient.GetAsync("https://jsonplaceholder.typicode.com/posts");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Post>>(json);
        }
    }

    public class Post
    {
        public int UserId { get; set; }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
    }
}
