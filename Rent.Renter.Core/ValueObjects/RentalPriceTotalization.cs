namespace Rent.Renter.Core.ValueObjects;

public record RentalPriceTotalization(
    int DaysCount = 0,
    int SpareDaysCount = 0,
    int DaysNotEffectiveCount = 0,
    decimal FinePerDaysNotEffectiveTotalPrice = 0,
    decimal SpareDaysTotalPrice = 0,
    decimal TotalPrice = 0);