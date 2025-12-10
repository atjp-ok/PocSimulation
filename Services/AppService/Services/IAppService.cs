using Shared.SharedModels.AppserviceModels;

namespace AppService.Services;
public interface IAppService
{
    Task<ParkingResponse> StartParkingAsync(ParkingRequest request);
    Task<StopParkingResponse?> StopParkingAsync(int parkingId);
    Task<ParkingResponse?> GetParkingStatusAsync(int parkingId);

}
