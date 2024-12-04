using Azure.Identity;
using BlazorApp1;
using BlazorApp1.Components;
using Microsoft.SemanticKernel;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents().AddInteractiveServerComponents();
builder.Services.AddKernel();



//builder.Services.AddScoped(sp => KernelPluginFactory.CreateFromType<ThemePlugin>(serviceProvider:sp)); 
string apiKey = builder.Configuration["AI:OpenAI:API_KEY"]!;
string endpoint = builder.Configuration["AI:OpenAI:END_POINT"]!;
if (string.IsNullOrEmpty(apiKey))
{
    throw new ArgumentNullException("API key is not set in the user secrets.");
}
builder.Services.AddAzureOpenAIChatCompletion(
    deploymentName: "gpt-4o-mini", 
    endpoint:endpoint,
    apiKey
   );

builder.Services.AddScoped(sp => KernelPluginFactory.CreateFromType<ThemePlugin>(serviceProvider: sp));


var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.Run();
