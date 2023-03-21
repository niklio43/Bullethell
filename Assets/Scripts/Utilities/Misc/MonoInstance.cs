using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoInstance : Singleton<MonoInstance>
{
    public static MonoBehaviour GetInstance() => Instance;
}
