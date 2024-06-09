using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class SkillData
{
    public SkillType type;
    public int amount;
}

[System.Serializable]
public class SkillDataWrapper
{
    public List<SkillData> skillDataList;

    public SkillDataWrapper(Dictionary<SkillType, int> dict)
    {
        skillDataList = dict.Select(pair => new SkillData { type = pair.Key, amount = pair.Value }).ToList();
    }

    public Dictionary<SkillType, int> ToDictionary()
    {
        return skillDataList.ToDictionary(data => data.type, data => data.amount);
    }
}

