using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using RestAPIDemo.Models;
using RestAPIDemo.Repositories.Authenticators;
using RestAPIDemo.Repositories.PasswordHashers;
using RestAPIDemo.Repositories.RefreshTokenReopsitory;
using RestAPIDemo.Repositories.RefreshTokenRepository;
using RestAPIDemo.Repositories.TokenGenerators;
using RestAPIDemo.Repositories.TokenValidators;
using RestAPIDemo.Repositories.UserRepositories;
using RestAPIDemo.StockData;
using RestAPIDemo.StockDatas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestAPIDemo
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        private const string SECRET_KEY = "thisisabigsecretkeybubuyukgizlibiranahtardir";
        public static readonly SymmetricSecurityKey SIGNING_KEY =
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SECRET_KEY));
        public Startup(IConfiguration configuration, IConfiguration configuration1)
        {
            Configuration = configuration;
            _configuration = configuration1;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options => options.AddDefaultPolicy( 
                builder => builder.AllowAnyOrigin()));  
                
            services.AddHealthChecks();
            services.AddMvc(options =>
            {
                options.AllowEmptyInputInBodyModelBinding = true;
                foreach (var formatter in options.InputFormatters)
                {
                    if (formatter.GetType() == typeof(SystemTextJsonInputFormatter))
                        ((SystemTextJsonInputFormatter)formatter).SupportedMediaTypes.Add(
                        Microsoft.Net.Http.Headers.MediaTypeHeaderValue.Parse("text/plain"));
                }
            }).AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
            });
            services.AddControllers().AddNewtonsoftJson();
            services.AddControllers();
            services.AddDbContextPool<StockContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("StockContextConnectionString")));
            services.AddDbContext<AuthenticationDbContext>(options =>
            options.UseSqlServer(_configuration.GetConnectionString("Connstr"))); 
            services.AddDbContext<AuthsDbContext>(options =>
            options.UseSqlServer(_configuration.GetConnectionString("myConn")));
            services.AddScoped<IStockData, SqlStockData>();
            //services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString()));

            //For Identity
            //services.AddIdentity<ApplicationDbContext, IdentityRole>()
            //    .AddEntityFrameworkStores<ApplicationDbContext>()
            //    .AddDefaultTokenProviders();

            //services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme.ToString();
            //})
            //.AddJwtBearer(options => 
            //{
            //    options.SaveToken = true;
            //    options.RequireHttpsMetadata = false;
            //    options.TokenValidationParameters = new TokenValidationParameters()
            //    {
            //        ValidateIssuer = true,
            //        ValidateAudience = true,
            //        ValidIssuer = Configuration["JWT:ValidIssuer"],
            //        ValidAudience = Configuration["JWT:ValidAudience"],
            //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration))

            //    };
            //});

            ////Configure the JWT Authentication Service
            // services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = "JwtBearer";
            //    options.DefaultChallengeScheme = "JwtBearer";
            //})
            // .AddJwtBearer("JwtBearer", jwtOptions =>
            // {
            //     jwtOptions.TokenValidationParameters = new TokenValidationParameters()
            //     {
            //         //Signing key is defined in the TokenController class
            //         IssuerSigningKey = SIGNING_KEY,
            //         ValidateIssuer = true,
            //         ValidateAudience = true,
            //         ValidIssuer = "https://localhost:5001",
            //         ValidAudience = "https://localhost:5001",
            //         ValidateLifetime = true

            //     };
            // });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
           {
               AuthenticationConfiguration authenticationConfiguration = new AuthenticationConfiguration();
               _configuration.Bind("Authentication", authenticationConfiguration);

               options.TokenValidationParameters = new TokenValidationParameters()
               {
                   IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationConfiguration.AccessTokenSecret)),
                   ValidIssuer = authenticationConfiguration.Issuer,
                   ValidAudience = authenticationConfiguration.Audience,
                   ValidateIssuerSigningKey = true,
                   ValidateIssuer = true,
                   ValidateAudience = true,
                   ClockSkew = TimeSpan.Zero
               };
           });
        //.AddJwtBearer("JwtBearer", jwtOptions =>
        //{
        //    jwtOptions.TokenValidationParameters = new TokenValidationParameters()
        //    {
        //            //Signing key is defined in the TokenController class
        //            IssuerSigningKey = SIGNING_KEY,
        //        ValidateIssuer = true,
        //        ValidateAudience = true,
        //        ValidIssuer = "https://localhost:5001",
        //        ValidAudience = "https://localhost:5001",
        //        ValidateLifetime = true

        //    };
        //});

            AuthenticationConfiguration authenticationConfiguration = new AuthenticationConfiguration();
            _configuration.Bind("Authentication", authenticationConfiguration);
            services.AddSingleton(authenticationConfiguration);

            services.AddSingleton<RefreshTokenValidator>();
            services.AddSingleton<TokenGenerator>();
            services.AddSingleton<RefreshTokenGenerator>();
            services.AddSingleton<AccessTokenGenerator>();
            services.AddScoped<Authenticator>();
            services.AddSingleton<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IUserRepository, DbUserRepository>();
            services.AddScoped<IRefreshTokenRepository, DbRefreshTokenRepository>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseCors(options =>
            //    options.WithOrigins("http://localhost:8080")
            //    .AllowAnyHeader()
            //    .AllowAnyMethod());

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
