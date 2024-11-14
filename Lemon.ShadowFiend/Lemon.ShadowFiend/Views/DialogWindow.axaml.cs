using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Lemon.ModuleNavigation.Avaloniaui.Dialogs;

namespace Lemon.ShadowFiend.Views;

public partial class DialogWindow : Window, IAvaDialogWindow
{
    public const string Key = nameof(DialogWindow);

    public DialogWindow()
    {
        InitializeComponent();
    }
}