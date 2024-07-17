using UnityEngine;

public class Window_QuestPointer : MonoBehaviour
{
    public Transform targetTransform; // Das Zielobjekt, auf das der Pfeil zeigen soll
    public Transform fireTargetTransform;
    private RectTransform pointerRectTransform;
    private Canvas canvas;
    public PlayerController playerController;

    private void Awake()
    {
        pointerRectTransform = transform.Find("Pointer").GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    private void Update()
    {
        if (playerController == null)
        {
            Debug.LogWarning("PlayerController is not assigned!");
            return;
        }

        if (playerController.BucketState && fireTargetTransform != null)
        {
            targetTransform = fireTargetTransform;
        }

        if (targetTransform == null)
        {
            Debug.LogWarning("Target Transform is not assigned!");
            return;
        }

        // Zielposition in Weltkoordinaten
        Vector3 toPosition = targetTransform.position;
        // Kameraposition in Weltkoordinaten
        Vector3 fromPosition = Camera.main.transform.position;
        fromPosition.z = 0f; // Falls das Ziel und die Kamera nicht auf derselben Z-Ebene liegen

        // Richtung vom Kameraposition zur Zielposition berechnen
        Vector3 dir = (toPosition - fromPosition).normalized;

        // Winkel berechnen
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        angle += 270f;

        // Winkel auf den Pfeil anwenden
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

            // Bildschirmkoordinaten in Canvas-Koordinaten umwandeln
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, cappedTargetScreenPosition, canvas.worldCamera, out Vector2 localPoint);
            pointerRectTransform.localPosition = localPoint;
            pointerRectTransform.gameObject.SetActive(true); // Pfeil anzeigen
        }
        else
        {
            // Pfeil ausblenden
            pointerRectTransform.gameObject.SetActive(false); // Pfeil anzeigen
        }
    }
}
