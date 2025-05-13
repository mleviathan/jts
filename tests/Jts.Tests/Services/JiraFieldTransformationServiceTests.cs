using Jts.Models;
using Jts.Models.Jira;
using Jts.Services;

namespace Jts.Tests.Services;

public class JiraFieldTransformationServiceTests
{
    private readonly JiraFieldTransformationService _service;

    public JiraFieldTransformationServiceTests()
    {
        _service = new JiraFieldTransformationService();
    }

    [Fact]
    public void ElaborateRequiredFieldsValues_ShouldHandleParentIssueField()
    {
        // Arrange
        var requestFields = new List<RequestTypeField>
        {
            new() { Name = "Parent Issue", FieldId = "parent", Required = true }
        };
        var parentIssue = new Issue { Key = "TEST-123" };

        // Act
        var result = _service.ElaborateRequiredFieldsValues(requestFields, parentIssue, "user");

        // Assert
        Assert.Single(result);
        Assert.Equal("TEST-123", result["parent"]);
    }

    [Fact]
    public void ElaborateRequiredFieldsValues_ShouldHandleReferenteField()
    {
        // Arrange
        var requestFields = new List<RequestTypeField>
        {
            new() { Name = "Referente", FieldId = "referente", Required = true }
        };
        var parentIssue = new Issue { Key = "TEST-123" };

        // Act
        var result = _service.ElaborateRequiredFieldsValues(requestFields, parentIssue, "user");

        // Assert
        Assert.Single(result);
        var value = Assert.IsType<Dictionary<string, string>>(result["referente"]);
        Assert.Equal("user", value["name"]);
    }

    [Fact]
    public void ElaborateRequiredFieldsValues_ShouldHandleDescriptionField()
    {
        // Arrange
        var requestFields = new List<RequestTypeField>
        {
            new() { Name = "Description", FieldId = "description", Required = true }
        };
        var parentIssue = new Issue { Key = "TEST-123", Description = "Test Description" };

        // Act
        var result = _service.ElaborateRequiredFieldsValues(requestFields, parentIssue, "user");

        // Assert
        Assert.Single(result);
        var value = Assert.IsType<string>(result["description"]);
        Assert.Contains("Cloned from TEST-123", value);
        Assert.Contains("Test Description", value);
    }

    [Fact]
    public void ElaborateRequiredFieldsValues_ShouldHandleSummaryField()
    {
        // Arrange
        var requestFields = new List<RequestTypeField>
        {
            new() { Name = "Summary", FieldId = "summary", Required = true }
        };
        var parentIssue = new Issue { Key = "TEST-123", Summary = "Test Summary" };

        // Act
        var result = _service.ElaborateRequiredFieldsValues(requestFields, parentIssue, "user");

        // Assert
        Assert.Single(result);
        Assert.Equal("Test Summary", result["summary"]);
    }

    [Fact]
    public void ElaborateRequiredFieldsValues_ShouldHandleArrayTypeFields()
    {
        // Arrange
        var requestFields = new List<RequestTypeField>
        {
            new()
            {
                Name = "Components",
                FieldId = "components",
                Required = true,
                JiraSchema = new JiraSchema { Type = "array" },
                ValidValues =
                [
                    new ValidValue { Label = "Frontend", Value = "frontend" },
                    new ValidValue { Label = "Backend", Value = "backend" }
                ]
            }
        };
        var parentIssue = new Issue { Key = "TEST-123" };

        // Act
        var result = _service.ElaborateRequiredFieldsValues(requestFields, parentIssue, "user");

        // Assert
        Assert.Single(result);
        var components = Assert.IsType<List<Dictionary<string, string>>>(result["components"]);
        Assert.Equal(2, components.Count);
        Assert.Equal("Frontend", components[0]["name"]);
        Assert.Equal("Backend", components[1]["name"]);
    }

    [Fact]
    public void ElaborateRequiredFieldsValues_ShouldSkipNonRequiredFields()
    {
        // Arrange
        var requestFields = new List<RequestTypeField>
        {
            new() { Name = "Optional Field", FieldId = "optional", Required = false }
        };
        var parentIssue = new Issue { Key = "TEST-123" };

        // Act
        var result = _service.ElaborateRequiredFieldsValues(requestFields, parentIssue, "user");

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void ElaborateRequiredFieldsValues_ShouldThrowException_WhenDefaultFieldHasNoValues()
    {
        // Arrange
        var requestFields = new List<RequestTypeField>
        {
            new()
            {
                Name = "Custom Field",
                FieldId = "custom",
                Required = true,
                JiraSchema = new JiraSchema { Type = "string" },
                ValidValues = []
            }
        };
        var parentIssue = new Issue { Key = "TEST-123" };

        // Act & Assert
        var ex = Assert.Throws<Exception>(() =>
            _service.ElaborateRequiredFieldsValues(requestFields, parentIssue, "user"));
        Assert.Contains("has no default value", ex.Message);
    }

    [Fact]
    public void ElaborateRequiredFieldsValues_ShouldThrowException_WhenValidValuesIsNull()
    {
        // Arrange
        var requestFields = new List<RequestTypeField>
        {
            new()
            {
                Name = "Custom Field",
                FieldId = "custom",
                Required = true,
                JiraSchema = new JiraSchema { Type = "string" },
                ValidValues = null!
            }
        };
        var parentIssue = new Issue { Key = "TEST-123" };

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            _service.ElaborateRequiredFieldsValues(requestFields, parentIssue, "user"));
    }

    [Fact]
    public void ElaborateRequiredFieldsValues_ShouldThrowException_WhenJiraSchemaIsNull()
    {
        // Arrange
        var requestFields = new List<RequestTypeField>
        {
            new()
            {
                Name = "Custom Field",
                FieldId = "custom",
                Required = true,
                JiraSchema = null!,
                ValidValues = []
            }
        };
        var parentIssue = new Issue { Key = "TEST-123" };

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            _service.ElaborateRequiredFieldsValues(requestFields, parentIssue, "user"));
    }
    
    [Fact] public void ElaborateRequiredFieldsValues_ShouldElaborate_ValidValues() {
        // Arrange
        var requestFields = new List<RequestTypeField> {
            new() {
                Name = "Custom Field",
                FieldId = "custom",
                Required = true,
                JiraSchema = new JiraSchema { Type = "string" },
                ValidValues = [new ValidValue { Label = "Value1", Value = "value1" }]
            }
        };
        var parentIssue = new Issue { Key = "TEST-123" };

        // Act
        var result = _service.ElaborateRequiredFieldsValues(requestFields, parentIssue, "user");

        // Assert
        Assert.Single(result);
        Assert.Equal("value1", result["custom"]);
    }
}
