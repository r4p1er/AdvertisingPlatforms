# Advertising Platforms

Веб-сервис для управления рекламными площадками и их локациями. Позволяет загружать данные из файла и находить подходящие рекламные площадки для указанной локации.

## Запуск приложения

### Способ 1: Запуск через .NET CLI

1. Убедитесь, что установлен [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
2. Клонируйте репозиторий:
```bash
git clone git@github.com:r4p1er/AdvertisingPlatforms.git
cd AdvertisingPlatforms
```
3. Запустите приложение:
```bash
dotnet run --project AdvertisingPlatforms
```
4. В консоли отобразится адрес, по которому доступен сервис

### Способ 2: Запуск через Docker

1. Убедитесь, что установлен [Docker](https://www.docker.com/)
2. Соберите образ:
```bash
docker build -t advertising-platforms -f Dockerfile .
```
3. Запустите контейнер:
```bash
docker run -d -p 5000:8080 advertising-platforms
```
4. Сервис будет доступен по адресу: http://localhost:5000


## Использование сервиса

После успешного запуска приложения откройте в браузере основную страницу сервиса. Вас автоматически перенаправит на страницу Swagger UI, где вы сможете:
- Просматривать все доступные API-методы
- Тестировать endpoints непосредственно из браузера
- Ознакомиться с форматом запросов и ответов
