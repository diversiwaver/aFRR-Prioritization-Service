using DataAccessLayer;
using DataAccessLayer.Interfaces;
using PrioritizationModel;

var builder = WebApplication.CreateBuilder(args);

string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (connectionString is null)
{
    // TODO: Log an error when retrieving the connection string
    throw new ArgumentNullException(nameof(connectionString), "Failed to retrieve connection string. Is it defined in the configuration?");
}
// Add services to the container.
// Add Logging
builder.Services.AddLogging(configure =>
{
    configure.AddConsole(); // enables logging in console
    configure.AddDebug(); // enables logging to the debgger
    configure.AddEventSourceLogger(); // Enables logging using the EventSource class.
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped((prioritizationModelController) => new PrioritizationModelController(DataAccessFactory.GetDataAccess<IAssetDataAccess>(connectionString), PrioritizationModelFactory.GetPrioritizationModel("v1")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
