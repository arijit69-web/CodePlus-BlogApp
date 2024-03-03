using CodePlus.API.Data;
using CodePlus.API.Repositories.Implementations;
using CodePlus.API.Repositories.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Before Building the Service : Inject the DBContext class

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("CodePlusConnectionString"));
});

builder.Services.AddDbContext<AuthDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("CodePlusConnectionString"));
});

/*
Transient objects are always different; a new instance is provided to every controller and every service.

Scoped objects are the same within a request, but different across different requests.

Singleton objects are the same for every object and every request. 



A scoped repository is created once per HTTP request, and then disposed of at the end of the request. This means that any components that are created within the scope of the repository will be reused throughout the entire HTTP request.

Here’s an example:
services.AddScoped<IRepository, Repository>();
In this example, a new instance of Repository will be created for each HTTP request. Any components that are created within the scope of the repository will be reused throughout the entire request.
Scoped repositories are often the preferred choice for repositories that are used frequently within a single HTTP request.
For example, if multiple services within the same request need to use the same repository, using a scoped repository would ensure that they’re all using the same instance.
*/

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>(); // That is saying whenever a ICategoryRepository is required, create an instance of CategoryRepository to pass into the constructor.
builder.Services.AddScoped<IBlogPostRepository,BlogPostRepository>(); // That is saying whenever a ICategoryRepository is required, create an instance of CategoryRepository to pass into the constructor.
builder.Services.AddScoped<IImageRepository, ImageRepository>();
builder.Services.AddScoped<ITokenRepository, TokenRepository>();


builder.Services.AddIdentityCore<IdentityUser>().AddRoles<IdentityRole>().AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("CodePlus").AddEntityFrameworkStores<AuthDbContext>().AddDefaultTokenProviders();
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            AuthenticationType = "Jwt",
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });
var app = builder.Build();

// Configure the HTTP request pipeline.
/*
The Request Pipeline is the mechanism by which requests are processed beginning with a Request and ending with a Response.
The pipeline specifies how the application should respond to the HTTP request. The Request arriving from the browser goes through the pipeline and return back.
The individual components that make up the pipeline are called Middleware.

Middleware is software that's assembled into an app pipeline to handle requests and responses. 
Each component:
- Chooses whether to pass the request to the next component in the pipeline.
- Can perform work before and after the next component in the pipeline.
Request delegates are used to build the request pipeline. The request delegates handle each HTTP request.

The order of the middlewares also matters
*/
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(options =>
{
    options.AllowAnyHeader();
    options.AllowAnyMethod();
    options.AllowAnyOrigin();
});

app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Images")),
    RequestPath = "/Images"
});

app.MapControllers();

app.Run();
