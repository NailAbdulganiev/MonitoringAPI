using Microsoft.AspNetCore.Mvc;
using MonitoringAPI.Models;
using MonitoringAPI.Services;

namespace MonitoringAPI.Controllers;

[ApiController]
[Route("api/devices")]
public class DeviceController : ControllerBase
{
    private readonly IDeviceService _deviceService;
    private readonly ILogger<DeviceController> _logger;

    public DeviceController(IDeviceService deviceService, ILogger<DeviceController> logger)
    {
        _deviceService = deviceService;
        _logger = logger;
    }

    [HttpPost]
    public IActionResult AddDevice([FromBody] Device? device)
    {
        if (device == null || string.IsNullOrWhiteSpace(device.Id))
        {
            _logger.LogWarning("Не получилось добавить устройство. Невалидные данные.");
            return BadRequest("Данные устройства не корректны.");
        }
        
        _deviceService.AddDevice(device);
        _logger.LogInformation($"Устройство успешно добавлено. ID: {device.Id}, Name: {device.Name}");

        return CreatedAtAction(nameof(GetDeviceLogs), new { deviceId = device.Id }, device);
    }

    [HttpGet]
    public IActionResult GetAllDevices()
    {
        _logger.LogInformation("Извлечение всех устройств...");
        var devices = _deviceService.GetAllDevices();
        _logger.LogInformation($"Извлечено {devices.Count()} устройств.");

        return Ok(devices);
    }

    [HttpGet("{deviceId}/logs")]
    public IActionResult GetDeviceLogs(string deviceId)
    {
        if (string.IsNullOrWhiteSpace(deviceId))
        {
            _logger.LogWarning("В запросе нет ID устройства.");
            return BadRequest("Необходим ID устройства.");
        }

        _logger.LogInformation($"Извлечение записей для устройства с ID: {deviceId}");
        var logs = _deviceService.GetDeviceLogs(deviceId);

        if (!logs.Any())
        {
            _logger.LogWarning($"Не найдено записей для устройства с ID: {deviceId}");
            return NotFound($"Не найдено записей для устройства с ID: {deviceId}");
        }
        
        _logger.LogInformation($"Извлечено {logs.Count()} записей для устройства с ID: {deviceId}");
        return Ok(logs);
    }

    [HttpDelete("{deviceId/remove}")]
    public IActionResult RemoveRecords(string deviceId, [FromQuery] DateTime olderThan)
    {
        if (string.IsNullOrWhiteSpace(deviceId))
        {
            _logger.LogWarning("Не указан ID устройства для удаления записей.");
            return BadRequest("Необходим ID устройства");
        }
        
        _logger.LogInformation($"Удаление записей старше чем {olderThan} для устройства с ID: {deviceId}");
        _logger.LogInformation($"Записи были успешно удалены для устройства с ID: {deviceId}");
        return Ok($"Записи для устройства {deviceId} старше чем {olderThan} были удалены.");
    }
}