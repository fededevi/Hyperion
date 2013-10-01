using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class DrawCameraDebugFrustum : MonoBehaviour 
{
	public int precision = 100;
	Vector3[] points;
	public Color renderBorderColor = Color.red;
	public Color frustumBorderColor= Color.green;
	
	void Start ()
	{
		points = new Vector3[precision];
	}
	
	void Update()
	{
		if (Application.isEditor) DrawFrustum();
	}
	
	void DrawFrustum () 
	{
		int precisionPerEdge = precision / 4;
		int count = 0;
		for (int i = 0; i < precision; i++)
		{
			points[i] = new Vector3(0.0f,0.0f,0.0f);	
		}
		for (int i = 0; i < precisionPerEdge; i++)
		{
			Ray ray = camera.ViewportPointToRay (new Vector3((float)i/(float)precisionPerEdge,0.0f,0.0f)); 
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit)) 
			{
				points[count] = hit.point;
			}		
			count++;
		}		
		for (int i = 0; i < precisionPerEdge; i++)
		{
			Ray ray = camera.ViewportPointToRay (new Vector3(1.0f,(float)i/(float)precisionPerEdge,0.0f)); 
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit)) 
			{
				points[count] = hit.point;
			}		
			count++;
		}		
		for (int i = 0; i < precisionPerEdge; i++)
		{
			Ray ray = camera.ViewportPointToRay (new Vector3(1.0f-((float)i/(float)precisionPerEdge),1.0f,0.0f)); 
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit)) 
			{
				points[count] = hit.point;
			}		
			count++;
		}		
		for (int i = 0; i < precisionPerEdge; i++)
		{
			Ray ray = camera.ViewportPointToRay (new Vector3( 0.0f, 1.0f-((float)i/(float)precisionPerEdge),  0.0f ) ); 
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit)) 
			{
				points[count] = hit.point;
			}		
			count++;
		}
		for (int i = 0; i < precision; i++)
		{
			int j = i+1;
			if (j == precision)  j = 0;
			if ( !(points[i].x==0.0f || points[j].x == 0.0f ) ) Debug.DrawLine(points[i],points[j],renderBorderColor);	
		}
		
		Vector3 zll = camera.ScreenToWorldPoint ( new Vector3 (0.0f,0.0f,camera.nearClipPlane) );
		Vector3 zlr = camera.ScreenToWorldPoint ( new Vector3 (0.0f,camera.pixelHeight,camera.nearClipPlane) );
		Vector3 zul = camera.ScreenToWorldPoint ( new Vector3 (camera.pixelWidth,0.0f,camera.nearClipPlane) );
		Vector3 zur = camera.ScreenToWorldPoint ( new Vector3 (camera.pixelWidth,camera.pixelHeight,camera.nearClipPlane) );
		
		Vector3 wll = camera.ScreenToWorldPoint ( new Vector3 (0.0f,0.0f,camera.farClipPlane) );
		Vector3 wlr = camera.ScreenToWorldPoint ( new Vector3 (0.0f,camera.pixelHeight,camera.farClipPlane) );
		Vector3 wul = camera.ScreenToWorldPoint ( new Vector3 (camera.pixelWidth,0.0f,camera.farClipPlane) );
		Vector3 wur = camera.ScreenToWorldPoint ( new Vector3 (camera.pixelWidth,camera.pixelHeight,camera.farClipPlane) );
		
		Debug.DrawLine(zll,zlr,frustumBorderColor);Debug.DrawLine(zlr,zur,frustumBorderColor);Debug.DrawLine(zur,zul,frustumBorderColor);Debug.DrawLine(zul,zll,frustumBorderColor);
		Debug.DrawLine(wll,wlr,frustumBorderColor);Debug.DrawLine(wlr,wur,frustumBorderColor);Debug.DrawLine(wur,wul,frustumBorderColor);Debug.DrawLine(wul,wll,frustumBorderColor);
		Debug.DrawLine(zll,wll,frustumBorderColor);Debug.DrawLine(zlr,wlr,frustumBorderColor);Debug.DrawLine(zur,wur,frustumBorderColor);Debug.DrawLine(zul,wul,frustumBorderColor);
		
	}
}


