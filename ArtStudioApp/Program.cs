using System;
using ArtStudioApp;

var db = new DbContext();

Console.WriteLine("=== Инициализация базы данных ===");
db.InitializeDatabase();
Console.WriteLine("База данных создана и заполнена тестовыми данными");

Console.WriteLine("\n=== РЕЗУЛЬТАТ ЗАПРОСА ===\n");
db.PrintProjectParticipants();