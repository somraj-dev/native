using AxioVital.Domain.Entities;
using AxioVital.Infrastructure.Persistence;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AxioVital.IntegrationTests.Persistence;

public class DbContextTests
{
    [Fact]
    public async Task SaveChangesAsync_ShouldSetCreatedAtUtc_OnNewEntity()
    {
        var options = new DbContextOptionsBuilder<AxioVitalDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new AxioVitalDbContext(options);

        var tenant = new Tenant { Name = "St. Jude Hospital", Identifier = "st-jude" };
        context.Tenants.Add(tenant);
        await context.SaveChangesAsync();

        tenant.CreatedAtUtc.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));

        var savedTenant = await context.Tenants.FirstOrDefaultAsync(t => t.Identifier == "st-jude");
        savedTenant.Should().NotBeNull();
        savedTenant!.Name.Should().Be("St. Jude Hospital");
    }
}
