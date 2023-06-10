# Система администрирования пользователей
## Студент: Елисеев Дмитрий Андреевич
## Группа: БФМО-01-22
## Язык: C#
## Структура
Структура системы выглядит следующим образом:
![image](https://github.com/4kMonik/AdministrationSystem/assets/77895085/525182c1-4873-4ece-94b3-f98360419032)
![image](https://github.com/4kMonik/AdministrationSystem/assets/77895085/34234782-422f-491b-bcfe-beda225c8974)

### PostgreSQL
В качестве источника данных была создана локальная PostgeSQL BD на порте 5549
Для доступа к таблицам был прописан пользователь admin с паролем 123
БД состоит из трёх таблиц:

Users - записи пользователей

![image](https://github.com/4kMonik/AdministrationSystem/assets/77895085/657692d6-20fe-47b9-a01d-14d0e96fc1e8)

Roles - для хранения доступных ролей

![image](https://github.com/4kMonik/AdministrationSystem/assets/77895085/5cac2b0f-a602-4c91-9bb6-6697279722aa)

LoginTime - таблица содержащая время входа пользователей в систему

![image](https://github.com/4kMonik/AdministrationSystem/assets/77895085/333e6a8e-b4d2-4f81-b7e7-94ac77a5417e)

### DataAccessLayer
Для подключения к БД использовалась библиотека Npgsql
На данном слое реализованы методы, отправляющие SQL-запросы и выводящие данные в виде Buisness object объектов
### BuisnessObject
Представление полученных данных из БД для передачи между DAL и BLL
### BuisnessLogicLayer
Представляет набор методов работы с DAL для UIL.

Данные на слой представления передаются в виде объектов-страниц (PageObject)

### Page Object

Представляет из себя классы содержащий данные загруженной страницы или загруженного пользователя в строковом формате

### UserInterfaceLayer
Пользовательский интерфейс реадизован в виде приложения командной строки 

Получает данные в виде объектов страниц от слоя BL, а также отвечает за вывод информации и получения команд

Всего реализовано 3 экрана:
- Главный экран

![image](https://github.com/4kMonik/AdministrationSystem/assets/77895085/8ec839e8-6ed2-4667-a4a2-219a4baac623)

- Экран таблцы пользователей

![image](https://github.com/4kMonik/AdministrationSystem/assets/77895085/cb8b694c-375c-4553-a442-95a575919871)

- Экран конкретного пользователя

![image](https://github.com/4kMonik/AdministrationSystem/assets/77895085/40362ebe-9c7e-445e-b109-1ed67db644f8)

