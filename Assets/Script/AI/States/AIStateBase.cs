﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIStateBase : ScriptableObject {

    public abstract void Init();
    public abstract void Update();

}