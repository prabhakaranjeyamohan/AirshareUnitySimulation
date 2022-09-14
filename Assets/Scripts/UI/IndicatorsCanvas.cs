using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI Indicators drawer
/// </summary>
public class IndicatorsCanvas : MonoBehaviour
{
    [SerializeField]
    private GameObject _indicatorPrefab;

    [SerializeField]
    private float _indicatorOffsetY;

    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float _indicatorAlpha;

    [SerializeField]
    private Color _hostileColor;
    public Color HostileColor { get { return _hostileColor; } }

    [SerializeField]
    private Color _friendlyColor;
    public Color FriendlyColor { get { return _friendlyColor; } }

    [SerializeField]
    private Color _protectedZoneColor;
    public Color ProtectedZoneColor { get { return _protectedZoneColor; } }

    [SerializeField]
    private Color _disabledColor;
    public Color DisabledColor { get { return _disabledColor; } }

    /// <summary>
	/// Updates objects state
	/// </summary>
    private void Update()
    {
        UpdateContainerIndicators(AADManager.Instance.AI_DroneManager.gameObject, ColorAlpha(HostileColor));
        UpdateContainerIndicators(AADManager.Instance.MissileLauncherManager.gameObject, ColorAlpha(FriendlyColor));
        UpdateContainerIndicators(AADManager.Instance.CMContainer, ColorAlpha(FriendlyColor));
        UpdateContainerIndicators(AADManager.Instance.ProtectedZonesManager.gameObject, ColorAlpha(ProtectedZoneColor));
    }

    /// <summary>
	/// Update indicators for each of the actor containers
	/// </summary>
	/// <param name="actorContainer">Actor container object</param>
	/// <param name="activeColor">Active color of the container</param>
    private void UpdateContainerIndicators(GameObject actorContainer, Color activeColor)
    {
        for (int i = 0; i < actorContainer.transform.childCount; i++)
        {
            Transform actor = actorContainer.transform.GetChild(i);
            UpdateActorIndicator(actor.gameObject, activeColor);
        }
    }

    /// <summary>
	/// Update actor indicator
	/// </summary>
	/// <param name="actor">Actor object</param>
	/// <param name="activeColor">Actor object active color</param>
    private void UpdateActorIndicator(GameObject actor, Color activeColor)
    {
        GameObject indicator = actor.GetComponent<Actor>().Indicator;

        if (!indicator)
        {
            indicator = Instantiate(_indicatorPrefab);
            indicator.transform.SetParent(gameObject.transform, worldPositionStays: false);
            actor.GetComponent<Actor>().Indicator = indicator;
        }

        indicator.SetActive(actor.GetComponent<MeshRenderer>().isVisible);

        if (!indicator.activeSelf) return;

        indicator.GetComponent<Image>().color = actor.GetComponent<Actor>().ActorState == ActorState.Active ? activeColor : ColorAlpha(DisabledColor);

        Vector3 indicatorPos = actor.transform.position + new Vector3(0.0f, _indicatorOffsetY, 0.0f);

        Vector2 canvasPos;
        Vector2 screenPos = Camera.main.WorldToScreenPoint(indicatorPos);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(gameObject.GetComponent<RectTransform>(), screenPos, Camera.main, out canvasPos);

        indicator.GetComponent<RectTransform>().localPosition = canvasPos;

    }

    /// <summary>
	/// Adjusts color for alpha
	/// </summary>
	/// <param name="color"></param>
	/// <returns></returns>
    private Color ColorAlpha(Color color)
    {
        color.a = _indicatorAlpha;
        return color;
    }
}
