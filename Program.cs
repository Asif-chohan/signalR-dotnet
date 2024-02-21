using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.SignalR;
// Include the namespace where ChatHub is located
using SignalRServer.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSignalR();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:3000") // The React app's origin
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Use CORS policy
app.UseCors("CorsPolicy");

app.MapRazorPages();
app.MapHub<ChatHub>("/chatHub"); // Map the SignalR Hub

var hubContext = app.Services.GetRequiredService<IHubContext<ChatHub>>();
app.MapGet("/send", async () => {
    await hubContext.Clients.All.SendAsync("ReceiveMessage", "Hello from the server!");
    return Results.Ok("Message sent to all clients");
});

app.Run();
