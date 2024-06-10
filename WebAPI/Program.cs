
using ApplicationServices.Implementations;
using ApplicationServices.Interfaces;
using Data.Contexts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Repositories.Implementations;
using Repositories.Interfaces;
using Serilog;
using System.Reflection;
using System.Text;

namespace WebAPI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            //var builder = WebApplication.CreateBuilder(args);

            //// Add services to the container.
            //var connectionString = builder.Configuration.GetConnectionString("DefaultConnectionString");
            //builder.Services.AddDbContext<ForumDbContext>(options => options.UseSqlServer(connectionString,
            //            x => x.MigrationsAssembly("WebAPI")));

            //// Add services to the container.

            //builder.Services.AddControllers();
            //// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            //builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen();

            //var app = builder.Build();

            //// Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
            //    app.UseSwagger();
            //    app.UseSwaggerUI();
            //}

            //app.UseHttpsRedirection();

            //app.UseAuthorization();


            //app.MapControllers();

            //app.Run();
            var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", true)
            .Build();

            Log.Logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(configuration)
                    .CreateLogger();

            try
            {
                Log.Logger.Information("Application is starting!");

                var builder = WebApplication.CreateBuilder(args);

                // Add services to the container.
                var connectionString = builder.Configuration.GetConnectionString("DefaultConnectionString");
                builder.Services.AddDbContext<ForumDbContext>(options => options.UseSqlServer(connectionString,
                            x => x.MigrationsAssembly("WebAPI")));

                builder.Services.AddControllers();
                // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen(options =>
                {
                    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                    {
                        Version = "v1",
                        Title = "Forum",
                        Description = "RESTful API for an online forum application.",
                        TermsOfService = new Uri("https://example.com/terms"),
                        Contact = new OpenApiContact
                        {
                            Name = "Example Contact",
                            Url = new Uri("https://example.com/contact")
                        },
                        License = new OpenApiLicense
                        {
                            Name = "Example License",
                            Url = new Uri("https://example.com/license")
                        },
                    });
                    //
                    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Description = "Please enter token",
                        Name = "Authorization",
                        Type = SecuritySchemeType.Http,
                        BearerFormat = "JWT",
                        Scheme = "bearer"
                    });

                    options.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type=ReferenceType.SecurityScheme,
                                    Id="Bearer"
                                }
                            },
                            Array.Empty<string>()
                        }
                    });
                    //

                    //var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    //options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
                    // Include XML comments (ensure the file path is correct)
                    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    options.IncludeXmlComments(xmlPath);
                    
                    var modelsXmlFilename = $"{Assembly.Load("Messaging").GetName().Name}.xml";
                    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, modelsXmlFilename));
                });
                // Add serilog
                builder.Services.AddSerilog();

                // Authentication
                string tokenKey = configuration["Authentication:TokenKey"] ?? "Not working token key";
                builder.Services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                }).AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new()
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(tokenKey)),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                    };
                });



                // Start SERVICE DI
                builder.Services.AddScoped<DbContext, ForumDbContext>();//
                builder.Services.AddScoped<ICommentsRepository, CommentsRepository>();//
                builder.Services.AddScoped<ICommentVotesRepository, CommentVotesRepository>();//
                builder.Services.AddScoped<IDiscussions_TopicsRepository, Discussions_TopicsRepository>();//
                builder.Services.AddScoped<IDiscussionsRepository, DiscussionsRepository>();//
                builder.Services.AddScoped<IDiscussionVotesRepository, DiscussionVotesRepository>();//
                builder.Services.AddScoped<ITopicsRepository, TopicsRepository>();//
                builder.Services.AddScoped<IUsersRepository, UsersRepository>();//
                builder.Services.AddScoped<IViewsRepository, ViewsRepository>();//
                builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();//

                builder.Services.AddScoped<ICommentManagementService, CommentManagementService>();
                builder.Services.AddScoped<IDiscussionVoteManagementService, DiscussionVoteManagementService>();
                builder.Services.AddScoped<IDiscussionManagementService, DiscussionManagementService>();
                builder.Services.AddScoped<IDiscussions_TopicsManagementService, Discussions_TopicsManagementService>();
                builder.Services.AddScoped<ICommentVoteManagementService, CommentVoteManagementService>();
                builder.Services.AddScoped<ITopicManagementService, TopicManagementService>();
                builder.Services.AddScoped<IUserManagementService, UserManagementService>();
                builder.Services.AddScoped<IViewManagementService, ViewManagementService>();

                //builder.Services.AddSingleton<IJWTAuthenticationManager>(new JWTAuthenticationManager(tokenKey));
                //builder.Services.AddSingleton<IJWTAuthenticationManager>(provider => new JWTAuthenticationManager(tokenKey, provider.GetRequiredService<IUnitOfWork>()));//
                builder.Services.AddScoped<IJWTAuthenticationManager>(provider => new JWTAuthenticationManager(tokenKey, provider.GetRequiredService<IUnitOfWork>()));

                //builder.Services.AddScoped<IJWTAuthenticationManager, JWTAuthenticationManager>();

                // End SERVICE DI

                var app = builder.Build();

                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

                app.UseAuthentication();//
                app.UseAuthorization();

                app.MapControllers();

                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Unhandled exception!");
            }
            finally
            {
                await Log.CloseAndFlushAsync();
            }
        }
    }
}