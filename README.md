# Система администрирования пользователей
## Студент: Елисеев Дмитрий Андреевич
## Группа: БФМО-01-22
## Язык: C#
## Структура
Структура системы выглядит следующим образом:
![image](https://github.com/4kMonik/AdministrationSystem/assets/77895085/f3509262-413c-4e3e-83e1-9aa226982705)
![image](https://github.com/4kMonik/AdministrationSystem/assets/77895085/17e40be6-02bd-4ee6-b6d7-abd08dd2df41)

### PostgreSQL
В качестве источника данных была создана локальная PostgeSQL BD на порте 5549
Для доступа к таблицам был прописан пользователь admin с паролем 123
БД состоит из трёх таблиц:

Users - записи пользователей

![image](https://github.com/4kMonik/AdministrationSystem/assets/77895085/0ed5a7e6-edeb-41be-978a-fc615cfe8cbc)

Roles - для хранения доступных ролей

![image](https://github.com/4kMonik/AdministrationSystem/assets/77895085/71d60650-8524-4814-bb7e-fab46ff19bf7)

LoginTime - таблица содержащая время входа пользователей в систему

![image](https://github.com/4kMonik/AdministrationSystem/assets/77895085/3c4afdf7-f490-453f-9db4-f55d095ff0c4)

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

![image](https://github.com/4kMonik/AdministrationSystem/assets/77895085/7b664a89-9a2c-4007-9eca-306631ab0697)

- Экран таблцы пользователей

![image](https://github.com/4kMonik/AdministrationSystem/assets/77895085/201d411b-51b1-4dbc-8e36-268f6a6159b4)

- Экран конкретного пользователя

![image](https://github.com/4kMonik/AdministrationSystem/assets/77895085/f80eef91-2368-4a23-a384-589c39a94626)

