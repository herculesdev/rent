using MediatR;
using Microsoft.Extensions.Logging;
using Rent.Renter.Core.Data;
using Rent.Renter.Core.Features.DeliveryPerson.Shared;
using Rent.Shared.Library.Extensions;
using Rent.Shared.Library.Results;

namespace Rent.Renter.Core.Features.DeliveryPerson.Update;

public class UpdateDeliveryPersonCommandHandler(IDeliveryPersonRepository deliveryPersonRepository, IFileStorage fileStorage, ILogger<UpdateDeliveryPersonCommandHandler> logger) : IRequestHandler<UpdateDeliveryPersonCommand, Result<DeliveryPersonResponse>>
{
    public async Task<Result<DeliveryPersonResponse>> Handle(UpdateDeliveryPersonCommand command, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Validating");
        var validation = await new UpdateDeliveryPersonCommandValidator().ValidateAsync(command, cancellationToken);
        if (!validation.IsValid)
            return Result.Failure(validation.Errors);

        var deliveryPerson = await deliveryPersonRepository.GetById(command.Id, cancellationToken);
        if (deliveryPerson is null)
            return Result.Failure("Delivery person not found");

        if (command.DocumentNumber != deliveryPerson.DocumentNumber)
        {
            var deliveryPersonGotByDocumentNumber =
                await deliveryPersonRepository.GetByDocumentNumber(command.DocumentNumber, cancellationToken);
            if (deliveryPersonGotByDocumentNumber is not null)
                return Result.Failure(nameof(command.DocumentNumber), "Document number already exists");
        }

        if (command.DriverLicenseNumber != deliveryPerson.DriverLicenseNumber)
        {
            var deliveryPersonGotByDriverLicenseNumber =
                await deliveryPersonRepository.GetByDriverLicenseNumber(command.DriverLicenseNumber, cancellationToken);
            if (deliveryPersonGotByDriverLicenseNumber is not null)
                return Result.Failure(nameof(command.DriverLicenseNumber), "Driver license number already exists");
        }

        if (!string.IsNullOrEmpty(command.Base64DriverLicenseImage))
        {
            logger.LogInformation("Decoding image saving into file storage");
            var driverLicenseImageBytes = Convert.FromBase64String(command.Base64DriverLicenseImage);
            var driverLicenseImageExtension = driverLicenseImageBytes.GetFileExtensionFromBytes();
            var driverLicenseImageFileName = $"{Guid.NewGuid()}.{driverLicenseImageExtension}";
            fileStorage.Save(driverLicenseImageFileName, driverLicenseImageBytes);
            deliveryPerson.DriverLicenseImageName = driverLicenseImageFileName;
        }

        logger.LogInformation("Building entity and saving into repository");
        deliveryPerson.Name = command.Name;
        deliveryPerson.DocumentNumber = command.DocumentNumber;
        deliveryPerson.DriverLicenseNumber = command.DriverLicenseNumber;
        deliveryPerson.DriverLicenseType = command.DriverLicenseType;
        deliveryPerson.BirthDate = DateOnly.FromDateTime(command.BirthDate);
        await deliveryPersonRepository.Update(deliveryPerson, cancellationToken);
        
        logger.LogInformation("Building response and returning");
        var response = DeliveryPersonResponse.From(deliveryPerson);
        
        return Result.Success(response);
    }
}