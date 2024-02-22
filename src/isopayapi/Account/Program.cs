
using Account.Service.BusinessLogic;
using Org.Eclipse.TractusX.Portal.Backend.Framework.Web; 
using Org.Eclipse.TractusX.Portal.Backend.PortalBackend.DBAccess;

var VERSION = "v2";

WebApplicationBuildRunner
    .BuildAndRunWebApplication<Program>(args, "account", VERSION, builder =>
    {
        builder.Services
            .AddPortalRepositories(builder.Configuration);

        builder.Services
            .AddTransient<ITransaction, Transaction>();
        //    .ConfigureNotificationSettings(builder.Configuration.GetSection("Notifications"));
    });


//namespace Account
//{
//    public class Program
//    {
//        public static void Main(string[] args)
//        {
//            var builder = WebApplication.CreateBuilder(args);

//            // Add services to the container.

//            builder.Services.AddControllers();
//            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//            builder.Services.AddEndpointsApiExplorer();
//            builder.Services.AddSwaggerGen();

//            var app = builder.Build();

//            // Configure the HTTP request pipeline.
//            if (app.Environment.IsDevelopment())
//            {
//                app.UseSwagger();
//                app.UseSwaggerUI();
//            }

//            app.UseHttpsRedirection();

//            app.UseAuthorization();


//            app.MapControllers();

//            app.Run();
//        }
//    }
//}
