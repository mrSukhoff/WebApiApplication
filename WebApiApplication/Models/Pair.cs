using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiApplication.Models
{
    public struct Pair : IComparable<Pair>
    {
        public int Code;
        public string Value;

        public int CompareTo(Pair obj)
        {
            return Code.CompareTo(obj.Code);
        }
    }
}