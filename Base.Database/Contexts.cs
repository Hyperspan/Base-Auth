using Auth.Domain.Context;
using Microsoft.EntityFrameworkCore;

namespace Base.Database
{
    public class Contexts : AuthContext<Guid>
    {
        public Contexts(DbContextOptions options)
            : base(options)
        {
        }
    }
}
