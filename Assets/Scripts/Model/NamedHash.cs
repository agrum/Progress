using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleJSON;

namespace Assets.Scripts.Model
{
    public class NamedHash
    {
        public class TypeComparer : Comparer<NamedHash>
        {
            public override int Compare(NamedHash x, NamedHash y)
            {
                return x.Value.CompareTo(y.Value);
            }
        }

        string String;
        int Value;

        public NamedHash(string string_)
        {
            String = string_;
            Value = String.GetHashCode();
        }

        public static implicit operator NamedHash(JSONNode jNode_)
        {
            return jNode_;
        }

        public static implicit operator NamedHash(string string_)
        {
            return string_;
        }

        public static implicit operator JSONNode(NamedHash numeric_)
        {
            return numeric_.String;
        }
    }
}
