using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// controllers
builder.Services.AddControllers();

// mongoDB
var connectionString = builder.Configuration["MongoDB:ConnectionString"];
var databaseName = builder.Configuration["MongoDB:DatabaseName"];

MongoClient client = new MongoClient(connectionString);
IMongoDatabase database = client.GetDatabase(databaseName);

builder.Services.AddSingleton<IMongoDatabase>(database);

// services
builder.Services.AddSingleton<ProdutoService>();


var app = builder.Build();

// pipeline
app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

app.MapControllers();

app.Run();