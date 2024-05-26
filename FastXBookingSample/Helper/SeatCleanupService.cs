using FastXBookingSample.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class SeatCleanupService : BackgroundService
{
    private readonly ILogger<SeatCleanupService> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public SeatCleanupService(ILogger<SeatCleanupService> logger, IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        TimeSpan cleanupInterval = TimeSpan.FromDays(1);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // Execute your cleanup logic within a scoped context
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<BookingContext>();
                    await DeleteSeatsAfterDepartureDate(context);
                }

                // Wait for the specified interval before the next execution
                await Task.Delay(cleanupInterval, stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during seat cleanup.");
            }
        }
    }

    private async Task DeleteSeatsAfterDepartureDate(BookingContext context)
    {
        try
        {
            DateTime today = DateTime.UtcNow.Date;
            List<int> busIds = context.BusDepartures
                            .Where(x => x.DepartureDate < today && x.BusId != null)
                            .Select(x => x.BusId.Value)
                            .ToList();
            List<int> bookingIdsToRemove = context.Bookings
                .Where(booking =>
                    context.Buses
                        .Any(bus => busIds.Contains(bus.BusId))
                )
                .Select(booking => booking.BookingId)
                .ToList();

            List<Seat> seatsToRemove = context.Seats
                .Where(seat => bookingIdsToRemove.Contains(seat.BookingId.Value))
                .ToList();

            foreach (Seat seat in seatsToRemove)
            {   
                context.Seats.Remove(seat);
                BusSeat busseat = context.BusSeats.FirstOrDefault(x => x.SeatNo == seat.SeatNumber && x.BusId == (context.Bookings.FirstOrDefault(x => x.BookingId == seat.BookingId).BusId));
                busseat.IsBooked = false;
            }

            await context.SaveChangesAsync(); // Save changes to the database
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting seats.");
            throw;
        }
    }
}
