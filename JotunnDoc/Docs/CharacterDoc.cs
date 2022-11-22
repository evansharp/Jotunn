﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using Jotunn.Entities;
using Jotunn.Managers;
using JotunnDoc.Patches;
using UnityEngine;

namespace JotunnDoc.Docs
{
    public class CharacterDoc : Doc
    {
        public CharacterDoc() : base("prefabs/character-list.md")
        {
            GameEvents.OnPlayerSpawned += DocCharacters;
        }

        private void DocCharacters(Player self)
        {
            if (Generated)
            {
                return;
            }

            var imageDirectory = Path.Combine(DocumentationDirConfig.Value, "images/characters");
            Directory.CreateDirectory(imageDirectory);

            Jotunn.Logger.LogInfo("Documenting characters");

            AddHeader(1, "Character list");
            AddText("All of the Character prefabs currently in the game.");
            AddText("This file is automatically generated from Valheim using the JotunnDoc mod found on our GitHub.");
            AddTableHeader("Name", "Components", "DamageModifiers");

            List<GameObject> allPrefabs = new List<GameObject>();
            allPrefabs.AddRange(ZNetScene.instance.m_nonNetViewPrefabs);
            allPrefabs.AddRange(ZNetScene.instance.m_prefabs);

            foreach (GameObject obj in allPrefabs.Where(x => !CustomPrefab.IsCustomPrefab(x.name) && x.GetComponent<Character>() != null).OrderBy(x => x.name))
            {
                string name = obj.name;
                if (RequestSprite(Path.Combine(imageDirectory, $"{name}.png"), obj, RenderManager.IsometricRotation))
                {
                    name += $"<br><img src=\"../../images/characters/{name}.png\">";
                }

                string components = "<ul>";

                foreach (Component comp in obj.GetComponents<Component>())
                {
                    components += "<li>" + comp.GetType().Name + "</li>";
                }

                components += "</ul>";

                string damagemods = "<ul>";
                HitData.DamageModifiers mods = obj.GetComponent<Character>().m_damageModifiers;
                damagemods += $"<li>Blunt: {mods.m_blunt}</li>";
                damagemods += $"<li>Slash: {mods.m_slash}</li>";
                damagemods += $"<li>Pierce: {mods.m_pierce}</li>";
                damagemods += $"<li>Chop: {mods.m_chop}</li>";
                damagemods += $"<li>Pickaxe: {mods.m_pickaxe}</li>";
                damagemods += $"<li>Fire: {mods.m_fire}</li>";
                damagemods += $"<li>Frost: {mods.m_frost}</li>";
                damagemods += $"<li>Lightning: {mods.m_lightning}</li>";
                damagemods += $"<li>Poison: {mods.m_poison}</li>";
                damagemods += $"<li>Spirit: {mods.m_spirit}</li>";
                damagemods += "/<ul>";

                AddTableRow(
                    name,
                    components,
                    damagemods
                );
            }

            Save();
        }
    }
}
