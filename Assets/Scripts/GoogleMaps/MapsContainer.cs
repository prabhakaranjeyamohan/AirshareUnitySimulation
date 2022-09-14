using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google.Maps.Scripts;

/// <summary>
/// Singleton class to access Google Maps services
/// </summary>
[RequireComponent(typeof(DynamicMapsService))]
public class MapsContainer : MonoBehaviour
{
    private static MapsContainer _instance;

    public static MapsContainer Instance { get { return _instance; } }

	private void Awake()
	{
		if (_instance != null && _instance != this)
		{
			Destroy(this.gameObject);
		} else {
			_instance = this;
		}
	}
}
