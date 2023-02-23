namespace qsign.server.Models;

// public class User
// {
//     public string Id { get; set; }
//     public Guid PublicId { get; set; }
//     public string? FirstName { get; set; }
//     public string? LastName { get; set; }
//     public string Email { get; set; }
//     public string? Password { get; set; }
//     public string PubKeyPem { get; set; }
//     public string PrivKeyPem { get; set; }
// }

public class UserAccount
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PasswordHash { get; set; }
    public string Salt { get; set; }
    public string PublicKey { get; set; }
    public string PrivateKey { get; set; }
}

public class UserLoginDTO
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class UserRegisterDTO
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}