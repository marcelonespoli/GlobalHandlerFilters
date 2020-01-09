using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Enterprise.Common.Authentication;
using Enterprise.Common.Authentication.Extensions;
using Enterprise.Common.Authentication.Services.BearerToken;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Serilog;
using Serilog.Context;
using Swashbuckle.AspNetCore.Swagger;
using UAParser;
using SameSiteMode = Microsoft.AspNetCore.Http.SameSiteMode;
using Microsoft.OpenApi.Models;
using MyProject.Api.GlobalFilters;
using Microsoft.Extensions.Logging;

namespace MyProject.Api
{
	public class Startup
	{
		public IConfiguration Configuration { get; }

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;			
		}

		public void ConfigureServices(IServiceCollection services)
		{
			// Add MVC
			services.AddMvc(options =>
			{
				options.Filters.Add(new ProducesAttribute("application/json"));
				options.Filters.Add(typeof(GlobalExceptionHandlingFilter));
			})
			.SetCompatibilityVersion(CompatibilityVersion.Version_2_2);


			services.AddScoped<ILogger<GlobalExceptionHandlingFilter>, Logger<GlobalExceptionHandlingFilter>>();
			services.AddScoped<GlobalExceptionHandlingFilter>();

		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			
			app.MapWhen(x => !x.Request.Path.Value.StartsWith("/api/"), builder =>
			{
				builder.UseSpa(spa =>
				{
					spa.Options.SourcePath = "app";
				});
			});

			app.UseMvc();
		}


	}
}
