using StardewModdingAPI;

namespace SecondInventory
{
    public class ModConfig
    {
        public ModConfigControls Buttons { get; set; } = new ModConfigControls();

        public class ModConfigControls
        {
            public SButton[] SecondInvOpen { get; set; } = { SButton.OemPipe };
        }
    }
}