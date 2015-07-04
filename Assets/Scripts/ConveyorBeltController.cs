using UnityEngine;
using System.Collections;

public class ConveyorBeltController : MonoBehaviour {

	private Renderer conveyer;
	public float conveyerSpeed;
	private Vector2 beltOffset;
	public Texture2D frameTexture;

	void Awake()
	{
		conveyer = this.GetComponent<MeshRenderer>();
		beltOffset = new Vector2(0,0);
		conveyer.materials[0].SetTexture("_MainTex",frameTexture);
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		beltOffset.x+=Time.deltaTime*conveyerSpeed % 1.0f;
		conveyer.materials[1].SetTextureOffset("_MainTex", beltOffset);
	}
}
