using Rent.Renter.Core.ValueObjects;
using Rent.Shared.Library.Entities;

namespace Rent.Renter.Core.Entities;

public class Rental : Entity
{
    public Guid DeliveryPersonId { get; set; }
    public DeliveryPerson DeliveryPerson { get; set; } = null!;
    public Guid PlanId { get; set; }
    public Guid MotorbikeId { get; set; }
    public DateOnly StartDateUtc { get; set; }
    public DateOnly EstimatedEndDateUtc { get; set; }
    public DateOnly? EffectiveEndDateUtc { get; set; }
    
    // Computed fields
    public Plan Plan => Plan.AvailablePlans.FirstOrDefault(x => x.Id == PlanId)!;

    public static Rental Create(DeliveryPerson person, Plan plan, Guid motorbikeId)
    {
        var rental = new Rental();
        rental.DeliveryPersonId = person.Id;
        rental.DeliveryPerson = person;
        rental.PlanId = plan.Id;
        rental.MotorbikeId = motorbikeId;
        
        rental.StartDateUtc = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1).Date);
        rental.EstimatedEndDateUtc = rental.StartDateUtc.AddDays(plan.Days);

        return rental;
    }

    public RentalPriceTotalization GetTotalization(DateOnly returnDate)
    {
        var rentalDaysCount = (returnDate.ToDateTime(TimeOnly.MinValue) - StartDateUtc.ToDateTime(TimeOnly.MinValue)).Days;
        var priceTotalization = new RentalPriceTotalization(rentalDaysCount);
        
        if (rentalDaysCount > Plan.Days)
        {
            var regularDaysTotalPrice = Plan.Days * Plan.PricePerDay;
            var rentalSpareDaysCount = rentalDaysCount - Plan.Days;
            var spareDaysTotalPrice = rentalSpareDaysCount * Plan.PricePerAdditionalDay;
            
            return priceTotalization with
            {
                SpareDaysCount = rentalSpareDaysCount,
                SpareDaysTotalPrice = spareDaysTotalPrice,
                TotalPrice = regularDaysTotalPrice + spareDaysTotalPrice
            };
        }
        
        if (rentalDaysCount < Plan.Days)
        {
            var regularDaysTotalPrice = rentalDaysCount * Plan.PricePerDay;
            var daysNotEffectiveCount = Plan.Days - rentalDaysCount;
            var finePerDaysNotEffective = (Plan.PercentFineForDaysNotEffective / 100) * Plan.PricePerDay;
            var finePerDaysNotEffectiveTotalPrice = finePerDaysNotEffective * daysNotEffectiveCount;
            return priceTotalization with
            {
                DaysNotEffectiveCount = daysNotEffectiveCount,
                FinePerDaysNotEffectiveTotalPrice = finePerDaysNotEffectiveTotalPrice,
                TotalPrice = regularDaysTotalPrice + finePerDaysNotEffectiveTotalPrice
            };
        }

        return priceTotalization with { TotalPrice = Plan.Days * Plan.PricePerDay };

    }
}