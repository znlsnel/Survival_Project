using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EndingCutScene : MonoBehaviour
{
    [SerializeField] private GameObject airplane;   // 출발하는 비행기
    [SerializeField] private Transform skyTarget;   // 목표 상승 지점
    [SerializeField] private CanvasGroup fadeCanvas; // 페이드 인/아웃용 캔버스
    [SerializeField] private CanvasGroup endingUI;


    [SerializeField] private CinemachineVirtualCamera vCam;
    [SerializeField] private float takeoffDuration = 5f; // 이륙 시간
    [SerializeField] private float fadeDuration = 2f; // 페이드아웃 시간
    [SerializeField] private float finalSpeed = 10f; // 최종 상승 속도
    [SerializeField] private float rotationSpeed = 2f; // 회전 속도

    [SerializeField] private float uiFadeInDuration = 2f; // 엔딩 UI 등장 시간

    private float defaultFOV;
    private bool isTakeoff = true;

    private void Awake()
    {
        if (airplane == null) airplane = GameObject.Find("CutSceneOBJ_Ending");
        if (vCam == null) vCam = FindObjectOfType<CinemachineVirtualCamera>();
        if (fadeCanvas == null) fadeCanvas = GameObject.Find("fade").GetComponent<CanvasGroup>();
        if (endingUI == null) endingUI = GameObject.Find("EndingUI").GetComponent<CanvasGroup>();


        if (endingUI != null)
        {
            endingUI.alpha = 0;
            endingUI.gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        vCam.Follow = airplane.transform;
        vCam.LookAt = airplane.transform;

        defaultFOV = vCam.m_Lens.FieldOfView;
        StartCoroutine(PlayEndingCutScene());
    }

    public void InitScene(CinemachineVirtualCamera cam, Transform target)
    {
        skyTarget = target;
        vCam = cam;
    }

    private IEnumerator PlayEndingCutScene()
    {
		GameManager.Instance.ActiveUI(false);

		Vector3 startPos = airplane.transform.position;
        Vector3 endPos = skyTarget.position;

        float startFOV = defaultFOV;
        float targetFOV = 30f; // 점점 멀어지는 효과

        SoundManager.Play("Sounds/UI/Flying");


        CinemachineTransposer transposer = vCam.GetCinemachineComponent<CinemachineTransposer>();
        if (transposer == null)
        {
            yield break;
        }

        Vector3 initialOffset = transposer.m_FollowOffset;
        Vector3 targetOffset = initialOffset + new Vector3(0, 10f, 0); // 카메라 상승 효과

        Vector3 velocity = Vector3.up * 2f; // 초반 이동 속도
        float acceleration = finalSpeed / takeoffDuration; // 가속도

        Quaternion targetRotation = Quaternion.LookRotation((skyTarget.position - airplane.transform.position).normalized);

        float elapsedTime = 0f;
        while (elapsedTime < takeoffDuration)
        {
            float t = Mathf.SmoothStep(0, 1, elapsedTime / takeoffDuration);

            velocity += airplane.transform.forward * (acceleration * Time.deltaTime); // 기울어진 방향으로 가속
            airplane.transform.position += velocity * Time.deltaTime;

            // 부드럽게 목표 방향을 바라보도록 회전
            airplane.transform.rotation = Quaternion.Slerp(airplane.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            vCam.m_Lens.FieldOfView = Mathf.Lerp(startFOV, targetFOV, t);
            transposer.m_FollowOffset = Vector3.Lerp(initialOffset, targetOffset, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        vCam.Follow = null;

        StartCoroutine(ContinuousTakeoff()); // 대각선 방향으로 계속 이동

        yield return new WaitForSeconds(3.0f);

        StartCoroutine(CameraZoom(60f, 3.0f));

        yield return new WaitForSeconds(3.0f);
        yield return StartCoroutine(Fade(0, 1, fadeDuration));

		GameManager.Instance.ActiveUI(true);

		Debug.Log("게임 엔딩 컷씬 종료!");
    }

    private IEnumerator ContinuousTakeoff()
    {
        while (isTakeoff)
        {
            airplane.transform.position += airplane.transform.forward * (finalSpeed * Time.deltaTime);
            yield return null;
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

        if (endingUI != null)
        {
            endingUI.gameObject.SetActive(true);

            elapsedTime = 0f;
            while (elapsedTime < uiFadeInDuration)
            {
                endingUI.alpha = Mathf.Lerp(0, 1, elapsedTime / uiFadeInDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            endingUI.alpha = 1;
        }

        isTakeoff = false; // 상승 종료
    }
}
