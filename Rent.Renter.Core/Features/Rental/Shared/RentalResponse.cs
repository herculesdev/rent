namespace Rent.Renter.Core.Features.Rental.Shared;

public record RentalResponse(
    Guid Id,
    Guid DeliveryPersonId,
    PlanModel Plan,
    DateOnly StartDateUtc,
    DateOnly EstimatedEndDateUtc,
    DateOnly? EffectiveEndDateUtc)
{
    public static RentalResponse From(Entities.Rental src)
    {
        return new RentalResponse(
            src.Id,
            src.DeliveryPersonId,
            new PlanModel(src.Plan.Id, src.Plan.Days, src.Plan.PricePerDay), 
            src.StartDateUtc,
            src.EstimatedEndDateUtc, 
            src.EffectiveEndDateUtc);
    }
}
public record PlanModel(Guid Id, int Days, decimal PricePerDay);