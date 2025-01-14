using MonitoringAPI.Models;

namespace MonitoringAPI.Services;

public interface IDeviceService
{
    void AddDevice(Device device);
    IEnumerable<Device> GetAllDevices();
    IEnumerable<Device> GetDeviceLogs(string deviceId);
    void RemoveRecords(string deviceId, DateTime olderThan);
    Task BackupToFileAsync(string filepath);
    Task RestoreFromFileAsync(string filepath);
}