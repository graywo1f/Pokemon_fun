using System;
using System.Collections.Generic;
using System.Text;

namespace Pokemon_Data.Responce
{
   public class GenericListResponce<T> : GenericResponce<T> where T :List<T>
    {
        public int Count { get { return Responce.Count; } }
    }
}
