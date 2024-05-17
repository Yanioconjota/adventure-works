using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

public class SalesController
{
    private readonly ILogger<SalesController> _logger;

    public SalesController()
    {
        _logger = _logger;
    }

    // [HttpGet]
    // public async Task<IEnumerable<VStoreWithContact>> GetStoresWithContacts()
    // {
    //     return await _salesService.GetStoresWithContacts();
    // }
}