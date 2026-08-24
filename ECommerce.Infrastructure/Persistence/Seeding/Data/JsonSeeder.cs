using ECommerce.Domain.Entities;
using ECommerce.Domain.Common;
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
            Func<TModel, Result<TEnitity>> map,
            CancellationToken ct = default) where TEnitity : BaseEntity
    {
        // check if table is empty
        if (await dbSet.AnyAsync(ct)) return;

        // get the file
        var filePath = Path.Combine(AppContext.BaseDirectory, "Persistence", "Seeding", "Data", fileName);
        if (!File.Exists(filePath)) return;

        // open file stream to read file as chunks
        await using var stream = File.OpenRead(filePath);

        // convert JSON file to List
        var models = await JsonSerializer.DeserializeAsync<List<TModel>>(stream, _options, ct);

        // check if there are data in the JSON file, or the data failed to convert to list
        if (models is null || models.Count == 0) return;

        // convert the list of models to a list of Result<TEnitity>
        var results = models.Select(map).ToList();

        // check if there are any failure results
        var failures = results.Where(r => r.IsFailure).ToList();
        if (failures.Count > 0)
        {
            var errors = string.Join(" | ", failures.Select(f => f.Error!.Message));
            throw new InvalidDataException($"Seeding failed for {fileName}. Errors: {errors}");
        }

        var entities = results.Select(r => r.Value!).ToList();

        await dbSet.AddRangeAsync(entities, ct);

        // SaveChangesAsync() in the Central Place, Main Seed Class (DatabaseSeeder)
    }
}