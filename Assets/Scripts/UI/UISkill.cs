using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISkill : MonoBehaviour
{
    private int skillId;

    public int SkillId
    {
        get
        {
            return skillId;
        }

        set
        {
            skillId = value;
        }
    }
}
