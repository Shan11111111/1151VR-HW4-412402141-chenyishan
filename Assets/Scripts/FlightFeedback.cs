using UnityEngine;

[RequireComponent(typeof(AirplaneController))]
public class FlightFeedback : MonoBehaviour
{
    [Header("Keep existing audio references")]
    public AudioClip flyingSound;
    public AudioClip collisionSound;
    public AudioClip successSound;
    public AudioSource engineSource;
    public AudioSource effectsSource;

    [Header("Goal")]
    public int targetCheckpoints = 3;
    public int PassedCount { get; private set; }
    public bool IsCrashed { get; private set; }
    public bool IsCompleted { get; private set; }
    public bool IsStarted { get; private set; }

    AirplaneController controller;
    void Awake() { controller = GetComponent<AirplaneController>(); }
    void Start() { controller.SetFlying(false); }

    public void StartFlight()
    {
        if (IsStarted || IsCrashed || IsCompleted) return;
        IsStarted = true;
        controller.SetFlying(true);
        if (engineSource && flyingSound)
        {
            engineSource.clip = flyingSound;
            engineSource.loop = true;
            engineSource.playOnAwake = false;
            engineSource.spatialBlend = 0f;
            engineSource.Play();
        }
    }

    public void HitObstacle()
    {
        if (!IsStarted || IsCrashed || IsCompleted) return;
        IsCrashed = true;
        StopFlight();
        if (effectsSource && collisionSound) effectsSource.PlayOneShot(collisionSound);
        Debug.Log("飛行失敗！");
    }

    public void PassCheckpoint()
    {
        if (!IsStarted || IsCrashed || IsCompleted) return;
        PassedCount++;
        if (effectsSource && successSound) effectsSource.PlayOneShot(successSound);
        Debug.Log("通過關卡：" + PassedCount);
    }

    public void ReachFinish()
    {
        if (!IsStarted || IsCrashed || IsCompleted) return;
        if (PassedCount < targetCheckpoints)
        {
            Debug.Log("尚未通過所有關卡！");
            return;
        }
        IsCompleted = true;
        StopFlight();
        if (effectsSource && successSound) effectsSource.PlayOneShot(successSound);
        Debug.Log("成功抵達終點！");
    }

    void StopFlight()
    {
        controller.SetFlying(false);
        if (engineSource) engineSource.Stop();
    }
}
