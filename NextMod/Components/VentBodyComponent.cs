using NEXT.Utilities;
using Reactor.Utilities.Attributes;
using Reactor.Utilities.Extensions;
using System;
using UnityEngine;

namespace NEXT.Components;

[RegisterInIl2Cpp]
public class VentBodyComponent(IntPtr ptr) : MonoBehaviour(ptr)
{
    public DeadBody deadBody = null!;

    public void ExposeBody()
    {
        deadBody.ShowBody(false);
        this.Destroy();
    }
}