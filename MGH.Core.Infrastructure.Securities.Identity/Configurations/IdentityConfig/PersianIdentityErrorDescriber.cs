using Microsoft.AspNetCore.Identity;

namespace MGH.Core.Infrastructure.Securities.Identity.Configurations.IdentityConfig;

public class PersianIdentityErrorDescriber : IdentityErrorDescriber
{
    public override IdentityError DuplicateEmail(string email)
        => new()
        {
            Code = nameof(DuplicateEmail),
            Description = $"ایمیل '{email}' قبلا توسط شخص دیگری انتخاب شده است"
        };

    public override IdentityError DuplicateUserName(string userName)
        => new()
        {
            Code = nameof(DuplicateUserName),
            Description = $"نام کاربری {userName} قبلا توسط شخص دیگری انتخاب شده است"
        };

    public override IdentityError InvalidEmail(string email)
        => new()
        {
            Code = nameof(InvalidEmail),
            Description = $"ایمیل '{email}' ، یک ایمیل معتبر نیست"
        };

    public override IdentityError DuplicateRoleName(string role)
        => new()
        {
            Code = nameof(DuplicateRoleName),
            Description = $"مقام '{role}' قبلا ثبت شده است"
        };

    public override IdentityError InvalidRoleName(string role)
        => new()
        {
            Code = nameof(InvalidRoleName),
            Description = $"نام '{role}' معتبر نیست"
        };

    public override IdentityError PasswordRequiresDigit()
        => new()
        {
            Code = nameof(PasswordRequiresDigit),
            Description = $"رمز عبور باید حداقل دارای یک عدد باشد"
        };

    public override IdentityError PasswordRequiresLower()
        => new()
        {
            Code = nameof(PasswordRequiresLower),
            Description = $"رمز عبور باید حداقل دارای یک کاراکتر انگلیسی کوچک باشد ('a'-'z')"
        };

    public override IdentityError PasswordRequiresUpper()
        => new()
        {
            Code = nameof(PasswordRequiresUpper),
            Description = $"رمز عبور باید حداقل دارای یک کاراکتر انگلیسی بزرگ باشد ('A'-'Z')"
        };

    public override IdentityError PasswordRequiresNonAlphanumeric()
        => new()
        {
            Code = nameof(PasswordRequiresNonAlphanumeric),
            Description = $"رمز عبور باید حداقل دارای یک کاراکتر ویژه باشد مثل '@#%^&'"
        };

    public override IdentityError PasswordRequiresUniqueChars(int uniqueChars)
        => new()
        {
            Code = nameof(PasswordRequiresUniqueChars),
            Description = $"رمز عبور باید حداقل دارای {uniqueChars} کاراکتر منحصر به فرد باشد"
        };

    public override IdentityError PasswordTooShort(int length)
        => new()
        {
            Code = nameof(PasswordTooShort),
            Description = $"رمز عبور نباید کمتر از {length} کاراکتر باشد"
        };

    public override IdentityError InvalidUserName(string userName)
        => new()
        {
            Code = nameof(InvalidUserName),
            Description = $"نام کاربری '{userName}' معتبر نیست، نام کاربری فقط میتواند دارای حروف یا عدد باشد"
        };

    public override IdentityError UserNotInRole(string role)
        => new()
        {
            Code = nameof(UserNotInRole),
            Description = $"کاربر در مقام '{role}' نیست"
        };

    public override IdentityError UserAlreadyInRole(string role)
        => new()
        {
            Code = nameof(UserAlreadyInRole),
            Description = $"کاربر در مقام '{role}' است"
        };

    public override IdentityError DefaultError()
        => new()
        {
            Code = nameof(DefaultError),
            Description = $"خطای پیشبینی نشده رخ داد"
        };
}