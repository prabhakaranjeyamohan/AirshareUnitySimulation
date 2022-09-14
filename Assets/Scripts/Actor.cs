using UnityEngine;

/// <summary>
/// Enum of possible Actor states
/// </summary>
public enum ActorState { Active, Disabled }

/// <summary>
/// Base class for all major scene entities
/// </summary>
public class Actor : MonoBehaviour
{
	public ActorState ActorState { get; set; }

	public GameObject Indicator { get; set; }

	/// <summary>
	/// Performs initial setup
	/// </summary>
	protected virtual void Start()
	{
		ActorState = ActorState.Active;
	}
}
