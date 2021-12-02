using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LenusTechTest.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LenusTechTest
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGen();

            services.AddDbContext<BookStoreContext>(opt => opt.UseInMemoryDatabase(databaseName: "BookStore"));

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger(); 
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                    options.RoutePrefix = string.Empty;
                });
            }
            else
            {
                app.UseHsts();
            }

            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<BookStoreContext>();

                AddTestData(context);
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }

        private static void AddTestData(BookStoreContext context)
        {
            context.Books.AddRange(
                    new Book(1, "A. A. Milne", "Winnie-the-Pooh", 19.25),
                    new Book(2, "Jane Austen", "Pride and Prejudice", 5.49),
                    new Book(3, "William Shakespeare", "Romeo and Juliet", 6.95)
                );
            context.SaveChanges();
        }
    }
}
