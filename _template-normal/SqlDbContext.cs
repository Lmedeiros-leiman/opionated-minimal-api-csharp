using Microsoft.EntityFrameworkCore;

namespace TemplateNormal;

public class SqlDbContext(DbContextOptions<SqlDbContext> options) : DbContext(options)
{

}
