using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookingNextPoint : StateMachineBehaviour {
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        AI aI = animator.gameObject.GetComponent<AI>();
        aI.SetMaterial(0);
        aI.SetNextPoint();
    }
}
