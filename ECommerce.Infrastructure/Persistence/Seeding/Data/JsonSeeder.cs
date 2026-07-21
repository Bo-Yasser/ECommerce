using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace ECommerce.Infrastructure.Persistence.Seeding.Data;

public class JsonSeeder
{
    private static readonly JsonSerializerOptions _options = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public static async Task SeedIfEmpty<TEnitity, TModel>(
            DbSet<TEnitity> dbSet,
            string fileName,
            Func<TModel, TEnitity> map,
            CancellationToken ct = default) where TEnitity : BaseEntity
    {
        // check if table is empty
        if (await dbSet.AnyAsync()) return;

        // get the file
        var filePath = Path.Combine(AppContext.BaseDirectory, "Persistence", "Seeding", "Data", fileName);
        if(!File.Exists(filePath)) return;

        // open file stream to read file as chunks
        await using var stream = File.OpenRead(filePath);

        // convert JSON file to List
        var models = await JsonSerializer.DeserializeAsync<List<TModel>>(stream, _options, ct);

        // checl if there are data in the JSON file, or the data converted successfully to list
        if (models is null || models.Count == 0) return;

        // convert seed models to entities, to can add it to the table
        var entities = models.Select(map).ToList();
        await dbSet.AddRangeAsync(entities, ct);

        // SaveChangesAsync() in the Central Place, Main Seed Class (DatabaseSeeder)
    }
}
