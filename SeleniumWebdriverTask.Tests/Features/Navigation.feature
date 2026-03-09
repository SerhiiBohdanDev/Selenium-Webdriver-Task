Feature: Navigation

As an EPAM website user  
I want to be able to navigate to correct pages

Scenario: Navigating to article about AI
	Given I hover over 'Services' link
	When I click on link to the '<article>' article
	Then Browser navigates to the '<article>' page
    Then 'Our Related Expertise' section is displayed

    Examples:
    |article            |
    |Generative AI      |
    |Responsible AI     |
