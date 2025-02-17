using System;
using Sirenix.OdinInspector;
using UnityEngine;

public enum Days {
    Sunday = 1,
    Monday,
    Wednesday,
    Friday,
    Saturday,
}

public enum Months {
    January = 1,
    April,
    May,
    July,
    October,
    December
}
public class TimeManager : MonoBehaviour {
    [SerializeField, BoxGroup("Debug")]
    private float tickRate = 0.1f;
    
    private ChainedInt Minute;
    private ChainedInt Hour;
    private ChainedInt Day;
    private ChainedInt Week;
    private ChainedInt Month;
    private ChainedInt Year;

    private float TimeTillNextTick;

    public static Action<int,int,int,int,int,int> OnTimeChanged;

    private void Awake() {
        TimeTillNextTick = Time.time + 3;
        Year = new ChainedInt(9999);
        Month = new ChainedInt(6).Link(Year);
        Week = new ChainedInt(4).Link(Month);
        Day = new ChainedInt(5).Link(Week);
        Hour = new ChainedInt(24).Link(Day);
        Minute = new ChainedInt(60).Link(Hour);
    }

    private void OnEnable() {
        Minute.ObservedValue.OnValueChanged += delegate { HandleTimeChanged(); };
        Hour.ObservedValue.OnValueChanged += delegate { HandleTimeChanged(); };
        Day.ObservedValue.OnValueChanged += delegate { HandleTimeChanged(); };
        Week.ObservedValue.OnValueChanged += delegate { HandleTimeChanged(); };
        Month.ObservedValue.OnValueChanged += delegate { HandleTimeChanged(); };
        Year.ObservedValue.OnValueChanged += delegate { HandleTimeChanged(); };
    }

    private void OnDisable() {
        Minute.ObservedValue.OnValueChanged -= delegate { HandleTimeChanged(); };
        Hour.ObservedValue.OnValueChanged -= delegate { HandleTimeChanged(); };
        Day.ObservedValue.OnValueChanged -= delegate { HandleTimeChanged(); };
        Week.ObservedValue.OnValueChanged -= delegate { HandleTimeChanged(); };
        Month.ObservedValue.OnValueChanged -= delegate { HandleTimeChanged(); };
        Year.ObservedValue.OnValueChanged -= delegate { HandleTimeChanged(); };
    }

    private void Update() {
        if (Time.time > TimeTillNextTick) {
            Minute.AddValue(1);
            TimeTillNextTick = Time.time + tickRate;
            Debug.Log($"Minute: {Minute.Value}, Hour: {Hour.Value}, Day: {Day.Value}, Week: {Week.Value}, Month: {Month.Value}, Year: {Year.Value}");
        }
        // CurrentTime.Value.AddSeconds() += Time.deltaTime;
    }

    [Button]
    private void AddMinutes(int value = 120) {
        Minute.AddValue(value);
    }

    [Button]
    private void AddHours(int value = 72) {
        Hour.AddValue(value);
    }

    [Button]
    private void AddDays(int value = 25) {
        Day.AddValue(value);
    }

    private void HandleTimeChanged() {
        OnTimeChanged?.Invoke(
            Minute.Value,
            Hour.Value,
            Day.Value,
            Week.Value,
            Month.Value,
            Year.Value);
    }

    public static string AddOrdinal(int num)
    {
        if( num <= 0 ) return num.ToString();

        switch(num % 100)
        {
            case 11:
            case 12:
            case 13:
                return num + "th";
        }
    
        switch(num % 10)
        {
            case 1:
                return num + "st";
            case 2:
                return num + "nd";
            case 3:
                return num + "rd";
            default:
                return num + "th";
        }
    }
}