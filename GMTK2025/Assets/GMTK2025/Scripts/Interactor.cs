using TMPro;
using UnityEngine;
using UnityEngine.UI;

interface IInteractable
{
    public void Interact();
    public string Prompt();
    //give objects specific interact components
}
public class Interactor : MonoBehaviour
{
    [SerializeField] private Transform interactorSource;
    [SerializeField] private float interactRange;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private TMP_Text prompt;
    [SerializeField] private RectTransform promptUI;
    [SerializeField] private Camera cam;

    private IInteractable currentInteractable;
    private Transform targetUIPos;

    // Update is called once per frame
    void Start()
    {
        targetUIPos = null;
    }
    void Update()
    {
        FindNearestInteractable();

        if (currentInteractable != null)
        {
            prompt.gameObject.SetActive(true);

            Vector3 screenPos = cam.WorldToScreenPoint(targetUIPos.position);
            prompt.text = currentInteractable.Prompt();
            promptUI.position = screenPos;

            if (Input.GetKeyDown(KeyCode.E))
            {
                currentInteractable.Interact();
            }
        }
        else
        {
            prompt.gameObject.SetActive(false);
        }
    }

    void FindNearestInteractable()
    {
        Collider[] colliders = Physics.OverlapSphere(interactorSource.position, interactRange, interactableLayer);
        float closestDistance = Mathf.Infinity;
        currentInteractable = null;

        foreach (var col in colliders)
        {
            if (col.TryGetComponent(out IInteractable interactable))
            {
                float distance = Vector3.Distance(interactorSource.position, col.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    currentInteractable = interactable;
                    targetUIPos = col.transform;
                }
            }
        }
    }
}
