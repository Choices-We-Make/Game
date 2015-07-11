using UnityEngine;
using System.Collections;

public class TextureAnimator : MonoBehaviour {
	public int uvAnimationTileX; //number of columns in texture
	public int uvAnimationTileY; //number of rows in texture
	public int framesPerSecond; //how fast to play the animation
	public int materialIndex; //which material to control
	private Vector2 size;
	private Vector2 offset;
	private Renderer[] wheels;

	void Awake()
	{
		//size of each tile
		size = new Vector2(1.0f/uvAnimationTileX, 1.0f/uvAnimationTileY);
		offset = new Vector2(0.0f, 0.0f);
		wheels = GetComponentsInChildren<Renderer>();
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		// Calculate index
		int index = (int)(Time.time * framesPerSecond);
		//repeat when exhausting all frames
		index = index % (uvAnimationTileX*uvAnimationTileY);
		
		//split into horizontal and vertical index;
		int uIndex = index % uvAnimationTileX;
		int vIndex = index / uvAnimationTileX;
		
		//calculate offset
		//v coordinate is the bottom of teh image in opengl so we need to invert
		offset.x = uIndex*size.x;
		offset.y = 1.0f-size.y-vIndex*size.y;

		foreach (Renderer r in wheels)
		{
			r.materials[materialIndex].SetTextureOffset("_MainTex", offset);
			r.materials[materialIndex].SetTextureScale("_MainTex", size);
		}
	}
}
