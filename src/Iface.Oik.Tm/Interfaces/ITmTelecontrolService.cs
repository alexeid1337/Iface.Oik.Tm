using System.Collections.Generic;
using System.Threading.Tasks;

namespace Iface.Oik.Tm.Interfaces
{
  public interface ITmTelecontrolService
  {
    TmTelecontrolValidationResult Validate(TmStatus tmStatus);

    bool CanOverrideScript();

    Task<(bool, IReadOnlyCollection<TmControlScriptCondition>)> CheckScript(TmStatus tmStatus);

    Task<TmTelecontrolResult> Execute(TmStatus tmStatus, bool overrideScript = false);

    Task<(bool, IReadOnlyCollection<TmControlScriptCondition>)> CheckScriptExplicitly(TmStatus tmStatus,
                                                                                      int      explicitNewStatus);


    Task<TmTelecontrolResult> ExecuteExplicitly(TmStatus tmStatus,
                                                int      explicitNewStatus,
                                                bool     overrideScript = false);
    
    
    TmTeleregulateValidationResult Validate(TmAnalog tmAnalog);

    Task<TmTelecontrolResult> TeleregulateByStepUp(TmAnalog analog);

    Task<TmTelecontrolResult> TeleregulateByStepDown(TmAnalog analog);

    Task<TmTelecontrolResult> TeleregulateByCode(TmAnalog analog, int code);

    Task<TmTelecontrolResult> TeleregulateByValue(TmAnalog analog, float value);
  }
}