using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace WebApiApplication.Models
{
    public class Model
    {
        public string ConnectionString { get;}
        private static bool created = false;
        public Model()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["RecordsConnection"].ConnectionString;

            //если таблицу уже создавали - выходим
            if (created) return;
            //пытаемся удалить и создать таблицу
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    string expression = "DROP TABLE [dbo].[Records];";
                    SqlCommand dropCommand = new SqlCommand(expression, connection);
                    dropCommand.ExecuteNonQuery(); 
                    expression = "CREATE TABLE [dbo].[Records] ([Id] INT NOT NULL, [Code] INT NOT NULL, [Value] NVARCHAR(MAX) NULL);";
                    SqlCommand createCommand = new SqlCommand(expression, connection);
                    createCommand.ExecuteNonQuery();
                    created = true;
                }
            }
            catch { };
        }
        
        /// <summary>
        ////Метод считывает все записи из таблицы Records
        /// </summary>
        public List<Record> GetAllRecords()
        {
            List<Record> records = new List<Record>();
            string sqlExpression = "SELECT * FROM Records";
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand readCommand = new SqlCommand(sqlExpression, connection);
                SqlDataReader reader = readCommand.ExecuteReader();

                if (reader.HasRows) 
                {
                    
                    while (reader.Read()) // построчно считываем данные
                    {
                        Record rec = new Record //создаем запись
                        {
                            Id = (int)reader.GetValue(0),
                            Code = (int)reader.GetValue(1),
                            Value = reader.GetValue(2).ToString()
                        };
                        records.Add(rec); //и добавляем ее в список
                    }
                }
                reader.Close();
            }
            return records;
        }

        /// <summary>
        /// Метод считывает из базы запись с конкретным Id. 
        /// Если запись в базе не найдена, то возвращает пустую запись.
        /// </summary>
        public Record GetRecordWithId(int Id)
        {
            string sqlExpression = "SELECT * FROM Records WHERE Id=" + Id.ToString();

            Record rec = new Record();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();
                    rec.Id = (int)reader.GetValue(0);
                    rec.Code = (int)reader.GetValue(1);
                    rec.Value = reader.GetValue(2).ToString();
                }
                reader.Close();
            }
            return rec;
        }

        public void SaveList(IEnumerable<Pair> list)
        {
            string sqlExpression;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand clearCommand = new SqlCommand("DELETE Records;",connection);
                clearCommand.ExecuteNonQuery();

                SqlCommand addCommand;
                int i = 0;
                foreach (Pair p in list)
                {
                    sqlExpression = "INSERT INTO Records (Id,Code,Value) VALUES (" + i + "," + p.Code + ",'" + p.Value.Trim('"') + "');";
                    addCommand = new SqlCommand(sqlExpression, connection);
                    addCommand.ExecuteNonQuery();
                    i++;
                }

             }
        }
    }
}