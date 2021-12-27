using System;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.IL2CPP;
using Il2CppSystem;
using HarmonyLib;
using UnityEngine;
using UnhollowerBaseLib;
using System.IO;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace TheOtherRoles.Modules
{
    [HarmonyPatch]
    class CustomVisors
    {
        class CustomVisor
        {
            public string Idle;
            public string Floor;
            public string Name;
            public string Id;
            public bool FromDisk;

        }

        private static Sprite CreateVisorSprite(string path, bool fromDisk = false)
        {
            UnityEngine.Debug.Log(path + "\n");
            Texture2D texture = fromDisk ? Helpers.loadTextureFromDisk(path) : Helpers.loadTextureFromResources(path);
            if (texture == null)
            {
//                System.Console.WriteLine("Texture Error:"+path+"\n");
                return null;
            }
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.53f, 0.575f), texture.width * 0.5f);
            if (sprite == null)
            {
//                System.Console.WriteLine("Sprite Error\n");
                return null;
            }
            texture.hideFlags |= HideFlags.HideAndDontSave | HideFlags.DontUnloadUnusedAsset;
            sprite.hideFlags |= HideFlags.HideAndDontSave | HideFlags.DontUnloadUnusedAsset;
            return sprite;
        }
        private static bool IsExistImage(string path, bool fromDisk)
        {
            if (fromDisk)
            {
                return File.Exists(path);
            }
            else
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                try {
                    var st = assembly.GetManifestResourceInfo(path);
                    return st != null;
                }
                catch
                {
                    return false;
                }
            }
        }
        private static VisorData CreateVisorData(CustomVisor cv, bool fromDisk = false, bool testOnly = false)
        {
            VisorData visor = new VisorData
            {
                name = cv.Name,
                ProductId = cv.Id,
                ChipOffset = new Vector2(0f, 0.0f),
                Free = true,
                NotInStore = true
            };

            System.Console.WriteLine(cv.Idle + "\n");
            var ext = Path.GetExtension(cv.Idle);
            //            System.Console.WriteLine(ext + "\n");
            var basename = cv.Idle.Substring(0, cv.Idle.Length - ext.Length);
            //            System.Console.WriteLine(basename + "\n");

            visor.IdleFrame = CreateVisorSprite(cv.Idle, fromDisk);

            var IdleLeft = basename  + "_Left" + ext;
//            System.Console.WriteLine(IdleLeft + "\n");
            if (IsExistImage(IdleLeft, fromDisk))
            {
                visor.LeftIdleFrame = CreateVisorSprite(IdleLeft, fromDisk);

            }
//            visor.FloorFrame= CreateVisorSprite(cv.Floor, fromDisk);
            int order;
            if (basename.Last() == 'L')
            {
                order = 99;
            }
            else if (basename.Last() == 'H')
            {
                order = 99;
            }
            else
            {
                order = 98;
            }

            visor.Order = order;

            return visor;
        }
        private static List<CustomVisor> visorList;

        public static void CustomVisorSetup()
        {
            visorList = new List<CustomVisor>
                {
                new CustomVisor{
                    Idle ="TheOtherRoles.Resources.VisorTest.siune_heart_galss1.png",
//                    Floor="TheOtherRoles.Resources.VisorTest.siune_heart_galss1.png",
                    Name ="しうねPのハートサングラス",
                    Id   ="visor_siune_sea_glass",
                    FromDisk=false
                }
                ,
                new CustomVisor(){
                    Idle ="TheOtherRoles.Resources.VisorTest.siune_sun_galss1.png",
//                    Floor="TheOtherRoles.Resources.VisorTest.siune_sun_galss1.png",
                    Name ="しうねPのサングラス",
                    Id   ="visor_siune_sun_glass",
                    FromDisk=false
                },
                new CustomVisor(){
                    Idle ="TheOtherRoles.Resources.VisorTest.Visor_cat1.png",
//                    Floor="TheOtherRoles.Resources.VisorTest.Visor_cat1.png",
                    Name ="猫マズル",
                    Id   ="visor_cat",
                    FromDisk=false
                },
                new CustomVisor(){
                    Idle ="TheOtherRoles.Resources.VisorTest.visor_flog1.png",
//                    Floor="TheOtherRoles.Resources.VisorTest.visor_flog1.png",
                    Name ="カエルお面",
                    Id   ="visor_flog",
                    FromDisk=false
                },
                new CustomVisor(){
                    Idle ="TheOtherRoles.Resources.VisorTest.visorGundam1.png",
//                    Floor="TheOtherRoles.Resources.VisorTest.visorGundam1.png",
                    Name ="機動戦士〇ンダム",
                    Id   ="visor_gundam",
                    FromDisk=false
                },
                new CustomVisor(){
                    Idle ="TheOtherRoles.Resources.VisorTest.VisorZaku.png",
//                    Floor="TheOtherRoles.Resources.VisorTest.visorGundam1.png",
                    Name ="ZK",
                    Id   ="visorZaku",
                    FromDisk=false
                },
                new CustomVisor(){
                    Idle ="TheOtherRoles.Resources.VisorTest.VisorZakuA.png",
//                    Floor="TheOtherRoles.Resources.VisorTest.visorGundam1.png",
                    Name ="ZK(カラー)",
                    Id   ="visorZakuA",
                    FromDisk=false
                },
                new CustomVisor(){
                    Idle ="TheOtherRoles.Resources.VisorTest.VisorVR.png",
//                    Floor="TheOtherRoles.Resources.VisorTest.VisorVR.png",
                    Name ="VRゴーグル",
                    Id   ="VisorVR",
                    FromDisk=false
                },
                new CustomVisor(){
                    Idle ="TheOtherRoles.Resources.VisorTest.VisorClackL.png",
//                    Floor="TheOtherRoles.Resources.VisorTest.VisorClack.png",
                    Name ="ひび割れ",
                    Id   ="VisorClackL",
                    FromDisk=false
                },
                new CustomVisor(){
                    Idle ="TheOtherRoles.Resources.VisorTest.VisorSushi.png",
//                    Floor="TheOtherRoles.Resources.VisorTest.VisorSushi.png",
                    Name ="寿司（サーモン）",
                    Id   ="VisorSushi",
                    FromDisk=false
                },
                new CustomVisor(){
                    Idle ="TheOtherRoles.Resources.VisorTest.VisorMasamune.png",
//                    Floor="TheOtherRoles.Resources.VisorTest.VisorTemplate.png",
                    Name ="独眼竜",
                    Id   ="VisorMasamune",
                    FromDisk=false
                },
                new CustomVisor(){
                    Idle ="TheOtherRoles.Resources.VisorTest.VisorFrontHairL.png",
//                    Floor="TheOtherRoles.Resources.VisorTest.VisorTemplate.png",
                    Name ="キザ髪",
                    Id   ="VisorFrontHairL",
                    FromDisk=false
                },
                new CustomVisor(){
                    Idle ="TheOtherRoles.Resources.VisorTest.VisorAngry.png",
//                    Floor="TheOtherRoles.Resources.VisorTest.VisorTemplate.png",
                    Name ="おこ！",
                    Id   ="VisorAngry",
                    FromDisk=false
                },
                new CustomVisor(){
                    Idle ="TheOtherRoles.Resources.VisorTest.VisorHeart.png",
//                    Floor="TheOtherRoles.Resources.VisorTest.VisorTemplate.png",
                    Name ="はぁと♪",
                    Id   ="VisorHeart",
                    FromDisk=false
                },
                new CustomVisor(){
                    Idle ="TheOtherRoles.Resources.VisorTest.VisorMouth.png",
//                    Floor="TheOtherRoles.Resources.VisorTest.VisorTemplate.png",
                    Name ="ビッグマウス",
                    Id   ="VisorMouth",
                    FromDisk=false
                },
/*
                new customVisor(){
                    Idle ="TheOtherRoles.Resources.VisorTest.VisorTemplate.png",
//                    Floor="TheOtherRoles.Resources.VisorTest.VisorTemplate.png",
                    Name ="テンプレ",
                    Id   ="VisorTemplate",
                    FromDisk=false
                },
//*/
            };
            string filePath = Path.Combine(Path.GetDirectoryName(Application.dataPath), "VisorTest");
            if (Directory.Exists(filePath))
            {
                var files = Directory.GetFiles(filePath);
                foreach (var text in files.Where(x => x.EndsWith(".txt")))
                {
                    var visorSetting = File.ReadAllLines(text);
                    var visor = new CustomVisor
                    {
                        Idle = Path.Combine(filePath, visorSetting[0]),
                        Floor = Path.Combine(filePath, visorSetting[1]),
                        Name = visorSetting[2],
                        Id = visorSetting[3],
                        FromDisk = true
                    };

                    visorList.Add(visor);
                }

            }

        }

        [HarmonyPatch(typeof(HatManager), nameof(HatManager.GetVisorById), typeof(string))]
        public static class HatManagerPatch
        {
            private static bool LOADED;
            private static bool RUNNING;

            static void Prefix(string visorId, HatManager __instance)
            {
                FileLog.Log("Prefix HatManager.GetVisorById");
                FileLog.Log(" Visor:" + visorId);
                if (RUNNING) return;
                RUNNING = true; // prevent simultanious execution

                try
                {
                    if (!LOADED)
                    {
                        FileLog.Log(" Visor Loading");
                        while (visorList.Count != 0)
                        {
                            var cv = visorList[0];
                            visorList.RemoveAt(0);
                            //                            UnityEngine.Debug.Log("visor Add:" + cv.ToString() + "\n");
                            __instance.AllVisors.Add(CreateVisorData(cv, cv.FromDisk));
                        }
                    }
                }
                catch (System.Exception)
                {
                    if (!LOADED)
                    {
                        //                        UnityEngine.Debug.Log("Unable to add Custom Hats\n" + e);

                    }
                }
                LOADED = true;
            }
            static void Postfix(HatManager __instance)
            {
                RUNNING = false;
            }
        }
    }
    /*
        [HarmonyPatch(typeof(HatManager.__c__DisplayClass16_0), nameof(HatManager.__c__DisplayClass16_0._GetVisorById_b__0), typeof(VisorData))]
        public static class HatManagerDC16Patch
        {
            static void Prefix(VisorData v, HatManager.__c__DisplayClass16_0 __instance)
            {
                FileLog.Log("Prefix HatManager.__c__DisplayClass16_0.GetVisorById Visor:" + v.ProductId);
            }
        }
    */

    //*
    [HarmonyPatch(typeof(VisorsTab))]
    public class VisorsTabPatch
    {
        //*
        [HarmonyPostfix]
        [HarmonyPatch(nameof(VisorsTab.SelectVisor), new System.Type[] { typeof(ColorChip), typeof(VisorData) })]
        public static void SelectVisorPath(ColorChip chip, VisorData visor, VisorsTab __instance)
        {
            FileLog.Log("Pre.VisorsTab.SelectVisor");
            if (visor!=null)
            {
                FileLog.Log(" Visor: " + visor?.ProductId);
            }
            else
            {
                FileLog.Log(" Visor: null");
            }
            if (VisorLayerPatch.altShader != null)
            {
                if (visor.ProductId.Last() == 'A')
                {
                    FileLog.Log(" visor Shader Changed");
                    __instance.PlayerPreview.VisorSlot.Image.material = VisorLayerPatch.altShader;
                    //                    int colorID = PlayerControl.LocalPlayer.CurrentOutfit.ColorId;
                    //                    Color bodycolor = Palette.PlayerColors[colorID];
                    //                    Color shadowcolor = Palette.ShadowColors[colorID];
                    //FileLog.Log(" colorID:" + colorID);
                    Color bodycolor = __instance.PlayerPreview.Body.material.GetColor(2);
                    Color shadowcolor = __instance.PlayerPreview.Body.material.GetColor(1);
                    FileLog.Log(" Body Color:" + bodycolor.ToString());
                    __instance.PlayerPreview.VisorSlot.Image.material.SetColor(2, bodycolor);
                    __instance.PlayerPreview.VisorSlot.Image.material.SetColor(1,shadowcolor);
                    FileLog.Log(" altShader:" + VisorLayerPatch.altShader);
                    //                    FileLog.Log(" BodyColor:" + __instance.PlayerPreview.Body.color.ToString());
                }
                else
                {
                    FileLog.Log(" visor Shader Original");
                    __instance.PlayerPreview.VisorSlot.Image.material = VisorLayerPatch.orgShader;
                    //                    __instance.PlayerPreview.VisorSlot.Image.color = VisorLayerSetVisorPatch.orgColor;

                }
            }
        }
        //*/
    }
    /*
    [HarmonyPatch(typeof(HatParent))]
    public static class HatParentPatch
    {
        [HarmonyPatch(nameof(HatParent.FrontLayer), MethodType.Setter)]
        static void Prefix(SpriteRenderer value, HatParent __instance)
        {
            FileLog.Log("HatParent.FrontLayer Setter");

        }
    }
    */
    /*
        [HarmonyPatch(typeof(ColorChip))]
        public static class ColorChiptPatch
        {
            [HarmonyPrefix]
            [HarmonyPatch(nameof(ColorChip.Inner), MethodType.Setter)]
            static void Prefix(ColorChip __instance, HatParent value)
            {
                FileLog.Log("ColorChip.Inner Setter");
                FileLog.Log(" HatParentType:" + value.GetType().ToString());
            }
        }
    */
    [HarmonyPatch(typeof(VisorLayer))]
    public class VisorLayerPatch
    {
        public static Material orgShader;
        public static Color orgColor;
        public static Material altShader;

        [HarmonyPostfix]
        [HarmonyPatch(nameof(VisorLayer.SetVisor), typeof(VisorData))]
        public static void fix_SetVisor_VisorData(VisorData data,VisorLayer __instance)
        {
            FileLog.Log("post.VisorLayer.SetVisor(VisorData)");
            FileLog.Log(" Visor:" + data.ProductId);
            UnityEngine.Debug.Log("VisorLayer.SetVisor:" + data.ProductId);
            //           UnityEngine.Debug.Log(" org_z:" + org_z);
            UnityEngine.Debug.Log(" localpos.z:" + __instance.transform.localPosition.z);

            /*
                        if (org_z == 0.0f)
                        {
                            UnityEngine.Debug.Log(" Set org_z");
                            org_z = __instance.transform.position.z;
                        }
                        if (data.ProductId.Last() == 'L')
                        {
                            var newpos = __instance.transform.localPosition;
                            newpos.z = -0.0001f;
                            __instance.transform.localPosition = newpos;
                        }
                        else
                        {
                            var newpos = __instance.transform.localPosition;
                            newpos.z = -0.0002f;
                            __instance.transform.localPosition = newpos;

                        }
            */
            if (orgColor == null) orgColor = __instance.Image.color;
            if (orgShader == null) orgShader = __instance.Image.material;
            if (altShader == null)
            {
                if (DestroyableSingleton<HatManager>.InstanceExists)
                {
                    FileLog.Log(" Serch AltShader");
                    foreach (HatBehaviour h in DestroyableSingleton<HatManager>.Instance.AllHats)
                    {
                        if (h.AltShader != null)
                        {
                            FileLog.Log(" AltShader found");
                            altShader = h.AltShader;
                            break;
                        }
                    }
                }
            }
            FileLog.Log(" orgColor:" + orgColor.ToString());
            FileLog.Log(" orgShader:" + orgShader.ToString());
            FileLog.Log(" altShader:" + altShader.ToString());

            /*
            if (altShader!=null)
            {
                if (data.ProductId.Last() == 'A' )
                {
                    FileLog.Log(" visor Shader Changed\n");
                    __instance.Image.material = altShader;
                    __instance.Image.color = Palette.PlayerColors[7];

                }
                else
                {
                    FileLog.Log(" visor Original Shader");
                    __instance.Image.material = orgShader;
                    __instance.Image.color = orgColor;
                }
            }
            //*/
        }

        [HarmonyPostfix]
        [HarmonyPatch(nameof(VisorLayer.SetVisor), typeof(string))]
        public static void fix_SetVisor_string(string visorId, VisorLayer __instance)
        {
            FileLog.Log("post.VisorLayer.SetVisor(string)");
            FileLog.Log(" Visor:" + visorId);
            /*
            if (orgColor == null) orgColor = __instance.Image.color;
            if (orgShader == null) orgShader = __instance.Image.material;
            if (altShader == null && DestroyableSingleton<HatManager>.InstanceExists)
            {
                FileLog.Log(" Serch AltShader");
                foreach (HatBehaviour h in DestroyableSingleton<HatManager>.Instance.AllHats)
                {
                    if (h.AltShader != null)
                    {
                        FileLog.Log(" AltShader found");
                        altShader = h.AltShader;
                        break;
                    }
                }
            }

            if (altShader!=null)
            {
                if (visorId.Last() == 'A')
                {
                    FileLog.Log(" visor Shader Changed\n");
                    __instance.Image.material = altShader;
                    __instance.Image.color = Palette.PlayerColors[7];

                }
                else
                {
                    FileLog.Log(" visor Original Shader");
                    __instance.Image.material = orgShader;
                    __instance.Image.color = orgColor;
                }
            }
            */
        }
    }
}
