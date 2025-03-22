using Godot;

public partial class BuildingManager : Node3D
{
    public static BuildingManager Instance { get; private set; }
    [Export] public float SnapDistance = 0.1f;  // The distance to snap the building to the floor
    [Export] public StandardMaterial3D PreviewMaterial;

    private Camera3D camera;
    private Node3D previewBuilding = null;  // The preview building instance
    private PackedScene buildingScene = null; // The actual building scene to place
    private bool inPlaceMode = false;         // Whether we're currently in "place mode"
    private bool canPlace = true;             // Detect overlapping geometry

    public override void _Ready()
    {
        if (Instance == null)
        {
            Instance = this;
            camera = GetViewport().GetCamera3D();
            InputManager.CancelAction += CleanUp;
            InputManager.PrimaryClickAction += OnConfirmBuild;
        }
        else
        {
            QueueFree(); // Prevent duplicate GameManagers
        }
    }

    public override void _Process(double delta)
    {
        if (inPlaceMode && previewBuilding != null)
        {
            Vector3 mouseWorldPosition = GetMouseWorldPosition();
            previewBuilding.Position = mouseWorldPosition;
        }
    }

    public static Node3D Build(PackedScene prefab, Vector3 position)
    {
        Node3D newBuilding = (Node3D)prefab.Instantiate();
        newBuilding.Position = position;

        return newBuilding;
    }

    public void StartPlacing(PackedScene buildingScene)
    {
        CleanUp(); // Remove any existing preview

        this.buildingScene = buildingScene;

        // Create a separate preview instance
        previewBuilding = (Node3D)buildingScene.Instantiate();
        ApplyPreviewEffects(previewBuilding);
        AddChild(previewBuilding);
        inPlaceMode = true;
    }

    private void OnConfirmBuild()
    {
        if (inPlaceMode) FinalizePlacing();
    }

    private void FinalizePlacing()
    {
        if (previewBuilding == null) return;

        // Get final placement position
        Vector3 finalPosition = GetMouseWorldPosition();

        // Instantiate the actual building
        var newBuilding = Build(buildingScene, finalPosition);
        AddChild(newBuilding);

        // Cleanup preview
        CleanUp();
    }

    private void CleanUp()
    {
        if (previewBuilding != null)
        {
            previewBuilding.QueueFree();
            previewBuilding = null;
        }

        inPlaceMode = false;
        buildingScene = null;
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector2 mousePosition = GetViewport().GetMousePosition();

        if (camera == null)
        {
            GD.PrintErr("No Camera3D found in the viewport.");
            return Vector3.Zero;
        }

        Vector3 rayOrigin = camera.ProjectRayOrigin(mousePosition);
        Vector3 rayDirection = camera.ProjectRayNormal(mousePosition);

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
