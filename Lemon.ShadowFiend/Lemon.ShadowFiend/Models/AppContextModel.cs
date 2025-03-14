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
        Busy = new BindableReactiveProperty<bool>(false);
        IsTopMost = new BindableReactiveProperty<bool>(false);
        Title = new BindableReactiveProperty<string>("ShadowFiend");
    }

    public BindableReactiveProperty<RdpType> CurrentRdpType
    {
        get; 
    }

    public IEnumerable<RdpType> RdpTypes 
    { 
        get; 
    }

    public BindableReactiveProperty<bool> Busy 
    { 
        get; 
    }
    public BindableReactiveProperty<string> Title
    {
        get;
    }
    public BindableReactiveProperty<bool> IsTopMost

    {
        get;
    }
}