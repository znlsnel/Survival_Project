using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CutScene : MonoBehaviour
{
    [SerializeField] private GameObject dropship;      // ¶³¾îÁö´Â µå·Ó½Ê
    [SerializeField] private GameObject explosionPrefab; // Æø¹ß ÀÌÆåÆ® ÇÁ¸®ÆÕ
    [SerializeField] private Transform dropTarget;     // Âø·ú ÁöÁ¡
    [SerializeField] private CanvasGroup fadeCanvas;   // ÆäÀÌµå ÀÎ/¾Æ¿ô¿ë Äµ¹ö½º
    [SerializeField] private float dropDuration = 5f;  // µå·Ó½Ê ¶³¾îÁö´Â ½Ã°£
    [SerializeField] private CinemachineVirtualCamera vCam;

    private float defaultFOV;
    private Vector3 defaultOffset;

    private void OnValidate()
    {
        if (dropship == null) dropship = GameObject.Find("CutSceneDropship");
        if (vCam == null) vCam = FindObjectOfType<CinemachineVirtualCamera>();
        if (fadeCanvas == null) fadeCanvas = FindObjectOfType<CanvasGroup>();

        vCam.Follow = dropship.transform;
        vCam.LookAt = dropship.transform;
    }

    private void Start()
    {
        defaultFOV = vCam.m_Lens.FieldOfView;
        StartCoroutine(PlayCutScene());
    }

    private IEnumerator PlayCutScene()
    {
        Vector3 startPos = dropship.transform.position;
        Vector3 endPos = dropTarget.position;

        float startFOV = defaultFOV;
        float targetFOV = 30f;


        float elapsedTime = 0f;
        while (elapsedTime < dropDuration)
        {
            dropship.transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / dropDuration);
            vCam.m_Lens.FieldOfView = Mathf.Lerp(startFOV, targetFOV, elapsedTime / dropDuration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Time.timeScale = 0.2f;
        yield return new WaitForSecondsRealtime(1.5f);
        dropship.transform.position = endPos;
        Instantiate(explosionPrefab, endPos, Quaternion.identity);
        Time.timeScale = 1.0f;
        vCam.m_Lens.FieldOfView = defaultFOV;
        yield return new WaitForSeconds(0.5f);

        
      //  StartCoroutine(ShakeCamera(0.5f, 0.2f));





        yield return StartCoroutine(BlinkScreen());

        Debug.Log("ÄÆ¾À Á¾·á!");

        vCam.enabled = false;
    }

    private IEnumerator BlinkScreen()
    {
        float fadeDuration = 0.5f;

        for (int i = 0; i < 3; i++) // 3¹ø ±ôºý°Å¸²
        {
            yield return StartCoroutine(Fade(0, 1, fadeDuration)); // ¹à¾ÆÁü
            yield return StartCoroutine(Fade(1, 0, fadeDuration)); // ¾îµÎ¿öÁü
        }
    }

    private IEnumerator CameraZoom(float targetFOV, float duration)
    {
        float startFOV = vCam.m_Lens.FieldOfView;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            vCam.m_Lens.FieldOfView = Mathf.Lerp(startFOV, targetFOV, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        vCam.m_Lens.FieldOfView = targetFOV;
    }

    private IEnumerator Fade(float startAlpha, float endAlpha, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            fadeCanvas.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        fadeCanvas.alpha = endAlpha;
    }

    private IEnumerator ShakeCamera(float duration, float intensity)
    {
        CinemachineTransposer transposer = vCam.GetCinemachineComponent<CinemachineTransposer>();
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float xShake = Random.Range(-intensity, intensity);
            float yShake = Random.Range(-intensity, intensity);

            transposer.m_FollowOffset = defaultOffset + new Vector3(xShake, yShake, 0);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transposer.m_FollowOffset = defaultOffset; // ¿ø·¡ À§Ä¡·Î º¹±Í
    }

}
