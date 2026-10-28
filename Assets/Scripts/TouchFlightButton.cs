using UnityEngine;
using UnityEngine.EventSystems;

public class TouchFlightButton : MonoBehaviour, IPointerDownHandler,
    IPointerUpHandler, IPointerExitHandler
{
    public enum Direction { Left, Right, Up, Down }
    public Direction direction;
    public AirplaneController airplane;
    public void OnPointerDown(PointerEventData e) { if (airplane) airplane.SetButtonState((int)direction, true); }
    public void OnPointerUp(PointerEventData e) { Release(); }
    public void OnPointerExit(PointerEventData e) { Release(); }
    void OnDisable() { Release(); }
    void Release() { if (airplane) airplane.SetButtonState((int)direction, false); }
}
