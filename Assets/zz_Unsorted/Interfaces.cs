using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICalendarSignature
{
    public bool IsToday(int day, int month, int year);
}
