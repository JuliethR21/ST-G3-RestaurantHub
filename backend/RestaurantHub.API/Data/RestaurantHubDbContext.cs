using Microsoft.EntityFrameworkCore;

namespace RestaurantHub.API.Data;

public class RestaurantHubDbContext : DbContext
{
    public RestaurantHubDbContext(DbContextOptions<RestaurantHubDbContext> options)
        : base(options)
    {
    }
}