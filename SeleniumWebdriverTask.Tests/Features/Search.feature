Feature: Search

As an EPAM website user  
I want to be able to use search

Scenario: Searching terms on the main page
	Given I click on magnifying glass on main page
	When I enter '<term>' in the input field and click search
	Then A list of results displayed containing '<term>' in the title

    Examples:
    |term            |
    |BLOCKCHAIN      |
    |Cloud           |
    |Automation      |
