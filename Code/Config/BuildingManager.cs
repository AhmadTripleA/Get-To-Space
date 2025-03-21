using Godot;

public partial class BuildingManager : Node3D
{
    public static BuildingManager Instance { get; private set; }
    [Export] public float SnapDistance = 0.1f;  // The distance to snap the building to the floor
    [Export] public StandardMaterial3D PreviewMaterial;
    private Camera3D Camera;

    private Node3D _previewBuilding = null;  // The preview building instance
    private PackedScene _currentBuildingScene = null; // The actual building scene to place
    private bool _isPlacing = false;         // Whether we're currently in "place mode"

    public override void _Ready()
    {
        if (Instance == null)
        {
            Instance = this;
            Camera = GetViewport().GetCamera3D();
            InputManager.CancelAction += CleanUp;
            InputManager.PrimaryClickAction += OnPlace;
        }
        else
        {
            QueueFree(); // Prevent duplicate GameManagers
        }
    }

    public override void _Process(double delta)
    {
        if (_isPlacing && _previewBuilding != null)
        {
            Vector3 mouseWorldPosition = GetMouseWorldPosition();
            _previewBuilding.Position = mouseWorldPosition;
        }
    }

    private void OnPlace()
    {
        if (_isPlacing) FinalizePlacing();
    }

    public void StartPlacing(PackedScene buildingScene)
    {
        CleanUp(); // Remove any existing preview

        _currentBuildingScene = buildingScene;

        // Create a separate preview instance
        _previewBuilding = (Node3D)buildingScene.Instantiate();
        ApplyPreviewEffects(_previewBuilding);
        AddChild(_previewBuilding);
        _isPlacing = true;
    }

    private void FinalizePlacing()
    {
        if (_previewBuilding == null) return;

        // Get final placement position
        Vector3 finalPosition = GetMouseWorldPosition();

        // Instantiate the actual building
        Node3D placedBuilding = (Node3D)_currentBuildingScene.Instantiate();
        placedBuilding.Position = finalPosition;
        AddChild(placedBuilding);

        // Cleanup preview
        CleanUp();
    }

    private void CleanUp()
    {
        if (_previewBuilding != null)
        {
            _previewBuilding.QueueFree();
            _previewBuilding = null;
        }

        _isPlacing = false;
        _currentBuildingScene = null;
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector2 mousePosition = GetViewport().GetMousePosition();

        if (Camera == null)
        {
            GD.PrintErr("No Camera3D found in the viewport.");
            return Vector3.Zero;
        }

        Vector3 rayOrigin = Camera.ProjectRayOrigin(mousePosition);
        Vector3 rayDirection = Camera.ProjectRayNormal(mousePosition);

        PhysicsRayQueryParameters3D query = new()
        {
            From = rayOrigin,
            To = rayOrigin + rayDirection * 1000
        };

        var spaceState = GetWorld3D().DirectSpaceState;
        var result = spaceState.IntersectRay(query);

        if (result.Count > 0)
        {
            Vector3 intersectionPoint = (Vector3)result["position"];
            intersectionPoint.Y = SnapDistance;
            return intersectionPoint;
        }

        return rayOrigin;
    }

    private void ApplyPreviewEffects(Node3D preview)
    {
        DisableCollisionsRecursively(preview);
        ApplyPreviewMaterial(preview);
    }

    /// <summary>
    /// Recursively disables all CollisionShape3D nodes inside a given node.
    /// </summary>
    private void DisableCollisionsRecursively(Node node)
    {
        foreach (Node child in node.GetChildren())
        {
            if (child is CollisionShape3D col)
            {
                col.Disabled = true;
            }

            // Recursively search in all children
            DisableCollisionsRecursively(child);
        }
    }

    /// <summary>
    /// Recursively applies the assigned preview material to all MeshInstance3D nodes inside a given node.
    /// </summary>
    private void ApplyPreviewMaterial(Node node)
    {
        foreach (Node child in node.GetChildren())
        {
            if (child is MeshInstance3D meshInstance && PreviewMaterial != null)
            {
                // Apply the material to all surfaces of the mesh
                for (int i = 0; i < meshInstance.GetSurfaceOverrideMaterialCount(); i++)
                {
                    meshInstance.SetSurfaceOverrideMaterial(i, PreviewMaterial);
                }
            }

            // Recursively apply the effect to all children
            ApplyPreviewMaterial(child);
        }
    }

}
