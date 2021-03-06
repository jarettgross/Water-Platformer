﻿using UnityEngine;
using System.Collections;

/*
Copyright (c) 2014, Tuts+
All rights reserved.

Redistribution and use in source and binary forms, with or without modification,
are permitted provided that the following conditions are met:

* Redistributions of source code must retain the above copyright notice, this
  list of conditions and the following disclaimer.

* Redistributions in binary form must reproduce the above copyright notice, this
  list of conditions and the following disclaimer in the documentation and/or
  other materials provided with the distribution.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR
ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON
ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

public class WaterDetector : MonoBehaviour {

	/**
	 * Play the splash effect when the player enters the puddle
	**/

	void OnTriggerEnter2D(Collider2D Hit) {
		if (Hit.GetComponent<Rigidbody2D>() != null && !transform.parent.GetComponent<Puddle>().isRefillingPlayer) {
			transform.parent.GetComponent<Puddle>().Splash(transform.position.x, Hit.GetComponent<Rigidbody2D>().velocity.y*Hit.GetComponent<Rigidbody2D>().mass / 40f);
			if (!transform.parent.GetComponent<Puddle> ().bsm.isSFXMuted) {
				AudioSource.PlayClipAtPoint (transform.parent.GetComponent<Puddle> ().splashSound, transform.position);
			}
		}
	}
}