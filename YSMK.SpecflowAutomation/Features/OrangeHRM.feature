Feature: OrangeHRM
	Simple calculator for adding two numbers
	Adding this line to checkin GIT

@Login
Scenario: Login to Orange HRM
	Given User is in Login Page
	And User enters UserId and Password
	Then User logout from the Application

Scenario: Open Admin Page of Orange HRM
	Given User is in Login Page
	And User Clicks on Admin Page
	Then Verify user is in Admin Page
	Then User logout from the Application