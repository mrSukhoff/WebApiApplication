using System;
using System.Collections.Generic;
using System.Web.Http;
using WebApiApplication.Models;

namespace WebApiApplication.Controllers
{
    public class ValuesController : ApiController
    {
        Model m = new Model();

        /// <summary>
        /// Контроллер GET. Возвращает весь список записей из базы
        /// </summary>
        public IEnumerable<Record> Get()
        {
            return m.GetAllRecords();
        }


        /// <summary>
        ///  Контроллер GET. Возвращает из базы запись с идентификатором N из /values/api/N
        /// </summary>
        public Record Get(int id)
        {
            return m.GetRecordWithId(id);
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

            //создаем список пар
            List<Pair> list = new List<Pair>();
            try
            {
                char[] symbols2 = { '{', '}' };
                foreach (string line in pairs)
                {
                    //убираем фигурные скобки и делим каждую пару на ключ и значение
                    string[] lines = line.Trim(symbols2).Split(':');
                    Pair p = new Pair
                    {
                        Code = int.Parse(lines[0]),
                        Value = lines[1]
                    };
                    list.Add(p); // добавляем пару в список
                }
                list.Sort(); //в конце сортируем его. Сортировка идёт по полю "Code" - сравнение прописано в объекте.
                m.SaveList(list);
            }
            catch (Exception ex)
            {
                //nobody care :(
            }
        }
    }
}
