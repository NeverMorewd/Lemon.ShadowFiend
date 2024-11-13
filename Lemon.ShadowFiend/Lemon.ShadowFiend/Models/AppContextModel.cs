using System.Collections.Generic;
using Lemon.ShadowFiend.Utils;
using Lemon.ShadowFiend.ViewModels;
using R3;

namespace Lemon.ShadowFiend.Models;

public class AppContextModel : ViewModelBase
{
    public static readonly AppContextModel Current = new();

    private AppContextModel()
    {
        RdpTypes = EnumUtils.GetAllMembers<RdpType>();
        CurrentRdpType = new BindableReactiveProperty<RdpType>(RdpType.ChildSession);
    }

    public BindableReactiveProperty<RdpType> CurrentRdpType
    {
        get; 
    }

    public IEnumerable<RdpType> RdpTypes { get; }
}