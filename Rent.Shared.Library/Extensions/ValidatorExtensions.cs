using System.Text.RegularExpressions;
using FluentValidation;

namespace Rent.Shared.Library.Extensions;

public static class ValidatorExtensions
{
    public static IRuleBuilderOptions<T, TElement> IsBrazilianValidLicensePlate<T, TElement>(this IRuleBuilder<T, TElement> ruleBuilder)
    {
        return ruleBuilder.Must(IsBrazilianLicensePlate);
    }

    private static bool IsBrazilianLicensePlate<TElement>(TElement value)
    {
        var valueStr = value?.ToString();
        if (valueStr is null)
            return false;
        
        const string brazilianLicensePlateRegex = "[a-zA-Z]{3}[0-9][0-9a-zA-z][0-9]{2}$";
        return Regex.IsMatch(valueStr, brazilianLicensePlateRegex);
    }
}