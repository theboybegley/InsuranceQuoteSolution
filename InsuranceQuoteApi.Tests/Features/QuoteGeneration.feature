Feature: Insurance Quote Generation
    As an insurance customer
    I want to receive a motor insurance quote
    So that I can understand the cost of my cover

Scenario: Standard driver receives a medium risk quote
    Given a driver aged 45 with a vehicle worth 15000
    When they request a quote for postcode "SG2"
    Then the annual premium should be 750.00
    And the risk rating should be "Medium"

Scenario: Young driver receives a high risk quote
    Given a driver aged 23 with a vehicle worth 15000
    When they request a quote for postcode "SG2"
    Then the annual premium should be 1350.00
    And the risk rating should be "High"

Scenario: Standard driver with low value vehicle receives a low risk quote
    Given a driver aged 45 with a vehicle worth 5000
    When they request a quote for postcode "SG2"
    Then the annual premium should be 250.00
    And the risk rating should be "Low"
