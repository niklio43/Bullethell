using BulletHell.Scenes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] MainSceneData data;

    public void TestTest()
    {
        SceneLoader.LoadMainScene(data);
    }
}
