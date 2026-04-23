namespace ECommerce.BLL
{
    public record TokenDto
    (
        string AccessToken,
        int DurationInMinutes,
        string TokenType = "Bearer"
    );
}
