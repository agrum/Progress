using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleJSON;

namespace Assets.Scripts.Data
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

        public override int GetHashCode()
        {
            return Value;
        }

        public override bool Equals(object other_)
        {
            var asNamedHash = other_ as NamedHash;
            return asNamedHash != null && Value == asNamedHash.Value;
        }

        public string String { get; private set; }
        public int Value { get; private set; }

        public NamedHash(string string_)
        {
            String = string_;
            Value = String.GetHashCode();
        }

        public static implicit operator NamedHash(JSONNode jNode_)
        {
            return new NamedHash(jNode_.Value);
        }

        public static implicit operator NamedHash(string string_)
        {
            return new NamedHash(string_);
        }

        public static implicit operator JSONNode(NamedHash numeric_)
        {
            return new JSONString(numeric_.String);
        }
    }
}
