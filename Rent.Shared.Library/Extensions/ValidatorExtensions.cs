using System.Text;
using System.Text.RegularExpressions;
using FluentValidation;
using Rent.Shared.Library.Consts;

namespace Rent.Shared.Library.Extensions;

public static class ValidatorExtensions
{
    public static IRuleBuilderOptions<T, TElement> IsBrazilianValidLicensePlate<T, TElement>(this IRuleBuilder<T, TElement> ruleBuilder)
    {
        return ruleBuilder.Must(IsBrazilianLicensePlate);
    }
    
    public static IRuleBuilderOptions<T, TElement> IsBrazilianValidDriverLicenseNumber<T, TElement>(this IRuleBuilder<T, TElement> ruleBuilder)
    {
        return ruleBuilder.Must(IsBrazilianValidDriverLicenseNumber);
    }
    
    public static IRuleBuilderOptions<T, TElement> IsCnpj<T, TElement>(this IRuleBuilder<T, TElement> ruleBuilder)
    {
        return ruleBuilder.Must(IsCnpj);
    }
    
    public static IRuleBuilderOptions<T, TElement> IsBase64ValidJpegOrBmp<T, TElement>(this IRuleBuilder<T, TElement> ruleBuilder)
    {
        return ruleBuilder.Must(IsBase64ValidJpegOrBmp);
    }

    private static bool IsBrazilianLicensePlate<TElement>(TElement value)
    {
        var valueStr = value?.ToString();
        if (valueStr is null)
            return false;
        
        const string brazilianLicensePlateRegex = "[a-zA-Z]{3}[0-9][0-9a-zA-z][0-9]{2}$";
        return Regex.IsMatch(valueStr, brazilianLicensePlateRegex);
    }
    
    private static bool IsCpf<TElement>(TElement value)
    {
        var valueStr = value?.ToString();
        if (string.IsNullOrWhiteSpace(valueStr))
            return false;
        
        if (valueStr.Length != 11)
            return false;
        
        if (!valueStr.IsDigitsOnly())
            return false;
        
        if (valueStr.Distinct().Count() == 1)
            return false;
        
        int[] firstMultiplier = [10, 9, 8, 7, 6, 5, 4, 3, 2];
        var sum = 0;
        for (var i = 0; i < 9; i++)
            sum += int.Parse(valueStr[i].ToString()) * firstMultiplier[i];

        var rest = sum % 11;
        var firstVerifierDigit = rest < 2 ? 0 : 11 - rest;
        
        int[] secondMultiplier = [11, 10, 9, 8, 7, 6, 5, 4, 3, 2];
        sum = 0;
        for (var i = 0; i < 10; i++)
            sum += int.Parse(valueStr[i].ToString()) * secondMultiplier[i];

        rest = sum % 11;
        var secondVerifierDigit = rest < 2 ? 0 : 11 - rest;
        
        return valueStr[9] == firstVerifierDigit.ToString()[0] &&
               valueStr[10] == secondVerifierDigit.ToString()[0];
    }
    
    private static bool IsCnpj<TElement>(TElement value)
    {
        var valueStr = value?.ToString();
        if (string.IsNullOrWhiteSpace(valueStr))
            return false;
        
        if (valueStr.Length != 14)
            return false;
        
        if (!valueStr.IsDigitsOnly())
            return false;
        
        if (valueStr.Distinct().Count() == 1)
            return false;
        
        int[] firstMultiplier = [5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];
        var sum = 0;
        for (var i = 0; i < 12; i++)
            sum += int.Parse(valueStr[i].ToString()) * firstMultiplier[i];

        var rest = sum % 11;
        var firstVerifierDigit = rest < 2 ? 0 : 11 - rest;
        
        int[] secondMultiplier = [6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];
        sum = 0;
        for (var i = 0; i < 13; i++)
            sum += int.Parse(valueStr[i].ToString()) * secondMultiplier[i];

        rest = sum % 11;
        var secondVerifierDigit = rest < 2 ? 0 : 11 - rest;
        
        return valueStr[12] == firstVerifierDigit.ToString()[0] &&
               valueStr[13] == secondVerifierDigit.ToString()[0];
        
    }

    private static bool IsBrazilianValidDriverLicenseNumber<TElement>(TElement value)
    {
        var driverLicenseNumber = value?.ToString();
        if (string.IsNullOrWhiteSpace(driverLicenseNumber))
            return false;
        
        if (driverLicenseNumber.Length != 11 || !long.TryParse(driverLicenseNumber, out _))
            return false;
        
        if (driverLicenseNumber.Distinct().Count() == 1)
            return false;

        int[] firstDigitMultipliers = [1, 2, 3, 4, 5, 6, 7, 8, 9];
        int[] secondDigitMultipliers = [9, 8, 7, 6, 5, 4, 3, 2, 1];

        var driverLicenseNumberWithoutVerifierDigits = driverLicenseNumber[..9];
        
        var sum = 0;
        for (var i = 0; i < 9; i++)
            sum += int.Parse(driverLicenseNumberWithoutVerifierDigits[i].ToString()) * firstDigitMultipliers[i];

        var rest = sum % 11;
        var firstDigit = rest == 10 ? 0 : rest;
        
        sum = 0;
        for (var i = 0; i < 9; i++)
            sum += int.Parse(driverLicenseNumberWithoutVerifierDigits[i].ToString()) * secondDigitMultipliers[i];

        rest = sum % 11;
        var secondDigit = rest == 10 ? 0 : rest;

        var verifierDigits = firstDigit.ToString() + secondDigit.ToString();

        return driverLicenseNumber.EndsWith(verifierDigits);
    }

    private static bool IsBase64ValidJpegOrBmp<TElement>(TElement value)
    {
        var valueStr = value?.ToString();
        if (string.IsNullOrWhiteSpace(valueStr))
            return false;
        
        var fileBytes = Convert.FromBase64String(valueStr);

        if (fileBytes.StartsWith(FileMagicNumberConsts.Jpeg) || fileBytes.StartsWith(FileMagicNumberConsts.Bmp))
            return true;

        return false;

    }
}