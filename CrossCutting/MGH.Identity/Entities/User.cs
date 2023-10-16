using Microsoft.AspNetCore.Identity;

namespace MGH.Identity.Entities;

public class User : IdentityUser<int>
{
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string CellNumber { get; set; }
    public DateTime? BirthDate { get; set; }
    public string Address { get; set; }
    public string Image { get; set; }
    public string Fullname => Firstname + ' ' + Lastname;
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public DateTime? DeletedDate { get; set; }
    public string CreatedBy { get; set; }
    public string UpdatedBy { get; set; }
    public string DeletedBy { get; set; }
    public bool IsActive { get; set; }
    public bool IsUpdated { get; set; }

    public bool IsDeleted { get; set; }

    public ICollection<UserRefreshToken> UserRefreshTokens { get; set; }

}