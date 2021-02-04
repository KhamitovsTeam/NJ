using System.Collections.Generic;

namespace Chip
{
    /// <summary>
    /// Словарь катсцен.
    ///
    /// Ключом является строка, которая прописывается в OgmoEditor, а значением является
    /// класс катсцены.
    /// </summary>
    public static class Cutscenes
    {
        public static readonly Dictionary<string, CutsceneEntity> AllCutscenes = new Dictionary<string, CutsceneEntity>();

        static Cutscenes()
        {
            AllCutscenes.Add("cs100", new CS100_RocketLanding(Player.Instance));
            AllCutscenes.Add("cs120", new CS120_ShowElevator());
            AllCutscenes.Add("cs121", new CS121_StartElevator());
            AllCutscenes.Add("cs122", new CS122_BossKilled());
            AllCutscenes.Add("cs123", new CS123_KittensSaved());
            AllCutscenes.Add("cs130", new CS130_DisableLaser());
            AllCutscenes.Add("cs225", new CS225_OpenDoor1());
            AllCutscenes.Add("cs245", new CS245_OpenDoor2());
            AllCutscenes.Add("cs235", new CS235_OpenDoor3());
            AllCutscenes.Add("cs234", new CS234_OpenDoor4());
            AllCutscenes.Add("cs238", new CS238_OpenDoor5());
            AllCutscenes.Add("cs310", new CS310_DisableLaser());
            AllCutscenes.Add("cs325", new CS325_BossKilled());
            AllCutscenes.Add("cs349", new CS349_DisableLaser());
            AllCutscenes.Add("cs521", new CS521_OpenDoor());
        }
    }
}