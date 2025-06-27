namespace PetVideoApp.Models.DTOs;

public record LogoutDto(string RefreshToken)
{
    public static implicit operator LogoutDto(string refreshToken) 
        => new(refreshToken);
}