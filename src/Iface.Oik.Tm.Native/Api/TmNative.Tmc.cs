using System;
using System.Text;
using Iface.Oik.Tm.Native.Interfaces;

namespace Iface.Oik.Tm.Native.Api
{
  public partial class TmNative : ITmNative
  {
    public Int32 TmcConnect(string           server,
                            string           pipe,
                            string           user,
                            TmNativeCallback callback,
                            IntPtr           callbackParameter)
    {
      return tmcConnect(server, pipe, user, callback, callbackParameter);
    }


    public void TmcDisconnect(Int32 cid)
    {
      tmcDisconnect(cid);
    }


    public UInt32 TmcIsConnected(Int32 cid)
    {
      return tmcIsConnected(cid);
    }


    public void TmcUpdateConnection(Int32 cid)
    {
      tmcUpdateConnection(cid);
    }


    public bool TmcGetCurrentServer(int        cid,
                                    ref byte[] machineBuf,
                                    uint       cbMachine,
                                    ref byte[] pipeBuf,
                                    uint       cbPipe)
    {
      return tmcGetCurrentServer(cid, machineBuf, cbMachine, pipeBuf, cbPipe);
    }


    public void TmcFreeMemory(IntPtr memory)
    {
      tmcFreeMemory(memory);
    }


    public UInt32 TmcGetLastError()
    {
      return tmcGetLastError();
    }


    public Int32 TmcGetLastErrorText(Int32  cid,
                                     IntPtr buf)
    {
      return tmcGetLastErrorText(cid, buf);
    }


    public UInt32 TmcSetDgrmFlags(Int32  cid,
                                  UInt32 flags)
    {
      return tmcSetDgrmFlags(cid, flags);
    }


    public UInt32 TmcClrDgrmFlags(Int32  cid,
                                  UInt32 flags)
    {
      return tmcClrDgrmFlags(cid, flags);
    }


    public Int16 TmcSetFeedbackItems(Int32  cid,
                                     UInt16 type,
                                     Int16  ch,
                                     Int16  rtu,
                                     Int16  point,
                                     Byte   count,
                                     Byte   fbType,
                                     UInt32 id)
    {
      return tmcSetFeedbackItems(cid, type, ch, rtu, point, count, fbType, id);
    }


    public Int16 TmcClrFeedback(Int32 cid)
    {
      return tmcClrFeedback(cid);
    }


    public IntPtr TmcGetCfsHandle(int cid)
    {
      return tmcGetCfsHandle(cid);
    }


    public UInt32 TmcReconnectCount(Int32 cid)
    {
      return tmcReconnectCount(cid);
    }


    public UInt32 TmcConnState(Int32 cid)
    {
      return tmcConnState(cid);
    }


    public UInt32 TmcGetServerCaps(Int32      cid,
                                   ref Byte[] pCaps)
    {
      return tmcGetServerCaps(cid, pCaps);
    }


    public Int32 TmcGetServerFeature(Int32  cid,
                                     UInt32 dwCode)
    {
      return tmcGetServerFeature(cid, dwCode);
    }


    public short TmcSystemTime(int        cid,
                               ref byte[] timeBuf,
                               IntPtr     tmStruct)
    {
      return tmcSystemTime(cid, timeBuf, tmStruct);
    }


    public Boolean TmcGetUserInfo(Int32                      cid,
                                  UInt32                     usid,
                                  ref TmNativeDefs.TUserInfo userInfo)
    {
      return tmcGetUserInfo(cid, usid, ref userInfo);
    }


    public bool TmcGetUserInfoEx(int                        cid,
                                 uint                       userId,
                                 ref TmNativeDefs.TUserInfo userInfo,
                                 ref byte[]                 appxBuf,
                                 uint                       cbAppx)
    {
      return tmcGetUserInfoEx(cid, userId, ref userInfo, appxBuf, cbAppx);
    }


    public IntPtr TmcGetUserList(Int32 cid)
    {
      return tmcGetUserList(cid);
    }


    public Int16 TmcStatus(Int32 cid,
                           Int16 ch,
                           Int16 rtu,
                           Int16 point)
    {
      return tmcStatus(cid, ch, rtu, point);
    }


    public Single TmcAnalog(Int32  cid,
                            Int16  ch,
                            Int16  rtu,
                            Int16  point,
                            string dateTime,
                            Int16  retroNum)
    {
      return tmcAnalog(cid, ch, rtu, point, dateTime, retroNum);
    }

    public Int16 TmcAnalogFull(Int32                         cid,
                               Int16                         ch,
                               Int16                         rtu,
                               Int16                         point,
                               ref TmNativeDefs.TAnalogPoint analogPoint,
                               string                        dateTime,
                               Int16                         retroNum)
    {
      return tmcAnalogFull(cid, ch, rtu, point, ref analogPoint, dateTime, retroNum);
    }
    

    public IntPtr TmcTmValuesByListEx(Int32                 cid,
                                      UInt16                tmType,
                                      Byte                  qFlags,
                                      UInt32                count,
                                      TmNativeDefs.TAdrTm[] addr)
    {
      return tmcTMValuesByListEx(cid, tmType, qFlags, count, addr);
    }


    public IntPtr TmcGetValuesByFlagMask(Int32      cid,
                                         UInt16     tmType,
                                         UInt32     tmFlags,
                                         Byte       qFlags,
                                         out UInt32 pCount)
    {
      return tmcGetValuesByFlagMask(cid, tmType, tmFlags, qFlags, out pCount);
    }


    public Int16 TmcRegEvent(Int32               cid,
                             TmNativeDefs.TEvent tmEvent)
    {
      return tmcRegEvent(cid, ref tmEvent);
    }


    public Boolean TmcEvlogPutStrBin(Int32  cid,
                                     UInt32 unixTime,
                                     Byte   unixHund,
                                     Byte   importance,
                                     UInt32 sourceTag,
                                     string str,
                                     Byte[] bin,
                                     UInt32 cbBin)
    {
      return tmcEvlogPutStrBin(cid, unixTime, unixHund, importance, sourceTag, str, bin, cbBin);
    }


    public IntPtr TmcEventLogByElix(Int32                     cid,
                                    ref TmNativeDefs.TTMSElix elix,
                                    UInt16                    eventMask,
                                    UInt32                    startUnixTime,
                                    UInt32                    endUnixTime)
    {
      return tmcEventLogByElix(cid, ref elix, eventMask, startUnixTime, endUnixTime);
    }


    public Boolean TmcGetCurrentElix(Int32                     cid,
                                     ref TmNativeDefs.TTMSElix elix)
    {
      return tmcGetCurrentElix(cid, ref elix);
    }


    public Boolean TmcAlertListRemove(Int32                       cid,
                                      TmNativeDefs.TAlertListId[] listIds)
    {
      return tmcAlertListRemove(cid, listIds);
    }


    public Int16 TmcEvaluateExpression(int cid, string expr, byte[] res, uint cbBytes)
    {
      return tmcEvaluateExpression(cid, expr, res, cbBytes);
    }


    public Int16 TmcSetValues(Int32                         cid,
                              UInt32                        count,
                              TmNativeDefs.TValueAndFlags[] values)
    {
      return tmcSetValues(cid, count, values);
    }


    public Int16 TmcSetValuesUnion(Int32                              cid,
                                   UInt32                             count,
                                   TmNativeDefs.TValueAndFlagsUnion[] values)
    {
      return tmcSetValues(cid, count, values);
    }


    public Int16 TmcSetTimedValues(Int32                              cid,
                                   UInt32                             count,
                                   TmNativeDefs.TTimedValueAndFlags[] values)
    {
      return tmcSetTimedValues(cid, count, values);
    }


    public Int32 TmcExecuteControlScript(Int32 cid,
                                         Int16 ch,
                                         Int16 rtu,
                                         Int16 point,
                                         Int16 cmd)
    {
      return tmcExecuteControlScript(cid, ch, rtu, point, cmd);
    }


    public Boolean TmcOverrideControlScript(Int32 cid,
                                            bool  fOverride)
    {
      return tmcOverrideControlScript(cid, fOverride);
    }


    public Int16 TmcControlByStatus(Int32 cid,
                                    Int16 ch,
                                    Int16 rtu,
                                    Int16 point,
                                    Int16 cmd)
    {
      return tmcControlByStatus(cid, ch, rtu, point, cmd);
    }


    public Int16 TmcControlCmdResult(Int32  cid,
                                     UInt32 id,
                                     bool   fSuccess)
    {
      return tmcControlCmdResult(cid, id, fSuccess);
    }


    public Int16 TmcRegulationByAnalog(Int32  cid,
                                       Int16  ch,
                                       Int16  rtu,
                                       Int16  point,
                                       Byte   regType,
                                       IntPtr regData)
    {
      return tmcRegulationByAnalog(cid, ch, rtu, point, regType, regData);
    }


    public Int16 TmcSetStatusNormal(Int32  cid,
                                    Int16  ch,
                                    Int16  rtu,
                                    Int16  point,
                                    UInt16 nValue)
    {
      return tmcSetStatusNormal(cid, ch, rtu, point, nValue);
    }


    public Int16 TmcGetStatusNormal(Int32      cid,
                                    Int16      ch,
                                    Int16      rtu,
                                    Int16      point,
                                    out UInt16 nValue)
    {
      return tmcGetStatusNormal(cid, ch, rtu, point, out nValue);
    }


    public Int16 TmcDriverCall(Int32  cid,
                               UInt32 addr,
                               Int16  qCode,
                               Int16  command)
    {
      return tmcDriverCall(cid, addr, qCode, command);
    }


    public Boolean TmcEventLogAckRecords(Int32                     cid,
                                         ref TmNativeDefs.TTMSElix elix,
                                         UInt32                    count)
    {
      return tmcEventLogAckRecords(cid, ref elix, count);
    }


    public void TmcStatusByList(Int32                       cid,
                                UInt16                      count,
                                TmNativeDefs.TAdrTm[]       addr,
                                TmNativeDefs.TStatusPoint[] statuses)
    {
      tmcStatusByList(cid, count, addr, statuses);
    }


    public void TmcAnalogByList(Int32                       cid,
                                UInt16                      count,
                                TmNativeDefs.TAdrTm[]       addr,
                                TmNativeDefs.TAnalogPoint[] analogs,
                                UInt32 time,
                                UInt16 retroNum)
    {
      tmcAnalogByList(cid, count, addr, analogs, time, retroNum);
    }


    public short TmcGetObjectProperties(int        cid,
                                        ushort     objectType,
                                        short      ch,
                                        short      rtu,
                                        short      point,
                                        ref byte[] buf,
                                        int        bufSize)
    {
      return tmcGetObjectProperties(cid, objectType, ch, rtu, point, buf, bufSize);
    }


    public IntPtr TmcGetStatusClassData(Int32                 cid,
                                        UInt32                count,
                                        TmNativeDefs.TAdrTm[] statuses)
    {
      return tmcGetStatusClassData(cid, count, statuses);
    }


    public IntPtr TmcGetAnalogClassData(Int32                 cid,
                                        UInt32                count,
                                        TmNativeDefs.TAdrTm[] analogs)
    {
      return tmcGetAnalogClassData(cid, count, analogs);
    }


    public void TmcTakeRetroTit(Int32                                cid,
                                Int16                                ch,
                                Int16                                rtu,
                                Int16                                point,
                                UInt32                               unixTime,
                                UInt16                               retroNum,
                                UInt16                               count,
                                UInt16                               step,
                                ref TmNativeDefs.TAnalogPointShort[] analogs)
    {
      tmcTakeRetroTit(cid, ch, rtu, point, unixTime, retroNum, count, step, analogs);
    }


    public Int16 TmcAnalogMicroSeries(Int32                 cid,
                                      UInt32                cnt,
                                      TmNativeDefs.TAdrTm[] addrList,
                                      IntPtr[]              resultList)
    {
      return tmcAnalogMicroSeries(cid, cnt, addrList, resultList);
    }


    public Boolean TmcGetAnalogTechParms(Int32                             cid,
                                         ref TmNativeDefs.TAdrTm           addr,
                                         ref TmNativeDefs.TAnalogTechParms tpr)
    {
      return tmcGetAnalogTechParms(cid, ref addr, ref tpr);
    }


    public Boolean TmcSetAnalogTechParms(Int32                             cid,
                                         ref TmNativeDefs.TAdrTm           addr,
                                         ref TmNativeDefs.TAnalogTechParms tpr)
    {
      return tmcSetAnalogTechParms(cid, ref addr, ref tpr);
    }


    public Int16 TmcSetStatus(Int32  cid,
                              Int16  ch,
                              Int16  rtu,
                              Int16  point,
                              Byte   value,
                              string dateTime,
                              Int16  hund)
    {
      return tmcSetStatus(cid, ch, rtu, point, value, dateTime, hund);
    }


    public Int16 TmcSetStatusFlags(Int32 cid,
                                   Int16 ch,
                                   Int16 rtu,
                                   Int16 point,
                                   Int16 flags)
    {
      return tmcSetStatusFlags(cid, ch, rtu, point, flags);
    }


    public Int16 TmcClrStatusFlags(Int32 cid,
                                   Int16 ch,
                                   Int16 rtu,
                                   Int16 point,
                                   Int16 flags)
    {
      return tmcClrStatusFlags(cid, ch, rtu, point, flags);
    }


    public Int16 TmcSetAnalog(Int32  cid,
                              Int16  ch,
                              Int16  rtu,
                              Int16  point,
                              Single value,
                              string dateTime)
    {
      return tmcSetAnalog(cid, ch, rtu, point, value, dateTime);
    }


    public Int16 TmcSetAnalogFlags(Int32 cid,
                                   Int16 ch, Int16 rtu, Int16 point,
                                   Int16 flags)
    {
      return tmcSetAnalogFlags(cid, ch, rtu, point, flags);
    }


    public Int16 TmcClrAnalogFlags(Int32 cid,
                                   Int16 ch,
                                   Int16 rtu,
                                   Int16 point,
                                   Int16 flags)
    {
      return tmcClrAnalogFlags(cid, ch, rtu, point, flags);
    }


    public Int16 TmcSetAccumFlags(Int32 cid,
                                  Int16 ch, Int16 rtu, Int16 point,
                                  Int16 flags)
    {
      return tmcSetAccumFlags(cid, ch, rtu, point, flags);
    }


    public Int16 TmcClrAccumFlags(Int32 cid,
                                  Int16 ch,
                                  Int16 rtu,
                                  Int16 point,
                                  Int16 flags)
    {
      return tmcClrAccumFlags(cid, ch, rtu, point, flags);
    }


    public Int16 TmcPeekAlarm(Int32                   cid,
                              Int16                   ch,
                              Int16                   rtu,
                              Int16                   point,
                              Int16                   alarmId,
                              ref TmNativeDefs.TAlarm alarm)
    {
      return tmcPeekAlarm(cid, ch, rtu, point, alarmId, ref alarm);
    }


    public Int16 TmcPokeAlarm(Int32                   cid,
                              Int16                   ch,
                              Int16                   rtu,
                              Int16                   point,
                              Int16                   alarmId,
                              ref TmNativeDefs.TAlarm alarm)
    {
      return tmcPokeAlarm(cid, ch, rtu, point, alarmId, ref alarm);
    }


    public IntPtr TmcAanReadArchive(Int32            cid,
                                    UInt32           tmAddr,
                                    UInt32           startUnixTime,
                                    UInt32           endUnixTime,
                                    UInt32           step,
                                    UInt32           flags,
                                    out UInt32       count,
                                    TmNativeCallback progress,
                                    IntPtr           progressParam)
    {
      return tmcAanReadArchive(cid,       tmAddr,   startUnixTime, endUnixTime, step, flags,
                               out count, progress, progressParam);
    }


    public Boolean TmcAanGetStats(Int32                         cid,
                                  ref TmNativeDefs.TM_AAN_STATS stats,
                                  UInt32                        cbStats)
    {
      return tmcAanGetStats(cid, ref stats, cbStats);
    }


    public UInt32 String2Utime_(string dateTime)
    {
      return String2Utime(dateTime);
    }


    public Byte TmcEnumObjects(Int32        cid,
                               UInt16       objectType,
                               Byte         count,
                               ref UInt16[] buf,
                               Int16        ch,
                               Int16        rtu,
                               Int16        point)
    {
      return tmcEnumObjects(cid, objectType, count, buf, ch, rtu, point);
    }


    public Int16 TmcGetObjectName(Int32      cid,
                                  UInt16     objectType,
                                  Int16      ch,
                                  Int16      rtu,
                                  Int16      point,
                                  ref byte[] buf,
                                  Int32      bufSize)
    {
      return tmcGetObjectName(cid, objectType, ch, rtu, point, buf, bufSize);
    }


    public short TmcGetObjectNameEx(int        cid,
                                    ushort     objectType,
                                    short      ch,    short rtu,
                                    short      point, short subObjectId,
                                    ref byte[] buf,
                                    int        bufSize)
    {
      return tmcGetObjectNameEx(cid, objectType, ch, rtu, point, subObjectId, buf, bufSize);
    }


    public IntPtr TmcTechObjReadValues(Int32                   cid,
                                       TmNativeDefs.TTechObj[] objects,
                                       UInt32                  count)
    {
      return tmcTechObjReadValues(cid, objects, count);
    }


    public IntPtr TmcTechObjEnumValues(Int32      cid,
                                       UInt32     tobS,
                                       UInt32     tobT,
                                       IntPtr     props,
                                       out UInt32 count)
    {
      return tmcTechObjEnumValues(cid, tobS, tobT, props, out count);
    }


    public Boolean TmcTechObjBeginUpdate(Int32 cid)
    {
      return tmcTechObjBeginUpdate(cid);
    }


    public Boolean TmcTechObjEndUpdate(Int32 cid)
    {
      return tmcTechObjEndUpdate(cid);
    }


    public Int32 TmcTechObjWriteValues(Int32                        cid,
                                       TmNativeDefs.TTechObjProps[] props,
                                       UInt32                       count)
    {
      return tmcTechObjWriteValues(cid, props, count);
    }


    public UInt32 TmcEventGetAdditionalRecData(UInt32     id,
                                               ref byte[] buf,
                                               UInt32     bufSize)
    {
      return tmcEventGetAdditionalRecData(id, buf, bufSize);
    }


    public IntPtr TmcComtradeEnumDays(Int32 cid)
    {
      return tmcComtradeEnumDays(cid);
    }


    public IntPtr TmcComtradeEnumFiles(Int32 cid, string date)
    {
      return tmcComtradeEnumFiles(cid, date);
    }


    public Boolean TmcComtradeGetFile(Int32 cid, string fileName, string localDirectory)
    {
      return tmcComtradeGetFile(cid, fileName, localDirectory);
    }


    public bool TmcSetTracer(Int32  cid,
                             Int16  ch,
                             Int16  rtu,
                             Int16  point,
                             UInt16 tmType,
                             UInt16 msgFilter)
    {
      return tmcSetTracer(cid, ch, rtu, point, tmType, msgFilter);
    }


    public Int32 TmcConnectEx(string           server,
                              string           pipe,
                              string           user,
                              TmNativeCallback callback,
                              IntPtr           callbackParameter,
                              UInt32           propsCount,
                              UInt32[]         pProps,
                              UInt32[]         pPropValues)
    {
      return tmcConnectEx(server, pipe, user, callback, callbackParameter, propsCount, pProps, pPropValues);
    }


    public Int16 TmcSetRetransInfoEx(Int32                              cid,
                                     UInt16                             count,
                                     ref TmNativeDefs.TRetransInfo      ri,
                                     ref TmNativeDefs.TRetransInfoReply rir)
    {
      return tmcSetRetransInfoEx(cid, count, ref ri, ref rir);
    }


    public Int16 TmcSetRetransInfo(Int32                         cid,
                                   UInt16                        count,
                                   ref TmNativeDefs.TRetransInfo ri)
    {
      return tmcSetRetransInfo(cid, count, ref ri);
    }


    public Int16 TmcClrRetransInfo(Int32 cid)
    {
      return tmcClrRetransInfo(cid);
    }


    public bool TmcDntGetConfig(Int32 cid, string fileName)
    {
      return tmcDntGetConfig(cid, fileName);
    }


    public Int32 TmcDntTreeChange(Int32 cid)
    {
      return tmcDntTreeChange(cid);
    }


    public IntPtr TmcDntOpenItem(Int32 cid, UInt32 traceChainLength, UInt32[] traceChain)
    {
      return tmcDntOpenItem(cid, traceChainLength, traceChain);
    }


    public IntPtr TmcDntGetNextItem(IntPtr componentItemsPtr)
    {
      return tmcDntGetNextItem(componentItemsPtr);
    }


    public void TmcDntCloseItem(IntPtr componentItemsPtr)
    {
      tmcDntCloseItem(componentItemsPtr);
    }


    public bool TmcDntGetObjectName(int        cid,
                                    ushort     objectType,
                                    short      ch,
                                    short      rtu,
                                    short      point,
                                    ref byte[] buf,
                                    int        bufSize)
    {
      return tmcDntGetObjectName(cid, objectType, ch, rtu, point, buf, bufSize);
    }


    public Boolean TmcDntRegisterUser(Int32 cid)
    {
      return tmcDntRegisterUser(cid);
    }


    public Boolean TmcDntUnRegisterUser(Int32 cid)
    {
      return tmcDntUnRegisterUser(cid);
    }


    public Boolean TmcDntBeginTraceEx(Int32    cid,
                                      UInt32   count,
                                      UInt32[] traceChain,
                                      UInt32   traceFlags,
                                      UInt32   res1,
                                      UInt32   res2)
    {
      return tmcDntBeginTraceEx(cid, count, traceChain, traceFlags, res1, res2);
    }


    public Boolean TmcDntStopTrace(Int32 cid)
    {
      return tmcDntStopTrace(cid);
    }


    public Boolean TmcDntBeginDebug(Int32 cid)
    {
      return tmcDntBeginDebug(cid);
    }


    public Boolean TmcDntStopDebug(Int32 cid)
    {
      return tmcDntStopDebug(cid);
    }


    public UInt32 TmcDntGetLiveInfo(Int32      cid,
                                    UInt32     count,
                                    UInt32[]   traceChain,
                                    out UInt32 pData,
                                    UInt32     pDataSize)
    {
      return tmcDntGetLiveInfo(cid, count, traceChain, out pData, pDataSize);
    }


    public uint TmcDntGetPortStats(int        cid,
                                   uint[]     pDap,
                                   ref byte[] buf,
                                   int        bufSize)
    {
      return tmcDntGetPortStats(cid, pDap, buf, bufSize);
    }


    public Int16 TmcGetServerInfo(Int32                        cid,
                                  ref TmNativeDefs.TServerInfo info)
    {
      return tmcGetServerInfo(cid, ref info);
    }


    public IntPtr TmcGetServerThreads(Int32 cid)
    {
      return tmcGetServerThreads(cid);
    }


    public Boolean TmcGetGrantedAccess(Int32      cid,
                                       out UInt32 pAccess)
    {
      return tmcGetGrantedAccess(cid, out pAccess);
    }


    public IntPtr TmcTakeAPS(Int32 cid)
    {
      return tmcTakeAPS(cid);
    }


    public IntPtr TmcTextSearch(Int32      cid,
                                UInt16     type,
                                string     text,
                                out UInt32 pCount)
    {
      return tmcTextSearch(cid, type, text, out pCount);
    }

    public Int16 TmcRetroInfoEx(Int32                         cid, 
                                UInt16                        id, 
                                ref TmNativeDefs.TRetroInfoEx info)
    {
      return tmcRetroInfoEx(cid, id, ref info);
    }
  }
}