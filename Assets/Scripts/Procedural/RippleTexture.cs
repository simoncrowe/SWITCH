using UnityEngine;
using System.Collections;

public abstract class RippleTexture : MonoBehaviour {
	
	public int nmWidth = 128;
	public int nmHeight = 128;
	public int splashForce = 1023;
	public int intensityShift = 2;
	public int longevityShift = 5;
	public int iterationsPerSecond = 25;
	
	protected int pixelsX;  // By default pixelsX and PixelsY
	protected int pixelsY;  // are set to nmWidth and nmHeight at initialisation.
                            // They can be modified in a child class.
	int[] buffer0;
	int[] buffer1;
	bool swap = true;
	int[] currentBuffer;
	Texture2D nm;
	Color[] nmTemp;

    protected abstract void Initialisation();
    protected abstract void UpdateLogic();
	protected void ConstructMaps () {
		nm = new Texture2D (nmWidth,nmHeight,TextureFormat.RGBA32,false);
		GetComponent<Renderer>().material.SetTexture("_BumpMap",nm);
		nmTemp = nm.GetPixels(0);
		buffer0 = new int[pixelsX * pixelsY];
		buffer1 = new int[pixelsX * pixelsY];
		
		for (int i = 0; i < nmTemp.Length; i ++){
			nmTemp[i] = new Color32 (127,127,127,127);
		}
		nm.SetPixels(nmTemp, 0);
		nm.wrapMode = TextureWrapMode.Clamp;
		nm.Apply(false,false); 
	}
	void Awake () {
        pixelsX = nmWidth;
        pixelsY = nmHeight;
        Initialisation ();
		ConstructMaps ();
		InvokeRepeating ("UpdateMaps", 0.1f, 1.0f / (float)iterationsPerSecond);
	}
	void UpdateMaps () {
		swap = !swap;
        if (swap) {
			processSurface(buffer0,buffer1);
			currentBuffer = buffer1;
		} else {
			processSurface(buffer1,buffer0);
			currentBuffer = buffer0;
		}
		for (int row = 1; row < pixelsY - 1; row ++){
			for (int column = 1; column < pixelsX - 1; column ++){
				int pos = (row * pixelsX) + column;
				byte x = (byte) (127 + (((currentBuffer[pos - 1] 
					+ currentBuffer[pos + 1])>>2) >> intensityShift));
			 	byte y = (byte) (127 + (((currentBuffer[pos - pixelsX] 
					+ currentBuffer [pos + pixelsX])>>2) >> intensityShift));
				nmTemp[pos] = new Color32 (0,y,0,x);		
			}
		}
		nm.SetPixels (0,0,pixelsX,pixelsY,nmTemp);
		nm.Apply(false,false);

        UpdateLogic();
	}
	void processSurface(int[] source, int[] dest) {
		int position;
		for (int y = 1; y < pixelsY - 1; y ++) {
			for (int x = 1; x < pixelsX - 1; x ++) {
				position = (y * pixelsX) + x;
				dest [position] = (((source[position - 1] + 
									 source[position + 1] + 
									 source[position - pixelsX] + 
									 source[position + pixelsX]) >> 2) - dest[position]);  
			   dest[position] -= (int)(dest[position] >> longevityShift);
			}			
		}	
	}
	public void SplashAtPoint (int x, int y, int force) {
		int position = ((y * pixelsX)+ x);
		buffer0[position] = force;
		buffer0[position + 1] = force;
		buffer0[position - pixelsX + 1] = force;
		buffer0[position - pixelsX] = force;
		buffer0[position - pixelsX - 1] = force;
		buffer0[position - 1] = force;
		buffer0[position + pixelsX - 1] = force;
		buffer0[position + pixelsX] = force;
		buffer0[position + pixelsX + 1] = force; 
	}
}