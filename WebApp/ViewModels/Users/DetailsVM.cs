namespace WebApp.ViewModels.Users
{
    public class DetailsVM
    {
        public int? Id { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool IsVisible { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Country { get; set; }
        public string Biography { get; set; }
        public DateTime BirthDay { get; set; }
    }
}
