﻿using System;

namespace TemplarSkins
{
    public class RegisterModdedAchievementAttribute : Attribute
    {
        public readonly string identifier;
        public readonly string unlockableRewardIdentifier;
        public string prerequisiteAchievementIdentifier;
        public readonly uint lunarCoinReward;
        public readonly Type serverTrackerType;
        public readonly string[] mods;

        public RegisterModdedAchievementAttribute(
          string identifier,
          string unlockableRewardIdentifier,
          string prerequisiteAchievementIdentifier,
          uint lunarCoinReward,
          Type serverTrackerType = null,
          params string[] mods)
        {
            this.identifier = identifier;
            this.unlockableRewardIdentifier = unlockableRewardIdentifier;
            this.prerequisiteAchievementIdentifier = prerequisiteAchievementIdentifier;
            this.serverTrackerType = serverTrackerType;
            this.mods = mods;
            this.lunarCoinReward = lunarCoinReward;
        }
    }
}
