using PlatformService.DTOs;

namespace PlatformService.SyncDataServies.Http
{
    public interface ICommandDataClient
    {
        Task SendPlatformToCommand(PlatformReadDTO plat);
    }
}