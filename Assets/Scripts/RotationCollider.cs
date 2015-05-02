﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RotationCollider : MonoBehaviour
{

	private Cube Cube;
	
	private void Awake()
	{
		Cube = FindObjectOfType<Cube>();
	}

	public GameObject[] GetAllObjectsToMove()
	{
		var results = Cube.QuadList
			.Where( quad => collider.bounds.Contains( quad.transform.position ) )
			.ToList();
		
		if ( Cube.EndPointCollider.bounds.Intersects( collider.bounds ) )
		{
			results.Add( Cube.EndPointCollider.gameObject );
		}

		if ( Cube.StartPointCollider.bounds.Intersects( collider.bounds ) )
		{
			results.Add( Cube.StartPointCollider.gameObject );
		}

		var ray = new Ray( Cube.GoatCollider.transform.position + Cube.GoatCollider.transform.up, -Cube.GoatCollider.transform.up );
		var layerMask = LayerMask.GetMask( "Rotaters" );

		var didHit = Physics.RaycastAll( ray, 1.5f, layerMask );
		if ( didHit.Length > 0 )
		{
			results.AddRange(
				didHit.Where( hit => hit.collider.gameObject == gameObject )
				.Select( hit => Cube.GoatCollider.transform.parent.gameObject )
			);
		}
		return results.ToArray();
	}
}
