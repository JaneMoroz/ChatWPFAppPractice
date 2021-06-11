using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Dna;

namespace ChatApp.Web.Server
{
    /// <summary>
    /// A shorthand access class to get DI services with nice clean short code
    /// </summary>
    public static class DI
    {
        /// <summary>
        /// The scoped instance of the <see cref="ApplicationDbContext"/>
        /// </summary>
        public static ApplicationDbContext ApplicationDbContext => Framework.Provider.GetService<ApplicationDbContext>();
    }
}
