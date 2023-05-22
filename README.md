# UrbanFiesta
## ⚙️Стек
- C#
- ASP.NET
- EF Core
- PostgreSQL
- AutoMapper
## 👤Пользователь 
### Возможности пользователя
- Просмотр мероприятий с пагинацией, самых популярных мероприятий, мероприятия по дате
- Подписка на рассылку
- Получение уведомлений о мероприятиях
## ⚜️Администраторы
- login: vanya@mail.ru, password: 123
- login: vasiliy@mail.ru, password: 123
### Возможности администраторов
- CRUD операции над мероприятиями
- Блокировка/разблокировка пользователей
- Рассылка сообщений пользователям
## 🛠️Сервисы 
- Сервис по отправке Email
- Сервис автоматического отслеживания статуса мероприятий
- Сервис автоматического напоминания о мероприятиях
## 🚬Использование собственных ActionFilter
- ExistAttribute<T> -- проверяет существует ли сущность в БД
- ModelStateIsValid -- проверяет ModelState
## 🍗Дополнительные функции
- Подтверждение почты
- Восстановление пароля
## 🐳Как запустить
  1. Скачать DockerDesktop
  2. Запустить DockerDesktop
  3. git clone https://github.com/alimskeetit/UrbanFiesta
  4. В папке UrbanFiesta выполняем docker compose up --build
## 🌐Как запустить вместе с FrontEnd
  Инструкция в https://github.com/VasilyRazdorsky/RTUITLab_recruit
