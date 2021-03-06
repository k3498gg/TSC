﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr : MonoBehaviour
{
    private bool init = false;
    private Transform m_cameraRoot;
    private Transform m_playerRoot;
    private Transform m_itemRoot;
    private Transform m_entityRoot;
    private Transform m_obstacleRoot;
    private Transform m_particleRoot;
    private int mapId;
    private bool isEnterGame = false;
    private static GameMgr instance;

    private Entity m_MainEntity; //主角
    private Dictionary<string, int> downBinInfoDic;
    private CharacterController m_characController; //主角控制器
    private ARPGCameraController m_cameraController; //相机跟随控制
    private ARPGAnimatorController m_animController; //主角动作控制器

    public static GameMgr Instance
    {
        get
        {
            return instance;
        }
    }


    public ARPGAnimatorController ARPGAnimatController
    {
        get
        {
            if (null == m_animController)
            {
                m_animController = MainEntity.CacheModel.GetComponent<ARPGAnimatorController>();
            }
            return m_animController;
        }

        set
        {
            m_animController = value;
        }
    }

    public ARPGCameraController CameraController
    {
        get
        {
            if (null == m_cameraController)
            {
                m_cameraController = m_cameraRoot.Find("MainCamera").GetComponent<ARPGCameraController>();
            }
            return m_cameraController;
        }
    }

    public Entity MainEntity
    {
        get
        {
            if (null == m_MainEntity)
            {
                m_MainEntity = m_playerRoot.Find("Charactor").GetComponent<Entity>();
            }
            return m_MainEntity;
        }
    }

    public CharacterController CharacController
    {
        get
        {
            if (null == m_characController)
            {
                if (null != MainEntity)
                {
                    m_characController = MainEntity.CacheModel.GetComponent<CharacterController>();
                }
                else
                {
                    Debuger.LogError("MainEntity is Null!");
                }
            }
            return m_characController;
        }

        set
        {
            m_characController = value;
        }
    }

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

    public void Awake()
    {
        Init();
    }

    //private void Start()
    //{
    //    BeginGame();
    //}

    void Init()
    {
        if (init)
        {
            return;
        }
        DontDestroyOnLoad(gameObject);
        init = true;
        instance = this;
        Transform m_self = transform;
        m_cameraRoot = m_self.Find("Camera");
        m_playerRoot = m_self.Find("Player");
        m_itemRoot = m_self.Find("ItemRoot");
        m_entityRoot = m_self.Find("EntityRoot");
        ObstacleRoot = m_self.Find("ObstacleRoot");
        BeginGame();
        //LoadTemplate();
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

    //加载配置表数据
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

        Util.InitMap(path + "map.bin");
        Debug.LogError("Load Lanague");
        //初始化常量表数据
        AppConst.InitConstData();

        //BeginGame();
    }

    public void BeginGame()
    {
        if (IsEnterGame)
            return;
        IsEnterGame = true;
        TSCData.Instance.Clear();
        MapId = 2;
        InitMap();
        CreateEntity();
        UIManager.Instance.ShowWindow(WindowID.WindowID_MainUI);
        UIManager.Instance.HideWindow(WindowID.WindowID_FirstUI);
        StartCoroutine(CreateNetEntity());
    }

    void InitMap()
    {
        ItemDropMgr.Instance.InitMapDrop();
        ObstacleMgr.Instance.InitMapObstacle();
    }

    //保存版本号
    void SaveVersion()
    {
        PlayerPrefs.SetString(AppConst.VersionKey, AppConst.Version);
        PlayerPrefs.Save();
    }

    //判断当前是否最新版本
    bool IsNewVersion
    {
        get
        {
            string version = PlayerPrefs.GetString(AppConst.VersionKey);
            return !Util.Equals(version, AppConst.Version);
        }
    }

    public Transform ItemRoot
    {
        get
        {
            if (null == m_itemRoot)
            {
                m_itemRoot = transform.Find("ItemRoot");
            }
            return m_itemRoot;
        }

        set
        {
            m_itemRoot = value;
        }
    }

    public int MapId
    {
        get
        {
            return mapId;
        }

        set
        {
            mapId = value;
        }
    }

    public Transform EntityRoot
    {
        get
        {
            if (null == m_entityRoot)
            {
                m_entityRoot = transform.Find("EntityRoot");
            }
            return m_entityRoot;
        }

        set
        {
            m_entityRoot = value;
        }
    }

    public Transform ObstacleRoot
    {
        get
        {
            if (null == m_obstacleRoot)
            {
                m_obstacleRoot = transform.Find("ObstacleRoot");
            }
            return m_obstacleRoot;
        }

        set
        {
            m_obstacleRoot = value;
        }
    }

    public Transform ParticleRoot
    {
        get
        {
            if (null == m_particleRoot)
            {
                m_particleRoot = transform.Find("ParticleRoot");
            }
            return m_particleRoot;
        }

        set
        {
            m_particleRoot = value;
        }
    }

    public bool IsEnterGame
    {
        get
        {
            return isEnterGame;
        }

        set
        {
            isEnterGame = value;
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


    public Vector3 RandomLocation()
    {
        MapInfo map = TSCData.Instance.GetCurrentMapInfo(MapId);
        if (null == map)
        {
            return Vector3.one;
        }
        float width = map.Width;
        float height = map.Height;
        Vector3 v = new Vector3(Random.Range(1 - width, width - 1), 0, Random.Range(1 - height, height - 1));
        //while (true)
        //{
        foreach (KeyValuePair<int, ObstacleEntity> kv in TSCData.Instance.ObstacleDic)
        {
            if (Util.PtInRectArea(v, kv.Value.transform, kv.Value.Width + 0.5f, kv.Value.Height + 0.5f))
            {
                return RandomLocation();
            }
            return v;
        }
        return v;
        //}
    }

    #region 资源模型加载
    //主角模型加载
    void CreateEntity()
    {
        MainEntity.CreateEntity();
    }

    public void StopCoroutines()
    {
        StopAllCoroutines();
    }

    IEnumerator CreateNetEntity()
    {
        int count = Random.Range(AppConst.MinCount, AppConst.MaxCount);
        TSCData.Instance.EntityDic.Clear();
        for (int i = 0; i < count; i++)
        {
            GameObject go = ResourcesMgr.Instance.Spawner(AppConst.NET_Entity, ResourceType.RESOURCE_NET, EntityRoot);
            NetEntity net = Util.AddComponent<NetEntity>(go);
            net.CreateNetEntity(i);
            TSCData.Instance.EntityDic[net.Id] = net;
            yield return new WaitForSeconds(1);
        }
    }

    #endregion
}
