namespace Attributes {
    public class AttributeStat : Attribute
    {
        public AttributeStat(AttributeDetails details) : base(details) {
        }
    
        // Todo: rename this variable
        public readonly ObservableValue<int> StatValue = new();
    }
}
