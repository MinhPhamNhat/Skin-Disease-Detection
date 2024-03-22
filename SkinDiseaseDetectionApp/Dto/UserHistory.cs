public class UserHistory
{
    public string UserId { get; set; }
    public string FullName { get; set; }
    public string? PhoneNumber { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
    public DateTime DateOfBirth { get; set; }
    public bool Gender { get; set; }

    public string Image { get; set; }
    public DateTime DateCreated { get; set; }
    public string DiagnoseResult { get; set; }
    public double DiagnoseResultAccuracy { get; set; }

    public string DoctorId { get; set; }
    public string Information { get; set; }
}