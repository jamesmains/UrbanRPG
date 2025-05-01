using Parent_House_Framework.Values;

namespace DialogueEditor
{
    public abstract class Parameter
    {
        public Parameter(string name)
        {
            ParameterName = name;
        }

        public string ParameterName;
    }

    public class BoolParameter : Parameter
    {
        public BoolParameter(string name, bool defaultValue) : base(name)
        {
            BoolValue = defaultValue;
        }

        public bool BoolValue;
    }

    public class IntParameter : Parameter
    {
        public IntParameter(string name, int defaultValue) : base(name)
        {
            IntValue = defaultValue;
        }

        public int IntValue;
    }

    public class ChainedIntParameter : Parameter {
        public ChainedIntParameter(string name) : base(name) {
        }
        public ChainedInt ChainedIntValue;
    }
}