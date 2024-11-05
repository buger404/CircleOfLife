using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CircleOfLife
{
    

    /////////Stats
    public enum AnimalStat
    {

    }

    public enum EnemyStat
    {

    }

    public enum BuildStat
    {
        TreatmentStation, LogisticsService, SignalTransmitter, ProtectiveNet,
        ElectricFencing, BladeFencing
    }

    /////////GameObject
    public enum AnimatonPrefab
    {

    }

    public enum EnemySkillType
    {
        test1
    }

    public enum PlayerSkillType
    {
        Whack, Slash, Skill3, test4, test5, test6
    }

    public enum BuildSkillType
    {
        TreatmentStation,SignalTransmitter1,TestBuildFriendFire
    }

    public enum AnimalSkillType
    {
        test1
    }

    public static class EnumExtendFuncs
    {
        public static FactionType Reversal(this FactionType factionType)
        {
            if (factionType.Equals(FactionType.Enemy)) return FactionType.Friend;
            return FactionType.Enemy;
        }
    }


}
