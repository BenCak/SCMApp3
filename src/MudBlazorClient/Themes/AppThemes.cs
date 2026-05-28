using MudBlazor;

namespace MudBlazorClient.Themes;

public record NamedTheme(string Name, string Icon, MudTheme Theme, bool IsDark = false);

public static class AppThemes
{
    // ----------------------------------------------------------------
    // Add new themes here — they appear automatically in the dropdown.
    // ----------------------------------------------------------------
    public static readonly List<NamedTheme> All =
    [
        new("Default",      Icons.Material.Filled.LightMode,    Default()),
        new("Dark",         Icons.Material.Filled.DarkMode,     Default(),      IsDark: true),
        new("Navy",         Icons.Material.Filled.Waves,        Navy()),
        new("Navy Dark",    Icons.Material.Filled.NightShelter, Navy(),         IsDark: true),
        new("Forest",       Icons.Material.Filled.Forest,       Forest()),
        new("Crimson",      Icons.Material.Filled.LocalFireDepartment, Crimson()),
    ];

    public static NamedTheme DefaultTheme => All[0];

    // ── Theme definitions ────────────────────────────────────────────

    private static MudTheme Default() => new();

    private static MudTheme Navy() => new()
    {
        PaletteLight = new PaletteLight
        {
            Primary = "#1A3A5C",
            PrimaryContrastText = "#FFFFFF",
            Secondary = "#2E86AB",
            AppbarBackground = "#1A3A5C",
            AppbarText = "#FFFFFF",
            DrawerBackground = "#F0F4F8",
            DrawerText = "#1A3A5C",
            Background = "#F8FAFC",
            Surface = "#FFFFFF",
            TextPrimary = "#1A3A5C",
            TextSecondary = "#4A6080",
        },
        PaletteDark = new PaletteDark
        {
            Primary = "#2E86AB",
            PrimaryContrastText = "#FFFFFF",
            Secondary = "#1A3A5C",
            AppbarBackground = "#0D1E2E",
            DrawerBackground = "#0D1E2E",
            Background = "#0A1628",
            Surface = "#0D1E2E",
        }
    };

    private static MudTheme Forest() => new()
    {
        PaletteLight = new PaletteLight
        {
            Primary = "#2D6A4F",
            PrimaryContrastText = "#FFFFFF",
            Secondary = "#52B788",
            AppbarBackground = "#2D6A4F",
            AppbarText = "#FFFFFF",
            DrawerBackground = "#F0F7F4",
            DrawerText = "#2D6A4F",
            Background = "#F5FAF7",
            Surface = "#FFFFFF",
            TextPrimary = "#1B4332",
            TextSecondary = "#40916C",
        },
        PaletteDark = new PaletteDark
        {
            Primary = "#52B788",
            PrimaryContrastText = "#1B4332",
            Secondary = "#2D6A4F",
            AppbarBackground = "#1B4332",
            DrawerBackground = "#1B4332",
            Background = "#081C15",
            Surface = "#1B4332",
        }
    };

    private static MudTheme Crimson() => new()
    {
        PaletteLight = new PaletteLight
        {
            Primary = "#9B2335",
            PrimaryContrastText = "#FFFFFF",
            Secondary = "#C0392B",
            AppbarBackground = "#9B2335",
            AppbarText = "#FFFFFF",
            DrawerBackground = "#FDF3F4",
            DrawerText = "#9B2335",
            Background = "#FEF9F9",
            Surface = "#FFFFFF",
            TextPrimary = "#5C1A22",
            TextSecondary = "#9B2335",
        },
        PaletteDark = new PaletteDark
        {
            Primary = "#E05A6A",
            PrimaryContrastText = "#FFFFFF",
            Secondary = "#9B2335",
            AppbarBackground = "#3D0A11",
            DrawerBackground = "#3D0A11",
            Background = "#1A0509",
            Surface = "#3D0A11",
        }
    };
}
