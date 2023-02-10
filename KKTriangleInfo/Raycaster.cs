using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KKTriangleInfo
{
	class Raycaster : MonoBehaviour
	{
		Camera mainCamera;
		string guiText;
		SinglePolygon displayTri;
		
		void Start()
		{
			mainCamera = Camera.main;
			guiText = "";
		}

		void Update()
		{
			try
			{
				if (Input.GetKeyDown("t"))
				{
					if (displayTri != null)
						Destroy(displayTri);
					KKTICharaController.UpdateCollidersEvent.Invoke(this, null);
					if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, Mathf.Infinity, 1 << KKTICharaController.KKTICOLLLAYER))
					{
						KKTICollider hitColl = hit.collider.gameObject.GetComponent<KKTICollider>();
						int[] tempTris = hitColl.accessMesh.triangles;
						int[] vertInds = new int[3];
						Vector3[] verts = new Vector3[3];
						for (int i = 0; i < 3; ++i)
						{
							vertInds[i] = tempTris[hit.triangleIndex * 3 + i];
							verts[i] = hitColl.transform.TransformPoint(hitColl.accessVerts[vertInds[i]]);
						}
						guiText = "Mesh name:\t" + hitColl.name +
									"\nTriangle Index:\t" + hit.triangleIndex +
									"\nVertex Numbers:\t" + vertInds[0] + "," + vertInds[1] + "," + vertInds[2] +
									"\nVertex Positions:\t" + verts[0] + "," + verts[1] + "," + verts[2] +
									"\nBarycentric Coords:\t" + hit.barycentricCoordinate.ToString();
						displayTri = SinglePolygon.MakeSinglePolygonObj(hit.collider.gameObject, hit.triangleIndex);
					}
					else
						guiText = "Raycast failed to collide with anything!";
				}
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.Log("Found exception in Raycaster.Update: " + ex.ToString() + "!");
			}
		}

		void OnGUI()
		{
			GUI.Label(new Rect(new Vector2(0f, 110f), new Vector2(500f, 500f)), guiText);
		}
	}
}
