
using System.Text.Json.Serialization;

namespace RadioThermLib.Models;


public sealed record ThermostatState(
    [property: JsonPropertyName("temp")] float Temperature,
    [property: JsonPropertyName("tmode")] ThermostatModeEnum ThermostatMode,
    [property: JsonPropertyName("fmode")] FanModeEnum FanMode,
    [property: JsonPropertyName("override")] int IsOverride,
    [property: JsonPropertyName("hold")] int IsHold,
    [property: JsonPropertyName("t_cool")] float TemporaryCoolSetPoint,
    [property: JsonPropertyName("t_heat")] float TemporaryHeatSetPoint,
    [property: JsonPropertyName("a_cool")] float AbsoluteCoolSetPoint,
    [property: JsonPropertyName("a_heat")] float AbsoluteHeatSetPoint,
    [property: JsonPropertyName("it_cool")] float TemporaryCoolSetPointNoMode,
    [property: JsonPropertyName("it_heat")] float TemporaryHeatSetPointNoMode,
    [property: JsonPropertyName("tstate")] ThermostatStateEnum CurrentState,
    [property: JsonPropertyName("fstate")] FanStateEnum FanState,
    [property: JsonPropertyName("time")] TimeObj ThermostatTime,
    [property: JsonPropertyName("t_type_post")] int TargetTempPost
    );

public static class Default
{
    public static ThermostatState EmptyState 
        = new ThermostatState(0.0f,
                              ThermostatModeEnum.Off,
                              FanModeEnum.Auto,
                              0,
                              0,
                              0.0f,
                              0.0f,
                              0.0f,
                              0.0f,
                              0.0f,
                              0.0f,
                              ThermostatStateEnum.Off,
                              FanStateEnum.Off,
                              new TimeObj { day = 0, hour = 0, minute = 0 },
                              0);
}

//public class ThermostatState
//{
//    public float temp { get; set; }
//    public ThermostatModeEnum tmode { get; set; }
//    public FanModeEnum fmode { get; set; }
//    [JsonPropertyName("override")]
//    public int _override { get; set; }
//    public int hold { get; set; }
//    public float t_cool { get; set; }
//    public float t_heat { get; set; }
//    public float a_cool { get; set; }
//    public float a_heat { get; set; }
//    public float it_cool { get; set; }
//    public float it_heat { get; set; }
//    public ThermostatStateEnum tstate { get; set; }
//    public FanStateEnum fstate { get; set; }
//    public TimeObj time { get; set; }
//    public int t_type_post { get; set; }

//    public string SetPointState
//    {
//        get
//        {
//            return (_override == 1 ? "Temporary" : "Programmed");
//        }
//    }
//}

public sealed class TimeObj
{
    // Day 0 = Monday
    public int day { get; set; }
    public int hour { get; set; }
    public int minute { get; set; }
}

public enum ThermostatModeEnum { Off, Heat, Cool, Auto }

public enum ThermostatStateEnum { Off, Heat, Cool }

public enum FanModeEnum { Auto, AutoCirculate, On }

public enum FanStateEnum { Off, On }