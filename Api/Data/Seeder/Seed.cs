using Api.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Api.Data.Seeder
{
    public class Seed
    {

        public static async Task SeedIris(DataContext _context)
        {
            if (await _context.Irises.AnyAsync()) return;
            var IrisData =
                await File.ReadAllTextAsync("Data/Seeder/iris.json");
            var irises = JsonConvert.DeserializeObject<List<Iris>>(IrisData);
            foreach (var iris in irises)
            {
                _context.Irises.Add(iris);
            }

            await _context.SaveChangesAsync();
        }
    }
}
