# Ресторанная система управления "Ksushenka"

## Описание
Ksushenka - это система автоматизации рабочих процессов ресторана, которая помогает управлять сменами сотрудников, чеками и формировать отчеты. Система разработана для повышения эффективности работы ресторана и обеспечения удобного интерфейса для персонала.

## Функциональные возможности

### Управление сменами
- Открытие новой смены с выбором сотрудника и кассы
- Закрытие смены с автоматическим подсчетом выручки
- Просмотр списка всех смен с информацией о сотрудниках и выручке

### Управление чеками
- Создание новых чеков
- Добавление товаров в чек с указанием количества и скидки
- Выбор типа оплаты (наличные или карта)
- Закрытие чека с фиксацией суммы и типа оплаты

### Отчеты
- Формирование отчета о выручке по сменам
- Отображение топ-5 самых популярных товаров

## Технические требования
- Windows 7/8/10/11
- .NET Framework 4.8
- PostgreSQL 12 или выше
- Минимум 4 ГБ ОЗУ
- Разрешение экрана не менее 1280x720

## Инструкция по установке

1. Установите PostgreSQL (если не установлен):
   - Скачайте и установите PostgreSQL с сайта https://www.postgresql.org/download/
   - Создайте базу данных "postgres" (или используйте существующую)
   - Создайте пользователя с именем "postgres" и паролем "5472"

2. Настройка базы данных:
   - Выполните SQL-скрипты из папки "SQL" для создания необходимых таблиц
   - Загрузите начальные данные с помощью скрипта init_data.sql

3. Установка приложения:
   - Распакуйте архив с программой
   - Отредактируйте файл App.config, указав правильные параметры подключения к базе данных
   - Запустите файл Ksushenka.exe

## Использование приложения

1. **Главное меню** - позволяет перейти к управлению сменами, чеками или отчетам.

2. **Управление сменами**:
   - Выберите сотрудника и кассу
   - Нажмите "Начать смену"
   - Для завершения смены выберите смену в списке и нажмите "Закончить смену"

3. **Управление чеками**:
   - Выберите активную смену
   - Добавьте товары в чек, указав количество и скидку
   - Выберите тип оплаты (наличные или карта)
   - Нажмите "Оплатить чек" для закрытия чека

4. **Отчеты**:
   - "Выручка по сменам" - отображает общую выручку за каждую смену
   - "Топ-5 товаров" - показывает самые популярные товары по количеству продаж

## Контактная информация
При возникновении вопросов или проблем обращайтесь:
- Email: support@ksushenka.ru
- Телефон: +7 (XXX) XXX-XX-XX

## Лицензия
© 2025 Ksushenka. Все права защищены. 

## Текст от меня
Это тебе от меня мини подарок, чтобы не начинать нам с нуля

##
Структура базы
//////////////
CREATE TABLE Employees (
    employee_id SERIAL PRIMARY KEY,
    full_name VARCHAR(100) NOT NULL,
    position VARCHAR(50) NOT NULL, -- Должность (официант, бармен, повар)
    hourly_rate DECIMAL(10,2) NOT NULL, -- Почасовая ставка
    fixed_salary DECIMAL(10,2), -- Фиксированная зарплата (если есть)
    phone VARCHAR(20),
    email VARCHAR(100),
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE Shifts (
    shift_id SERIAL PRIMARY KEY,
    employee_id INT REFERENCES Employees(employee_id),
    cash_register_id INT REFERENCES CashRegisters(cash_register_id),
    start_time TIMESTAMP NOT NULL,
    end_time TIMESTAMP, -- NULL, если смена не завершена
    total_sales DECIMAL(10,2) DEFAULT 0, -- Автоматически обновляется
    cash_in_drawer DECIMAL(10,2) DEFAULT 0, -- Наличные в кассе при закрытии
    status VARCHAR(20) DEFAULT 'active' -- active/closed
);

CREATE TABLE CashRegisters (
    cash_register_id SERIAL PRIMARY KEY,
    location VARCHAR(100) NOT NULL, -- Местоположение (зал, бар)
    status VARCHAR(20) DEFAULT 'active' -- active/broken
);

CREATE TABLE Receipts (
    receipt_id SERIAL PRIMARY KEY,
    shift_id INT REFERENCES Shifts(shift_id),
    employee_id INT REFERENCES Employees(employee_id),
    total_amount DECIMAL(10,2) NOT NULL,
    payment_type VARCHAR(30) NOT NULL, -- cash/card/online
    status VARCHAR(20) DEFAULT 'open', -- open/closed/cancelled
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE MenuItems (
    item_id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    price DECIMAL(10,2) NOT NULL,
    category VARCHAR(50) NOT NULL, -- еда, напитки, десерты
    is_available BOOLEAN DEFAULT TRUE
);

CREATE TABLE ReceiptDetails (
    receipt_detail_id SERIAL PRIMARY KEY,
    receipt_id INT REFERENCES Receipts(receipt_id),
    item_id INT REFERENCES MenuItems(item_id),
    quantity INT NOT NULL,
    price_at_time DECIMAL(10,2) NOT NULL, -- Цена на момент продажи
    discount DECIMAL(10,2) DEFAULT 0
);

CREATE TABLE Payments (
    payment_id SERIAL PRIMARY KEY,
    receipt_id INT REFERENCES Receipts(receipt_id),
    amount DECIMAL(10,2) NOT NULL,
    payment_method VARCHAR(30) NOT NULL, -- cash/card/online
    payment_time TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE BonusesPenalties (
    bonus_id SERIAL PRIMARY KEY,
    employee_id INT REFERENCES Employees(employee_id),
    amount DECIMAL(10,2) NOT NULL,
    type VARCHAR(20) NOT NULL, -- bonus/penalty
    reason TEXT,
    date DATE NOT NULL
);

CREATE TABLE EmployeePayouts (
    payout_id SERIAL PRIMARY KEY,
    employee_id INT REFERENCES Employees(employee_id),
    shift_id INT REFERENCES Shifts(shift_id),
    base_salary DECIMAL(10,2) NOT NULL, -- Заработано за смену
    bonuses_total DECIMAL(10,2) DEFAULT 0, -- Сумма бонусов
    penalties_total DECIMAL(10,2) DEFAULT 0, -- Сумма штрафов
    total DECIMAL(10,2) NOT NULL, -- base + bonuses - penalties
    payout_date DATE NOT NULL,
    is_paid BOOLEAN DEFAULT FALSE
);


Данные для заполнения бд
/////////////////////////

INSERT INTO Employees (full_name, position, hourly_rate, fixed_salary, phone, email) VALUES
('Иван Петров', 'официант', 200.00, NULL, '+79123456789', 'ivan@example.com'),
('Ольга Смирнова', 'бармен', 250.00, NULL, '+79876543210', 'olga@example.com'),
('Алексей Иванов', 'повар', NULL, 50000.00, '+79012345678', 'alex@example.com'),
('Наталья Козлова', 'официант', 200.00, NULL, '+79998887766', 'natalya@example.com');

////////////////////////////////////////////////////////////////////////////////////////

INSERT INTO CashRegisters (location, status) VALUES
('Основной зал', 'active'),
('Барная стойка', 'active'),
('Терраса', 'broken');

////////////////////////////////////////////////////////////////////////////////////////

-- Активная смена
INSERT INTO Shifts (employee_id, cash_register_id, start_time, status) VALUES
(1, 1, '2023-10-25 09:00:00', 'active');

-- Завершенная смена
INSERT INTO Shifts (employee_id, cash_register_id, start_time, end_time, total_sales, cash_in_drawer, status) VALUES
(2, 2, '2023-10-25 14:00:00', '2023-10-25 22:00:00', 45000.00, 45000.00, 'closed');

////////////////////////////////////////////////////////////////////////////////////////

INSERT INTO MenuItems (name, price, category, is_available) VALUES
('Стейк из говядины', 800.00, 'основные блюда', TRUE),
('Цезарь с курицей', 450.00, 'салаты', TRUE),
('Мохито', 300.00, 'напитки', TRUE),
('Чизкейк', 250.00, 'десерты', TRUE),
('Латте', 150.00, 'напитки', TRUE);

////////////////////////////////////////////////////////////////////////////////////////

-- Чек для активной смены
INSERT INTO Receipts (shift_id, employee_id, total_amount, payment_type, status) VALUES
(1, 1, 1200.00, 'card', 'open');

-- Чек для завершенной смены
INSERT INTO Receipts (shift_id, employee_id, total_amount, payment_type, status) VALUES
(2, 2, 750.00, 'cash', 'closed');

////////////////////////////////////////////////////////////////////////////////////////

-- Детализация для первого чека
INSERT INTO ReceiptDetails (receipt_id, item_id, quantity, price_at_time, discount) VALUES
(1, 1, 1, 800.00, 0.00),
(1, 3, 1, 300.00, 0.00),
(1, 5, 2, 150.00, 10.00); -- Скидка 10% на 2 латте

-- Детализация для второго чека
INSERT INTO ReceiptDetails (receipt_id, item_id, quantity, price_at_time, discount) VALUES
(2, 2, 1, 450.00, 0.00),
(2, 4, 1, 250.00, 0.00);

////////////////////////////////////////////////////////////////////////////////////////

-- Оплата картой
INSERT INTO Payments (receipt_id, amount, payment_method) VALUES
(1, 1200.00, 'card');

-- Оплата наличными
INSERT INTO Payments (receipt_id, amount, payment_method) VALUES
(2, 750.00, 'cash');

////////////////////////////////////////////////////////////////////////////////////////

-- Бонус за отличный сервис
INSERT INTO BonusesPenalties (employee_id, amount, type, reason, date) VALUES
(1, 1000.00, 'bonus', 'Высокая оценка клиентов', '2023-10-25');

-- Штраф за опоздание
INSERT INTO BonusesPenalties (employee_id, amount, type, reason, date) VALUES
(4, 500.00, 'penalty', 'Опоздание на 30 минут', '2023-10-25');

////////////////////////////////////////////////////////////////////////////////////////

-- Расчёт за смену
INSERT INTO EmployeePayouts (employee_id, shift_id, base_salary, bonuses_total, penalties_total, total, payout_date, is_paid) VALUES
(1, 1, 1600.00, 1000.00, 0.00, 2600.00, '2023-10-25', FALSE),
(2, 2, 2000.00, 0.00, 500.00, 1500.00, '2023-10-25', TRUE);

////////////////////////////////////////////////////////////////////////////////////////
