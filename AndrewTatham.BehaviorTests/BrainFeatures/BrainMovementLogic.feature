Feature: BrainMovementLogic

Scenario: move
	Given A new Brain is created
	And RunInit is called
	And the move to absolute location is set
	When Run is called
	Then the robot moves toward the location





