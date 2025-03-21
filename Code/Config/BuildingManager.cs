using Godot;

public partial class BuildingManager : Node3D
{
    public static BuildingManager Instance { get; private set; }
    [Export] public float SnapDistance = 0.1f;  // The distance to snap the building to the floor
    private Camera3D Camera;
    private Node3D _currentBuilding = null;  // The currently placed (preview) building
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

        if (_isPlacing)
        {
            Vector3 mouseWorldPosition = GetMouseWorldPosition();
            // Update building's position to follow the mouse position
            if (_currentBuilding != null)
            {
                _currentBuilding.Position = mouseWorldPosition;
            }
        }
    }

    private void OnPlace()
    {
        if(_isPlacing) FinalizePlacing();
    }

    public void StartPlacing(PackedScene Building)
    {
        // Instantiate the building scene as a preview object
        if (_currentBuilding != null)
        {
            _currentBuilding.QueueFree();
        }

        _currentBuilding = (Node3D)Building.Instantiate();
        AddChild(_currentBuilding);
        _isPlacing = true;
    }

    public void FinalizePlacing()
    {
        if (_currentBuilding == null) return;

        // Finalize placement and create the building
        Vector3 mouseWorldPosition = GetMouseWorldPosition();
        _currentBuilding.Position = mouseWorldPosition;

        CleanUp();
    }

    public void CleanUp()
    {
        if (_isPlacing)
        {
            _currentBuilding = null;
            _isPlacing = false;
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        // Retrieve the mouse position in viewport coordinates
        Vector2 mousePosition = GetViewport().GetMousePosition();

        if (Camera == null)
        {
            GD.PrintErr("No Camera3D found in the viewport.");
            return Vector3.Zero;
        }

        // Calculate the ray's origin and direction from the camera through the mouse position
        Vector3 rayOrigin = Camera.ProjectRayOrigin(mousePosition);
        Vector3 rayDirection = Camera.ProjectRayNormal(mousePosition);

        // Create the PhysicsRayQueryParameters3D object
        PhysicsRayQueryParameters3D query = new()
        {
            // Set the ray's origin and end positions
            From = rayOrigin,
            To = rayOrigin + rayDirection * 1000, // Raycasting with a distance of 1000 units

            // Optionally set the collision mask, for example, to only hit a specific layer
            // CollisionMask = 1 // Adjust as needed
        };

        // Perform the ray intersection
        var spaceState = GetWorld3D().DirectSpaceState;
        var result = spaceState.IntersectRay(query);

        // Check if we have a valid intersection result
        if (result.Count > 0)
        {
            // Extract the intersection point
            Vector3 intersectionPoint = (Vector3)result["position"];

            // Snap to a specific Y value, for example, ground level (optional)
            intersectionPoint.Y = SnapDistance;

            return intersectionPoint;
        }

        // If no intersection, return the original ray origin
        return rayOrigin;
    }



}
