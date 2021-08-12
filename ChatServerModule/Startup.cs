using ChatServerModule.Hubs;
using ChatServerModule.MiniRepo;
using ChatServerModule.Mocks;
using ChatServerModule.TokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatServerModule
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            // real deal
            //services.AddSingleton<ITokenValidator, TokenValidator>();
            //services.AddSingleton<IConversationRepo, ConversationRepo>();

            // fake stuff used for testing
            services.AddSingleton<ITokenValidator, FakeTokenValidator>();
            services.AddSingleton<IConversationRepo, FakeConversationRepo>();


            // cors and signalr
            services.AddCors();
            services.AddSignalR(options => options.MaximumReceiveMessageSize = 102400000);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            //app.UseCors(builder => builder
            //    //.AllowAnyOrigin()
            //    .WithOrigins("null")
            //    .AllowAnyHeader()
            //    .AllowAnyMethod()
            //    .AllowCredentials());

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<ChatHub>("/chat");
            });
        }
    }
}
