using MudBlazorClient.Themes;

namespace MudBlazorClient.Services;

// Scoped = one instance per Blazor Server circuit (one per connected user).
public class ThemeService
{
    public NamedTheme Current { get; private set; } = AppThemes.DefaultTheme;

    public event Action? OnThemeChanged;

    public void SetTheme(NamedTheme theme)
    {
        if (theme == Current) return;
        Current = theme;
        OnThemeChanged?.Invoke();
    }
}
