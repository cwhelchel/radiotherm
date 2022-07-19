
using System.Text.Json.Serialization;

public class ThermostatObj
{
    public float temp { get; set; }
    public ThermostatMode tmode { get; set; }
    public FanMode fmode { get; set; }
    [JsonPropertyName("override")]
    public int _override { get; set; }
    public int hold { get; set; }
    public float t_cool { get; set; }
    public float t_heat { get; set; }
    public float a_cool { get; set; }
    public float a_heat { get; set; }
    public float it_cool { get; set; }
    public float it_heat { get; set; }
    public ThermostatState tstate { get; set; }
    public FanState fstate { get; set; }
    public TimeObj time { get; set; }
    public int t_type_post { get; set; }

    public string SetPointState
    {
        get
        {
            return (_override == 1 ? "Temporary" : "Programmed");
        }
    }
}

public class TimeObj
{
    // Day 0 = Monday
    public int day { get; set; }
    public int hour { get; set; }
    public int minute { get; set; }
}

public enum ThermostatMode { Off, Heat, Cool, Auto }

public enum ThermostatState { Off, Heat, Cool }

public enum FanMode { Auto, AutoCirculate, On }

public enum FanState { Off, On }