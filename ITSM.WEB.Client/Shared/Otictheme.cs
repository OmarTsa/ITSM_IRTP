using MudBlazor;

namespace ITSM.WEB.Client.Shared
{
    public static class OticTheme
    {
        public static MudTheme Theme => new()
        {
            PaletteLight = new PaletteLight
            {
                Primary = "#B91C2E",           // Rojo OTIC Principal
                Secondary = "#8B1523",         // Rojo OTIC Oscuro
                Tertiary = "#D4364A",          // Rojo OTIC Claro
                Info = "#2196F3",
                Success = "#4CAF50",
                Warning = "#FF9800",
                Error = "#F44336",
                Dark = "#2C3E50",
                TextPrimary = "rgba(0, 0, 0, 0.87)",
                TextSecondary = "rgba(0, 0, 0, 0.6)",
                TextDisabled = "rgba(0, 0, 0, 0.38)",
                Background = "#F5F5F5",
                Surface = "#FFFFFF",
                AppbarBackground = "#B91C2E",
                AppbarText = "#FFFFFF",
                DrawerBackground = "#FFFFFF",
                DrawerText = "rgba(0, 0, 0, 0.87)",
                DrawerIcon = "rgba(0, 0, 0, 0.54)",
                Divider = "rgba(0, 0, 0, 0.12)",
                LinesDefault = "rgba(0, 0, 0, 0.12)",
                LinesInputs = "rgba(0, 0, 0, 0.42)"
            },

            PaletteDark = new PaletteDark
            {
                Primary = "#D4364A",
                Secondary = "#B91C2E",
                Tertiary = "#8B1523",
                Info = "#2196F3",
                Success = "#4CAF50",
                Warning = "#FF9800",
                Error = "#F44336",
                TextPrimary = "rgba(255, 255, 255, 1)",
                TextSecondary = "rgba(255, 255, 255, 0.7)",
                TextDisabled = "rgba(255, 255, 255, 0.5)",
                Background = "#1E1E1E",
                Surface = "#2D2D2D",
                AppbarBackground = "#B91C2E",
                AppbarText = "#FFFFFF",
                DrawerBackground = "#2D2D2D",
                DrawerText = "rgba(255, 255, 255, 0.87)",
                DrawerIcon = "rgba(255, 255, 255, 0.7)",
                Divider = "rgba(255, 255, 255, 0.12)",
                LinesDefault = "rgba(255, 255, 255, 0.12)",
                LinesInputs = "rgba(255, 255, 255, 0.3)"
            },

            LayoutProperties = new LayoutProperties
            {
                DrawerWidthLeft = "260px",
                DrawerWidthRight = "300px",
                AppbarHeight = "64px",
                DefaultBorderRadius = "8px"
            },

            ZIndex = new ZIndex
            {
                AppBar = 1100,
                Drawer = 1200,
                Dialog = 1300,
                Popover = 1400,
                Snackbar = 1500,
                Tooltip = 1600
            },

            Shadows = new Shadow(),
            Typography = new Typography()
        };
    }
}
