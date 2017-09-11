using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataLoad : MonoBehaviour
{
    private Dictionary<string, int> downBinInfoDic;

    public Dictionary<string, int> DownBinInfoDic
    {
        get
        {
            if (null == downBinInfoDic)
            {
                downBinInfoDic = new Dictionary<string, int>();
            }
            return downBinInfoDic;
        }
        set
        {
            downBinInfoDic = value;
        }
    }

    void Start()
    {
        LoadTemplate();
    }

    bool IsNewVersion
    {
        get
        {
            string version = PlayerPrefs.GetString(AppConst.VersionKey);
            return !Util.Equals(version, AppConst.Version);
        }
    }

    //加载模板数据
    void LoadTemplate()
    {
        if (IsNewVersion)
        {
            StartCoroutine(DownFiles(AppConst.AppContentPath, AppConst.AppPersistentPath));
        }
        else
        {
            InitTemplate();
        }
    }

    IEnumerator DownFiles(string from, string dest)
    {
        yield return StartCoroutine(DownBinFileInfo(from + AppConst.FileBin));

        yield return StartCoroutine(PersistFiles(from + AppConst.TextDir + "/", dest + "/" + AppConst.TextDir + "/"));

        DownBinInfoDic.Clear();

        SaveVersion();

        InitTemplate();
    }

    void SaveVersion()
    {
        PlayerPrefs.SetString(AppConst.VersionKey, AppConst.Version);
        PlayerPrefs.Save();
    }

    IEnumerator DownBinFileInfo(string path)
    {
        using (WWW www = new WWW(path))
        {
            yield return www;
            if (string.IsNullOrEmpty(www.error))
            {
                if (www.isDone)
                {
                    string text = www.text;
                    string[] lines = text.Split('\r', '\n');
                    for (int i = 0; i < lines.Length; i++)
                    {
                        if (string.IsNullOrEmpty(lines[i]))
                        {
                            continue;
                        }
                        string[] contents = lines[i].Split(AppConst.Separate);
                        if (contents.Length > 1)
                        {
                            DownBinInfoDic[contents[0]] = int.Parse(contents[1]);
                        }
                    }
                }
            }
            else
            {
                Debuger.LogError(www.error + " " + path);
            }
        }
    }

    IEnumerator PersistFiles(string from, string dest)
    {
        if (!System.IO.Directory.Exists(dest))
        {
            System.IO.Directory.CreateDirectory(dest);
        }

        foreach (KeyValuePair<string, int> kv in DownBinInfoDic)
        {
            string fromFile = from + kv.Key;
            string destFile = dest + kv.Key;

            using (WWW www = new WWW(fromFile))
            {
                yield return www;

                if (string.IsNullOrEmpty(www.error))
                {
                    if (www.isDone)
                    {
                        System.IO.File.WriteAllBytes(destFile, www.bytes);
                    }
                }
                else
                {
                    Debuger.LogError(www.error);
                }

                yield return null;
            }
        }
        yield return new WaitForEndOfFrame();
    }

    void InitTemplate()
    {
        string path = new System.Text.StringBuilder().Append(AppConst.AppPersistentPath).Append('/').Append(AppConst.TextDir).Append('/').ToString();
        Util.Init<HeroInfo>(path);
        Util.Init<EffectInfo>(path);
        Util.Init<ParticleInfo>(path);
        Util.Init<SkillInfo>(path);
        Util.Init<LanSurInfo>(path);
        Util.Init<LanTxtInfo>(path);
        Util.Init<ConstInfo>(path);
        Util.Init<OccupationInfo>(path);
        Util.Init<ItemInfo>(path);
        Util.Init<LevelInfo>(path);
        Util.Init<ItemEffectInfo>(path);
        Util.Init<ObstacleInfo>(path);
        Util.Init<NameInfo>(path);
        Util.Init<EquipInfo>(path);
        Util.Init<FashionInfo>(path);
        Util.InitMap(path + "map.bin");
        TSCData.Instance.ReadHeroData();
        Util.GetHeroUseSkin();

        AppConst.InitConstData();
        Debug.LogError("Start Game");
        UIManager.Instance.ShowWindow(WindowID.WindowID_FirstUI);
    }
}
