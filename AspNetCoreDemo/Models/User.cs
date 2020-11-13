namespace AspNetCoreDemo.Models
{
    public class User
    {
        public string Name { get; set; }
        public string Title { get; set; }

        public override string ToString() => $"{Name} - {Title}";
    }
}
