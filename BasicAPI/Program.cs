

using BasicAPI.Constants;
using BasicAPI.HealthChecks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace BasicAPI;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers();
        
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();

        //swagger
        builder.Services.AddSwaggerGen(options =>
        {
              //configuring the default swagger page
              var title = "BasicAPI";
            var description = "This is a basic API";
            var terms = new Uri("https://example.com/terms");
            var license = new OpenApiLicense()
            {
                Name = "MIT",
                Url = new Uri("https://opensource.org/licenses/MIT")
            };
            var contact = new OpenApiContact()
            {
                Name = "Ahmed Aladdin",
                Email = "aladdin.fullstack@gmail.com",
                Url = new Uri("https://www.linkedin.com/in/my-linked-in/")
            };
            options.SwaggerDoc("v1.3", new OpenApiInfo
            {
                Version = "v1.3",
                Title = $"{title} v1.3 (deprecated)",
                Description = description,
                TermsOfService = terms,
                License = license,
                Contact = contact
            });
            options.SwaggerDoc("v2.6", new OpenApiInfo
            {
                Version = "v2.6",
                Title = $"{title} v2.6",
                Description = description,
                TermsOfService = terms,
                License = license,
                Contact = contact
            });
            options.SwaggerDoc("v3.9", new OpenApiInfo
            {
                Version = "v3.9",
                Title = $"{title} v3.9",
                Description = description,
                TermsOfService = terms,
                License = license,
                Contact = contact
            });

        });

        //api versioning
        builder.Services.AddApiVersioning(options =>
        {
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new(1, 3);
            options.ReportApiVersions = true;
        });
        builder.Services.AddVersionedApiExplorer(options =>
        {
                                          
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                //listing different version manually (can be added automatically through code, but the trouble out weights the benefits)
                //add version 2 at the top, because we want the latest version to be the default
                options.SwaggerEndpoint("/swagger/v3.9/swagger.json", "BasicAPI v3.9");
                options.SwaggerEndpoint("/swagger/v2.6/swagger.json", "BasicAPI v2.6");
                options.SwaggerEndpoint("/swagger/v1.3/swagger.json", "BasicAPI v1.3");
            });
        }

        app.UseHttpsRedirection();

        //builder.Services.AddAuthentication("Bearer").AddJwtBearer(options =>
        //{
        //    options.TokenValidationParameters = new()
        //    {
        //        ValidateIssuer = true,
        //        ValidateAudience = true,
        //        ValidateIssuerSigningKey = true,
        //        ValidIssuer = builder.Configuration.GetValue<string>("Authentication:Issuer"),
        //        ValidAudience = builder.Configuration.GetValue<string>("Authentication:Audience"),
        //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("Authentication:SecretKey")))
        //    };
        //});

        //builder.Services.AddAuthorization(options =>
        //{
        //    //custom policies
        //    options.AddPolicy(PolicyConstants.MustHaveEmployeeId, policy => policy.RequireClaim("employeeId"));
        //    options.AddPolicy(PolicyConstants.MustBeAdmin, policy => policy.RequireClaim("role", "Admin"));

        //    //must match both claims to be authorized
        //    options.AddPolicy(PolicyConstants.MustBeOwner, policy =>
        //    {
        //        policy.RequireClaim("role", "Admin");
        //        //to match any of the employeeId values less than 11
        //        policy.RequireClaim("employeeId", "E001", "E002", "E003", "E004", "E005", "E006", "E007", "E008", "E009");
        //    });

        //    //locking down the app
        //    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        //        .RequireAuthenticatedUser()
        //        .Build();
        //});

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
