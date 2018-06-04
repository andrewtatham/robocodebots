Feature: BrainRadarLogic


Scenario: Radar defaults to full scan
	Given A new Brain is created
	And RunInit is called
	And the radar result is NOT set
	When Run is called
	Then the radar performs a full scan


Scenario: Radar full scan
	Given A new Brain is created
	And RunInit is called
	And the radar result is set to full scan
	When Run is called
	Then the radar performs a full scan

	Scenario: Radar target scan
	Given A new Brain is created
	And RunInit is called
	And a target is set
	And the radar result is set to scan target
	When Run is called
	Then the radar performs a target scan

