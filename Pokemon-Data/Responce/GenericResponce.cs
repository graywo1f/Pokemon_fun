using System;
using System.Collections.Generic;
using System.Text;

namespace Pokemon_Data.Responce
{
    public class GenericResponce<T> //where T : class, new()
    {
        public T Responce { get; set; }
        public bool ResponceOK { get; set; }
        public string Error { get; set; }

        public void Save(T t)
        {
            Responce = t;
        }
    }

    
}
