﻿using System;
using System.Collections;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using BepInEx;
using UnityEngine;
using JotunnDoc.Docs;
using System.IO;
using HarmonyLib;

namespace JotunnDoc
{
    [BepInPlugin("com.bepinex.plugins.jotunndoc", "JotunnDoc", "0.1.0")]
    [BepInDependency(Jotunn.Main.ModGuid)]
    public class JotunnDoc : BaseUnityPlugin
    {
        public static JotunnDoc Instance { get; private set; }
        private List<Doc> docs;
        private Harmony harmony;

        private void Awake()
        {
            Instance = this;

            harmony = new Harmony("com.jotunn.jotunndoc");
            harmony.PatchAll();

            Doc.DocumentationDirConfig = Config.Bind(new BepInEx.Configuration.ConfigDefinition("Folders", "Documentation"), Path.Combine(Paths.PluginPath, nameof(JotunnDoc), "Docs"));

            docs = new List<Doc>()
            {
//                new CharacterDoc(),
//                new InputDoc(),
//                new ItemDoc(),
                new ItemDocFull(),
//                new LanguageDoc(),
//                new LocationDoc(),
//                new MaterialDoc(),
//                new PieceDoc(),
//                new PieceTableDoc(),
//                new PrefabDoc(),
//                new RecipeDoc(),
//                new RPCDoc(),
//                new ShaderDoc(),
//                new SpriteDoc(),
//                new StatusEffectDoc(),
//                new VegetationDoc(),
//                new EventDoc(),
            };

            Debug.Log("Initialized JotunnDoc");
        }

        // Localize and strip text from text
        public static string Localize(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return "NULL";
            }

            string text = Localization.instance.Localize(key).Replace("\n", "<br/>");
            text = Regex.Replace(text, @"[^\u0000-\u007F]+", string.Empty);

            return text;
        }

        public static void StartDelayed(Action action)
        {
            Instance.StartCoroutine(RunDelayed(action));
        }

        private static IEnumerator RunDelayed(Action action)
        {
            yield return null;
            action();
        }
    }
}
