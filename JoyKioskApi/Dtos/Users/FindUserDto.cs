namespace JoyKioskApi.Dtos.Users
{
    public class FindUserResDto
    {
        public Guid? Id { get; set; }
        public int? UserId { get; set; }
        public int? CustId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? MobileNo { get; set; }
        public bool IsActive { get; set; }
        public string? Username { get; set; }
    }
}
