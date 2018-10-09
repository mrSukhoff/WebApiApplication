using System;
using System.Collections.Generic;
using System.Web.Http;
using WebApiApplication.Models;

namespace WebApiApplication.Controllers
{
    public class ValuesController : ApiController
    {
        RecordContext db = new RecordContext();

        /// <summary>
        /// Контроллер GET. Возвращает весь список записей из базы
        /// </summary>
        public IEnumerable<Record> Get()
        {
            return db.Records;
        }

        /// <summary>
        ///  Контроллер GET. Возвращает из базы запись с идентификатором N из /values/api/N
        /// </summary>
        public Record Get(int id)
        {
            return db.Records.Find(id);
        }

        /// <summary>
        /// По запросу POST api/values разбирает json на строки, сортирует и передает для занесения в базу
        /// </summary>
        public async void Post()
        {
            //получаем тело запроса в виде строки
            string str =await Request.Content.ReadAsStringAsync();

            //удаляем квадратные скобки
            char[] symbols1 = { '[', ']' };
            str = str.Trim(symbols1);

            //делим строку на подстроки
            string[] pairs = str.Split(',');

            //создаем список записей
            List<Record> list = new List<Record>();
            try
            {
                char[] symbols2 = { '{', '}' };
                foreach (string line in pairs)
                {
                    //убираем фигурные скобки и делим каждую пару на ключ и значение
                    string[] lines = line.Trim(symbols2).Split(':');
                    Record rec = new Record
                    {
                        Code = int.Parse(lines[0]),
                        Value = lines[1].Trim('"')
                    };
                    list.Add(rec); // добавляем каждую пару в список
                }

                list.Sort(); //в конце сортируем его. 

                //чистим базу
                db.Records.RemoveRange(db.Records);
                db.SaveChanges();

                //добавляем список в базу
                foreach (Record rec in list)
                {
                    db.Records.Add(rec);
                }
                db.SaveChanges();
                var tmp = db.Records;
            }
            catch (Exception ex)
            {
                //nobody care :(
            }
            
        }
    }
}
