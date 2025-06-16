using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;


public class TileStateHandler : MonoBehaviour
{
    private TileStateMachine tileStateMachine;
    // Getter;
    public TileStateMachine TileStateMachine => tileStateMachine;

    // Properties
    private Renderer rend;                                                      // Game Object Renderer Component
    private Material baseMaterial;                                              // Initialized in start from Renderer Materials
    [SerializeField] private Material hoverMaterial;                            // Selected by User to determine which material to use on hover
    private Vector3 posOffset = new Vector3(0, .5f, 0);                         // Offset to apply to tile position when building on this tile
    private ITurretProduct turret;
    QuickOutline outline;
    QuickOutline turretOutline;

    private EventSystem eventSystem;
    private bool isCoroutineRunning = false;
    public bool IsEmpty { get => transform.childCount == 0; }                   // Wheither or not a turret has already been built on this tile


    private void Awake()
    {
        tileStateMachine = new TileStateMachine(this);
    }

    private void Start()
    {
        rend = GetComponent<Renderer>();
        baseMaterial = rend.sharedMaterial;
        outline = GetComponent<QuickOutline>();
        if (outline is not null && outline.enabled) outline.enabled = false;
        eventSystem = EventSystem.current;
        tileStateMachine.Init(TileStateMachine.baseState);
        tileStateMachine.stateChanged += OnStateChanged;
    }

    private void OnDestroy()
    {
        tileStateMachine.stateChanged -= OnStateChanged;
        if (turret is not null && turret.gameObject is not null)
        {
            turret.gameObject.GetComponent<TurretController>().DestroyEvent -= OnTurretDestroy;
        }
    }

    private void OnStateChanged(ITileState state)
    {
        Material[] materials = rend.sharedMaterials;
        materials[0] = state.GetType().Name.Equals("TileBaseState") ? baseMaterial : hoverMaterial;
        rend.materials = materials;
    }

    public void EnableOutline()
    {
        if (outline is not null) outline.enabled = true;
        if (turretOutline is not null) turretOutline.enabled = true;
    }

    public void DisableOutline()
    {
        if (outline is not null) outline.enabled = false;
        if (turretOutline is not null) turretOutline.enabled = false;
    }

    private void OnMouseEnter()
    {
        // Animation
        // iTween.PunchScale(gameObject, new Vector3(60, 60, 60), 0.5f);
        if (!isCoroutineRunning)
        {
            StartCoroutine(PunchScaleCoroutine(new Vector3(60, 60, 60), 0.5f));
        }
    }

    private void OnMouseOver()
    {
        if (TurretMenu.Instance.gameObject.activeInHierarchy)
            return;

        if (IsEmpty)
        {
            // Get position to build at for this tile
            Vector3 pos = transform.position + posOffset;
            // Show turret preview at this position
            TurretBuilder.Instance.ShowPreview(pos);
        }

        if (tileStateMachine.CurrentState != tileStateMachine.hoverState)
        {
            tileStateMachine.TransitionTo(tileStateMachine.hoverState);
        }
    }

    private void OnMouseExit()
    {
        TurretBuilder.Instance.HidePreview();

        tileStateMachine.TransitionTo(tileStateMachine.baseState);
    }

    private void OnMouseUp()
    {
        if (TurretMenu.Instance.gameObject.activeInHierarchy)
        {
            eventSystem.SetSelectedGameObject(gameObject);            
            return;
        }

        if (IsEmpty)
        {
            // Get position to build at for this tile
            Vector3 pos = transform.position + posOffset;
            // Hide preview at this pos 
            TurretBuilder.Instance.HidePreview();
            // Build turret at this position
            turret = TurretBuilder.Instance.Build(pos);

            if (turret != null)
            {
                // Turret becomes a Child of this Tile
                turret.gameObject.transform.parent = transform;
                // Tiny ScreenShake
                CamShake.Instance.Shake(0.1f, 0.3f, 0.5f);
                // Listen turret event
                turret.gameObject.GetComponent<TurretController>().DestroyEvent += OnTurretDestroy;
                // Add Turret Outline
                turretOutline = turret.gameObject.GetComponent<QuickOutline>();
                EnableOutline();
            }
        }
        else
        {
            TurretMenu.Instance.SetTurret(turret);
            TurretMenu.Instance.ShowMenu(0);
        }
    }

    private void OnTurretDestroy()
    {
        turret.gameObject.GetComponent<TurretController>().DestroyEvent -= OnTurretDestroy;
        turretOutline = null;
    }

    IEnumerator PunchScaleCoroutine(Vector3 amount, float duration)
    {
        isCoroutineRunning = true;

        Vector3 scale = transform.localScale;
        float elapsed = 0f;

		// values holder [0] from, [1] to, [2] calculated value from ease equation:
		Vector3[] vector3s = new Vector3[3];
		
		// from values:
		vector3s[0] = transform.localScale;
		vector3s[1] = amount;

        while (elapsed < duration)
        {
            float percentage = elapsed / duration;

            // Calculat vector3[2]

            // x
            if (vector3s[1].x > 0)
            {
                vector3s[2].x = Punch(vector3s[1].x, percentage);
            }
            else if (vector3s[1].x < 0)
            {
                vector3s[2].x=-Punch(Mathf.Abs(vector3s[1].x), percentage); 
            }
            // y
            if (vector3s[1].y > 0)
            {
                vector3s[2].y = Punch(vector3s[1].y, percentage);
            }
            else if (vector3s[1].y < 0)
            {
                vector3s[2].y = -Punch(Mathf.Abs(vector3s[1].y), percentage); 
            }
            // z 
            if (vector3s[1].z > 0)
            {
                vector3s[2].z = Punch(vector3s[1].z, percentage);
            }
            else if (vector3s[1].z < 0)
            {
                vector3s[2].z = -Punch(Mathf.Abs(vector3s[1].z), percentage); 
            }
		
            //apply:
            transform.localScale = vector3s[0] + vector3s[2];

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localScale = vector3s[0];
        isCoroutineRunning = false;

        yield return null;
    }

	private float Punch(float amplitude, float value)
    {
		float s = 9;
		if (value == 0){
			return 0;
		}
		else if (value == 1){
			return 0;
		}
		float period = 1 * 0.3f;
		s = period / (2 * Mathf.PI) * Mathf.Asin(0);
		return (amplitude * Mathf.Pow(2, -10 * value) * Mathf.Sin((value * 1 - s) * (2 * Mathf.PI) / period));
    }

}
