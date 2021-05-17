using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SecondInventory
{
    public class ModEntry : StardewModdingAPI.Mod
    {
        private ModConfig Config;

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

        private int myTabIndex = -1;

        private void OpenSecondInv()
        {
            this.Monitor.Log($"{Game1.player.Name} opened Second Inventory", LogLevel.Debug);

            if (Game1.activeClickableMenu is GameMenu menu)
            {
                var pages = this.Helper.Reflection.GetField<List<IClickableMenu>>(menu, "pages").GetValue();
                var tabs = this.Helper.Reflection.GetField<List<ClickableComponent>>(menu, "tabs").GetValue();
                this.Monitor.Log($"pages:{pages}, tabs:{tabs}", LogLevel.Debug);

                //tabs.Add(new ClickableComponent(new Rectangle(menu.xPositionOnScreen + 64, menu.yPositionOnScreen + IClickableMenu.tabYPositionRelativeToMenuY + 64 - 64, 64, 64), "inv2", "Inv2")
                //{
                //    myID = 212340,
                //    downNeighborID = 12340,
                //    rightNeighborID = 12341,
                //    leftNeighborID = 12339, 
                //    tryDefaultIfNoDownNeighborExists = true,
                //    fullyImmutable = true
                //});
                //tabs[1].upNeighborID = 212340;

                var invTabNum = menu.getTabNumberFromName("inventory");
                this.Monitor.Log($"invTabNum:{invTabNum}", LogLevel.Debug);
                
                myTabIndex = tabs.Count;
                tabs.Add(new ClickableComponent(new Rectangle(menu.xPositionOnScreen + 192, menu.yPositionOnScreen + IClickableMenu.tabYPositionRelativeToMenuY + 64 - 64, 64, 64), "inv2", "Inv2")
                {
                    myID = 912342,
                    downNeighborID = 12342,
                    rightNeighborID = 12343,
                    leftNeighborID = 12341,
                    tryDefaultIfNoDownNeighborExists = true,
                    fullyImmutable = true
                });
                tabs[1].upNeighborID = 912342;

                
                var inv2TabNum = menu.getTabNumberFromName("inv2");
                this.Monitor.Log($"inv2TabNum:{inv2TabNum}", LogLevel.Debug);
                var socialTabNum = menu.getTabNumberFromName("social");
                this.Monitor.Log($"socialTabNum:{socialTabNum}", LogLevel.Debug);

                pages.Add((IClickableMenu)new SecondInventoryPage(menu.xPositionOnScreen, menu.yPositionOnScreen, menu.width, menu.height));
                Helper.Events.Display.RenderedActiveMenu += DrawIcon;
            }
        }

        private void DrawIcon(object sender, RenderedActiveMenuEventArgs e)
        {
            if (!(Game1.activeClickableMenu is GameMenu menu))
            { return; }

            if (menu.invisible || myTabIndex == -1)
                return;

            var tabs = Helper.Reflection.GetField<List<ClickableComponent>>(menu, "tabs").GetValue();
            if (tabs.Count <= myTabIndex)
            {
                this.Monitor.Log($"Tab index error", LogLevel.Error);
                return;
            }
            var tab = tabs[myTabIndex];
            e.SpriteBatch.Draw(Game1.mouseCursors, new Vector2((float)tab.bounds.X, (float)(tab.bounds.Y + (menu.currentTab == menu.getTabNumberFromName(tab.name) ? 8 : 0))), new Rectangle?(new Rectangle(2 * 16, 368, 16, 16)), Color.White, 0.0f, Vector2.Zero, 4f, SpriteEffects.None, 0.0001f);
        }

    }
}