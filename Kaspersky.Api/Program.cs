using AutoMapper;
using FluentValidation.AspNetCore;
using Kaspersky.Api.Bookshelf.Service;
using Kaspersky.Api.Common;
using Kaspersky.Database;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;
using System.Reflection;

namespace Kaspersky.Api
{
    public sealed class Program
    {
        public static void Main( string[] args )
           => CreateWebHostBuilder( args )
           .Build()
           .Run();

        public static IWebHostBuilder CreateWebHostBuilder( string[] args )
            => WebHost
            .CreateDefaultBuilder( args )
            .UseStartup<Startup>();

        private sealed class Startup
        {
            private readonly IConfiguration _configuration;
            public Startup( IConfiguration configuration ) => _configuration = configuration;

            public void ConfigureServices( IServiceCollection services )
                => services
                .AddScoped<IBookshelfService, BookshelfService>()
                .ConfigureSwagger()
                .ConfigureDatabase()
                .AddAutoMapper()
                .AddMvc
                (
                    options =>
                    {
                        options.Filters.Add( new ModelValidatorFilter() );
                        options.Filters.Add( new DefaultExceptionFilter() );
                    }
                )
                .ConfigureJson()
                .AddFluentValidation( options => options.RegisterValidatorsFromAssemblyContaining<Program>() )
                .SetCompatibilityVersion( CompatibilityVersion.Version_2_1 );

            public void Configure( IApplicationBuilder appBuilder, IHostingEnvironment envirenment )
            {
                EnsureDb();

                if (envirenment.IsDevelopment())
                    appBuilder.UseDeveloperExceptionPage();

                appBuilder
                    .UseSwagger()
                    .UseSwaggerUI( options => options.SwaggerEndpoint( "/swagger/v1/swagger.json", typeof( Program ).Namespace.ToString() ) )
                    .UseMvc();

                void EnsureDb()
                {
                    using (var scope = appBuilder.ApplicationServices.CreateScope())
                    using (var db = scope.ServiceProvider.GetService<BookshelfContext>())
                    {
                        db.Database.EnsureCreated();
                        Mockdata.InsertAsync( db ).GetAwaiter().GetResult();
                    }
                }
            }
        }

        private sealed class ModelValidatorFilter : IActionFilter
        {
            public void OnActionExecuting( ActionExecutingContext context )
            {
                if (!context.ModelState.IsValid)
                    context.Result = new BadRequestObjectResult( context.ModelState );
            }

            public void OnActionExecuted( ActionExecutedContext context )
            {
            }
        }

        private sealed class DefaultExceptionFilter : IExceptionFilter
        {
            public void OnException( ExceptionContext context )
            {
                const int errorCode = (int)System.Net.HttpStatusCode.InternalServerError;
                context.Result = new ObjectResult
                (
                    new ApiError( errorCode, context.Exception.Message )
                )
                { StatusCode = errorCode };
            }
        }
    }

    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureSwagger( this IServiceCollection services )
            => services
            .AddSwaggerGen
            (
                options =>
                {
                    options.SwaggerDoc
                    (
                        "v1",
                        new Info { Title = typeof( Program ).Namespace.ToString() }
                    );

                    options.IncludeXmlComments
                    (
                        Path.Combine( AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml" )
                    );

                    options.DescribeAllEnumsAsStrings();
                }

            );

        public static IServiceCollection ConfigureDatabase( this IServiceCollection services )
        {
            var connection = new SqliteConnection( "DataSource=:memory:" );
            connection.Open();

            return services.AddDbContext<BookshelfContext>( options => options.UseSqlite( connection ) );
        }

        public static IMvcBuilder ConfigureJson( this IMvcBuilder mvcBuilder )
        {
            return mvcBuilder
                .AddJsonOptions
                (
                    options => SetupJsonSerializer( options.SerializerSettings )

               );

            void SetupJsonSerializer( JsonSerializerSettings settings )
            {
                settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                settings.NullValueHandling = NullValueHandling.Ignore;
            }
        }
    }
}
