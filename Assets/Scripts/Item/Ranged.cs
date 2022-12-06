using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.Emitters;
using BulletHell.Abilities;

public class Ranged : Weapon
{
    EmitterData _emitterData;

    public EmitterData EmitterData { get { return _emitterData; } set { _emitterData = value; } }
}