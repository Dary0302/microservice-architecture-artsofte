namespace CoreLib.Entities
{
    public class Restaurant
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public ICollection<Dish> Dishes { get; set; } = new List<Dish>();
    }
}
