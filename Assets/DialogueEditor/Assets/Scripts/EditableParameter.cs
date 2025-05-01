using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using Parent_House_Framework.Values;

namespace DialogueEditor
{
    [DataContract]
    public abstract class EditableParameter
    {
        public const int MAX_NAME_SIZE = 24;

        public enum eParamType
        {
            Bool,
            Int,
            ChainedInt
        }

        public EditableParameter(string name)
        {
            ParameterName = name;
        }

        public abstract eParamType ParameterType { get; }

        [DataMember] public string ParameterName;
    }

    [DataContract]
    public class EditableBoolParameter : EditableParameter
    {
        public EditableBoolParameter(string name) : base(name) { }

        public override eParamType ParameterType { get { return eParamType.Bool; } }

        [DataMember] public bool BoolValue;
    }

    [DataContract]
    public class EditableIntParameter : EditableParameter
    {
        public EditableIntParameter(string name) : base(name) { }

        public override eParamType ParameterType { get { return eParamType.Int; } }

        [DataMember] public int IntValue;
    }

    [DataContract]
    public class EditableChainedIntParameter : EditableParameter {
        public EditableChainedIntParameter(string name) : base(name) { }
        public override eParamType ParameterType { get { return eParamType.ChainedInt; } }
        [DataMember] public ChainedInt ChainedIntValue;
    }
}