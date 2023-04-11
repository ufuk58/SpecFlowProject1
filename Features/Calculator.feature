Feature: Go to Google home page
@mytag
Scenario: Search something on Google
	Given Navigate to landing page
	And Click Accept All
	When Enter text to search input and click search
	