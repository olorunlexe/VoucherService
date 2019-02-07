using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
//using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using RabbitMQ.Client;
using VoucherService.MQ;
using VoucherServiceBL.Repository;
using VoucherServiceBL.Service;

namespace VoucherService
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
            services.AddMvc();
            services.AddSingleton<IHostedService, Subscribers>();

            //services.AddSingleton<IHostedService, HostRunner>();
            services.AddSingleton(new ConnectionFactory()
            {
                HostName = "192.168.99.100",
                Port = 5672,
                UserName = "guest",
                RequestedHeartbeat = 120,
                Password = "guest"

            });
            services.AddTransient<IGiftVoucherService,GiftVoucherService>();

            services.AddTransient<IDiscountVoucherService,DiscountVoucherService>();

            services.AddTransient<IVoucherService, BaseService>();

            services.AddTransient<IGiftRepository, GiftRepository>();
            services.AddTransient<IDiscountRepository, DiscountRepository>();

            services.AddTransient<IValueRepository, ValueRepository>();
            services.AddTransient<IValueVoucherService, ValueVoucherService>();

            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));


            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication(options =>
            {
                // Store the session to cookies
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                // OpenId authentication
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie("Cookies")
            .AddOpenIdConnect(options =>
            {
                // URL of the Keycloak server
                options.Authority = "http://localhost:8080/auth/realms/Voucherz";
                // Client configured in the Keycloak
                options.ClientId = "voucher";

                // For testing we disable https (should be true for production)
                options.RequireHttpsMetadata = false;
                options.SaveTokens = true;

                // Client secret shared with Keycloak
                options.ClientSecret = "040519ee-bd56-4ec6-8d98-cc3f900f736b";
                options.GetClaimsFromUserInfoEndpoint = true;

                // OpenID flow to use
                options.ResponseType = OpenIdConnectResponseType.CodeIdToken;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCors("MyPolicy");
            // Add this line to ensure authentication is enabled
            app.UseAuthentication();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
