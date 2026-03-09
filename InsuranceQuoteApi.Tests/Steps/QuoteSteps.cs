using NUnit.Framework;
using TechTalk.SpecFlow;

namespace InsuranceQuoteApi.Tests.Steps;

[Binding]
public class QuoteSteps
{
    private int _age;
    private decimal _vehicleValue;
    private decimal _annualPremium;
    private string _riskRating = string.Empty;

    [Given(@"a driver aged (\d+) with a vehicle worth (\d+)")]
    public void GivenADriverAgedWithAVehicleWorth(int age, decimal vehicleValue)
    {
        _age = age;
        _vehicleValue = vehicleValue;
    }

    [When(@"they request a quote for postcode ""(.*)""")]
    public void WhenTheyRequestAQuoteForPostcode(string postcode)
    {
        var baseRate = 500m;
        var youngDriverAge = 25;
        var youngDriverFactor = 1.8m;
        var olderDriverAge = 70;
        var olderDriverFactor = 1.4m;
        var standardFactor = 1.0m;
        var valueDivisor = 10000m;
        var highRiskThreshold = 1000m;
        var mediumRiskThreshold = 500m;

        var ageFactor = _age < youngDriverAge ? youngDriverFactor
                      : _age > olderDriverAge ? olderDriverFactor
                      : standardFactor;

        _annualPremium = Math.Round(baseRate * ageFactor * (_vehicleValue / valueDivisor), 2);
        _riskRating = _annualPremium > highRiskThreshold ? "High"
                    : _annualPremium > mediumRiskThreshold ? "Medium"
                    : "Low";
    }

    [Then(@"the annual premium should be (.*)")]
    public void ThenTheAnnualPremiumShouldBe(decimal expectedPremium)
    {
        Assert.That(_annualPremium, Is.EqualTo(expectedPremium));
    }

    [Then(@"the risk rating should be ""(.*)""")]
    public void ThenTheRiskRatingShouldBe(string expectedRisk)
    {
        Assert.That(_riskRating, Is.EqualTo(expectedRisk));
    }
}