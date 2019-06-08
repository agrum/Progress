using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleJSON;

namespace Assets.Scripts.Data
{
    public class Serializer
    {
        public static JSONNode WriteEnum(Enum enum_)
        {
            return enum_.ToString();
        }

        public static T ReadEnum<T>(JSONNode jNode_) where T : struct, IConvertible
        {
            Type t = typeof(T);
            if (!t.IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }
            return (T)Enum.Parse(t, jNode_.ToString());
        }
    }
}

namespace SimpleJSON
{
    public abstract partial class JSONNode
    {
        public static implicit operator JSONNode(Enum enum_)
        {
            return Assets.Scripts.Data.Skill.Serializer.WriteEnum(enum_);
        }
    }
}
