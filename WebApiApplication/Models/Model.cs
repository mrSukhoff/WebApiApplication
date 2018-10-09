using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace WebApiApplication.Models
{
    public class Model
    {
        public string ConnectionInfo { get; set; }

        public string ConnectionString { get; }

        public Model()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["RecordsConnection"].ConnectionString;
            
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                ConnectionInfo = connection.Database + " - " + connection.State; 
            }
            
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
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows) 
                {
                    
                    while (reader.Read()) // построчно считываем данные
                    {
                        Record rec = new Record
                        {
                            Id = (int)reader.GetValue(0),
                            Code = (int)reader.GetValue(1),
                            Value = reader.GetValue(2).ToString()
                        };
                        records.Add(rec);
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