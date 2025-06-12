using Npgsql;
using System.Data;
namespace ReadMangaApp.DataAccess
{
    public class DBConnection
    {
        private readonly string _connectionString;
        public DBConnection(string connectionString)
        {
            _connectionString = connectionString;
        }
        // Вспомогательный метод для добавления параметров к команде во избежание дублирования кода
        private void AddParameters(NpgsqlCommand command, NpgsqlParameter[] parameters)
        {
            if (parameters != null && parameters.Length > 0)
            {
                command.Parameters.AddRange(parameters);
            }
        }
        // DataTable загружает все данные сразу в таблицу, мы с ними работаем, а соединение с бд закрывается автоматиечски из-за using
        public DataTable ExecuteReader(string query, params NpgsqlParameter[] parameters)
        {
            // Создаем соединение с базой данных
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                // Создаем команду с запросом и соединением
                using (var command = new NpgsqlCommand(query, connection))
                {
                    // Добавляем параметры к команде
                    AddParameters(command, parameters);
                    // Заполняем DataTable
                    var dataTable = new DataTable();
                    using (var reader = command.ExecuteReader())
                    {
                        dataTable.Load(reader);
                    }
                    return dataTable; // Возвращаем заполненный DataTable
                }
            }
        }
        public object? ExecuteScalar(string query, params NpgsqlParameter[] parameters)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(query, connection))
                {
                    // Добавляем параметры к команде с помощью вспомогательного метода
                    AddParameters(command, parameters);
                    return command.ExecuteScalar();
                }    
            }
        }
    }
}
