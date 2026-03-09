var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var config = app.Configuration.GetSection("RatingFactors");
var baseRate = config.GetValue<decimal>("BaseRate");
var youngDriverAge = config.GetValue<int>("YoungDriverAge");
var youngDriverFactor = config.GetValue<decimal>("YoungDriverFactor");
var olderDriverAge = config.GetValue<int>("OlderDriverAge");
var olderDriverFactor = config.GetValue<decimal>("OlderDriverFactor");
var standardFactor = config.GetValue<decimal>("StandardFactor");
var valueDivisor = config.GetValue<decimal>("ValueDivisor");
var highRiskThreshold = config.GetValue<decimal>("HighRiskThreshold");
var mediumRiskThreshold = config.GetValue<decimal>("MediumRiskThreshold");

app.MapGet("/quote", (int age, decimal vehicleValue, string postcode) =>
{
    var ageFactor = age < youngDriverAge ? youngDriverFactor 
                  : age > olderDriverAge ? olderDriverFactor 
                  : standardFactor;
                  
    var premium = baseRate * ageFactor * (vehicleValue / valueDivisor);
    var risk = premium > highRiskThreshold ? "High" 
             : premium > mediumRiskThreshold ? "Medium" 
             : "Low";

    return Results.Ok(new
    {
        QuoteReference = $"QT-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString()[..8].ToUpper()}",
        Age = age,
        VehicleValue = vehicleValue,
        Postcode = postcode,
        AnnualPremium = Math.Round(premium, 2),
        RiskRating = risk
    });
});

app.Run();