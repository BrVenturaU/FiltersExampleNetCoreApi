using FiltersExample.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<ActionFilterExample>(); // Filtro de controlador o de acción
builder.Services.AddScoped<MetadaActionFilter>(); // Filtro de controlador o de acción

builder.Services.AddControllers();
/*
 * Para aplicar el filtro de forma global.
builder.Services.AddControllers(options => options.Filters.Add(new MetadaActionFilter())); 
 
*/


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("Dev", conf =>
    {
        conf.AllowAnyHeader()
        .AllowAnyMethod().AllowAnyOrigin();
    });
});
var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("Dev");
app.UseAuthorization();

app.MapControllers();

app.Run();
