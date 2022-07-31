
using System.ComponentModel;
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
    [property: JsonPropertyName("tstate"), Description(ThermostatState.ObsoleteMsg)] ThermostatStateEnum CurrentState,
    [property: JsonPropertyName("fstate"), Description("Note: Only available with CT-30")] FanStateEnum FanState,
    [property: JsonPropertyName("time")] TimeObj ThermostatTime,
    [property: JsonPropertyName("t_type_post")] int TargetTempPost
    )
{
    private const string ObsoleteMsg = "Note: This functionality may not be available in all models of the thermostat.";
}

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