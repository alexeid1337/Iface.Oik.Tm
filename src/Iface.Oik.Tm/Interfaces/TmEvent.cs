﻿using System;
using System.Text;
using System.Text.RegularExpressions;
using Iface.Oik.Tm.Dto;
using Iface.Oik.Tm.Native.Interfaces;
using Iface.Oik.Tm.Utils;

namespace Iface.Oik.Tm.Interfaces
{
  public class TmEvent : TmNotifyPropertyChanged
  {
    private readonly int _hashCode;

    public TmEventElix  Elix                 { get; private set; } // may be null?
    public DateTime?    Time                 { get; private set; } // sql: update_time
    public string       Text                 { get; private set; } // sql: { name | rec_text }
    public string       StateString          { get; private set; } // sql: rec_state_text
    public string       ExplicitStateString  { get; private set; }
    public TmEventTypes Type                 { get; private set; } // sql: rec_type
    public string       TypeString           { get; private set; } // sql: { rec_type_name | tm_type_name }
    public string       ExplicitTypeString   { get; private set; }
    public string       Username             { get; private set; }
    public int          Importance           { get; private set; }
    public int          TmClassId            { get; private set; }
    public TmType       TmAddrType           { get; private set; }
    public uint         TmAddrComplexInteger { get; private set; } // sql: tma // one-based, 0=none
    public string       TmAddrString         { get; private set; }
    public object       Reference            { get; private set; }
    public DateTime?    FixTime              { get; private set; }

    private int       _num;
    private DateTime? _ackTime;
    private string    _ackUser;

    public int Num
    {
      get => _num;
      set
      {
        _num = value;
        NotifyOfPropertyChange();
      }
    }

    public DateTime? AckTime
    {
      get => _ackTime;
      set
      {
        _ackTime = value;
        NotifyOfPropertyChange();
        NotifyOfPropertyChange(nameof(IsAcked));
      }
    }

    public string AckUser
    {
      get => _ackUser;
      set
      {
        _ackUser = value;
        NotifyOfPropertyChange();
      }
    }

    public string             ImportanceAlias => ImportanceToAlias(Importance);
    public string             ImportanceName  => ImportanceToName(Importance);
    public TmEventImportances ImportanceFlag  => ImportanceToFlag(Importance);
    public bool               HasTmAddr       => TmAddrType != 0;
    public bool               HasTmStatus     => TmAddrType == TmType.Status;
    public bool               HasTmAnalog     => TmAddrType == TmType.Analog;
    public bool               IsAcked         => AckTime    != null; // TODO

    public float? AlarmInitialValue => Type == TmEventTypes.Alarm && Reference != null
      ? (float?) Reference
      : null;


    public TmEvent(int hashCode)
    {
      _hashCode = hashCode;
    }


    public static TmEvent CreateFromDto(TmEventDto dto)
    {
      return CreateFromDto(dto.Elix,
                           dto.UpdateTime,
                           dto.RecText,
                           dto.Name,
                           dto.RecStateText,
                           dto.RecType,
                           dto.RecTypeName,
                           dto.UserName,
                           dto.Importance,
                           dto.Tma,
                           dto.TmaStr,
                           dto.TmTypeName,
                           dto.TmType,
                           dto.ClassId,
                           dto.AlarmActive,
                           dto.VVal,
                           dto.AckTime,
                           dto.AckUser);
    }


    public static TmEvent CreateFromDto(byte[]    elixBytes,
                                        DateTime? time,
                                        string    text,
                                        string    name,
                                        string    stateString,
                                        short     type,
                                        string    typeString,
                                        string    username,
                                        short     importance,
                                        int       tmAddrComplexInteger, // one-based, 0=none
                                        string    tmAddrString,
                                        string    tmTypeString,
                                        short?    tmAddrNativeType,
                                        short?    classId,
                                        bool?     isAlarmActive,
                                        float?    alarmInitialValue,
                                        DateTime? ackTime,
                                        string    ackUser)
    {
      var eventType = (TmEventTypes) type;

      var eventTypeString = eventType == TmEventTypes.StatusChange    ||
                            eventType == TmEventTypes.ManualStatusSet ||
                            eventType == TmEventTypes.Alarm
        ? tmTypeString
        : typeString;

      var eventText = eventType == TmEventTypes.Extended
        ? text
        : name;

      var reference = eventType     == TmEventTypes.Alarm && 
                      isAlarmActive == true
        ? alarmInitialValue
        : null;

        var tmEvent = new TmEvent(elixBytes != null ? BitConverter.ToInt32(elixBytes, 8) : 0)
                    {
                      Time                 = time.NullIfEpoch(),
                      Text                 = eventText,
                      StateString          = stateString?.Trim(),
                      Type                 = eventType,
                      TypeString           = eventTypeString,
                      Username             = username,
                      Importance           = importance,
                      TmClassId            = classId ?? -1,
                      TmAddrType           = ((TmNativeDefs.TmDataTypes) (ushort) (tmAddrNativeType ?? 0)).ToTmType(),
                      TmAddrComplexInteger = (uint) tmAddrComplexInteger,
                      AckTime              = ackTime.NullIfEpoch(),
                      AckUser              = ackUser,
                      Reference            = reference,
                    };

      if (elixBytes != null)
      {
        tmEvent.Elix = TmEventElix.CreateFromByteArray(elixBytes);
      }

      if (tmAddrComplexInteger != 0)
      {
        tmEvent.TmAddrString = tmAddrString;
        if (false)
        {
          TmAddr.TryParseType(tmAddrString, out var tmType, TmType.Unknown);
          tmEvent.TmAddrType = tmType;
        }
      }
      else
      {
        tmEvent.TmAddrString = null;
        tmEvent.TmAddrType   = TmType.Unknown;
      }

      return tmEvent;
    }


    public static TmEvent CreateAlarmTmEvent(TmNativeDefs.TEventElix       tEventElix,
                                             TmNativeDefs.TTMSEventAddData eventAddData,
                                             string                        alarmTypeName,
                                             TmAnalog                      sourceAnalog,
                                             TmNativeDefs.AlarmData        data)
    {
      var alarmEvent = CreateFromTEventElix(tEventElix, eventAddData, sourceAnalog.Name);

      alarmEvent.TmAddrString = $"#TT{tEventElix.Event.Ch}:{tEventElix.Event.Rtu}:{tEventElix.Event.Point}";
      alarmEvent.TmAddrComplexInteger =
        (uint) (tEventElix.Event.Point + (tEventElix.Event.Rtu << 16) + (tEventElix.Event.Ch << 24));
      alarmEvent.TmAddrType = TmType.Analog;

      alarmEvent.TypeString         = alarmTypeName;
      alarmEvent.ExplicitTypeString = "Уставка";

      if (data.State > 0)
      {
        alarmEvent.StateString = "Взведена";
        alarmEvent.Reference   = data.Val;
      }
      else
      {
        alarmEvent.StateString = "Снята";
      }
      alarmEvent.ExplicitStateString = $"{data.Val} - {alarmEvent.StateString}";

      return alarmEvent;
    }


    public static TmEvent CreateManualAnalogSetEvent(TmNativeDefs.TEventElix       tEventElix,
                                                     TmNativeDefs.TTMSEventAddData eventAddData,
                                                     TmAnalog                      setAnalog,
                                                     TmNativeDefs.AnalogSetData    analogSetData)
    {
      var manualAnalogSetEvent = CreateFromTEventElix(tEventElix, eventAddData, setAnalog.Name);

      manualAnalogSetEvent.TmAddrString = $"#TT{tEventElix.Event.Ch}:{tEventElix.Event.Rtu}:{tEventElix.Event.Point}";
      manualAnalogSetEvent.TmAddrComplexInteger =
        (uint) (tEventElix.Event.Point + (tEventElix.Event.Rtu << 16) + (tEventElix.Event.Ch << 24));
      manualAnalogSetEvent.TmAddrType = TmType.Analog;

      manualAnalogSetEvent.TypeString         = "Ручн. ТИТ";
      manualAnalogSetEvent.ExplicitTypeString = "Ручн. ТИТ";

      manualAnalogSetEvent.Username = EncodingUtil.Cp866BytesToUtf8String(analogSetData.UserName);

      manualAnalogSetEvent.StateString = 
        $"{analogSetData.Value.ToString($"N{setAnalog.Precision}")}{(setAnalog.Unit.IsNullOrEmpty() ? "" : $" {setAnalog.Unit}")}";
      manualAnalogSetEvent.ExplicitStateString =
        $"{analogSetData.Value} - {(analogSetData.Cmd == 0 ? "Снято" : "Установлено")}";

      return manualAnalogSetEvent;
    }


    public static TmEvent CreateManualStatusSetEvent(TmNativeDefs.TEventElix       tEventElix,
                                                     TmNativeDefs.TTMSEventAddData eventAddData,
                                                     TmStatus                      setStatus,
                                                     TmNativeDefs.ControlData      mSData)
    {
      var manualStatusSetEvent = CreateFromTEventElix(tEventElix, eventAddData, setStatus.Name);

      manualStatusSetEvent.TmAddrString = $"#TС{tEventElix.Event.Ch}:{tEventElix.Event.Rtu}:{tEventElix.Event.Point}";
      manualStatusSetEvent.TmAddrComplexInteger =
        (uint) (tEventElix.Event.Point + (tEventElix.Event.Rtu << 16) + (tEventElix.Event.Ch << 24));
      manualStatusSetEvent.TmAddrType = TmType.Status;

      manualStatusSetEvent.Username = EncodingUtil.Cp866BytesToUtf8String(mSData.UserName);


      manualStatusSetEvent.ExplicitTypeString  = "Ручн. ТС";
      manualStatusSetEvent.ExplicitStateString = mSData.Cmd == 1 ? "ВКЛ" : "ОТКЛ";

      if (mSData.Ch == -1)
      {
        switch (mSData.Rtu)
        {
          case 1:
            manualStatusSetEvent.TypeString  = setStatus.Flag1Name;
            manualStatusSetEvent.StateString = mSData.Cmd == 1 ? setStatus.CaptionFlag1On : setStatus.CaptionFlag1Off;
            break;
          case 2:
            manualStatusSetEvent.TypeString  = setStatus.Flag2Name;
            manualStatusSetEvent.StateString = mSData.Cmd == 1 ? setStatus.CaptionFlag2On : setStatus.CaptionFlag2Off;
            break;
          case 3:
            manualStatusSetEvent.TypeString  = setStatus.Flag3Name;
            manualStatusSetEvent.StateString = mSData.Cmd == 1 ? setStatus.CaptionFlag3On : setStatus.CaptionFlag3Off;
            break;
          case 4:
            manualStatusSetEvent.TypeString  = setStatus.Flag4Name;
            manualStatusSetEvent.StateString = mSData.Cmd == 1 ? setStatus.CaptionFlag4On : setStatus.CaptionFlag4Off;
            break;
        }
      }
      else
      {
        manualStatusSetEvent.TypeString  = "Ручн. ТС";
        manualStatusSetEvent.StateString = mSData.Cmd == 1 ? setStatus.CaptionOn : setStatus.CaptionOff;
      }


      return manualStatusSetEvent;
    }


    public static TmEvent CreateAcknowledgeEvent(TmNativeDefs.TEventElix       tEventElix,
                                                 TmNativeDefs.TTMSEventAddData eventAddData,
                                                 string                        sourceObjectName,
                                                 TmNativeDefs.AcknowledgeData  acknowledgeData)
    {
      var acknowledgeEvent = CreateFromTEventElix(tEventElix, eventAddData, sourceObjectName);

      acknowledgeEvent.TmAddrComplexInteger =
        (uint) (tEventElix.Event.Point + (tEventElix.Event.Rtu << 16) + (tEventElix.Event.Ch << 24));

      acknowledgeEvent.TmAddrType = ((TmNativeDefs.TmDataTypes) acknowledgeData.TmType).ToTmType();

      switch (acknowledgeEvent.TmAddrType)
      {
        case TmType.Status:
          if (tEventElix.Event.Point == 0)
          {
            acknowledgeEvent.Text = "Общее квитирование ТС";
          }
          else
          {
            acknowledgeEvent.TmAddrString = $"#TC{tEventElix.Event.Ch}:{tEventElix.Event.Rtu}:{tEventElix.Event.Point}";
          }

          break;
        case TmType.Analog:
          
          if (tEventElix.Event.Point == 0)
          {
            acknowledgeEvent.Text = "Общее квитирование ТИ";
          }
          else
          {
            acknowledgeEvent.TmAddrString = $"#TT{tEventElix.Event.Ch}:{tEventElix.Event.Rtu}:{tEventElix.Event.Point}";
          }
          break;
        default:
          if (tEventElix.Event.Point == 0)
          {
            acknowledgeEvent.Text = "Общее квитирование";
          }

          break;
      }

      acknowledgeEvent.TypeString         = "Квитирование";
      acknowledgeEvent.ExplicitTypeString = "Квитирование";
      acknowledgeEvent.Username           = EncodingUtil.Cp866BytesToUtf8String(acknowledgeData.UserName);

      return acknowledgeEvent;
    }


    public static TmEvent CreateControlEvent(TmNativeDefs.TEventElix       tEventElix,
                                             TmNativeDefs.TTMSEventAddData eventAddData,
                                             TmStatus                      controlStatus,
                                             TmNativeDefs.ControlData      controlData)
    {
      var controlEvent = CreateFromTEventElix(tEventElix, eventAddData, controlStatus.Name);

      controlEvent.TmAddrString = $"#TC{tEventElix.Event.Ch}:{tEventElix.Event.Rtu}:{tEventElix.Event.Point}";
      controlEvent.TmAddrComplexInteger =
        (uint) (tEventElix.Event.Point + (tEventElix.Event.Rtu << 16) + (tEventElix.Event.Ch << 24));
      controlEvent.TmAddrType = TmType.Status;

      controlEvent.TypeString         = "ТУ";
      controlEvent.ExplicitTypeString = "ТУ";

      controlEvent.Username = EncodingUtil.Cp866BytesToUtf8String(controlData.UserName);

      var result = (TmTelecontrolResult) unchecked((sbyte) controlData.Result);

      if (result != TmTelecontrolResult.Success)
      {
        controlEvent.ExplicitStateString = $"{(controlData.Cmd == 1 ? "ВКЛ" : "ОТКЛ")} ОШИБКА {(int) result}";
        controlEvent.StateString         = result.GetDescription();
      }
      else
      {
        controlEvent.ExplicitStateString = controlData.Cmd == 1 ? "ВКЛ" : "ОТКЛ";
        controlEvent.StateString = $"Команда {(controlData.Cmd == 1 ? controlStatus.CaptionOn : controlStatus.CaptionOff)}";
      }

      return controlEvent;
    }


    public static TmEvent CreateExtendedEvent(TmNativeDefs.TEventElix       tEventElix,
                                              TmNativeDefs.TTMSEventAddData eventAddData,
                                              TmNativeDefs.StrBinData       strBinData)
    {
      var extendedEvent = CreateFromTEventElix(tEventElix, eventAddData, "");

      var extendedType = (TmNativeDefs.ExtendedEventTypes) tEventElix.Event.Ch;

      extendedEvent.TmAddrString         = null;
      extendedEvent.TmAddrComplexInteger = 0;
      extendedEvent.TmAddrType           = TmType.Unknown;

      extendedEvent.TypeString         = GetTypeStringByExtendedTypeString(extendedType);
      extendedEvent.ExplicitTypeString = GetTypeStringByExtendedTypeString(extendedType);
      (extendedEvent.Text, extendedEvent.Username) = GetMessageAndUserFromStrBinBytes(strBinData.StrBin);

      switch (extendedType)
      {
        case TmNativeDefs.ExtendedEventTypes.Message:
          if (strBinData.Source < 0x10000)
          {
            
            extendedEvent.Reference    = $"Источник: {strBinData.Source}";
          }
          else
          {
            extendedEvent.TmAddrString =
              $"#XX{(strBinData.Source & 0xff00_0000) >> 24}:{(strBinData.Source & 0x00ff_0000) >> 16}:0";
            extendedEvent.Reference =
              $"Ист: {strBinData.Source & 0x0000_ffff}, "        +
              $"К: {(strBinData.Source  & 0xff00_0000) >> 24}, " +
              $"КП: {(strBinData.Source & 0x00ff_0000) >> 16}";
          }

          break;
        case TmNativeDefs.ExtendedEventTypes.Model:
          extendedEvent.Reference    = $"Источник: {strBinData.Source}";
          break;
        default:
          extendedEvent.Reference    = "???";
          break;
      }

      return extendedEvent;
    }


    public static TmEvent CreateStatusChangeEvent(TmNativeDefs.TEventElix       tEventElix,
                                                  TmNativeDefs.TTMSEventAddData eventAddData,
                                                  TmStatus                      changedStatus,
                                                  TmNativeDefs.StatusData       statusData)
    {
      var statusChangeEvent = CreateStatusChangeEvent(tEventElix, eventAddData, changedStatus, statusData.State, 
                                                      statusData.Class, statusData.ExtSig, statusData.FixUT, 
                                                      statusData.FixMS, statusData.S2, statusData.Flags);
      
      return statusChangeEvent;
    }


    public static TmEvent CreateStatusChangeExtendedEvent(TmNativeDefs.TEventElix       tEventElix,
                                                    TmNativeDefs.TTMSEventAddData eventAddData,
                                                    TmStatus                      changedStatus,
                                                    TmNativeDefs.StatusDataEx     statusDataEx)
    {
      var statusChangeExtendedEvent = CreateStatusChangeEvent(tEventElix,       eventAddData,      changedStatus, statusDataEx.State, 
                                                                    statusDataEx.Class, statusDataEx.ExtSig, statusDataEx.FixUT, 
                                                                    statusDataEx.FixMS, statusDataEx.S2, statusDataEx.Flags, statusDataEx.OldFlags);

      statusChangeExtendedEvent.Username = EncodingUtil.Win1251BytesToUtf8(statusDataEx.UserName);
      return statusChangeExtendedEvent;
    }

    private static TmEvent CreateStatusChangeEvent(TmNativeDefs.TEventElix       tEventElix,
                                                   TmNativeDefs.TTMSEventAddData eventAddData,
                                                   TmStatus                      changedStatus,
                                                   byte state,
                                                   byte                          statusClass,
                                                   UInt32                        extSig, 
                                                   UInt32 fixUt, 
                                                   UInt16 fixMs,
                                                   UInt16                        s2,     
                                                   UInt32 flags,
                                                   UInt32?                       oldFlags = null)
    {
      var statusChangeEvent = CreateFromTEventElix(tEventElix, eventAddData, changedStatus.Name);
      
      var                  isS2Only = false;
      TmNativeDefs.S2Flags s2Flags       = 0x0000;

      statusChangeEvent.TmAddrString = $"#TC{tEventElix.Event.Ch}:{tEventElix.Event.Rtu}:{tEventElix.Event.Point}";
      statusChangeEvent.TmAddrComplexInteger =
        (uint) (tEventElix.Event.Point + (tEventElix.Event.Rtu << 16) + (tEventElix.Event.Ch << 24));
      statusChangeEvent.TmAddrType = TmType.Status;
      
      statusChangeEvent.TypeString = changedStatus.ClassName.IsNullOrEmpty() ? $"{(changedStatus.IsAps ? "АПС" : "ТС")}" : changedStatus.ClassName;
      statusChangeEvent.ExplicitTypeString = statusClass == 1 ? "АПС" : "ТС";

      var hasFixTime = false;
      var hasTmFlags = false;;
      var hasSecondary = false;
      var hasNoCurData = false;
      var hasS2 = false;
      if ((extSig & TmNativeDefs.ExtendedDataSignature) == TmNativeDefs.ExtendedDataSignature)
      {
        var extSigFlags = (TmNativeDefs.ExtendedDataSignatureFlag) (extSig ^
                                                                    TmNativeDefs.ExtendedDataSignature);
        hasFixTime = extSigFlags.HasFlag(TmNativeDefs.ExtendedDataSignatureFlag.FixTime);
        hasTmFlags = extSigFlags.HasFlag(TmNativeDefs.ExtendedDataSignatureFlag.TmFlags);
        hasSecondary = extSigFlags.HasFlag(TmNativeDefs.ExtendedDataSignatureFlag.Secondary);
        hasNoCurData = !extSigFlags.HasFlag(TmNativeDefs.ExtendedDataSignatureFlag.CurData);
        hasS2 = extSigFlags.HasFlag(TmNativeDefs.ExtendedDataSignatureFlag.S2);
      }
      
      
      if (hasFixTime)
      {
        statusChangeEvent.FixTime = DateUtil.GetDateTimeFromTimestamp(fixUt, fixMs);
      }
      
      if (oldFlags == null)
      {
        statusChangeEvent.Reference = hasTmFlags ? $"F=${flags:X8}" : "";
      }
      else
      {
        statusChangeEvent.Reference = oldFlags == flags ? $"F=${flags:X8}" : $"F=${oldFlags:X8} -> ${flags:X8}";
      }
      
      if (hasSecondary)
      {
        statusChangeEvent.Reference = statusChangeEvent.Reference == null ? "(s)" : $"{statusChangeEvent.Reference} (s)";
      }

      if (hasNoCurData)
      {
        statusChangeEvent.Reference = statusChangeEvent.Reference == null ? "(d)" : $"{statusChangeEvent.Reference} (d)";
      }

      if (hasS2)
      {
        isS2Only = (s2                        & 0x8000) != 0;
        s2Flags  = (TmNativeDefs.S2Flags) (s2 & 0x7fff);
      }


      if (s2Flags == 0)
      {
        statusChangeEvent.StateString = state == 1 ? changedStatus.CaptionOn : changedStatus.CaptionOff;
        statusChangeEvent.ExplicitStateString = isS2Only ? "ИЗМ. АТРИБУТОВ - НОРМА" 
                                                  : $"{(state == 1 ? "ВКЛ" : "ОТКЛ")}";
      }
      else
      {
        statusChangeEvent.StateString = GetS2StatusString(s2Flags, changedStatus);
        statusChangeEvent.ExplicitStateString = $"{(isS2Only ? "ИЗМ. АТРИБУТОВ" : $"{(state == 1 ? "ВКЛ" : "ОТКЛ")}")} {GetS2StatusString(s2Flags)}";

      }

      return statusChangeEvent;
    }


    public static TmEvent CreateStatusFlagsChangeEvent(TmNativeDefs.TEventElix            tEventElix,
                                                 TmNativeDefs.TTMSEventAddData      eventAddData,
                                                 TmStatus                           sourceStatus,
                                                 TmNativeDefs.FlagsChangeDataStatus flagsChangeDataStatus)
    {
      var flagsChangeEvent = CreateFlagsChangeEvent(tEventElix, 
                                                    eventAddData, 
                                                    sourceStatus.Name, 
                                                    TmType.Status, 
                                                    flagsChangeDataStatus.OldFlags, 
                                                    flagsChangeDataStatus.NewFlags,
                                                    EncodingUtil.Win1251BytesToUtf8(flagsChangeDataStatus.UserName));
      
      flagsChangeEvent.TypeString = $"Изм. флагов {(sourceStatus.ClassName.IsNullOrEmpty() ? $"{(sourceStatus.IsAps ? "АПС" : "ТС")}" : sourceStatus.ClassName)}";


      return flagsChangeEvent;
    }


    public static TmEvent CreateAnalogFlagsChangeEvent(TmNativeDefs.TEventElix            tEventElix,
                                                       TmNativeDefs.TTMSEventAddData      eventAddData,
                                                       TmAnalog                          sourceAnalog,
                                                       TmNativeDefs.FlagsChangeDataAnalog flagsChangeDataAnalog)
    {
      var flagsChangeEvent = CreateFlagsChangeEvent(tEventElix, 
                                                    eventAddData, 
                                                    sourceAnalog.Name, 
                                                    TmType.Analog, 
                                                    flagsChangeDataAnalog.OldFlags, 
                                                    flagsChangeDataAnalog.NewFlags,
                                                    EncodingUtil.Win1251BytesToUtf8(flagsChangeDataAnalog.UserName));

      flagsChangeEvent.TypeString = "Изм. флагов ТИ";

      return flagsChangeEvent;
    }


    public static TmEvent CreateAccumFlagsChangeEvent(TmNativeDefs.TEventElix       tEventElix,
                                                     TmNativeDefs.TTMSEventAddData eventAddData,
                                                     string                        sourceObjectName, 
                                                     TmNativeDefs.FlagsChangeData flagsChangeData)
    {
      var flagsChangeEvent = CreateFlagsChangeEvent(tEventElix, 
                                                    eventAddData, 
                                                    sourceObjectName, 
                                                    TmType.Accum, 
                                                    flagsChangeData.OldFlags, 
                                                    flagsChangeData.NewFlags,
                                                    "");
      
      flagsChangeEvent.TypeString = "Изм. флагов ТИИ";

      return flagsChangeEvent;
    }
    

    private static TmEvent CreateFlagsChangeEvent(TmNativeDefs.TEventElix       tEventElix,
                                                 TmNativeDefs.TTMSEventAddData eventAddData,   
                                                 string sourceObjectName,
                                                 TmType sourceDataType, 
                                                 uint   oldFlags,
                                                 uint                          newFlags, 
                                                 string username) 
    {
      var flagsChangeEvent = CreateFromTEventElix(tEventElix, eventAddData, sourceObjectName);
      
      flagsChangeEvent.TmAddrComplexInteger =
        (uint) (tEventElix.Event.Point + (tEventElix.Event.Rtu << 16) + (tEventElix.Event.Ch << 24));
      flagsChangeEvent.Username = username;

      switch (sourceDataType)
      {
        case TmType.Status:
          flagsChangeEvent.TmAddrString = $"#TC{tEventElix.Event.Ch}:{tEventElix.Event.Rtu}:{tEventElix.Event.Point}";
          flagsChangeEvent.ExplicitTypeString = "Изм. флагов ТС";
          flagsChangeEvent.TmAddrType = TmType.Status;
          break;
        case TmType.Analog:
          flagsChangeEvent.TmAddrString = $"#TТ{tEventElix.Event.Ch}:{tEventElix.Event.Rtu}:{tEventElix.Event.Point}";
          flagsChangeEvent.ExplicitTypeString = "Изм. флагов ТИТ";
          flagsChangeEvent.TmAddrType = TmType.Analog;
          break;
        case TmType.Accum:
          flagsChangeEvent.TmAddrString = $"#TИ{tEventElix.Event.Ch}:{tEventElix.Event.Rtu}:{tEventElix.Event.Point}";
          flagsChangeEvent.ExplicitTypeString = "Изм. флагов ТИИ";
          flagsChangeEvent.TmAddrType = TmType.Accum;
          break;
      }
      
      
      flagsChangeEvent.ExplicitStateString = $"${oldFlags:X8}X -> ${newFlags:X8}X";
      flagsChangeEvent.StateString         = $"${oldFlags:X8}X -> ${newFlags:X8}X";
      
      return flagsChangeEvent;
    }
    

    public static TmEvent CreateFromTEventElix(TmNativeDefs.TEventElix       tEventElix,
                                               TmNativeDefs.TTMSEventAddData eventAddData,
                                               string                        sourceObjectName)
    {
      var tmEventElix = new TmEventElix(tEventElix.Elix.R, tEventElix.Elix.M);
      var tmEventType = (TmEventTypes) tEventElix.Event.Id;

      var tmEvent = new TmEvent(BitConverter.ToInt32(tmEventElix.ToByteArray(), 8))
                    {
                      Elix = tmEventElix,
                      Time =
                        DateUtil.GetDateTime(System.Text.Encoding.Default.GetString(tEventElix.Event.DateTime)),
                      Type       = tmEventType,
                      Importance = tEventElix.Event.Imp,
                      Text       = sourceObjectName,
                    };

      if (eventAddData.AckSec != 0 && !eventAddData.UserName.IsNullOrEmpty())
      {
        tmEvent.AckTime = DateUtil.GetDateTimeFromTimestamp(eventAddData.AckSec, eventAddData.AckMs);
        tmEvent.AckUser = eventAddData.UserName;
      }

      return tmEvent;
    }


    public override int GetHashCode()
    {
      return _hashCode != 0
               ? _hashCode
               : base.GetHashCode();
    }


    public static TmEventImportances ImportanceToFlag(int importance)
    {
      return (TmEventImportances) (1 << importance);
    }


    public static string ImportanceToAlias(int importance)
    {
      switch (importance)
      {
        case 0:
          return "ОС";
        case 1:
          return "ПС2";
        case 2:
          return "ПС1";
        case 3:
          return "АС";
        default:
          return string.Empty;
      }
    }


    public static string ImportanceToName(int importance)
    {
      switch (importance)
      {
        case 0:
          return "Оперативного состояния";
        case 1:
          return "Предупредительные 2";
        case 2:
          return "Предупредительные 1";
        case 3:
          return "Аварийные";
        default:
          return string.Empty;
      }
    }


    private static string GetS2StatusString(TmNativeDefs.S2Flags s2Flag, TmStatus status = null)
    {
      if (s2Flag.HasFlag(TmNativeDefs.S2Flags.Break))
      {
        return status == null ? $"[{s2Flag:D}] <ОБРЫВ>" : status.CaptionBreak;
      }
      if (s2Flag.HasFlag(TmNativeDefs.S2Flags.Malfunction))
      {
        return  status == null ? $"[{s2Flag:D}] <НЕИСП.>" : status.CaptionMalfunction;
      }

      return $"[{s2Flag:D}]";
    }


    private static string GetTypeStringByExtendedTypeString(TmNativeDefs.ExtendedEventTypes eventType)
    {
      switch (eventType)
      {
        case TmNativeDefs.ExtendedEventTypes.Message:
          return "Сообщение";
        case TmNativeDefs.ExtendedEventTypes.Model:
          return "Модель";
        default:
          return "???";
      }
    }
    
    
    public static (string, string) GetMessageAndUserFromStrBinBytes(byte[] bytes)
    {
      var str = Encoding.GetEncoding(1251)
                        .GetString(bytes);
      
      var regex = new Regex(@"(.*?)\0(.*?)\0");
      var mc    = regex.Match(str);
      var text  = mc.Groups[1].Value;
      var user  = mc.Groups[2].Value;

      return (text, user);
    }
  }
}