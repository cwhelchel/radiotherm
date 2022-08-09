using System.ComponentModel;
using System.Text.Json.Serialization;

namespace RadioThermLib.Models;


/// <summary>
/// The Thermostat Program model as described by the Thermostat Resource API.
/// </summary>
/// <remarks>
/// <p>Data Format: “d”:[a1, b1, a2, b2, a3, b3, …] </p>
/// <p>
///     Where: value of d is one of 0, 1, 2, 3, 4, 5, 6 representing mon, tue, wed, thu, fri, sat and sun respectively.
///     value of aX is the time expressed as minutes from the start of the day(Integer)
///     value of bX is the temperature at time aX expressed in degree Fahrenheit(Floating point)
/// </p>
/// <p>
///     The above is not entirely true. The values returned are only INTEGERS and not floating point numbers.
/// </p>
/// </remarks>
/// <param name="Monday"></param>
/// <param name="Tuesday"></param>
/// <param name="Wednesday"></param>
/// <param name="Thursday"></param>
/// <param name="Friday"></param>
/// <param name="Saturday"></param>
/// <param name="Sunday"></param>
public sealed record ThermostatProgram(
    [property: JsonPropertyName("0")] List<int> Monday,
    [property: JsonPropertyName("1")] List<int> Tuesday,
    [property: JsonPropertyName("2")] List<int> Wednesday,
    [property: JsonPropertyName("3")] List<int> Thursday,
    [property: JsonPropertyName("4")] List<int> Friday,
    [property: JsonPropertyName("5")] List<int> Saturday,
    [property: JsonPropertyName("6")] List<int> Sunday
)
{
    /// <summary>
    /// Get the data as a usable <see cref="DayProgram"/> object.
    /// </summary>
    /// <param name="day"></param>
    /// <returns></returns>
    public DayProgram? GetDayProgram(int day)
    {
        switch (day)
        {
            case 0:
                return new DayProgram(0, Monday);
            case 1:
                return new DayProgram(1, Tuesday);
            case 2:
                return new DayProgram(2, Wednesday);
            case 3:
                return new DayProgram(3, Thursday);
            case 4:
                return new DayProgram(4, Friday);
            case 5:
                return new DayProgram(5, Saturday);
            case 6:
                return new DayProgram(6, Sunday);
        }

        return null;
    }
}


public sealed class DayProgram
{
    private readonly int day;
    private readonly int[] mapped = { 1, 2, 3, 4, 5, 6, 0 };

    public DayOfWeek Day { get; }

    public List<TimeProgram> ProgramData { get; }

    public DayProgram(int day, List<int> data)
    {
        ArgumentNullException.ThrowIfNull(data);

        if ((data.Count % 2) != 0)
            throw new ArgumentException(strings.InvalidCountMsg, nameof(data));

        if (day < 0 || day > 6)
            throw new ArgumentOutOfRangeException(nameof(day), strings.DayValidRangeMsg);
        
        this.day = day;
        
        // thermostat API 0 = monday, DayOfWeek has monday = 1 (sunday = 0)
        int temp = mapped[day];
        Day = (DayOfWeek)temp;

        ProgramData = new List<TimeProgram>();

        for (int i = 0; i < data.Count; i+=2)
        {
            ProgramData.Add(new TimeProgram(data[i], data[i+1]));
        }
    }
}

public sealed class TimeProgram
{
    public TimeSpan Time { get; private set; }

    public float ProgrammedTemp { get; private set; }

    public TimeProgram(int time, int temp)
    {
        Time = new TimeSpan(0, 0, time, 0);
        ProgrammedTemp = temp;
    }
}

