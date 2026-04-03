@BddUI
Feature: Download

As an EPAM website user  
I want to be able to download files

Scenario: Downloading quick facts document
	Given I click 'Corporate responsibility' link
	When I scroll down and click the download link on 'Corporate responsibility' page
	Then Browser successfully downloads the file '<filename>'

    Examples:
    |filename                      |
    |EPAM_ESG_Quick_Facts.pdf      |
