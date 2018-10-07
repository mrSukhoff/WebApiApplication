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
        public void Post([FromBody]string value)
        {
            tmp = value;
            
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
