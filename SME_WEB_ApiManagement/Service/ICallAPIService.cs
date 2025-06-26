using SME_WEB_ApiManagement.Models;

namespace SME_WEB_ApiManagement.Services
{
    public interface ICallAPIService
    {
       Task<string> GetDataApiAsync(MapiInformationModels apiModels, object xdata);

    }
}
