﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class Cube : MonoBehaviour
{
	public int DeathCount;
	
	public Transform CenterTransform;
	public AnimationCurve RotationCurve;
	public float SpinSpeed;
	public Graph Graph;
	public bool HasStarted;
	public Quad[] QuadArray;

	private Vector3[] originalQuadPositions;
	private Quaternion[] originalQuadRotations;

	private AudioSource rotateSound;

	public bool IsRotating { get; private set; }

	public Collider GoatCollider { get; private set; }

	public RotationCollider XRotationCollider { get; private set; }
	public RotationCollider YRotationCollider { get; private set; }
	public RotationCollider ZRotationCollider { get; private set; }

	public enum RotationAxis { X, Y, Z }

	void Awake()
	{
		Graph = Graph.LoadGraphFromCsv( "" );
		HasStarted = false;
	}

	void Start()
	{
		Transform t = transform.FindChild( "RotateSound" );
		if ( t != null )
		{
			rotateSound = t.audio;
		}

		var Colliders = GetComponentsInChildren<RotationCollider>();
		
		XRotationCollider = Colliders.Single( c => c.Axis == RotationAxis.X );
		YRotationCollider = Colliders.Single( c => c.Axis == RotationAxis.Y );
		ZRotationCollider = Colliders.Single( c => c.Axis == RotationAxis.Z );

		var goat = FindObjectOfType<TheGoat>();

		GoatCollider = goat.GetComponent<BoxCollider>();

		QuadArray = GetComponentsInChildren<Quad>();
		originalQuadPositions = new Vector3[ QuadArray.Length ];
		originalQuadRotations = new Quaternion[ QuadArray.Length ];
		for ( var i = 0; i < QuadArray.Length; i++ )
		{
			originalQuadPositions[ i ] = QuadArray[ i ].transform.position;
			originalQuadRotations[ i ] = QuadArray[ i ].transform.rotation;
			var childQuad = QuadArray[ i ];

			childQuad.Node = Graph.GetNodeByName( childQuad.NodeName );
		}


		
	}

	public void StartMovingGoat()
	{
		if (HasStarted)
			return;
	
		HasStarted = true;
		GoatCollider.GetComponent<TheGoat>().StartSound.Play();
		GoatCollider.GetComponent<TheGoat>().MoveSound.Play();
	}

	void Update()
	{

	}

	public void RotateX()
	{
		rotateSound.Play();
		StartCoroutine( Rotate( RotationAxis.X ) );
	}

	public void RotateY()
	{
		rotateSound.Play();
		StartCoroutine( Rotate( RotationAxis.Y ) );
	}

	public void RotateZ()
	{
		rotateSound.Play();
		StartCoroutine( Rotate( RotationAxis.Z ) );
	}

	IEnumerator Rotate( RotationAxis axis )
	{
		if ( IsRotating )
		{
			yield break;
		}



		if ( SpinSpeed < float.Epsilon )
		{
			Debug.LogError( "SpinSpeed is less than or equal to 0" );
			yield break;
		}

		RotationCollider activeCollider = null;
		switch ( axis )
		{
			case RotationAxis.X: activeCollider = XRotationCollider; break;
			case RotationAxis.Y: activeCollider = YRotationCollider; break;
			case RotationAxis.Z: activeCollider = ZRotationCollider; break;
		}

		if ( activeCollider == null )
		{
			Debug.Log( "WHHAATT" );
			yield break;
		}

		Transform[] transformArray;
		Transform[] transformParentArray;
		{
			var quadList = activeCollider.GetAllObjectsToMove();

			transformArray = quadList.Select( o => o.transform )
				.ToArray();

			transformParentArray = transformArray.Select( t => t.parent ).ToArray();
		}

		Vector3? rotationAxis = null;
		switch ( axis )
		{
			case RotationAxis.X: rotationAxis = new Vector3( x: 1, y: 0, z: 0 ); break;
			case RotationAxis.Y: rotationAxis = new Vector3( x: 0, y: 1, z: 0 ); break;
			case RotationAxis.Z: rotationAxis = new Vector3( x: 0, y: 0, z: 1 ); break;
		}

		if ( rotationAxis == null )
		{
			Debug.Log( "WWWHHHHAATTT" );
			yield break;
		}

		switch ( axis )
		{
			case RotationAxis.X: Graph.RotateX( RotationEnum.Clockwise ); break;
			case RotationAxis.Y: Graph.RotateY( RotationEnum.Clockwise ); break;
			//case RotationAxis.Z: Graph.RotateZ( RotationEnum.Clockwise ); break;
        }
		
		IsRotating = true;
		CenterTransform.rotation = Quaternion.identity;
		for ( var i = 0; i < transformArray.Length; i++ )
		{
			transformArray[ i ].parent = CenterTransform;
		}

		var progress = 0f;

		while ( progress < 1f )
		{
			var angle = RotationCurve.Evaluate( progress );
			CenterTransform.rotation = Quaternion.Euler( angle * rotationAxis.Value );
			progress += Time.deltaTime * SpinSpeed;
	
			yield return null;
		}

		CenterTransform.rotation = Quaternion.Euler( 90 * rotationAxis.Value ); 

		for ( var i = 0; i < transformArray.Length; i++ )
		{
			transformArray[ i ].parent = transformParentArray[ i ];
		}

		CenterTransform.rotation = Quaternion.identity;

		IsRotating = false;
		yield return null;
	}
	
	public void Reset()
	{
		DeathCount++;

		HasStarted = false;

		GoatCollider.GetComponent<TheGoat>().Reset();

		for ( int i = 0; i < QuadArray.Length; i ++ )
		{
			var quad = QuadArray[ i ];
			quad.transform.position = originalQuadPositions[ i ];
			quad.transform.rotation = originalQuadRotations[ i ];
		}
	}
}
