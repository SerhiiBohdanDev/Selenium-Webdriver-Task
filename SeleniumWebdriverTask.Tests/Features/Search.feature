@BddUiTest
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

Scenario: Searching language on the jobs page
    Given I click join us link
    When I input search '<language>', '<country>' and select remote option
    Then The last job in the search results should contain the searched '<language>'

    Examples:
    | language        | country |
    | javascript, js  | Armenia |
    | javascript, js  | Georgia |
    | NET             | Armenia |
    | NET             | Georgia |
    | python          | Armenia |
    | python          | Georgia |
