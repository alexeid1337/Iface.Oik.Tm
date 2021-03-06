using System.ComponentModel;
using Iface.Oik.Tm.Native.Interfaces;

namespace Iface.Oik.Tm.Interfaces
{
  public enum
    DeltaComponentTypes
  {
    Driver  = 0,
    Adapter = 1,
    Port    = 2,
    Rtu     = 3,
    Array   = 4,
    Block   = 5,
    Unknown = 6,
    Info    = 7
  }

  public enum DeltaItemTypes
  {
    Description = TmNativeDefs.DeltaItemTypes.Description,
    Status      = TmNativeDefs.DeltaItemTypes.Status,
    Analog      = TmNativeDefs.DeltaItemTypes.Analog,
    Accum       = TmNativeDefs.DeltaItemTypes.Accum,
    Control     = TmNativeDefs.DeltaItemTypes.Control,
    AnalogFloat = TmNativeDefs.DeltaItemTypes.AnalogF,
    AccumFloat  = TmNativeDefs.DeltaItemTypes.AccumF,
    StrVal      = TmNativeDefs.DeltaItemTypes.StrVal
  }


  public enum DeltaTraceTypes
  {
    Protocol = 0,
    Physical = 1,
    Logical  = 2
  }

  public enum DeltaTraceMessageTypes
  {
    Unknown = -1,
    Error   = TmNativeDefs.DntTraceMessageTypes.Error,
    Message = TmNativeDefs.DntTraceMessageTypes.Msg,
    Debug   = TmNativeDefs.DntTraceMessageTypes.Debug,
    In      = TmNativeDefs.DntTraceMessageTypes.TmIn,
    Out     = TmNativeDefs.DntTraceMessageTypes.TmOut,
    TmsIn   = TmNativeDefs.DntTraceMessageTypes.SIn,
    TmsOut  = TmNativeDefs.DntTraceMessageTypes.SOut,
  }

  public enum DeltaComponentStates
  {
    None                                      = -1,
    [Description("ОК")]          Ok           = 0,
    [Description("НЕТ ДИАГН.")]  NoDiags      = 1,
    [Description("НЕТ СВЯЗИ")]   Error        = 2,
    [Description("НЕ НАЙДЕН")]   NotFound     = 3,
    [Description("НЕ ПОДДЕРЖ.")] NotSupported = 4,
    [Description("???")]         Unknown      = 5
  }
}