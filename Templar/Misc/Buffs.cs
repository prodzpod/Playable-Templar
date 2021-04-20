using RoR2;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Templar
{
	public static class Buffs
	{
		internal static void RegisterBuffs()
		{
			Buffs.TemplArmorBuff = Buffs.AddNewBuff("TemplArmorBuff", Resources.Load<Sprite>("Textures/BuffIcons/texBuffGenericShield"), new Color(0.54f, 0.21f, 0.12f), false, false);
			Buffs.TemplarstationaryArmorBuff = Buffs.AddNewBuff("TemplarstationaryArmorBuff", Resources.Load<Sprite>("Textures/BuffIcons/texBuffGenericShield"), new Color(0.74f, 0.41f, 0.32f), false, false);
			Buffs.TemplarigniteDebuff = Buffs.AddNewBuff("TemplarScorchedDebuff", Resources.Load<Sprite>("Textures/BuffIcons/texBuffPulverizeIcon"), new Color(0.3f, 0.3f, 0.3f), false, true);
			Buffs.TemplarOverdriveBuff = Buffs.AddNewBuff("TemplarOverdriveBuff", Resources.Load<Sprite>("Textures/BuffIcons/texWarcryBuffIcon"), new Color(0.84f, 0.51f, 0.42f), false, false);
		}

		internal static BuffDef AddNewBuff(string buffName, Sprite buffIcon, Color buffColor, bool canStack, bool isDebuff)
		{
			BuffDef buffDef = ScriptableObject.CreateInstance<BuffDef>();
			buffDef.name = buffName;
			buffDef.buffColor = buffColor;
			buffDef.canStack = canStack;
			buffDef.isDebuff = isDebuff;
			buffDef.eliteDef = null;
			buffDef.iconSprite = buffIcon;
			Buffs.buffDefs.Add(buffDef);
			return buffDef;
		}

		internal static BuffDef TemplArmorBuff;

		internal static BuffDef TemplarstationaryArmorBuff;

		internal static BuffDef TemplarigniteDebuff;

		internal static BuffDef TemplarOverdriveBuff;

		internal static readonly Color CHAR_COLOR2 = new Color(0.784313738f, 0.294117659f, 0.05882353f);

		internal static List<BuffDef> buffDefs = new List<BuffDef>();
	}
}
