using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using System.IO;

public class AnimatorExport : Editor
{
    #region  提取FBX里面的动画组件
    [MenuItem("Mecanim/ExtractAnimClip", false, 1)]
    static void GenerateAnimationClip()
    {
        string clipPath = Application.dataPath + "/AnimationClip";
        if (System.IO.Directory.Exists(clipPath))
        {
            System.IO.Directory.Delete(clipPath, true);
        }

        Directory.CreateDirectory(clipPath);

        string[] paths = new string[]
        {
            Application.dataPath + "/Charactor"
        };
        for (int i = 0; i < paths.Length; i++)
        {
            string[] files = Directory.GetFiles(paths[i], "*.fbx", SearchOption.AllDirectories);
            for (int j = 0; j < files.Length; j++)
            {
                if (!string.IsNullOrEmpty(files[j]))
                {
                    string assetpath = files[j].Substring(files[j].IndexOf("Assets"));
                    Object[] objects = AssetDatabase.LoadAllAssetsAtPath(assetpath);
                    for (int m = 0; m < objects.Length; m++)
                    {
                        if (objects[m] is AnimationClip)
                        {
                            AnimationClip clip = (AnimationClip)objects[m];
                            ExtractFbxAnimClip(files[j], clip, clipPath);
                        }
                    }
                }
            }
        }
        AssetDatabase.Refresh();
    }

    //将FBX目录的clip动画文件保存到制定目录
    static void ExtractFbxAnimClip(string assetPath, Object objects, string clipPath)
    {
        AnimationClip newClip = new AnimationClip();
        EditorUtility.CopySerialized(objects, newClip);
        newClip.legacy = false;
        string fbxName = Path.GetFileNameWithoutExtension(assetPath);
        string filePath = clipPath + "/" + fbxName + "/" + objects.name + ".anim";
        string dir = Path.GetDirectoryName(filePath);
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
        string assetName = filePath.Substring(filePath.IndexOf("Assets"));
        AssetDatabase.CreateAsset(newClip, assetName);
    }
    #endregion


    #region 动态创建动画控制器
    [MenuItem("Mecanim/GenerateAnimatorController", false, 2)]
    static void GenerateAnimatorController()
    {
        EditorUtil.ClearConsole();
        string path = Application.dataPath + "/Animator";
        if (Directory.Exists(path))
        {
            Directory.Delete(path, true);
        }

        Directory.CreateDirectory(path);

        //对project目录下的prefab文件遍历创建动画状态机
        string projectPath = Application.dataPath + "/Resources/Model";
        string[] files = Directory.GetFiles(projectPath, "*.prefab", SearchOption.AllDirectories);
        CreateAnimator(files);
        AssetDatabase.SaveAssets();
    }

    static void CreateAnimator(string[] files)
    {
        string path = new System.Text.StringBuilder().Append(@Application.streamingAssetsPath).Append('/').Append(AppConst.TextDir).Append('/').ToString();
        EditorUtil.Init<AnimatorInfo>(path);
        EditorUtil.Init<AnimatorTransInfo>(path);
        EditorUtil.Init<AnimatorTransfer>(path);
        int width = 250;
        int height = 50;
        for (int i = 0; i < files.Length; i++)
        {
            string pName = Path.GetFileNameWithoutExtension(files[i]);
            AnimatorController controller = AnimatorController.CreateAnimatorControllerAtPath(AppConst.AssetPath + "/Animator/" + pName + ".controller");
            AnimatorControllerLayer currLayer = null;
            if (controller.layers.Length > 0)
            {
                currLayer = controller.layers[0];
            }
            else
            {
                currLayer = new AnimatorControllerLayer();
                currLayer.name = "Base Layer";
                controller.AddLayer(currLayer);
            }

            currLayer.stateMachine.anyStatePosition = new Vector3(0, height * 3, 0);
            currLayer.stateMachine.entryPosition = new Vector3(0, height, 0);
            currLayer.stateMachine.exitPosition = new Vector3(((int)AnimatState.AnimaState_MAX / 5 + 1) * width, height * 3, 0);
            AddAnimatorState(controller, currLayer, width, height, pName);
            AddParameters(controller);
            AddTransition(currLayer);
        }
    }

    static void AddAnimatorState(AnimatorController controller, AnimatorControllerLayer currLayer, int width, int height, string pName)
    {
        for (int j = 1; j < (int)AnimatState.AnimaState_MAX; j++)
        {
            AnimatorState state = currLayer.stateMachine.AddState(((AnimatState)j).ToString(), new Vector3((j / 5 + 1) * width, j % 5 * height, 0));
            if (j == (int)AnimatState.AnimaState_IDLE)
            {
                currLayer.stateMachine.defaultState = state; //设置默认状态
            }
            AnimatorInfo info = InfoMgr<AnimatorInfo>.Instance.GetInfo(j);
            AnimationClip clip = (AnimationClip)AssetDatabase.LoadAssetAtPath(AppConst.AssetPath + "/AnimationClip/" + pName + "/" + info.name + ".anim", typeof(AnimationClip));
            controller.SetStateEffectiveMotion(state, clip);
        }
    }

    static void AddTransition(AnimatorControllerLayer controllLayer)
    {
        ChildAnimatorState[] states = controllLayer.stateMachine.states;
        for (int i = 0; i < (int)AnimatState.AnimaState_MAX - 1; i++)
        {
            if (i < states.Length)
            {
                AnimatorInfo info = InfoMgr<AnimatorInfo>.Instance.GetInfo(i + 1);
                if (null == info)
                {
                    Debug.LogError("AnimatorInfo is Null");
                    continue;
                }
                AnimatorState currentState = states[i].state;
                if (info.idle == 1)
                {
                    AddTransition(currentState, states[(int)AnimatState.AnimaState_IDLE - 1].state, false);
                }
                if (info.walk == 1)
                {
                    AddTransition(currentState, states[(int)AnimatState.AnimaState_WALK - 1].state, false);
                }
                if (info.free == 1)
                {
                    AddTransition(currentState, states[(int)AnimatState.AnimaState_FREE - 1].state, false);
                }
                if (info.atk1 == 1)
                {
                    AddTransition(currentState, states[(int)AnimatState.AnimaState_ATK1 - 1].state, false);
                }
                if (info.atk2 == 1)
                {
                    AddTransition(currentState, states[(int)AnimatState.AnimaState_ATK2 - 1].state, false);
                }
                if (info.atk3 == 1)
                {
                    AddTransition(currentState, states[(int)AnimatState.AnimaState_ATK3 - 1].state, false);
                }
                if (info.skill1 == 1)
                {
                    AddTransition(currentState, states[(int)AnimatState.AnimaState_SKILL1 - 1].state, false);
                }
                if (info.skill2 == 1)
                {
                    AddTransition(currentState, states[(int)AnimatState.AnimaState_SKILL2 - 1].state, false);
                }
                if (info.skill3 == 1)
                {
                    AddTransition(currentState, states[(int)AnimatState.AnimaState_SKILL3 - 1].state, false);
                }
                if (info.die == 1)
                {
                    AddTransition(currentState, states[(int)AnimatState.AnimaState_DIE - 1].state, false);
                }
                if (info.live == 1)
                {
                    AddTransition(currentState, states[(int)AnimatState.AnimaState_LIVE - 1].state, false);
                }
            }
        }
    }

    static void AddTransition(AnimatorState from, AnimatorState to, bool exitTime)
    {
        AnimatorStateTransition transition = from.AddTransition(to, exitTime);
        AddTransitionParam(from, to, transition);
    }

    static void AddParameters(AnimatorController controller)
    {
        controller.AddParameter(TransferParam.FREE.ToString(), AnimatorControllerParameterType.Trigger);
        controller.AddParameter(TransferParam.WALK.ToString(), AnimatorControllerParameterType.Bool);
        controller.AddParameter(TransferParam.ATK.ToString(), AnimatorControllerParameterType.Int);
        controller.AddParameter(TransferParam.SKILL.ToString(), AnimatorControllerParameterType.Int);
        controller.AddParameter(TransferParam.DIE.ToString(), AnimatorControllerParameterType.Bool);
    }

    static void SetTransitionParam(AnimatorControllerLayer controllLayer)
    {
        ChildAnimatorState[] states = controllLayer.stateMachine.states;
        for (int i = 0; i < states.Length; i++)
        {
            AnimatState state = (AnimatState)System.Enum.Parse(typeof(AnimatState), states[i].state.name);
            switch (state)
            {
                case AnimatState.AnimaState_IDLE:

                    break;
                case AnimatState.AnimaState_FREE:

                    break;
                case AnimatState.AnimaState_WALK: break;
                case AnimatState.AnimaState_ATK1: break;
                case AnimatState.AnimaState_ATK2: break;
                case AnimatState.AnimaState_ATK3: break;
                case AnimatState.AnimaState_SKILL1: break;
                case AnimatState.AnimaState_SKILL2: break;
                case AnimatState.AnimaState_SKILL3: break;
                case AnimatState.AnimaState_DIE: break;
                case AnimatState.AnimaState_LIVE: break;
            }

            Debug.LogError(states[i].state.name);
            AnimatorStateTransition[] transtions = states[i].state.transitions;
            for (int j = 0; j < transtions.Length; j++)
            {
                //transtions[j].
            }
        }
    }

    static void AddTransitionParam(AnimatorState from, AnimatorState to, AnimatorStateTransition transition)
    {
        Debug.LogError("from:" + from.name + " ->   to:" + to);
        AnimatState f_state = (AnimatState)System.Enum.Parse(typeof(AnimatState), from.name);
        AnimatState t_state = (AnimatState)System.Enum.Parse(typeof(AnimatState), to.name);
        AnimatorTransInfo transInfo = InfoMgr<AnimatorTransInfo>.Instance.GetInfo((int)f_state);
        switch (t_state)
        {
            case AnimatState.AnimaState_IDLE:
                ParseTransitionParam(transInfo.idle, transition);
                break;
            case AnimatState.AnimaState_FREE:
                ParseTransitionParam(transInfo.free, transition);
                break;
            case AnimatState.AnimaState_WALK:
                ParseTransitionParam(transInfo.walk, transition);
                break;
            case AnimatState.AnimaState_ATK1:
                ParseTransitionParam(transInfo.atk1, transition);
                break;
            case AnimatState.AnimaState_ATK2:
                ParseTransitionParam(transInfo.atk2, transition);
                break;
            case AnimatState.AnimaState_ATK3:
                ParseTransitionParam(transInfo.atk3, transition);
                break;
            case AnimatState.AnimaState_SKILL1:
                ParseTransitionParam(transInfo.skill1, transition);
                break;
            case AnimatState.AnimaState_SKILL2:
                ParseTransitionParam(transInfo.skill2, transition);
                break;
            case AnimatState.AnimaState_SKILL3:
                ParseTransitionParam(transInfo.skill3, transition);
                break;
            case AnimatState.AnimaState_DIE:
                ParseTransitionParam(transInfo.die, transition);
                break;
            case AnimatState.AnimaState_LIVE:
                ParseTransitionParam(transInfo.live, transition);
                break;
        }
    }

    static void ParseTransitionParam(string val, AnimatorStateTransition transition)
    {
        if (string.IsNullOrEmpty(val) || val.Equals('0'))
        {
            return;
        }
        Debug.LogError(val);
        string[] parms = val.Split(',');
        for (int i = 0; i < parms.Length; i++)
        {
            int id = int.Parse(parms[i]);
            AnimatorTransfer transfer = InfoMgr<AnimatorTransfer>.Instance.GetInfo(id);
            if (null != transfer)
            {
                switch (transfer.type)
                {
                    case TransferValue.TransferValue_BOOL:
                        if (transfer.value == 1)
                        {
                            transition.AddCondition(AnimatorConditionMode.If, transfer.value, transfer.param.ToString());
                        }
                        else if (transfer.value == 0)
                        {
                            transition.AddCondition(AnimatorConditionMode.IfNot, transfer.value, transfer.param.ToString());
                        }
                        break;
                    case TransferValue.TransferValue_INT:
                        transition.AddCondition(AnimatorConditionMode.Equals, transfer.value, transfer.param.ToString());
                        break;
                    case TransferValue.TransferValue_TRIGGER:
                        transition.AddCondition(AnimatorConditionMode.If, transfer.value, transfer.param.ToString());
                        break;
                    case TransferValue.TransferValue_EXIT:
                        //transition.AddCondition(AnimatorConditionMode.If, transfer.value, transfer.param.ToString());
                        transition.hasExitTime = true;
                        break;
                }
            }
        }
    }



    #endregion
}
