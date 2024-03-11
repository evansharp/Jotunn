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
                ItemDropWithAttr item = new ItemDropWithAttr( obj.GetComponent<ItemDrop>() );


                ItemDrop.ItemData.SharedData shared = item.Payload.m_itemData.m_shared;

                item.EngName = JotunnDoc.Localize(shared.m_name);
                item.EngDesc = JotunnDoc.Localize(shared.m_description);
                item.EngType = shared.m_itemType.ToString();

                // ==== write item object to a line of the export file
                string itemToJson = JsonUtility.ToJson( item );
                AddText(itemToJson);

                //==== export render PNG
                string RenderPath = Path.Combine(DocumentationDirConfig.Value, "Renders", obj.name);
                RenderPath += ".png";

                // Create directory if it doesn't exist
                new FileInfo( RenderPath ).Directory.Create();

                //write sprite PNG
                RequestRender(RenderPath, obj, RenderManager.IsometricRotation);

                //==== export icon PNG
                string SpritePath = Path.Combine(DocumentationDirConfig.Value, "Sprites", obj.name);
                SpritePath += ".png";

                // Create directory if it doesn't exist
                new FileInfo(SpritePath).Directory.Create();

                //write icon PNG
                RequestSprite(SpritePath, obj);

                // Export a single item
                //if (shared.m_name == "$item_legs_iron")
                //{
                //    string itemToJson = JsonUtility.ToJson(item);
                //    AddText(itemToJson);
                //}

                // Original plugin data refs
                //    obj.name,                                       // Prefab
                //    shared.m_name,                                  // Token
                //    JotunnDoc.Localize(shared.m_name),              // Name
                //    shared.m_itemType.ToString(),                   // Type
                //    JotunnDoc.Localize(shared.m_description)        // Description

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
        public ItemDrop Payload;

        public ItemDropWithAttr( ItemDrop item)
        {
            Payload = item;
        }
    }
}
