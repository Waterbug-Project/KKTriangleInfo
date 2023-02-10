using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KKTriangleInfo
{
	class MyDebugTools
	{
		public struct TwoObjs
		{
			public GameObject objOne;
			public GameObject objTwo;

			public TwoObjs(GameObject one, GameObject two)
			{
				objOne = one;
				objTwo = two;
			}
		}

		public static GameObject MakeDebugShape(Vector3 inLocation, Vector3 inScale, PrimitiveType inType = PrimitiveType.Sphere, Transform inParent = null)
		{
			GameObject output = GameObject.CreatePrimitive(inType);
			UnityEngine.Object.Destroy(output.GetComponent<Collider>());
			UnityEngine.Object.Destroy(output.GetComponent<Rigidbody>());
			output.transform.position = inLocation;
			output.transform.localScale = inScale;
			if (inParent != null)
				output.transform.parent = inParent;
			return output;
		}

		public static TwoObjs MakeDebugLine(Ray inRay, float inLen, Color inColor, float inWidth = 0.001f, Transform inParent = null)
		{
			return MakeDebugLine(inRay.origin, inRay.origin + inRay.direction.normalized * inLen, inColor, inWidth, inParent);
		}

		public static TwoObjs MakeDebugLine(Vector3 inLineStart, Vector3 inLineEnd, Color inColor, float inWidth = 0.001f, Transform inParent = null)
		{
			GameObject newLine = new GameObject();
			LineRenderer newRend = newLine.AddComponent<LineRenderer>();
			newRend.alignment = LineAlignment.View;
			newRend.endColor = inColor;
			newRend.startColor = inColor;
			newRend.startWidth = inWidth;
			newRend.endWidth = inWidth;

			//Attempt to give the line a specific color...
			GameObject temp = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			newRend.material = temp.GetComponent<MeshRenderer>().sharedMaterial;
			newRend.material.color = inColor;
			UnityEngine.Object.Destroy(temp);

			Vector3[] points = new Vector3[2];
			points[0] = inLineStart;
			points[1] = inLineEnd;
			newRend.positionCount = 2;
			newRend.SetPositions(points);

			newLine.transform.position = inLineStart;
			if (inParent != null)
				newLine.transform.parent = inParent;
			GameObject lineEnd = new GameObject();
			lineEnd.transform.position = inLineEnd;
			lineEnd.transform.parent = inParent;

			return new TwoObjs(newLine, lineEnd);
		}
	}
}
