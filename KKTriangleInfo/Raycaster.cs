using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KKTriangleInfo
{
	class Raycaster : MonoBehaviour
	{
		Camera mainCamera;
		string guiText;
		int[] vertInds;
		Vector3[] verts;
		Vector3[] viewVerts;
		Material glMat;
		KKTICollider hitColl;
		
		void Start()
		{
			mainCamera = Camera.main;
			guiText = "Press \"" + KKTriangleInfo.CASTKEY + "\" to select the polygon underneath the cursor!";
			glMat = new Material(Shader.Find("Hidden/Internal-Colored"));
			glMat.color = KKTriangleInfo.SELECTCOLOR;
			viewVerts = new Vector3[3];
		}

		void Update()
		{
			if (KKTriangleInfo.CASTKEY.IsDown())
			{
				KKTICharaController.UpdateCollidersEvent.Invoke(this, null);
				if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, Mathf.Infinity, 1 << KKTICharaController.KKTICOLLLAYER))
				{
					hitColl = hit.collider.gameObject.GetComponent<KKTICollider>();
					int[] tempTris = hitColl.accessMesh.triangles;
					vertInds = new int[3];
					verts = new Vector3[3];
					for (int i = 0; i < 3; ++i)
					{
						vertInds[i] = tempTris[hit.triangleIndex * 3 + i];
						verts[i] = hitColl.transform.TransformPoint(hitColl.accessVerts[vertInds[i]]);
					}
					guiText = "Mesh name:\t" + hitColl.accessMesh.name +
								"\nTriangle Index:\t" + hit.triangleIndex +
								"\nVertex Numbers:\t" + vertInds[0] + "," + vertInds[1] + "," + vertInds[2] +
								"\nVertex Positions:\t" + verts[0] + "," + verts[1] + "," + verts[2] +
								"\nBarycentric Coords:\t" + hit.barycentricCoordinate.ToString();
				}
				else
				{
					verts = null;
					guiText = "Raycast failed to collide with anything!";
				}
			}
		}

		void OnGUI()
		{
			GUI.Label(new Rect(new Vector2(0f, Screen.height * 0.75f), new Vector2(500f, 500f)), guiText);
		}

		public void OnPostRender()
		{
			if (verts != null && hitColl.isActiveAndEnabled)
			{
				for (int i = 0; i < 3; ++i)
					viewVerts[i] = mainCamera.WorldToViewportPoint(hitColl.transform.TransformPoint(hitColl.accessVerts[vertInds[i]]));

				GL.PushMatrix();
				glMat.SetPass(0);
				GL.LoadOrtho();
				GL.Begin(GL.TRIANGLES);
				GL.Vertex3(viewVerts[0].x, viewVerts[0].y, viewVerts[0].z);
				GL.Vertex3(viewVerts[1].x, viewVerts[1].y, viewVerts[1].z);
				GL.Vertex3(viewVerts[2].x, viewVerts[2].y, viewVerts[2].z);
				GL.End();
				GL.PopMatrix();
			}
		}
	}
}
