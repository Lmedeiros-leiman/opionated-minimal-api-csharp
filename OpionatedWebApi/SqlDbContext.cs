using Microsoft.EntityFrameworkCore;

namespace OpionatedWebApi;

public class SqlDbContext(DbContextOptions<SqlDbContext> options) : DbContext(options)
{

}
