using System.Collections.ObjectModel;

namespace Vehicles.Api.Requests;

public class RequestValidationResult
{
    // Key is field name and Value is a list of error messages associated with that property
    private readonly Dictionary<string, List<string>> _errors = [];
    public ReadOnlyDictionary<string, List<string>> ValidationErrors => _errors.AsReadOnly();

    public void AddError(string fieldName, string errorMessage)
    {
        if (!_errors.ContainsKey(fieldName))
        {
            _errors.Add(fieldName, []);
        }
        
        _errors[fieldName].Add(errorMessage);;
    }
    
    public bool IsValid => _errors.Count == 0;
}