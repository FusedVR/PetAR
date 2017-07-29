using UnityEngine;
using System.Collections;

public class Locomotion
{
    private Animator m_Animator = null;
    
    private int m_SpeedId = 0;

    public float m_SpeedDampTime = 0.1f;
    
    
    public Locomotion(Animator animator)
    {
        m_Animator = animator;
        m_SpeedId = Animator.StringToHash("Speed");
    }

    public void Do(GameObject obj,float speed, float direction)
    {
        m_Animator.SetFloat(m_SpeedId, speed, m_SpeedDampTime, Time.deltaTime);
    }	
}
