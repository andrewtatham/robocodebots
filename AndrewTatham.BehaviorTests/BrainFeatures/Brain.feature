Feature: The brain executes the correct behavior

Scenario: Initialisation
	Given A new Brain is created
	When RunInit is called
	Then The robots colours should be set to Magenta
	And The robots radar and turret should move independantly of its body
	And a new context is initialised


Scenario: The selected behaviour is the first to return true for the condition
	Given A new Brain is created
	And RunInit is called
	When Run is called
	Then The behaviour whose condition is true is selected




Scenario: On Skipped Turn
	Given A new Brain is created
	When OnSkippedTurn is called
	Then "***OnSkippedTurn***" is logged

	
Scenario: On Scanned Robot
	Given A new Brain is created
	And RunInit is called
	And Run is called
	When OnScannedRobot is called
	Then the enemies collection is updated with the new robot

Scenario: On Bullet Hit Bullet
	Given A new Brain is created
	And RunInit is called
	And Run is called
	When OnBulletHitBullet is called


Scenario: On Hit By Bullet
	Given A new Brain is created
	And RunInit is called
	And Run is called
	When OnHitByBullet is called

	
Scenario: On Robot Death
	Given A new Brain is created
	And RunInit is called
	And Run is called
	When OnRobotDeath is called
	Then the enemies collection is updated with the deceased robot


Scenario: Render
	Given A new Brain is created
	And RunInit is called
	And Run is called
	When Render is called
	Then Render is called on the current behavior
	And Render is called on the context





