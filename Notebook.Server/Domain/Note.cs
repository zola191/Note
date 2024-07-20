namespace Notebook.Server.Domain
{
    public class Note
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Country { get; set; }
        public DateTime? BirthDay { get; set; }
        public string? Organization { get; set; }
        public string? Position { get; set; }
        public string? Other { get; set; }

        //one to many relationship with User
        public string? UserId { get; set; } //Foreign Key Property
        public User User { get; set; } // Navigation Property to represent the User

        public string? ExternalGoogleUserId { get; set; }
        public ExternalGoogleUser ExternalGoogleUser { get; set; }
    }
}
