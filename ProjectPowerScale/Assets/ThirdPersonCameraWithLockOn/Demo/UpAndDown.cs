/**
	Copyright (C) 2019 NyangireWorks. All Rights Reserved.
 */
using UnityEngine;
using System.Collections;

namespace ThirdPersonCameraWithLockOn{


	public class UpAndDown : MonoBehaviour {

		public float speed;
		public float upLimit;
		public float downLimit;

		private float direction;

		// Use this for initialization
		void Start () {

			direction = 1; // up
		
		}
		
		// Update is called once per frame
		void Update () {

			Vector3 pos = transform.position;
			pos.y += 1 * direction * speed;

			transform.position = pos;


			if (transform.position.y >= upLimit) {
				direction = -1;
			}
			if (transform.position.y <= downLimit) {
				direction = 1;
			}
		
		}
	}
}
