using WebshopBackend.Contracts;

namespace WebshopBackend.Endpoints
{
    public static class EndpointsMapper
    {
        public static void MapMyEndpoints(this WebApplication app)
        {
            var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider.GetServices<IEndpoints>();

            foreach (var service in services)
            {
                service.RegisterEndpoints(app);
            }
        }
    }
}
