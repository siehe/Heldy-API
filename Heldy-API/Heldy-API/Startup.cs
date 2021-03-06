﻿using Heldy.DataAccess;
using Heldy.DataAccess.Interfaces;
using Heldy.Services;
using Heldy.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace Heldy_API
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
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Title = "Heldy API",
                    Version = "v1"
                });
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = Configuration["AccessToken:Issuer"],
                        ValidateAudience = true,
                        ValidAudience = Configuration["AccessToken:Audience"],
                        ValidateLifetime = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["AccessToken:SecretKey"])),
                        ValidateIssuerSigningKey = true,
                    };
                });

            services.AddSingleton<ITaskService, TaskService>();
            services.AddSingleton<ITaskRepository, TaskRepository>();
            services.AddSingleton<IColumnService, ColumnService>();
            services.AddSingleton<IColumnsRepository, ColumnRepository>();
            services.AddSingleton<IPersonService, PersonService>();
            services.AddSingleton<IPersonRepository, PersonRepository>();
            services.AddSingleton<ISubjectService, SubjectService>();
            services.AddSingleton<ISubjectRepository, SubjectRepository>();
            services.AddSingleton<ITypeService, TypeService>();
            services.AddSingleton<ITypeRepository, TypeRepository>();
            services.AddSingleton<IUserRepository, UserRepository>();
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<IEmailService, EmailService>();
            services.AddSingleton<ICommentsService, CommentsService>();
            services.AddSingleton<ICommentsRespository, CommentsRepository>();
            services.AddControllers();
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins", builder => builder.AllowAnyOrigin());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Heldy API v1");
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("AllowAllOrigins");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
