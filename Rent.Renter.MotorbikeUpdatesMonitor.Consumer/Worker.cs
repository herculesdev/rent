using System.Text;
using System.Text.Json;
using MediatR;
using Rent.Renter.Core.Data;
using Rent.Renter.Core.Events.MotorbikeUpdated;
using Rent.Shared.Library.Messaging;

namespace Rent.Renter.MotorbikeUpdatesMonitor.Consumer
{
    public class Worker(
        IMessageConsumer messageConsumer,
        IMessageDispatcher messageDispatcher,
        IMotorbikeRepository motorbikeRepository,
        ISender sender,
        ILogger<Worker> logger)
        : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Delay(100, stoppingToken);
            logger.LogInformation("Worker started at: {time} (UTC)", DateTimeOffset.UtcNow);

            var lastProcessedOffset = motorbikeRepository.GetOffset();
            messageConsumer.ConsumeFromStream("motorbike-updates-stream", lastProcessedOffset, (_, message) =>
            {
                logger.LogInformation("Message received. Processing...");
                var messageOffset = (long)message.BasicProperties.Headers["x-stream-offset"];
                var messageBytes = message.Body.ToArray();
                var messageJson = Encoding.UTF8.GetString(messageBytes);
                
                try
                {
                    var messageObject = JsonSerializer.Deserialize<MotorbikeUpdatedEvent>(messageJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    if (messageObject is null)
                    {
                        logger.LogWarning("Message was deserialized as null. returning");
                        return;
                    }

                    var result = sender.Send(messageObject).Result;

                    if (result.IsSuccess)
                    {
                        motorbikeRepository.SaveOffset(messageOffset);
                        logger.LogInformation("Processed successfully");
                    }
                    else
                    {
                        logger.LogError("Failed to process message: {messages}", string.Join(", ", result.Errors.Select(x => x.Message).ToArray()));
                        SendForManualAnalysis(messageBytes, messageOffset);
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError("Failed to process message: {exception}", ex);
                    SendForManualAnalysis(messageBytes, messageOffset);
                }
            });
            
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
            logger.LogInformation("Worker stopped at: {time} (UTC)", DateTimeOffset.UtcNow);
        }

        private void SendForManualAnalysis(byte[] messageBytes, long messageOffset)
        {
            logger.LogInformation("Sending failed message for manual analysis");
            messageDispatcher.Publish("motorbike-updates-manual-check", messageBytes);
            motorbikeRepository.SaveOffset(messageOffset);
        }
    }
}
