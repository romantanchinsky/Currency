## Запуск

docker-compose up --build

## Эндпоинты

### User
- `POST /api/auth/register`
- `POST /api/auth/login`

### Finance
- `GET /api/currencies`

> Если у пользователя нет избранных валют, возвращается полный список валют.

## Demo данные

В базе присутствуют сиды (пользователи и избранные валюты).

### Пример

POST http://localhost:8080/user/api/auth/login
Content-Type: application/json

{
    "name": "user1",
    "password": "123"
}