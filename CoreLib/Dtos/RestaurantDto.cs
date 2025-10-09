namespace CoreLib.Dtos
{
    public record RestaurantDto(int Id, string Name, string Address);
    public record CreateRestaurantDto(string Name, string Address);
    public record UpdateRestaurantDto(string Name, string Address);
}
