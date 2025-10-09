namespace CoreLib.Dtos
{
    public record DishDto(int Id, string Name, decimal Price, int RestaurantId);
    public record CreateDishDto(string Name, decimal Price, int RestaurantId);
    public record UpdateDishDto(string Name, decimal Price);
}
