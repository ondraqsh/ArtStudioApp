using System;
using Microsoft.Data.Sqlite;

namespace ArtStudioApp;

public class DbContext
{
    private const string ConnectionString = @"Data Source=C:\Users\College\RiderProjects\ArtStudioApp\ArtStudio.db";
    private readonly SqliteConnection _db;

    public DbContext()
    {
        _db = new SqliteConnection(ConnectionString);
    }

    // Инициализация БД (создание таблиц и заполнение данными из .sql файла)
    public void InitializeDatabase()
    {
        _db.Open();

        // Читаем SQL файл
        var sqlScript = System.IO.File.ReadAllText(@"C:\Users\College\RiderProjects\ArtStudioApp\ArtStudioApp\create_db.sql");

        // Разделяем на отдельные команды (по точке с запятой)
        var commands = sqlScript.Split(new[] { ";\n", ";\r\n" }, StringSplitOptions.RemoveEmptyEntries);

        foreach (var commandText in commands)
        {
            if (string.IsNullOrWhiteSpace(commandText)) continue;

            using var command = new SqliteCommand(commandText, _db);
            command.ExecuteNonQuery();
        }

        _db.Close();
    }

    // ГЛАВНЫЙ ЗАПРОС: для каждого проекта - участники с ролью и паспортными данными
    public void PrintProjectParticipants()
    {
        _db.Open();

        const string sql = @"
            SELECT 
                p.name AS ProjectName,
                p.deadline AS ProjectDeadline,
                perf.nickname AS PerformerNickname,
                perf.first_name AS PerformerFirstName,
                perf.last_name AS PerformerLastName,
                pp.role AS PerformerRole,
                pf.passport_series AS PassportSeries,
                pf.passport_number AS PassportNumber,
                pf.address AS PerformerAddress
            FROM table_projects p
            LEFT JOIN table_performerprojects pp ON p.id = pp.project_id
            LEFT JOIN table_performers perf ON pp.performer_id = perf.id
            LEFT JOIN table_personalfiles pf ON perf.id = pf.performer_id
            ORDER BY p.name, perf.nickname;
        ";

        using var command = new SqliteCommand(sql, _db);
        using var reader = command.ExecuteReader();

        if (!reader.HasRows)
        {
            Console.WriteLine("Нет данных о проектах и участниках");
            _db.Close();
            return;
        }

        string currentProject = "";
        bool firstProject = true;

        while (reader.Read())
        {
            var projectName = reader.GetString(reader.GetOrdinal("ProjectName"));
            var projectDeadline = reader.GetString(reader.GetOrdinal("ProjectDeadline"));
            var nickname = reader.IsDBNull(reader.GetOrdinal("PerformerNickname"))
                ? "—"
                : reader.GetString(reader.GetOrdinal("PerformerNickname"));
            var firstName = reader.IsDBNull(reader.GetOrdinal("PerformerFirstName"))
                ? "—"
                : reader.GetString(reader.GetOrdinal("PerformerFirstName"));
            var lastName = reader.IsDBNull(reader.GetOrdinal("PerformerLastName"))
                ? "—"
                : reader.GetString(reader.GetOrdinal("PerformerLastName"));
            var role = reader.IsDBNull(reader.GetOrdinal("PerformerRole"))
                ? "—"
                : reader.GetString(reader.GetOrdinal("PerformerRole"));
            var passportSeries = reader.IsDBNull(reader.GetOrdinal("PassportSeries"))
                ? "—"
                : reader.GetString(reader.GetOrdinal("PassportSeries"));
            var passportNumber = reader.IsDBNull(reader.GetOrdinal("PassportNumber"))
                ? "—"
                : reader.GetString(reader.GetOrdinal("PassportNumber"));
            var address = reader.IsDBNull(reader.GetOrdinal("PerformerAddress"))
                ? "—"
                : reader.GetString(reader.GetOrdinal("PerformerAddress"));

            var fullName = $"{lastName} {firstName}";
            var passportFull = $"{passportSeries} {passportNumber}";

            if (projectName != currentProject)
            {
                if (!firstProject) Console.WriteLine();
                Console.WriteLine($"📁 ПРОЕКТ: {projectName}");
                Console.WriteLine($"   Дедлайн: {projectDeadline}");
                Console.WriteLine($"   Участники:");
                currentProject = projectName;
                firstProject = false;
            }

            Console.WriteLine($"      • {nickname} ({fullName}) | Роль: {role} | Паспорт: {passportFull} | Адрес: {address}");
        }

        _db.Close();
    }
}