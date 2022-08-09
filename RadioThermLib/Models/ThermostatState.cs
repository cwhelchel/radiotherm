
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace RadioThermLib.Models;

/// <summary>
/// The Thermostat State model as described by the Thermostat Resource API.
/// </summary>
/// <param name="Temperature">GET ONLY. Floating point representing current temperature in degrees Fahrenheit.</param>
/// <param name="ThermostatMode">Thermostat operating mode</param>
/// <param name="FanMode">Fan operating mode</param>
/// <param name="IsOverride">GET ONLY. Target temperature temporary override status. 1: Override is enabled. Note: Firmware versions prior to 1.04 can return any non-zero value if override is enabled.</param>
/// <param name="IsHold">Target temperature Hold status</param>
/// <param name="TemporaryCoolSetPoint">Temporary Target Heat set point: Sets target and <see cref="ThermostatMode"/> See remarks #</param>
/// <param name="TemporaryHeatSetPoint">Temporary Target Cool set point: Sets target and <see cref="ThermostatMode"/> See remarks #</param>
/// <param name="AbsoluteCoolSetPoint">Absolute Target Cool set point</param>
/// <param name="AbsoluteHeatSetPoint">Absolute Target Heat set point</param>
/// <param name="TemporaryCoolSetPointNoMode">Temporary Target Cool set point. Doesn't set <see cref="ThermostatMode"/></param>
/// <param name="TemporaryHeatSetPointNoMode">Temporary Target Heat set point. Doesn't set <see cref="ThermostatMode"/></param>
/// <param name="CurrentState">HVAC Operating State. Note: This functionality may not be available in all models of the thermostat.</param>
/// <param name="FanState">"GET ONLY. Fan Operating State. Note: Only available with CT-30"</param>
/// <param name="ThermostatTime">Thermostat’s internal representation of time</param>
/// <param name="TargetTempPost">GET ONLY. Target Temperature POST type. This attribute is deprecated and will be obsoleted in future versions of the API.#</param>
/// <remarks>
///     <p>
///         # - The default behavior of POST on t_heat and t_cool is to update the temporary target temperature. Some
///             custom flavors of the firmware update the absolute target temperature when t_heat or t_cool values are
///             updated.This distinction can be made by referring to the t_type_post attribute.This differing behavior is
///             deprecated and will be obsoleted in future versions of the API.No comment can be made about whether the
///             t_heat/t_cool data returned in a GET /tstat/ response indicates temporary or absolute target temperatures.
///     </p>
/// </remarks>
public sealed record ThermostatState(
    [property: JsonPropertyName("temp")] float Temperature,
    [property: JsonPropertyName("tmode")] ThermostatModeEnum ThermostatMode,
    [property: JsonPropertyName("fmode")] FanModeEnum FanMode,
    [property: JsonPropertyName("override")]
    int IsOverride,
    [property: JsonPropertyName("hold")] int IsHold,
    [property: JsonPropertyName("t_cool")] float TemporaryCoolSetPoint,
    [property: JsonPropertyName("t_heat")] float TemporaryHeatSetPoint,
    [property: JsonPropertyName("a_cool")] float AbsoluteCoolSetPoint,
    [property: JsonPropertyName("a_heat")] float AbsoluteHeatSetPoint,
    [property: JsonPropertyName("it_cool")]
    float TemporaryCoolSetPointNoMode,
    [property: JsonPropertyName("it_heat")]
    float TemporaryHeatSetPointNoMode,
    [property: JsonPropertyName("tstate")] ThermostatStateEnum CurrentState,
    [property: JsonPropertyName("fstate")] FanStateEnum FanState,
    [property: JsonPropertyName("time")] TimeObj ThermostatTime,
    [property: JsonPropertyName("t_type_post")]
    int TargetTempPost
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