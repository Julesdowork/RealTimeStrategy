using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class BuildingButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Building building = null;
    [SerializeField] private Image iconImage = null;
    [SerializeField] private TMP_Text priceText = null;
    [SerializeField] private LayerMask floorMask = new LayerMask();

    private Camera mainCam;
    private RTSPlayer player;
    private GameObject buildingPreview;
    private Renderer buildingRenderer;

	private void Start()
    {
        mainCam = Camera.main;

        iconImage.sprite = building.GetIcon();
        priceText.text = building.GetPrice().ToString();
    }

    private void Update()
    {
        if (player == null)
        {
            player = NetworkClient.connection.identity.GetComponent<RTSPlayer>();
        }

        if (buildingPreview == null) { return; }

        UpdateBuildingPreview();
    }

	public void OnPointerDown(PointerEventData eventData)
	{
		if (eventData.button != PointerEventData.InputButton.Left) { return; }

        buildingPreview = Instantiate(building.GetBuildingPreview());
        buildingRenderer = buildingPreview.GetComponentInChildren<Renderer>();

        buildingPreview.SetActive(false);
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		if (buildingPreview == null) { return; }

        Ray ray = mainCam.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, floorMask))
        {
            // place building
        }

        Destroy(buildingPreview);
	}

    private void UpdateBuildingPreview()
    {
        Ray ray = mainCam.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, floorMask)) { return; }
        
        buildingPreview.transform.position = hit.point;

        if (!buildingPreview.activeSelf)
        {
            buildingPreview.SetActive(true);
        }
    }
}
