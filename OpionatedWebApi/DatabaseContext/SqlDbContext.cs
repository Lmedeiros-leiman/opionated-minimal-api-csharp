using Microsoft.EntityFrameworkCore;

namespace OpionatedWebApi.DatabaseContext;

public class SqlDbContext(DbContextOptions<SqlDbContext> options) : DbContext(options)
{

}
