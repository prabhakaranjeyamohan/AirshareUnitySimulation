using UnityEngine;

/// <summary>
/// Abstract base class for all actor managers
/// </summary>
public abstract class ActorManager : MonoBehaviour
{
	protected bool _postMapLoadStarted = false;

	/// <summary>
	/// Performs post map load setup
	/// </summary>
	public virtual void PostMapLoadStart()
	{
		_postMapLoadStarted = true;
	}
}
