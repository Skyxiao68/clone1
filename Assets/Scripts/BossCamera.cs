using UnityEngine;
using Cinemachine;
using System.Collections;

public class BossCamera : MonoBehaviour
{
    [Header("虚拟相机")]
    public Cinemachine playerCamera;
    public Cinemachine bossCamera;
    
    [Header("Boss展示设置")]
    public float transitionTime = 1.5f;
    public float bossShowDuration = 3f;
    public float bossZoomSize = 4f;
    public float playerNormalSize = 7f;
    
    private CinemachineBrain brain;
    private float originalPlayerSize;
    private Coroutine currentTransition;
    
    void Start()
    {
        brain = Camera.main.GetComponent<CinemachineBrain>();
        originalPlayerSize = playerCamera.m_Lens.OrthographicSize;
        SwitchToPlayerCamera();
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            ShowBossWithZoom();
        }
    }
    
    public void SwitchToPlayerCamera()
    {
        playerCamera.Priority = 10;
        bossCamera.Priority = 0;
        playerCamera.m_Lens.OrthographicSize = originalPlayerSize;
    }
    
    public void ShowBossWithZoom()
    {
        if (currentTransition != null)
        {
            StopCoroutine(currentTransition);
        }
        currentTransition = StartCoroutine(BossShowSequence());
    }
    
    IEnumerator BossShowSequence()
    {
        // 先切换到Boss相机
        bossCamera.m_Lens.OrthographicSize = playerNormalSize;
        playerCamera.Priority = 0;
        bossCamera.Priority = 10;
        
        // 等待位置过渡完成
        yield return new WaitForSeconds(brain.m_DefaultBlend.m_Time);
        
        // 缩放特写
        float elapsed = 0f;
        float startSize = bossCamera.m_Lens.OrthographicSize;
        
        while (elapsed < 1f)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / 1f;
            bossCamera.m_Lens.OrthographicSize = Mathf.Lerp(startSize, bossZoomSize, t);
            yield return null;
        }
        
        // 停留展示
        yield return new WaitForSeconds(bossShowDuration);
        
        // 返回玩家
        SwitchToPlayerCamera();
        currentTransition = null;
    }
}