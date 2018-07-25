using System;
using System.Collections.Generic;
using System.Text;

namespace MongoTest
{
   public class TypeA
    {
        public EnumA Enum { get; set; }
    }

    public class TypeB
    {
        public EnumB Enum { get; set; }
    }

    public enum EnumA
    {
        None,
        First,
        Second
    }

    public enum EnumB
    {
        None,
        First,
        Second

    }
}
