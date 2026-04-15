using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// controllers
builder.Services.AddControllers();

// mongoDB
var connectionString = builder.Configuration["MongoDB:ConnectionString"];
var databaseName = builder.Configuration["MongoDB:DatabaseName"];

MongoClient client = new MongoClient(connectionString);
IMongoDatabase database = client.GetDatabase(databaseName);

builder.Services.AddSingleton<IMongoDatabase>(database);

// services
builder.Services.AddSingleton<NotaFiscalService>();
builder.Services.AddHttpClient();


var app = builder.Build();

// pipeline
app.UseHttpsRedirection();

app.MapControllers();

app.Run();