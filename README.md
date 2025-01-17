# Сервис мониторинга стороннего приложения

## Описание проекта

Данный проект представляет собой сервис мониторинга, который позволяет получать, обрабатывать и отображать статистику активности устройств. Стороннее приложение отправляет данные о сессиях активности устройств, а пользователь может просматривать статистику через API.

## Основной функционал
1. **API для получения статистической информации от устройств.**
2. **Хранение данных in-memory.**
3. **Отображение списка всех устройств через API.**
4. **Отображение записей об устройстве в контексте выбранного устройства через API.**

## Формат данных
### Пример сообщения от стороннего приложения:
```json
{
    "_id": "f695ea23-8662-4a57-975a-f5afd26655db",
    "name": "John Doe",
    "startTime": "1980-01-02T00:00:00.000Z",
    "endTime": "1980-01-04T00:00:00.000Z", 
    "version": "1.0.0.56"
}
```

### Передаваемые данные:
1. **Идентификационные данные устройства:**
   - Идентификатор
   - Имя пользователя
2. **Информация о сессии активности:**
   - Время включения
   - Время выключения
3. **Данные об установленном приложении:**
   - Версия

## Технологии
- **.NET 8**
- **JSON**

## Сценарий использования
1. Стороннее приложение отправляет данные в заданном формате.
2. WebAPI обрабатывает и сохраняет данные.
3. Пользователь запрашивает данные через API.
4. Пользователь выбирает устройство из списка.
5. API возвращает статистику использования выбранного устройства.

## Дополнительные задания
1. Покрыть функционал приложения логами для диагностики неисправностей без остановки приложения.
2. Добавить скрипт генерации схемы API.
3. Реализовать функционал ручного удаления устаревших записей.
4. Добавить возможность бэкапирования данных в файл.

---

## Установка и запуск

### 1. Клонирование репозитория
```bash
git clone <ссылка-на-репозиторий>
cd <папка-репозитория>
```

### 2. Запуск приложения
Соберите и запустите проект с помощью команд для вашей среды разработки, например:
```bash
dotnet run
```

### 3. Доступ к приложению
- **WebAPI:** `http://localhost:5000`

---

## Логи
Приложение ведёт логи для диагностики ошибок и анализа работы системы. Логи доступны в консоли или в файлах, если это предусмотрено настройками приложения.

---

## Автор
Этот проект разработан в учебных целях для демонстрации навыков работы с .NET.
