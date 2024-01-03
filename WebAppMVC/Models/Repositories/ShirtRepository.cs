namespace WebAppMVC.Models.Repositories
{
    public static class ShirtRepository
    {
        private static List<Shirt> shirts = new List<Shirt>()
        {
            new Shirt {Id = 1, Brand = "Nike", Color = "Black", Gender = "men", Price = 29.99, Size = 7},
            new Shirt {Id = 2, Brand = "Addidas", Color = "Yellow", Gender = "women", Price = 59.99, Size = 10},
            new Shirt {Id = 3, Brand = "Atletics", Color = "Brown", Gender = "men", Price = 19.99, Size = 9},
            new Shirt {Id = 4, Brand = "Puma", Color = "White", Gender = "women", Price = 39.99, Size = 12}
        };

        public static List<Shirt> GetShirts()
        {
            return shirts;
        }

        public static bool ShirtExists(int id)
        {
            return shirts.Any(shirt => shirt.Id == id);
        }

        public static Shirt? GetShirtById(int id)
        {
            return shirts.FirstOrDefault(x => x.Id == id);
        }

        public static Shirt? GetShirtByProperties(string? brand, string? gender, string? color, int? size)
        {
            return shirts.FirstOrDefault(x => 
                !string.IsNullOrWhiteSpace(brand) && !string.IsNullOrWhiteSpace(x.Brand) && x.Brand.Equals(brand, StringComparison.OrdinalIgnoreCase) &&
                !string.IsNullOrWhiteSpace(gender) && !string.IsNullOrWhiteSpace(x.Gender) && x.Gender.Equals(gender, StringComparison.OrdinalIgnoreCase) &&
                !string.IsNullOrWhiteSpace(color) && !string.IsNullOrWhiteSpace(x.Color) && x.Color.Equals(color, StringComparison.OrdinalIgnoreCase) && 
                size.HasValue && x.Size.HasValue && size.Value == x.Size.Value);
        }

        public static void AddShirt(Shirt shirt)
        {
            int maxId = shirts.Max(x => x.Id);
            shirt.Id = maxId + 1;

            shirts.Add(shirt);
        }

        public static void UpdateShirt(Shirt shirt)
        {
            var shirtToUpdate = shirts.FirstOrDefault(x => x.Id == shirt.Id);
            shirtToUpdate.Brand = shirt.Brand;
            shirtToUpdate.Price = shirt.Price;
            shirtToUpdate.Size = shirt.Size;
            shirtToUpdate.Color = shirt.Color;
            shirtToUpdate.Gender = shirt.Gender;
        }

        public static void DeleteShirt(int id)
        {
            var shirt = GetShirtById(id);
            if(shirt != null)
            {
                shirts.Remove(shirt);
            }
        }
    }
}
