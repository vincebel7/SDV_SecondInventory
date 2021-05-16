using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SecondInventory
{
    public class ModEntry : StardewModdingAPI.Mod
    {
        private ModConfig Config;
        private static int invTabID = 0;

        public override void Entry(IModHelper helper)
        {
            this.Config = helper.ReadConfig<ModConfig>();

            helper.Events.Input.ButtonPressed += this.OnButtonPressed;
        }

        private void OnButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            if (!Context.IsWorldReady) // ignore if player hasn't loaded a save yet
                return;

            if (this.Config.Buttons.SecondInvOpen.Contains(e.Button) && Game1.activeClickableMenu is GameMenu)
            {
                OpenSecondInv();
            }
        }

        private void OpenSecondInv()
        {
            this.Monitor.Log($"{Game1.player.Name} opened Second Inventory", LogLevel.Debug);

            if (Game1.activeClickableMenu is GameMenu menu)
            {
                IList<IClickableMenu> pages = this.Helper.Reflection.GetField<List<IClickableMenu>>(menu, "pages").GetValue();
                IList<ClickableComponent> tabs = this.Helper.Reflection.GetField<List<ClickableComponent>>(menu, "tabs").GetValue();
                this.Monitor.Log($"pages:{pages}", LogLevel.Debug);

                tabs.Add(new ClickableComponent(new Rectangle(menu.xPositionOnScreen + 64, menu.yPositionOnScreen + IClickableMenu.tabYPositionRelativeToMenuY + 64 - 64, 64, 64), "inv2", "Inv2")
                {
                    myID = 212340,
                    downNeighborID = 12340,
                    rightNeighborID = 12341,
                    leftNeighborID = 12339,
                    tryDefaultIfNoDownNeighborExists = true,
                    fullyImmutable = true
                });
                tabs[1].upNeighborID = 212340;
                pages.Add((IClickableMenu)new SecondInventoryPage(menu.xPositionOnScreen, menu.yPositionOnScreen, menu.width, menu.height));
            }
        }


    }
}