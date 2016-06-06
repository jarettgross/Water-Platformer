using UnityEngine;
using System.Collections;

public class WaterActions : MonoBehaviour {

	private const float delta = 0.2f; //box containment leeway amount

	void OnParticleCollision(GameObject other) {
		if (other.tag == "Heat" || other.tag == "Snow" || other.tag == "BubbleMaker") { //destroy particles that collided with heat/snow box
			ParticleSystem.Particle[] particles = new ParticleSystem.Particle[gameObject.GetComponent<ParticleSystem> ().particleCount];
			int numParticles = gameObject.GetComponent<ParticleSystem> ().GetParticles (particles);

			//set center and extents of collided object
			Vector3 colCenter;
			Vector3 colExtents;
			if (other.tag == "BubbleMaker") {
				colCenter = other.GetComponent<CircleCollider2D> ().bounds.center;
				colExtents = other.GetComponent<CircleCollider2D> ().bounds.extents;
			} else {
				colCenter = other.GetComponent<BoxCollider2D> ().bounds.center;
				colExtents = other.GetComponent<BoxCollider2D> ().bounds.extents;
			}

			for (int i = 0; i < numParticles; i++) { //for each particle in system, check if it intersects heat/snow box
				if (particles[i].position.x >= (colCenter.x - colExtents.x - delta) && particles[i].position.x <= (colCenter.x + colExtents.x + delta) && 
					particles[i].position.y >= (colCenter.y - colExtents.y - delta) && particles[i].position.y <= (colCenter.y + colExtents.y + delta)) {

					particles [i].lifetime = 0; //particle collided with heat/snow box, destroy

					//if collided with snow and ice block is less than max width
					if (other.tag == "Snow" && other.transform.GetChild(1).localScale.y <= other.transform.GetComponentInChildren<IceBlock>().maxIceWidth) {
						Vector3 tempScale = other.transform.GetChild(1).localScale;
						tempScale.y = Mathf.Lerp (tempScale.y, 1, Time.deltaTime * other.transform.GetChild (1).GetComponent<IceBlock> ().growSpeed);
						//tempScale.y += Time.deltaTime * other.transform.GetChild(1).GetComponent<IceBlock>().growSpeed;
						other.transform.GetChild(1).localScale = tempScale;

					} else if (other.tag == "BubbleMaker") { //create a bubble if allowed (based on time since last bubble was created)
						BubbleMaker bm = other.GetComponent<BubbleMaker> ();
						if (!bm.isBubbleActive) {
							bm.isBubbleActive = true;
							GameObject bubbleClone = (GameObject)Instantiate (bm.bubble, bm.transform.position, Quaternion.identity);
							bubbleClone.GetComponent<Bubble> ().bubbleMakerParent = bm;
							bm.bubbleList.Add (bubbleClone);
							bubbleClone.GetComponent<Rigidbody2D> ().velocity = new Vector2 (bm.velocityX, bm.velocityY);
						}
					}
				}
			}
			gameObject.GetComponent<ParticleSystem> ().SetParticles (particles, numParticles);
		}
	}
}
