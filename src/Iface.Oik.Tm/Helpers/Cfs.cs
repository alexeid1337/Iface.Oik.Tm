using System;
using System.Runtime.InteropServices;
using System.Text;
using Iface.Oik.Tm.Interfaces;
using Iface.Oik.Tm.Native.Api;
using Iface.Oik.Tm.Native.Interfaces;

namespace Iface.Oik.Tm.Helpers
{
  public static class Cfs
  {

    public const string ServerTypeTms = "tms$";
    
    public static readonly ITmNative Native = new TmNative();
    
    public static void InitializeCfsLibrary()
    {
      Native.CfsInitLibrary();
    }
    
    public static void SetUserCredentials(string user,
                                          string password)
    {
      Native.CfsSetUser(user, password);
    }

    public static (IntPtr cfId, string errString, int errorCode) ConnectToCfs(string host)
    {
      const int errStringLength = 1000;
      var       errString       = new StringBuilder(errStringLength);
      uint      errCode         = 0;

      var cfId = 
        Native.CfsConnect(host, out errCode, ref errString, errStringLength);

      if (cfId == IntPtr.Zero)
      {
        Console.WriteLine($"Ошибка соединения с мастер-сервисом: {errCode} - {errString}");
      }

      return (cfId, errString.ToString(), Convert.ToInt32(errCode));
    }

    public static (IntPtr, CfsDefs.InitializeConnectionResult) InitializeConnection(CfsOptions options)
    {
      SetUserCredentials(options.User, options.Password);

      var (cfsCid, errString, errorCode) = ConnectToCfs(options.Host);
      
      switch (errorCode)
      {
        case 0:
          return (cfsCid, CfsDefs.InitializeConnectionResult.Ok);
        case 87:
          Console.WriteLine("Соединение не установлено. " + errString);
          return (cfsCid, CfsDefs.InitializeConnectionResult.InvalidLoginOrPassword);
        default:
          Console.WriteLine("Соединение не установлено. " + errString);
          return (cfsCid,  CfsDefs.InitializeConnectionResult.NonSpecifiedError);
      }

      
    }
    
    public static bool IsConnected(IntPtr cfsCid)
    {
      return cfsCid != IntPtr.Zero;
    }

    public class CfsOptions
    {
      public string Host      { get; set; }
      public string User      { get; set; }
      public string Password { get; set; }
    }
    
    public static TmUserInfo GetUserInfo(IntPtr cfCid,
                                         string serverName, 
                                         string serverType)
    {
      var nativeUserInfoSize = Marshal.SizeOf(typeof(TmNativeDefs.TUserInfo));
      var nativeUserInfoPtr  = Marshal.AllocHGlobal(nativeUserInfoSize);
      
      var fetchResult = Native.CfsGetExtendedUserData(cfCid,
                                                      serverType,
                                                      serverName,
                                                      nativeUserInfoPtr,
                                                      (uint) nativeUserInfoSize);
      if (fetchResult == 0)
      {
        return null;
      }

      var nativeUserInfo = Marshal.PtrToStructure<TmNativeDefs.TUserInfo>(nativeUserInfoPtr);

      return new TmUserInfo(nativeUserInfo.UserId,
                            Encoding.GetEncoding(1251).GetString(nativeUserInfo.UserName).Trim('\0'),
                            Encoding.GetEncoding(1251).GetString(nativeUserInfo.KeyId).Trim('\0'),
                            nativeUserInfo.Group,
                            nativeUserInfo.Rights);
    }
  }
}