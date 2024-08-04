

using BasicAPI.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
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
        builder.Services.AddSwaggerGen();
        builder.Services.AddAuthentication("Bearer").AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new  ()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration.GetValue<string>("Authentication:Issuer"),
                ValidAudience = builder.Configuration.GetValue<string>("Authentication:Audience"),
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("Authentication:SecretKey")))
            };
        });
        builder.Services.AddAuthorization(options =>
        {
            //custom policies
            options.AddPolicy(PolicyConstants.MustHaveEmployeeId, policy => policy.RequireClaim("employeeId"));
            options.AddPolicy(PolicyConstants.MustBeAdmin, policy => policy.RequireClaim("role", "Admin"));

            //must match both claims to be authorized
            options.AddPolicy(PolicyConstants.MustBeOwner, policy => {
                policy.RequireClaim("role", "Admin");
                //to match any of the employeeId values less than 11
                policy.RequireClaim("employeeId", "E001", "E002", "E003", "E004", "E005", "E006", "E007", "E008", "E009");
            });

            //locking down the app
            options.FallbackPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
        });

        builder.Services.AddApiVersioning(options =>
        {
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new (0, 3);

            options.ReportApiVersions = true;
        });
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
