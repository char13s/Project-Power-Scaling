/**
	Copyright (C) 2019 NyangireWorks. All Rights Reserved.
 */

using UnityEngine;
using System.Collections;

namespace ThirdPersonCameraWithLockOn
{

	public class FadeObject : MonoBehaviour
	{

		private float opacity = 1f;

		//Renderer renderer;

		private float fadeOutSpeed = 10f;
		public float FadeOutSpeed
		{
			get { return fadeOutSpeed; }
			set
			{
				if (value < 0f)
				{
					fadeOutSpeed = 0f;
				}
				else
				{
					fadeOutSpeed = value;
				}
			}
		}

		private float fadeInSpeed = 10f;
		public float FadeInSpeed
		{
			get { return fadeInSpeed; }
			set
			{
				if (value < 0f)
				{
					fadeInSpeed = 0f;
				}
				else
				{
					fadeInSpeed = value;
				}
			}
		}

		private Shader fadeShader;
		public Shader FadeShader
		{
			get { return fadeShader; }
			set { fadeShader = value; }
		}

		private float targetTransparency;

		private Color[] originalColor;

		private Material[] originalMaterial;
		private Shader[] originalShader;
		//private BlendMode originalRenderMode;

		private bool fadeIn = true;

		private bool debugOn = false;
		public bool DebugOn
		{
			get { return debugOn; }
			set { debugOn = value; }
		}

		private bool[] fadeOutComplete;

		Color GetColor(Material mat)
		{
			if(mat.HasProperty("_BaseColor"))
			{
				//Debug.Log("_BaseColor");
				return mat.GetColor("_BaseColor");
			}
			else if(mat.HasProperty("_Color"))
			{
				//Debug.Log("_Color");
				return mat.GetColor("_Color");
			}
			else
			{
				//Debug.Log("CUSTOM");
				// FOR CUSTOM SHADER GRAPHS
				//return mat.GetColor("#YOUR COLOR PROPERTY#");
				return Color.magenta;
			}
		}

		void SetColor(int i, Color c)
		{
			Material mat = GetComponent<Renderer>().materials[i];
			if(mat.HasProperty("_BaseColor"))
			{
				//Debug.Log("_BaseColor");
				GetComponent<Renderer>().materials[i].SetColor("_BaseColor", c);
			}
			else if(mat.HasProperty("_Color"))
			{
				//Debug.Log("_Color");
				GetComponent<Renderer>().materials[i].SetColor("_Color", c);
			}
			else
			{
				//Debug.Log("NoColorSet");
				// FOR CUSTOM SHADER GRAPHS
				//GetComponent<Renderer>().materials[i].SetColor("#YOUR COLOR PROPERTY#", c);
			}
		}

		// Use this for initialization
		void Start()
		{
			//renderer = this.GetComponent<Renderer> ();

			//originalMaterial = Resources.FindObjectsOfTypeAll<Material>();
			originalMaterial = GetComponent<Renderer>().sharedMaterials;
			Material[] mat = GetComponent<Renderer>().materials;

			fadeOutComplete = new bool[mat.Length];

			originalColor = new Color[mat.Length];
			originalShader = new Shader[mat.Length];

			for (int i = 0; i < mat.Length; i++)
			{
				originalColor[i] = GetColor(mat[i]);

				if (fadeShader)
				{
					originalShader[i] = mat[i].shader;
					mat[i].shader = fadeShader;
				}
				// originalTransparency[i] = mat[i].color.a;

				// if (fadeShader)
				// {
				// 	originalShader[i] = mat[i].shader;
				// 	mat[i].shader = fadeShader;
				// }
			}


			// mat.SetFloat("_Mode", 2);
			// mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
			// mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
			// mat.SetInt("_ZWrite", 0);
			// mat.DisableKeyword("_ALPHATEST_ON");
			// mat.EnableKeyword("_ALPHABLEND_ON");
			// mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
			// mat.renderQueue = 3000;

		}

		// Update is called once per frame
		void Update()
		{

			//FadeIn
			if (fadeIn)
			{
				if (fadeInSpeed == 0)
				{
					opacity = targetTransparency;
				}
				else
				{
					opacity -= ((1.0f - targetTransparency) * Time.deltaTime) * fadeInSpeed;
				}
				//reached target transparency
				if (opacity <= targetTransparency)
				{
					opacity = targetTransparency;
					fadeIn = false;
				}

				Material[] mats = GetComponent<Renderer>().materials;

				for (int i = 0; i < mats.Length; i++)
				{
					Color C = GetColor(mats[i]);
					//Color C = mats[i].color;
					C.a = opacity;
					SetColor(i, C);
					//GetComponent<Renderer>().materials[i].color = C;
					fadeOutComplete[i] = false;
				}

			}

			//FadeOut
			else
			{
				Material[] mats = GetComponent<Renderer>().materials;
				for (int i = 0; i < mats.Length; i++)
				{
					if (opacity < originalColor[i].a)
					{
						Color C = GetColor(mats[i]);
						//Color C = mats[i].color;
						C.a = opacity;
						SetColor(i, C);
						//GetComponent<Renderer>().materials[i].color = C;
						fadeOutComplete[i] = false;
					}
					else if (!fadeOutComplete[i])
					{
						//Debug.Log(opacity);

						Material mat = GetComponent<Renderer>().materials[i];

						//Color C = GetColor(mat);
						//Color c = GetComponent<Renderer>().materials[i].color;
						Color C = originalColor[i];


						if (fadeShader)
						{
							mat.shader = originalShader[i];
							
							//mat.SetColor(originalShader[i].name, c);
							SetColor(i, C);
							//GetComponent<Renderer>().materials[i] = GetComponent<Renderer>().sharedMaterials[i];
							mat.EnableKeyword("_ALPHABLEND_ON");
							GetComponent<Renderer>().materials[i] = originalMaterial[i];
							
							mat.SetInt("_ZWrite", 1);
							mat.DisableKeyword("_ALPHABLEND_ON");

						}
						else
						{
							//mat.EnableKeyword("_ALPHABLEND_ON");
							//Destroy(GetComponent<Renderer>().materials[i]);
							//GetComponent<Renderer>().materials[i] = null;
							GetComponent<Renderer>().materials[i] = originalMaterial[i];
							//GetComponent<Renderer>().sharedMaterials[i] = originalMaterial[i];
							//GetComponent<Renderer>().materials[i] = GetComponent<Renderer>().sharedMaterials[i];
							//mat.color = c;
							SetColor(i, C);
						}

						//mat.SetFloat("_Mode", 0);
						//mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
						//mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
						//mat.DisableKeyword("_ALPHATEST_ON");
						//mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
						//mat.renderQueue = 2000;

						fadeOutComplete[i] = true;

					}

					if (fadeOutSpeed == 0)
					{
						opacity = originalColor[i].a;
					}
					else
					{
						opacity += ((1.0f - targetTransparency) * Time.deltaTime) * fadeOutSpeed;
					}




				}

				// check if everyhting returned to normal
				bool allClear = true;
				for (int i = 0; i < mats.Length; i++)
				{
					if (!fadeOutComplete[i])
						allClear = false;
				}
				// And remove this script
				if (allClear)
				{
					//Debug.Log("Destroyed");
					Destroy(this);
				}

			}

			//UnityEngine.Debug.Log("opacity: " + opacity);

		}

		public void SetTransparency(float newTP)
		{
			targetTransparency = newTP;
			fadeIn = true;
			//opacity = targetTransparency;
		}

		// void OnDestroy()
		// {
		// 	//Destroy the instance
		// 	//Destroy(m_Material);
		// 	//Output the amount of materials to show if the instance was deleted
		// 	print("Materials " + Resources.FindObjectsOfTypeAll(typeof(Material)).Length);
		// }

	}
}