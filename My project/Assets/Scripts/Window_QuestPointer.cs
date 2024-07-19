using UnityEngine;
// The following video is used as a basis https://www.youtube.com/watch?v=dHzeHh-3bp4 and adapted and extended to our own needs
public class Window_QuestPointer : MonoBehaviour
{
    private Transform targetTransform; // The target object to which the arrow should point
    public Transform bucketTargetTransform;
    public Transform fountainTargetTransform;
    public Transform fireTargetTransform;
    private RectTransform pointerRectTransform;
    private Canvas canvas;
    public PlayerController playerController;

    private void Awake()
    {
        pointerRectTransform = transform.Find("Pointer").GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        targetTransform = bucketTargetTransform; // First target is the bucket
    }

    private void Update() // Dynamic adjustment of the arrow depending on the player's current target
    {
        if (playerController == null)
        {
            Debug.LogWarning("PlayerController is not assigned!");
            return;
        }
        else if (!playerController.PickupBucket)
        {
            targetTransform = bucketTargetTransform;
        }

        else if (playerController.PickupBucket && !playerController.BucketState)
        {
            targetTransform = fountainTargetTransform;
        }

        else if (playerController.PickupBucket && playerController.BucketState)
        {
            targetTransform = fireTargetTransform;
        }

        else if (playerController.BucketState && !playerController.PickupBucket)
        {
            targetTransform = bucketTargetTransform;
        }

        else if (bucketTargetTransform == null)
        {
            Debug.LogWarning("Target Transform is not assigned!");
            return;
        }

        // Target position in world coordinates
        Vector3 toPosition = targetTransform.position;
        // Camera position in world coordinates
        Vector3 fromPosition = Camera.main.transform.position;
        fromPosition.z = 0f; // In case the target and the camera are not on the same Z-plane

        // Calculate direction from the camera position to the target position
        Vector3 dir = (toPosition - fromPosition).normalized;

        // Calculate angle
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        angle += 270f; // Required for the arrow to point in the right direction

        // Apply angle to the arrow
        pointerRectTransform.localEulerAngles = new Vector3(0, 0, angle);

        float borderSize = 100f;
        Vector3 targetPositionScreenPoint = Camera.main.WorldToScreenPoint(toPosition);
        bool isOffScreen = targetPositionScreenPoint.x <= borderSize || targetPositionScreenPoint.x >= Screen.width - borderSize || targetPositionScreenPoint.y <= borderSize || targetPositionScreenPoint.y >= Screen.height - borderSize;

        if (isOffScreen)
        {
            Vector3 cappedTargetScreenPosition = targetPositionScreenPoint;

            if (cappedTargetScreenPosition.x <= borderSize) cappedTargetScreenPosition.x = borderSize;
            if (cappedTargetScreenPosition.x >= Screen.width - borderSize) cappedTargetScreenPosition.x = Screen.width - borderSize;
            if (cappedTargetScreenPosition.y <= borderSize) cappedTargetScreenPosition.y = borderSize;
            if (cappedTargetScreenPosition.y >= Screen.height - borderSize) cappedTargetScreenPosition.y = Screen.height - borderSize;

            // Convert screen coordinates to canvas coordinates
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, cappedTargetScreenPosition, canvas.worldCamera, out Vector2 localPoint);
            pointerRectTransform.localPosition = localPoint;
            pointerRectTransform.gameObject.SetActive(true); // Show arrow
        }
        else
        {
            pointerRectTransform.gameObject.SetActive(false); // Hide arrow
        }
    }
}
