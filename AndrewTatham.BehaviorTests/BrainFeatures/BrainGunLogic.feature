Feature: BrainGunLogic




Scenario: The gun does not fire at the aim result until the gun has aimed
	Given A new Brain is created
	And RunInit is called
	And the gun is cool
	And an aim result is set
	And the gun is NOT aimed
	When Run is called
	Then the gun is aimed at the target
	And the gun is NOT fired at the target

Scenario: The gun does not fire at the aim result until it has cooled down
	Given A new Brain is created
	And RunInit is called
	And an aim result is set
	And the gun is aimed 
	And the gun is NOT cool
	When Run is called
	Then the gun is NOT fired at the target
	
Scenario: The gun fires at the aim result when the gun is aimed and cool
	Given A new Brain is created
	And RunInit is called
	And an aim result is set
	And the gun is aimed
	And the gun is cool
	When Run is called
	Then the gun is fired at the target
