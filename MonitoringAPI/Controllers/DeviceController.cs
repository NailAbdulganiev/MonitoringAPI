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
        _logger.LogInformation("Устройство успешно добавлено. ID: {DeviceId}, Name: {DeviceName}", device.Id, device.Name);

        return  Ok(device);
    }

    [HttpGet]
    public IActionResult GetAllDevices()
    {
        _logger.LogInformation("Извлечение всех устройств...");
        var devices = _deviceService.GetAllDevices();
        _logger.LogInformation("Извлечено {DeviceCount} устройств.", devices.Count());

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

        _logger.LogInformation("Извлечение записей для устройства с ID: {DeviceId}", deviceId);
        var logs = _deviceService.GetDeviceLogs(deviceId);

        if (!logs.Any())
        {
            _logger.LogWarning("Не найдено записей для устройства с ID: {DeviceId}", deviceId);
            return NotFound($"Не найдено записей для устройства с ID: {deviceId}");
        }
        
        _logger.LogInformation("Извлечено {LogCount} записей для устройства с ID: {DeviceId}", logs.Count(), deviceId);
        return Ok(logs);
    }

    [HttpDelete("{deviceId}")]
    public IActionResult RemoveRecords(string deviceId, [FromQuery] DateTime olderThan)
    {
        if (string.IsNullOrWhiteSpace(deviceId))
        {
            _logger.LogWarning("Не указан ID устройства для удаления записей.");
            return BadRequest("Необходим ID устройства.");
        }
        
        if (olderThan == default)
        {
            _logger.LogWarning("Параметр olderThan не указан или некорректен.");
            return BadRequest("Необходим параметр olderThan.");
        }

        _logger.LogInformation("Удаление записей старше чем {OlderThan} для устройства с ID: {DeviceId}", olderThan, deviceId);
        _deviceService.RemoveRecords(deviceId, olderThan);
        _logger.LogInformation("Записи успешно удалены для устройства с ID: {DeviceId}.", deviceId);

        return Ok($"Записи для устройства {deviceId} старше чем {olderThan} были удалены.");
    }

    [HttpPost("backup")]
    public async Task<IActionResult> BackupDataAsync([FromQuery] string filepath = "backup.json")
    {
        try
        {
            await _deviceService.BackupToFileAsync(filepath);
            _logger.LogInformation("Данные успешно сохранены в файл: {FilePath}", filepath);
            return Ok($"Данные успешно сохранены в файл: {filepath}");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Ошибка при сохранении данных в файл {FilePath}.", filepath);
            return StatusCode(500, "Произошла ошибка при сохранении данных.");
        }
    }

    [HttpPost("restore")]
    public async Task<IActionResult> RestoreDataAsync([FromQuery] string filepath = "backup.json")
    {
        try 
        {
            await _deviceService.RestoreFromFileAsync(filepath);
            _logger.LogInformation("Данные успешно восстановлены из файла: {FilePath}.", filepath);
            return Ok($"Данные успешно восстановлены из файла: {filepath}");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Ошибка при восстановлении данных из файла {FilePath}.", filepath);
            return Problem("500", "Произошла ошибка при восстановлении данных.");
        }
    }
}
