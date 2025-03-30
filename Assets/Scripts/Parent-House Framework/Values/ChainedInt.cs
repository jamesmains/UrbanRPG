using System;

namespace Parent_House_Framework.Values {
    [Serializable]
    public class ChainedInt {
        // Todo: Would be nice to have minValue so it's not hardcoded to be a time variable...
        public ChainedInt(int maxValue) {
            m_maxValue = maxValue;
            ObservedValue = new ObservableValue<int>();
            Value = 1;
        }

        public ChainedInt Link(ChainedInt chainedInt) {
            m_chainedInt = chainedInt;
            return this;
        }

        private readonly int m_maxValue;
        public ObservableValue<int> ObservedValue { get; }
        private ChainedInt m_chainedInt;

        public int Value {
            get => ObservedValue.Value;
            private set {
                if(ObservedValue.Value.Equals(value)) return;
                ObservedValue.Value = value;
                if (ObservedValue.Value <= m_maxValue) return;
                m_chainedInt?.AddValue(1);
                ObservedValue.Value = 1;
            }
        }

        public void AddValue(int value) {
            Value += value;
            while (Value >= m_maxValue) {
                Value = (ObservedValue.Value - m_maxValue);
                if(m_chainedInt != null)
                    m_chainedInt.Value = (m_chainedInt.Value + 1);
            }
        }
    }
}