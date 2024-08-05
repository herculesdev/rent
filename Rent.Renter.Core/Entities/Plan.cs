namespace Rent.Renter.Core.Entities;

public class Plan(Guid id, int days, decimal pricePerDay, decimal pricePerAdditionalDay = 50, decimal percentFineForDaysNotEffective = 0)
{
    public Guid Id { get; set; } = id;
    public int Days { get; set; } = days;
    public decimal PricePerDay { get; set; } = pricePerDay;
    public decimal PricePerAdditionalDay { get; set; } = pricePerAdditionalDay;
    public decimal PercentFineForDaysNotEffective { get; set; } = percentFineForDaysNotEffective;

    public static Plan[] AvailablePlans => 
    [
        new Plan(id: new Guid("13ae1bae-e8dd-47f8-978c-4c3a5ab0af2a"), days: 7, pricePerDay: 30, percentFineForDaysNotEffective: 20),
        new Plan(id: new Guid("23ae1bae-e8dd-47f8-978c-4c3a5ab0af2b"), days: 15, pricePerDay: 28, percentFineForDaysNotEffective: 40),
        new Plan(id: new Guid("33ae1bae-e8dd-47f8-978c-4c3a5ab0af2c"), days: 30, pricePerDay: 22),
        new Plan(id: new Guid("43ae1bae-e8dd-47f8-978c-4c3a5ab0af2d"), days: 45, pricePerDay: 20),
        new Plan(id: new Guid("53ae1bae-e8dd-47f8-978c-4c3a5ab0af2e"), days: 50, pricePerDay: 18)
    ];
}