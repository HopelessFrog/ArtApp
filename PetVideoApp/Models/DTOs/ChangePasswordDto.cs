namespace PetVideoApp.Models.DTOs;

public record ChangePasswordDto(string Password)
{
    public static implicit operator ChangePasswordDto(string password) 
        => new(password);
}