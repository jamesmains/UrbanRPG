using Parent_House_Framework.Values;

namespace Urban.Attributes {
    public class AttributeSkill : Attribute {
        public AttributeSkill(AttributeDetails details) : base(details) {
        }

        public readonly ObservableValue<int> Level = new ObservableValue<int>();
        public readonly ObservableValue<int> Experience = new ObservableValue<int>();

        public int ExperienceRequired => 5 * Level.Value; // Todo: Need better magic value

        public void AddExperience(int value) {
            Experience.Value += value;
            while (Experience.Value >= ExperienceRequired) {
                Experience.SetWithoutNotify(Experience.Value - ExperienceRequired);
                Level.SetWithoutNotify(Level.Value + 1);
            }

            Experience.ForceInvoke();
            Level.ForceInvoke();
        }

        public void AddLevel(int value) {
            Level.Value += value;
        }
    }
}