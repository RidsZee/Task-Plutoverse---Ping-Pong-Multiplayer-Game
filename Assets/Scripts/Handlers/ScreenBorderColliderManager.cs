using UnityEngine;

public class ScreenBorderColliderManager : MonoBehaviour
{
    [SerializeField] BoxCollider ColliderTop;
    [SerializeField] BoxCollider ColliderBottom;
    [SerializeField] BoxCollider ColliderLeft;
    [SerializeField] BoxCollider ColliderRight;

    [SerializeField] float ScreenDistanceTop;
    [SerializeField] float ScreenDistanceBottom;
    [SerializeField] float ScreenDistanceLeft;
    [SerializeField] float ScreenDistanceRight;

    Vector3 CameraPosition;
    Vector3 NewSize;
    Vector3 ScreenGap;
    float Length;

    void Start()
    {
        // Can also be controlled from Game Manager
        CameraPosition.z = GameManager.Instance.MainCamera.transform.position.y - GameManager.Instance.Y_Depth.position.y;

        // Align specific collider to its relevent screen side

        CameraPosition.x = Screen.width / 2;
        CameraPosition.y = Screen.height;
        ScreenGap = Vector3.zero;
        ScreenGap.z = ScreenDistanceTop;
        ColliderTop.transform.position = GameManager.Instance.MainCamera.ScreenToWorldPoint(CameraPosition) + ScreenGap;
        
        CameraPosition.x = Screen.width / 2;
        CameraPosition.y = 0;
        ScreenGap = Vector3.zero;
        ScreenGap.z = -ScreenDistanceBottom;
        ColliderBottom.transform.position = GameManager.Instance.MainCamera.ScreenToWorldPoint(CameraPosition) + ScreenGap;
        
        CameraPosition.x = 0;
        CameraPosition.y = Screen.height / 2;
        ScreenGap = Vector3.zero;
        ScreenGap.x = -ScreenDistanceLeft;
        ColliderLeft.transform.position = GameManager.Instance.MainCamera.ScreenToWorldPoint(CameraPosition) + ScreenGap;
        
        CameraPosition.x = Screen.width;
        CameraPosition.y = Screen.height / 2;
        ScreenGap = Vector3.zero;
        ScreenGap.x = ScreenDistanceRight;
        ColliderRight.transform.position = GameManager.Instance.MainCamera.ScreenToWorldPoint(CameraPosition) + ScreenGap;
        
        // Resize colliders based on screen size

        Length = Vector3.Distance(ColliderLeft.transform.position, ColliderRight.transform.position);
        NewSize = ColliderTop.size;
        NewSize.x = Length + ((ScreenDistanceLeft + ScreenDistanceRight) * 0.5f);
        ColliderTop.size = NewSize;
        ColliderBottom.size = NewSize;

        Length = Vector3.Distance(ColliderTop.transform.position, ColliderBottom.transform.position);
        NewSize = ColliderLeft.size;
        NewSize.z = Length + ((ScreenDistanceTop + ScreenDistanceBottom) * 0.5f);
        ColliderLeft.size = NewSize;
        ColliderRight.size = NewSize;
    }
}