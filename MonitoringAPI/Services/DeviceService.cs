using System.Collections.Concurrent;
using MonitoringAPI.Models;

namespace MonitoringAPI.Services;

public class DeviceService : IDeviceService
{
    // Словарь для хранения данных, полученных от стороннего приложения
    private readonly ConcurrentDictionary<string, List<Device>> _devices = new();
    private readonly ILogger<DeviceService> _logger;

    public DeviceService(ILogger<DeviceService> logger)
    {
        _logger = logger;
    }

    public void AddDevice(Device device)
    {
        if (!_devices.ContainsKey(device.Id))
        {
            _devices[device.Id] = new List<Device>();
            _logger.LogInformation($"Добавлено новое устройство с ID: {device.Id}");
        }
        
        _devices[device.Id].Add(device);
        _logger.LogInformation($"Добавлена новая запись для устройства с ID: {device.Id}");
    }
    
    public IEnumerable<Device> GetAllDevices()
    {
        _logger.LogInformation($"Получены все устройства. Общее количество устройств: {_devices.Count}");
        return _devices.Values.SelectMany(devices=>devices).DistinctBy(d => d.Id);
    }

    public IEnumerable<Device> GetDeviceLogs(string deviceId)
    {
        if (_devices.TryGetValue(deviceId, out var logs))
        {
            _logger.LogInformation($"Получено {logs.Count} записей для устройства с ID: {deviceId}");
            return logs;
        }

        _logger.LogWarning($"Не найдены записи для устройства с ID: {deviceId}");
        return [];
    }

    public void RemoveRecords(string deviceId, DateTime olderThan)
    {
        if (_devices.TryGetValue(deviceId, out var logs))
        {
            int initialCount = logs.Count;
            _devices[deviceId] = logs.Where(log => log.EndTime >= olderThan).ToList();
            int removedCount = initialCount - _devices[deviceId].Count;

            if (removedCount > 0)
            {
                _logger.LogInformation($"Удалено {removedCount} записей для устройства с ID: {deviceId}");
            }
            else
            {
                _logger.LogInformation($"Нет записей для удаления для устройства с ID: {deviceId}");
            }

            if (_devices[deviceId].Count == 0)
            {
                _devices.TryRemove(deviceId, out _);
                _logger.LogInformation($"Записей для устройства с ID {deviceId} не существует. Устройство удалено");
            }
        }
        else
        {
            _logger.LogWarning($"Нет устройства с ID: {deviceId} для удаления записей.");
        }
    }
}