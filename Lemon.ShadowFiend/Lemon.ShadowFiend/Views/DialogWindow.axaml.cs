using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Lemon.ModuleNavigation.Avaloniaui.Dialogs;
using System;

namespace Lemon.ShadowFiend.Views;

public partial class DialogWindow : Window, IAvaDialogWindow
{
    public const string Key = nameof(DialogWindow);
    protected override Type StyleKeyOverride => typeof(Window);

    public DialogWindow()
    {
        InitializeComponent();
    }
}