using System;

public class ObservableValue<T> {
    private T m_value;

    public T Value {
        get => m_value;
        set {
            m_value = value;
            OnValueChanged?.Invoke(m_value);
        }
    }

    public void SetWithoutNotify(T value) {
        m_value = value;
    }

    public void ForceInvoke() {
        OnValueChanged?.Invoke(m_value);
    }

    public Action<T> OnValueChanged;
}