using UnityEngine;
using System.Collections;

public class IcePlatformManager : MonoBehaviour {
	
	public float waveAmplitude = 1f, waveSpeed = 1f, lengthMultiplier = 1f;
	public Sprite left, mid, right;
	private SpriteRenderer oLeft, oMid, oRight;
	private PolygonCollider2D iceCollider;
	private Vector2[] iceColliderPoints;
	private Vector3 originalPosition;
	
	// Use this for initialization
	void Start () {
		originalPosition = gameObject.transform.position;
		oLeft = gameObject.transform.FindChild ("Left").GetComponent<SpriteRenderer> ();
		oMid = gameObject.transform.FindChild ("Mid").GetComponent<SpriteRenderer> ();
		oRight = gameObject.transform.FindChild ("Right").GetComponent<SpriteRenderer> ();
		iceCollider = gameObject.transform.FindChild ("Collider").GetComponent<PolygonCollider2D> ();

		oLeft.sprite = left;
		oMid.sprite = mid;
		oRight.sprite = right;

		oLeft.transform.localPosition = new Vector3 (-((left.textureRect.width / left.pixelsPerUnit)/2f) - (((mid.textureRect.width * lengthMultiplier) / mid.pixelsPerUnit)/2f), 0f);
		oRight.transform.localPosition = new Vector3 (((right.textureRect.width / right.pixelsPerUnit)/2f) + (((mid.textureRect.width * lengthMultiplier) / mid.pixelsPerUnit)/2f) - 0.02f, 0f);
		oRight.transform.localScale = new Vector3 (-1f, 1f, 1f);
		oMid.transform.localScale = new Vector3 (lengthMultiplier, 1f, 1f);

		iceColliderPoints = iceCollider.points;

		iceColliderPoints [0] = new Vector2 (-(left.textureRect.width / left.pixelsPerUnit) - (((mid.textureRect.width * lengthMultiplier) / mid.pixelsPerUnit) / 2f), -0.02f);
		iceColliderPoints [1] = new Vector2 ((right.textureRect.width / right.pixelsPerUnit) + (((mid.textureRect.width * lengthMultiplier) / mid.pixelsPerUnit) / 2f) - 0.02f, -0.02f);
		iceColliderPoints [3] = new Vector2 (-(left.textureRect.width / left.pixelsPerUnit) - (((mid.textureRect.width * lengthMultiplier) / mid.pixelsPerUnit) / 2f) * 0.7f, -1.5f);
		iceColliderPoints [2] = new Vector2 ((right.textureRect.width / right.pixelsPerUnit) + (((mid.textureRect.width * lengthMultiplier) / mid.pixelsPerUnit) / 2f) * 0.7f, -1.5f);

		iceCollider.SetPath (0, iceColliderPoints);
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3 (originalPosition.x, originalPosition.y + Mathf.Cos(Time.time*waveSpeed) * waveAmplitude, originalPosition.z);
	}
}
