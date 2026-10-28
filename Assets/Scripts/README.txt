Unity HW4 upgrade scripts: replace SAME-NAMED existing files (do not create duplicates).
Scripts: AirplaneController, TouchFlightButton, FlightFeedback, FlightZone, GameHUD.
Leave CameraFollow unchanged.
Airplane: root Rigidbody kinematic/no gravity, one BoxCollider; child fly_model visual only.
IMPORTANT: Disable/remove ZeroController and any audio/controller on imported fly_model.
Make StartPanel + StartButton under Canvas; bind GameHUD; start screen controlled by GameHUD.
Set AirplaneController x [-1.5,1.5], y [0.5,5], maxZ 71.
Create Gate01 at (0,0,15), Gate02 at (0,0,35), Gate03 at (0,0,55).
Each gate's children in local coordinates:
  LeftWall Cube pos(-5,2,0), scale(2,8,2), BoxCollider trigger+FlightZone Obstacle
  RightWall Cube pos(5,2,0), scale(2,8,2), BoxCollider trigger+FlightZone Obstacle
  TopWall Cube pos(0,7,0), scale(8,2,2), BoxCollider trigger+FlightZone Obstacle
  Checkpoint empty pos(0,3,2.5), BoxCollider center0 size(7,6,0.5) IsTrigger; FlightZone Checkpoint
Gate03 ends at z55; flag finish at z68 with Finish collider size(7,7,1), local offset as necessary.
Remove previous Obstacle_Left, Obstacle_Right, SuccessZone after creating replacement gates.
Set FlightFeedback targetCheckpoints=3.
Imported fly_model should be a visual child of Airplane. Remove/hide original Body/Wing meshes when visually aligned.
Test PC buttons, touch inputs, collision, checkpoints, finish, restart before Android build.
