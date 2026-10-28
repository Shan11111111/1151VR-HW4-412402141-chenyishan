using UnityEngine;
[RequireComponent(typeof(Collider))]
public class FlightZone : MonoBehaviour
{
    public enum ZoneType { Obstacle, Checkpoint, Finish }
    public ZoneType zoneType;
    private bool activated;

    void OnTriggerEnter(Collider other)
    {
        var plane = other.GetComponentInParent<FlightFeedback>();
        if (!plane || !plane.IsStarted) return;
        if (zoneType == ZoneType.Obstacle) plane.HitObstacle();
        else if (!activated && !plane.IsCrashed && !plane.IsCompleted)
        {
            if (zoneType == ZoneType.Checkpoint)
            {
                activated = true;
                plane.PassCheckpoint();
            }
            else if (zoneType == ZoneType.Finish)
            {
                if (plane.PassedCount >= plane.targetCheckpoints)
                {
                    activated = true;
                    plane.ReachFinish();
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        var c = GetComponent<BoxCollider>();
        if (!c) return;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = zoneType == ZoneType.Obstacle ? Color.red :
            (zoneType == ZoneType.Checkpoint ? Color.green : Color.yellow);
        Gizmos.DrawWireCube(c.center, c.size);
    }
}
