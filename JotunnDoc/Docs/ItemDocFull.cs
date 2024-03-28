using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Jotunn.Entities;
using Jotunn.Managers;
using Mono.Cecil;
using UnityEngine;
using static ItemDrop.ItemData;

namespace JotunnDoc.Docs
{
    // ==== Add English-localized Name, Description, and Type to item object
    //public static class ItemDrop
    //{
        //public static string TokenName(this ItemDrop self) => self.m_itemData.m_shared.m_name;
        //public static string EngName(this ItemDrop self) => JotunnDoc.Localize(self.m_itemData.m_shared.m_name);
        //public static string EngDesc(this ItemDrop self) => JotunnDoc.Localize(self.m_itemData.m_shared.m_description);
        //public static string EngType(this ItemDrop self) => self.m_itemData.m_shared.m_itemType.ToString();
    //}



    public class ItemDocFull : Doc
    {
        
        public ItemDocFull() : base("item-list-full.json")
        {
            ItemManager.OnItemsRegistered += DocItems;
        }

        private void DocItems()
        {
            Debug.Log("Initialized ItemDocFull");

            if (Generated)
            {
                return;
            }

            foreach (GameObject obj in ObjectDB.instance.m_items.Where(x => !CustomItem.IsCustomItem(x.name)))
            {
                ItemDrop drop = obj.GetComponent<ItemDrop>();

                ItemDropWithAttr item = new ItemDropWithAttr( drop.m_itemData.m_shared );

                ItemDrop.ItemData.SharedData shared = item.Payload;

                item.EngName = JotunnDoc.Localize(shared.m_name);
                item.EngDesc = JotunnDoc.Localize(shared.m_description);
                item.EngType = shared.m_itemType.ToString();

                // ==== write item object to a line of the export file
                string itemToJson = JsonUtility.ToJson( item );
                AddText(itemToJson);

                
                //==== export render PNG
                
                string RenderPath = Path.Combine(DocumentationDirConfig.Value, "Renders", obj.name);
                RenderPath += ".png";
                new FileInfo( RenderPath ).Directory.Create();

                //RequestRender(RenderPath, obj, RenderManager.IsometricRotation);

                //==== export icon PNG
                string IconPath = Path.Combine(DocumentationDirConfig.Value, "Icons", obj.name);
                IconPath += ".png";
                new FileInfo(IconPath).Directory.Create();

                Sprite icon = drop.m_itemData.GetIcon();
                //RequestIcon(IconPath, icon);

            }
            Save();
        }
    }

    /// <summary>
    ///     Extends ItemDrop with attributes for Equipprimary json export
    /// </summary>
    public class ItemDropWithAttr
    {
        public string EngName;
        public string EngDesc;
        public string EngType;
        public ItemDrop.ItemData.SharedData Payload;

        public ItemDropWithAttr(ItemDrop.ItemData.SharedData item)
        {
            Payload = item;
        }
    }
}
