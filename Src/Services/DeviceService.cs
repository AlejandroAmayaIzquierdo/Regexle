using Microsoft.EntityFrameworkCore;
using WebServer.Data;
using WebServer.Models.Auth;

namespace WebServer.Services;

public class DeviceService(ApplicationDbContext dbContext)
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<(Device? device, bool newDevice)> RegisterDevice(RegisterDeviceDto dto)
    {
        try
        {
            var parser = UAParser.Parser.GetDefault();
            var clientInfo = parser.Parse(dto.UserAgent);

            Device? device = await _dbContext.Devices.FirstOrDefaultAsync(u =>
                u.UserAgent == dto.UserAgent && u.IPAddress == dto.IpAddress
            );

            if (device != null)
                return (device, false);

            var deviceName = $"{clientInfo.OS} - {clientInfo.Device.Family}";

            device = new Device()
            {
                DeviceId = Guid.NewGuid(),
                UserAgent = dto.UserAgent,
                DeviceName = deviceName,
                IPAddress = dto.IpAddress,
            };

            _dbContext.Devices.Add(device);

            await _dbContext.SaveChangesAsync();

            return (device, true);
        }
        catch (Exception ex)
        {
            // LogService.Get()?.Error(ex.Message);
            return (null, false);
        }
    }

    public Device BuildDevice(RegisterDeviceDto dto)
    {
        var parser = UAParser.Parser.GetDefault();
        var clientInfo = parser.Parse(dto.UserAgent);

        var deviceName = $"{clientInfo.OS} - {clientInfo.Device.Family}";

        var device = new Models.Auth.Device()
        {
            UserAgent = dto.UserAgent,
            DeviceName = deviceName,
            IPAddress = dto.IpAddress,
        };
        return device;
    }

    public async Task<bool> ValidateDevice(Device deviceRequest, string accessToken)
    {
        var session = await _dbContext
            .Sessions.Include(s => s.Device)
            .FirstOrDefaultAsync(s => s.AccessToken == accessToken);
        if (session is null)
            return false; // TODO control this case to send correct message to front

        var device = session.Device;

        if (device is null)
            return false;

        if (
            device.UserAgent != deviceRequest.UserAgent
            || device.IPAddress != deviceRequest.IPAddress
        )
            return false;
        return true;
    }
}
