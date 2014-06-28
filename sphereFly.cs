using UnityEngine;
using System.Collections;
using Vectrosity;

public class sphereFly : MonoBehaviour
{

		public GameObject prefab;
		public ArrayList group;
		public ArrayList lines;
		public float r;
		private float theta;
		private float phi;
		private int ballNum;
		public VectorLine[] lll ;
		private int num;
		public float dis;

		public float Distance (float x1, float x2, float y1, float y2, float z1, float z2)
		{
				float Temp;
				Temp = ((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2) + (z1 - z2) * (z1 - z2));
				return Temp;
		}

		void Start ()
		{
				ballNum=100;
				num = 200;
				dis = 10.0f;
				VectorLine[] lll = new VectorLine[num];
				group = new ArrayList ();
				lines = new ArrayList ();

				r = 10.0f;
				theta = 0.0f;
				phi = 0.0f;
				for (int i=0; i<ballNum; i++) {
						group.Add (new Epoint (r, prefab,i));
				}

		}

		void Update ()
		{
				VectorLine.Destroy (lll);
				for (int i=0; i<ballNum; i++) {
						(group [i] as Epoint).Update ();	
						
				}
				num = 0;
				for (int i=0; i<ballNum; i++) {
						for (int j=i+1; j<ballNum; j++) {
								Epoint aa = (group [i] as Epoint);
								Epoint bb = (group [j] as Epoint);
								float t_x1 = aa.get_x ();
								float t_y1 = aa.get_y ();
								float t_z1 = aa.get_z ();

								float t_x2 = bb.get_x ();
								float t_y2 = bb.get_y ();
								float t_z2 = bb.get_z ();
								if (Distance (t_x1, t_x2, t_y1, t_y2, t_z1, t_z2) < dis) {	
										num = num + 1;
										if (num < 200) {
												Vector3[] linePos = {
														new Vector3 (t_x1, t_y1, t_z1),
														new Vector3 (t_x2, t_y2, t_z2)
												};						
												lll [num] = new VectorLine ("Line"+num, linePos, Color.white, null, 1.0f);
												lll [num].Draw3DAuto();
										}}}}
		}
	

		public class Epoint
		{

				public float r, theta, phi;
				public float v_t, v_p;
				public Vector3 pos;
				public GameObject prefab_;
				private GameObject instObj;
				public float px;
				public float py;
				public float pz;
		
				public Epoint (float e_r, GameObject prefab,int id)
				{
						r = e_r;
						theta = Random.Range (0.0f, 6.28f);
						phi = Random.Range (0.0f, 6.28f);

						v_t = Random.Range (0.01f, 0.02f);
						v_p = Random.Range (0.01f, 0.02f);
						prefab_ = prefab;

						px = r * Mathf.Cos ((theta)) * Mathf.Cos ((phi));
						py = r * Mathf.Sin ((phi));
						pz = r * Mathf.Sin ((theta)) * Mathf.Cos ((phi));

						pos = new Vector3 (px, py, pz);
						instObj = Instantiate (prefab_, pos, Quaternion.identity) as GameObject;
						instObj.name= "s_"+id;
						float scale = Random.Range (0.3f, 0.8f);

						instObj.transform.localScale = new Vector3 (scale, scale, scale);
				}
		
				public void Update ()
				{
						theta = theta + v_t;
						phi = phi + v_p;
						px = r * Mathf.Cos ((theta)) * Mathf.Cos ((phi));
						py = r * Mathf.Sin ((phi));
						pz = r * Mathf.Sin ((theta)) * Mathf.Cos ((phi));
						instObj.transform.position = new Vector3 (px, py, pz);
				}
		
				public float get_x ()
				{
						return r * Mathf.Cos ((theta)) * Mathf.Cos ((phi));
				}

				public float get_y ()
				{
						return r * Mathf.Sin ((phi));
				}

				public float get_z ()
				{
						return r * Mathf.Sin ((theta)) * Mathf.Cos ((phi));
				}
		}
	
}
