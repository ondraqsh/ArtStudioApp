CREATE TABLE IF NOT EXISTS table_performers
(
    id                INTEGER PRIMARY KEY AUTOINCREMENT,
    nickname          TEXT NOT NULL UNIQUE,
    first_name        TEXT NOT NULL,
    last_name         TEXT NOT NULL,
    registration_date TEXT NOT NULL
);

CREATE TABLE IF NOT EXISTS table_personalfiles
(
    id              INTEGER PRIMARY KEY AUTOINCREMENT,
    performer_id    INTEGER NOT NULL UNIQUE,
    passport_series TEXT    NOT NULL,
    passport_number TEXT    NOT NULL,
    address         TEXT    NOT NULL,
    FOREIGN KEY (performer_id) REFERENCES table_performers (id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS table_projects
(
    id       INTEGER PRIMARY KEY AUTOINCREMENT,
    name     TEXT NOT NULL,
    deadline TEXT NOT NULL
);

CREATE TABLE IF NOT EXISTS table_performerprojects
(
    performer_id INTEGER NOT NULL,
    project_id   INTEGER NOT NULL,
    role         TEXT    NOT NULL,
    PRIMARY KEY (performer_id, project_id),
    FOREIGN KEY (performer_id) REFERENCES table_performers (id) ON DELETE CASCADE,
    FOREIGN KEY (project_id) REFERENCES table_projects (id) ON DELETE CASCADE
);

INSERT INTO table_performers (nickname, first_name, last_name, registration_date)
VALUES ('StarLord', 'Пётр', 'Звёздный', '2024-01-15'),
       ('RocketMan', 'Илья', 'Ракетин', '2024-02-20'),
       ('GrootFan', 'Денис', 'Дубов', '2024-03-10'),
       ('NebulaGirl', 'Анна', 'Туманная', '2024-04-05'),
       ('DraxTheDestroyer', 'Артём', 'Крутов', '2024-05-12');

INSERT INTO table_personalfiles (performer_id, passport_series, passport_number, address)
VALUES (1, '4010', '123456', 'г. Москва, ул. Тверская, д. 10, кв. 5'),
       (2, '4015', '789012', 'г. Санкт-Петербург, Невский пр., д. 25, кв. 12'),
       (3, '4020', '345678', 'г. Казань, ул. Баумана, д. 7, кв. 3'),
       (4, '4025', '901234', 'г. Новосибирск, Красный пр., д. 45, кв. 8'),
       (5, '4030', '567890', 'г. Екатеринбург, ул. Ленина, д. 30, кв. 2');

INSERT INTO table_projects (name, deadline)
VALUES ('Космическое шоу', '2025-12-01'),
       ('Рокет-фест', '2025-10-15'),
       ('Супергеройский баттл', '2025-11-30');

INSERT INTO table_performerprojects (performer_id, project_id, role)
VALUES
    (1, 1, 'Ведущий'),
    (2, 1, 'Монтажёр'),
    (3, 1, 'Сценарист'),
    (4, 1, 'Оператор'),

    (1, 2, 'Сценарист'),
    (2, 2, 'Ведущий'),
    (5, 2, 'Звукорежиссёр'),

    (3, 3, 'Ведущий'),
    (4, 3, 'Монтажёр'),
    (5, 3, 'Сценарист');