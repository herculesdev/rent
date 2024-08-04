using MediatR;
using Microsoft.Extensions.Logging;
using Rent.Renter.Core.Data;
using Rent.Renter.Core.Features.DeliveryPerson.Shared;
using Rent.Shared.Library.Consts;
using Rent.Shared.Library.Extensions;
using Rent.Shared.Library.Results;

namespace Rent.Renter.Core.Features.DeliveryPerson.Create;

public class CreateDeliveryPersonCommandHandler(IDeliveryPersonRepository deliveryPersonRepository, IFileStorage fileStorage, ILogger<CreateDeliveryPersonCommandHandler> logger) : IRequestHandler<CreateDeliveryPersonCommand, Result<DeliveryPersonResponse>>
{
    public async Task<Result<DeliveryPersonResponse>> Handle(CreateDeliveryPersonCommand command, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Validating");
        var validation = await new CreateDeliveryPersonCommandValidator().ValidateAsync(command, cancellationToken);
        if (!validation.IsValid)
            return Result.Failure(validation.Errors);

        var deliveryPersonGotByDocumentNumber = await deliveryPersonRepository.GetByDocumentNumber(command.DocumentNumber, cancellationToken);
        if (deliveryPersonGotByDocumentNumber is not null)
            return Result.Failure(nameof(command.DocumentNumber), "Document number already exists");
        
        var deliveryPersonGotByDriverLicenseNumber = await deliveryPersonRepository.GetByDriverLicenseNumber(command.DriverLicenseNumber, cancellationToken);
        if (deliveryPersonGotByDriverLicenseNumber is not null)
            return Result.Failure(nameof(command.DriverLicenseNumber), "Driver license number already exists");
        
        logger.LogInformation("Decoding image saving into file storage");
        var driverLicenseImageBytes = Convert.FromBase64String(command.Base64DriverLicenseImage);
        var driverLicenseImageExtension = GetFileExtensionFromBytes(driverLicenseImageBytes);
        var driverLicenseImageFileName = $"{Guid.NewGuid()}.{driverLicenseImageExtension}";
        fileStorage.Save(driverLicenseImageFileName, driverLicenseImageBytes);
        
        logger.LogInformation("Building entity and saving into repository");
        var deliveryPerson = new Entities.DeliveryPerson();
        deliveryPerson.Name = command.Name;
        deliveryPerson.DocumentNumber = command.DocumentNumber;
        deliveryPerson.DriverLicenseNumber = command.DriverLicenseNumber;
        deliveryPerson.DriverLicenseType = command.DriverLicenseType;
        deliveryPerson.DriverLicenseImageName = driverLicenseImageFileName;
        deliveryPerson.BirthDate = DateOnly.FromDateTime(command.BirthDate);
        await deliveryPersonRepository.Add(deliveryPerson, cancellationToken);
        
        logger.LogInformation("Building response and returning");
        var response = DeliveryPersonResponse.From(deliveryPerson);
        
        return Result.Success(response);
    }

    private string GetFileExtensionFromBytes(byte[] fileBytes)
    {
        if (fileBytes.StartsWith(FileMagicNumberConsts.Jpeg))
            return "jpg";
        if (fileBytes.StartsWith(FileMagicNumberConsts.Bmp))
            return "bmp";

        return "unknown";
    }
}