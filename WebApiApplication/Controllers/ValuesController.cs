using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;
using WebApiApplication.Models;

namespace WebApiApplication.Controllers
{
    public class ValuesController : ApiController
    {
        Model m = new Model();
        string tmp;

        //GET
        public IEnumerable<Record> Get()
        {
            return m.GetAllRecords();
        }

        // GET api/values/5
        public Record Get(int id)
        {
            return m.GetRecordWithId(id);
        }

        // POST api/values
        public async void Post()
        {
            
            
            char[] symbols1 = {'[',']'};
            char[] symbols2 = { '{', '}' };
            //получаем тело запроса в виде строки
            string str =await Request.Content.ReadAsStringAsync();

            //удаляем квадратные скобки
            str = str.Trim(symbols1);

            //делим строку на подстроки
            string[] pairs = str.Split(',');

            //создаем список пар
            List<Pair> list = new List<Pair>();
            try
            {
                foreach (string line in pairs)
                {
                    //убираем фигурные скобки и делим каждую пару
                    string[] lines = line.Trim(symbols2).Split(':');
                    Pair p = new Pair
                    {
                        Code = int.Parse(lines[0]),
                        Value = lines[1]
                    };
                    list.Add(p); // добавляем пару в список
                }
                list.Sort(); //в конце сортируем его
                m.SaveList(list);
            }
            catch (Exception ex)
            {
                //nobody care :(
            }
        }



        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
