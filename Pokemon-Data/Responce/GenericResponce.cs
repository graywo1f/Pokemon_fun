using System;
using System.Collections.Generic;
using System.Text;

namespace Pokemon_Data.Responce
{
   public class GenericResponce<T>
    {
        public T Responce { get; set; }
        public int ResponceStatus { get; set; }
        public List<string> Errors { get; set; }
    }
}
