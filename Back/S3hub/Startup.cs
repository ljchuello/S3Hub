using Microsoft.OpenApi.Models;

namespace S3hub;

public class Startup
{
    private string cors = "LJChuello";

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        AddSwagger(services);
        services.AddCors(x =>
        {
            x.AddPolicy(cors, y =>
            {
                y.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
            });
        });
    }

    private void AddSwagger(IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            var groupName = "v1";

            options.SwaggerDoc(groupName, new OpenApiInfo
            {
                Title = $"Foo {groupName}",
                Version = groupName,
                Description = "Foo API",
                Contact = new OpenApiContact
                {
                    Name = "Foo Company",
                    Email = string.Empty,
                    Url = new Uri("https://foo.com/"),
                }
            });

            if (Environment.MachineName == "DESKTOP-DUJGKJ0")
                options.AddServer(new OpenApiServer
                {
                    Url = "https://localhost:57512"
                });
        });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseHttpsRedirection();

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Foo API V1");
        });

        app.UseRouting();

        app.UseCors(cors);

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapGet("/", async context =>
            {
                //await context.Response.WriteAsync("Welcome to running ASP.NET Core on AWS Lambda");
                await Task.Delay(1);
                context.Response.Redirect("/swagger/");
            });
        });
    }
}