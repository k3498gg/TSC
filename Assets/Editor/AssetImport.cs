using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class AssetImport : AssetPostprocessor
{
    void OnPostprocessModel(GameObject g)
    {
        //Debug.LogError(g.name + " " + this.assetImporter.assetPath);
        //Object[] objs = AssetDatabase.LoadAllAssetsAtPath(this.assetImporter.assetPath);

        //foreach (Object o in objs)
        //{
        //    if (o is AnimationClip)
        //    {
        //        Debug.Log(o.name + "is clip");
        //    }
        //}

        Random.InitState(123450192);
        Debug.Log(Random.Range(1000, 5000));
        Debug.Log(Random.Range(1000, 5000));
        Debug.Log(Random.Range(1000, 5000));
    }
}
