using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnitSelectionHandler : MonoBehaviour
{
    [SerializeField] private RectTransform unitSelectionBox = null;

    [SerializeField] private LayerMask layerMask = new LayerMask();

    private Vector2 startPos;

    private RTSPlayer player;
    private Camera mainCam;

    public List<Unit> SelectedUnits { get; } = new List<Unit>();

    private void Start()
    {
        mainCam = Camera.main;
    }

    private void Update()
    {
        if (player == null)
        {
            player = NetworkClient.connection.identity.GetComponent<RTSPlayer>();
        }
        
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            StartSelectionArea();
        }
        else if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            ClearSelectionArea();
        }
        else if (Mouse.current.leftButton.isPressed)
        {
            UpdateSelectionArea();
        }
    }

    private void StartSelectionArea()
    {
        foreach (Unit selectedUnit in SelectedUnits)
        {
            selectedUnit.Deselect();
        }

        SelectedUnits.Clear();

        unitSelectionBox.gameObject.SetActive(true);

        startPos = Mouse.current.position.ReadValue();

        UpdateSelectionArea();
    }

    private void UpdateSelectionArea()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();

        float areaWidth = mousePos.x - startPos.x;
        float areaHeight = mousePos.y - startPos.y;

        unitSelectionBox.sizeDelta = new Vector2(Mathf.Abs(areaWidth), Mathf.Abs(areaHeight));
        unitSelectionBox.anchoredPosition = startPos + new Vector2(areaWidth / 2, areaHeight / 2);
    }

	private void ClearSelectionArea()
	{
        unitSelectionBox.gameObject.SetActive(false);

        if (unitSelectionBox.sizeDelta.magnitude == 0)
        {
            Ray ray = mainCam.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask)) { return; }

            if (!hit.collider.TryGetComponent<Unit>(out Unit unit)) { return; }

            if (!unit.hasAuthority) { return; }

            SelectedUnits.Add(unit);

            foreach (Unit selectedUnit in SelectedUnits)
            {
                selectedUnit.Select();
            }

            return;
        }
		
        Vector2 min = unitSelectionBox.anchoredPosition - (unitSelectionBox.sizeDelta / 2);
        Vector2 max = unitSelectionBox.anchoredPosition + (unitSelectionBox.sizeDelta / 2);

        foreach (Unit unit in player.GetMyUnits())
        {
            Vector3 screenPos = mainCam.WorldToScreenPoint(unit.transform.position);

            if (screenPos.x > min.x && screenPos.x < max.x &&
                screenPos.y > min.y && screenPos.y < max.y)
            {
                SelectedUnits.Add(unit);
                unit.Select();
            }
        }
	}
}
